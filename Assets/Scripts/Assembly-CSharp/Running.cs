using System;
using System.Collections;
using UnityEngine;

public class Running : CharacterState
{
	public enum RunPositions
	{
		ground = 0,
		station = 1,
		train = 2,
		movingTrain = 3,
		air = 4
	}

	public float slowDownDuration = 0.625f;

	public float slowDownRatio = 1f;

	public float tunnelDelta = -20f;

	public Vector3 cameraOffset = new Vector3(0f, 33f, -33f);

	public float cameraOffsetSmoothDuration = 0.5f;

	public float cameraAimOffset = 20f;

	public float cameraFOV = 60f;

	public float smoothCameraXDuration = 0.05f;

	public float ySmoothDuration = 0.1f;

	public float characterChangeTrackLength = 30f;

	public AnimationCurve transitionFromJetpackCurve;

	private float tunnelStartZ;

	private Curve offsetDeltaCurve;

	private Game game;

	private Character character;

	private Transform characterTransform;

	private CharacterController characterController;

	private CharacterCamera characterCamera;

	private Transform characterCameraTransform;

	private Animation characterAnimation;

	public Transform spineAnimation;

	private Track track;

	public RunPositions currentRunPosition;

	private static Running instance;

	public static Running Instance
	{
		get
		{
			return instance ?? (instance = UnityEngine.Object.FindObjectOfType(typeof(Running)) as Running);
		}
	}

	public void Awake()
	{
		game = Game.Instance;
		track = Track.Instance;
		character = Character.Instance;
		characterTransform = character.transform;
		characterController = character.characterController;
		characterCamera = CharacterCamera.Instance;
		characterCameraTransform = characterCamera.transform;
		characterAnimation = character.characterAnimation;
		characterAnimation["stumbleCornerLeft"].AddMixingTransform(spineAnimation);
		characterAnimation["stumbleCornerLeft"].layer = 2;
		characterAnimation["stumbleCornerLeft"].weight = 1f;
		characterAnimation["stumbleCornerRight"].AddMixingTransform(spineAnimation);
		characterAnimation["stumbleCornerRight"].layer = 2;
		characterAnimation["stumbleCornerRight"].weight = 1f;
		Character obj = character;
		obj.OnStumble = (Character.OnStumbleDelegate)Delegate.Combine(obj.OnStumble, new Character.OnStumbleDelegate(character.characterCamera.Shake));
		character.OnGrounded += UpdateGroundTag;
	}

	public override IEnumerator Begin()
	{
		bool transitionFromJetpack = characterTransform.position.y > 70f;
		character.characterCollider.enabled = true;
		character.characterCamera.enabled = true;
		SmoothDampVector3 currentCameraOffset = new SmoothDampVector3(cameraOffset, cameraOffsetSmoothDuration)
		{
			Target = cameraOffset
		};
		SmoothDampFloat smoothCameraX = new SmoothDampFloat(characterCameraTransform.position.x, smoothCameraXDuration);
		SmoothDampFloat currentCameraAimOffset = new SmoothDampFloat(cameraAimOffset, cameraOffsetSmoothDuration);
		offsetDeltaCurve = new Curve();
		character.lastGroundedY = character.transform.position.y;
		AnimationEvent setAnimationSpeedEvent = new AnimationEvent
		{
			functionName = "SetAnimationSpeedEvent",
			time = 0.1f,
			messageOptions = SendMessageOptions.RequireReceiver
		};
		characterAnimation["run"].clip.AddEvent(setAnimationSpeedEvent);
		characterAnimation["run2"].clip.AddEvent(setAnimationSpeedEvent);
		characterAnimation["run3"].clip.AddEvent(setAnimationSpeedEvent);
		characterAnimation["run4_long"].clip.AddEvent(setAnimationSpeedEvent);
		ParticleSystem[] componentsInChildren = character.sprayCanModel.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem ps in componentsInChildren)
		{
			ps.enableEmission = false;
		}
		float transitionTimeMax = 2f;
		float transitionTime = 0f;
		Vector3 cameraPositionStart = characterCamera.position;
		Vector3 cameraTargetStart = characterCamera.target;
		character.fallAnim = false;
		characterController.Move(Vector3.down * 2f);
		character.fallAnim = true;
		character.ChangeAnimations();
		SmoothDampFloat y = new SmoothDampFloat(characterTransform.position.y, ySmoothDuration);
		while (true)
		{
			game.LayTrackChunks();
			game.currentSpeed = game.currentLevelSpeed;
			game.HandleControls();
			character.ApplyGravity();
			character.MoveForward();
			Vector3 position = character.transform.position;
			if (game.Modifiers.IsActive(game.Modifiers.SuperSneakes))
			{
				y.Target = 0.5f * (character.lastGroundedY + position.y);
			}
			else if (characterController.isGrounded)
			{
				y.Target = character.lastGroundedY;
			}
			else if (position.y < character.lastGroundedY)
			{
				y.Target = position.y;
			}
			Vector3 offsetDelta = offsetDeltaCurve.Evaluate(character.z - tunnelStartZ);
			smoothCameraX.Target = position.x * 0.75f;
			Vector3 cameraPositionEnd = new Vector3(smoothCameraX.Value, y.Value, position.z) + currentCameraOffset.Value + offsetDelta;
			Vector3 cameraTargetEnd = new Vector3(smoothCameraX.Value, 0f, position.z) + Vector3.up * (y.Value + currentCameraAimOffset.Value) + offsetDelta * 0.5f;
			if (transitionFromJetpack)
			{
				float transitionRatio = Mathf.Clamp01(transitionTime / transitionTimeMax);
				float warpedRatio = transitionFromJetpackCurve.Evaluate(transitionRatio);
				character.characterCamera.position = Vector3.Lerp(cameraPositionStart, cameraPositionEnd, warpedRatio);
				character.characterCamera.target = Vector3.Lerp(cameraTargetStart, cameraTargetEnd, warpedRatio);
				if (transitionRatio == 1f)
				{
					transitionFromJetpack = false;
				}
				transitionTime += Time.deltaTime;
			}
			else
			{
				character.characterCamera.position = cameraPositionEnd;
				character.characterCamera.target = cameraTargetEnd;
			}
			character.CheckInAirJump();
			y.Update();
			currentCameraOffset.Update();
			currentCameraAimOffset.Update();
			smoothCameraX.Update();
			game.UpdateMeters();
			UpdateInAirRunPosition();
			UpdateRunStateMeters();
			yield return 0;
		}
	}

	private void UpdateRunStateMeters()
	{
		float num = game.currentSpeed * Time.deltaTime;
		GameStats gameStats = GameStats.Instance;
		if (currentRunPosition != RunPositions.air)
		{
			if (character.trackIndex == 0)
			{
				gameStats.metersRunLeftTrack += num;
			}
			if (character.trackIndex == 1)
			{
				gameStats.metersRunCenterTrack += num;
			}
			if (character.trackIndex == 2)
			{
				gameStats.metersRunRightTrack += num;
			}
		}
		if (currentRunPosition == RunPositions.ground)
		{
			GameStats.Instance.metersRunGround += num;
		}
		if (currentRunPosition == RunPositions.air)
		{
			GameStats.Instance.metersFly += num;
		}
		if (currentRunPosition == RunPositions.station)
		{
			GameStats.Instance.metersRunStation += num;
		}
		if (currentRunPosition == RunPositions.train)
		{
			GameStats.Instance.metersRunTrain += num;
		}
		if (currentRunPosition == RunPositions.movingTrain)
		{
			GameStats.Instance.metersRunTrain += num;
		}
	}

	private void UpdateInAirRunPosition()
	{
		if (!characterController.isGrounded)
		{
			currentRunPosition = RunPositions.air;
		}
	}

	private void UpdateGroundTag()
	{
		Ray ray = new Ray(character.characterRoot.position, -Vector3.up);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo))
		{
			switch (hitInfo.collider.tag)
			{
			case "Ground":
				currentRunPosition = RunPositions.ground;
				break;
			case "HitTrain":
				currentRunPosition = RunPositions.train;
				break;
			case "HitMovingTrain":
				currentRunPosition = RunPositions.movingTrain;
				break;
			case "Station":
				currentRunPosition = RunPositions.station;
				break;
			}
		}
	}

	public override void HandleCriticalHit()
	{
		character.characterCamera.Shake();
		game.Die();
	}

	public override void HandleSwipe(SwipeDir swipeDir)
	{
		switch (swipeDir)
		{
		case SwipeDir.None:
			break;
		case SwipeDir.Left:
			character.ChangeTrack(-1, characterChangeTrackLength / game.currentSpeed);
			break;
		case SwipeDir.Right:
			character.ChangeTrack(1, characterChangeTrackLength / game.currentSpeed);
			break;
		case SwipeDir.Up:
			character.Jump();
			break;
		case SwipeDir.Down:
			character.Roll();
			break;
		}
	}

	public override void HandleDoubleTap()
	{
		int upgradeAmount = PlayerInfo.Instance.GetUpgradeAmount(PowerupType.hoverboard);
		if (upgradeAmount > 0 && !game.modifiers.IsActive(game.modifiers.Hoverboard))
		{
			game.Modifiers.Add(game.Modifiers.Hoverboard);
		}
	}

	public void StartTunnel(float tunnelLength)
	{
		tunnelStartZ = character.z;
		offsetDeltaCurve = new Curve();
		offsetDeltaCurve.AddKey(0f, Vector3.zero, -Vector3.up, -Vector3.up);
		offsetDeltaCurve.AddKey(tunnelLength / 2f, Vector3.up * tunnelDelta);
		offsetDeltaCurve.AddKey(tunnelLength, Vector3.zero, Vector3.up * 0.001f, Vector3.up * 0.001f);
	}

	public void EndTunnel()
	{
	}
}
