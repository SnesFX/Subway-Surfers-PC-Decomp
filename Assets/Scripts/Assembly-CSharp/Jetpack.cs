using System;
using System.Collections;
using UnityEngine;

public class Jetpack : CharacterState
{
	[Serializable]
	public class FlyAheadInfo
	{
		public AnimationCurve cameraMovement;
	}

	public delegate void OnStopDelegate();

	public Vector3 cameraOffset = new Vector3(0f, 33f, -33f);

	public float cameraOffsetSmoothDuration = 1f;

	public float cameraAimOffset = 20f;

	public float cameraFOV = 60f;

	public float ySmoothDuration = 0.5f;

	public float speedup = 2f;

	public float flyHeight = 95f;

	public float coinOffset = 200f;

	public float flyAheadDuration = 1.5f;

	private float flyingDuration;

	public float calmDownDuration = 2f;

	public float stopBeforeLandingChunkDistance = 50f;

	public float characterAngle = 45f;

	public float characterChangeTrackLength = 60f;

	public GameObject powerupMesh;

	public AudioClipInfo StartSound;

	public bool headStart;

	public float headStartDistance;

	public float headStartSpeed = 100f;

	public PowerupType powerType;

	public OnStopDelegate OnStop;

	public ActivePowerup Powerup;

	public FlyAheadInfo flyAhead;

	public GameObject JetpackParticles;

	private Game game;

	private Track track;

	private Character character;

	private CharacterController characterController;

	private Transform characterTransform;

	private CharacterCamera characterCamera;

	private Transform characterCameraTransform;

	private Animation characterAnimation;

	public InAirCoinsManager coinsManager;

	private float fuel;

	private float landingZ;

	private float landingTime;

	public AudioStateLoop audioStateLoop;

	public AnimationCurve fisso;

	private static Jetpack instance;

	public override bool PauseActiveModifiers
	{
		get
		{
			return true;
		}
	}

	public float LandingZ
	{
		get
		{
			return landingZ;
		}
	}

	public float LandingTime
	{
		get
		{
			return landingTime;
		}
	}

	public static Jetpack Instance
	{
		get
		{
			return instance ?? (instance = UnityEngine.Object.FindObjectOfType(typeof(Jetpack)) as Jetpack);
		}
	}

	public void Awake()
	{
		game = Game.Instance;
		track = Track.Instance;
		character = Character.Instance;
		characterController = character.characterController;
		characterTransform = characterController.transform;
		characterCamera = CharacterCamera.Instance;
		characterCameraTransform = characterCamera.transform;
		characterAnimation = character.characterAnimation;
		coinsManager = this.FindObject<InAirCoinsManager>();
		Variable<bool> isInGame2 = game.isInGame;
		isInGame2.OnChange = (Variable<bool>.OnChangeDelegate)Delegate.Combine(isInGame2.OnChange, (Variable<bool>.OnChangeDelegate)delegate(bool isInGame)
		{
			if (!isInGame)
			{
				TurnOffEffects();
			}
		});
	}

	public override IEnumerator Begin()
	{
		GameStats.Instance.usePowerups++;
		Powerup = GameStats.Instance.TriggerPowerup(powerType);
		audioStateLoop.ChangeLoop(AudioState.Jetpack);
		game.Modifiers.PauseInJetpackMode();
		character.Stumble = false;
		powerupMesh.active = true;
		character.shadow.active = false;
		ChangeAnimations();
		characterAnimation.CrossFade(character.animations.run);
		Vector3 startCameraOffset = characterCameraTransform.position - characterTransform.position;
		float startCameraAimOffset = game.Running.cameraAimOffset;
		float startY = characterTransform.position.y;
		characterController.detectCollisions = false;
		character.characterCollider.enabled = false;
		fuel = 1f;
		SmoothDampFloat y = new SmoothDampFloat(characterTransform.position.y, ySmoothDuration)
		{
			Target = flyHeight
		};
		float jetpackSpeed = ((!headStart) ? (game.currentLevelSpeed * speedup) : headStartSpeed);
		float flyingDuration = ((!headStart) ? Powerup.timeLeft : (headStartDistance / headStartSpeed));
		float flyDistance2 = jetpackSpeed * flyingDuration;
		float flyAheadDistance = jetpackSpeed * flyAheadDuration;
		float jetpackDistance2 = flyAheadDistance + flyDistance2;
		jetpackDistance2 = track.LayJetpackChunks(character.z, jetpackDistance2) - stopBeforeLandingChunkDistance * Game.Instance.NormalizedGameSpeed;
		float extendedJetpackDuration = jetpackDistance2 / jetpackSpeed;
		float extendedFlyDuration = extendedJetpackDuration - flyAheadDuration;
		flyDistance2 = extendedFlyDuration * jetpackSpeed;
		if (!headStart)
		{
			float coinsDistance = flyDistance2 - coinOffset;
			float coinsStartZ = character.z + flyAheadDistance + coinOffset;
			coinsManager.Spawn(coinsStartZ, coinsDistance, flyHeight);
		}
		else
		{
			So.Instance.playSound(StartSound);
		}
		landingTime = Time.time + extendedJetpackDuration;
		landingZ = character.z + jetpackDistance2;
		float cameraZ = character.z;
		float startTime2 = Time.time;
		float ratio2 = (Time.time - startTime2) / flyAheadDuration;
		JetpackParticles.SetActiveRecursively(true);
		game.currentSpeed = jetpackSpeed;
		Vector3 cameraPositionStart = characterCamera.position;
		Vector3 cameraTargetStart = characterCamera.target;
		Vector3 initRot = JetpackParticles.transform.rotation.eulerAngles;
		Vector3 initScale = JetpackParticles.transform.localScale;
		Debug.DrawLine(cameraPositionStart, cameraTargetStart, Color.red, 1000f);
		while (ratio2 < 1f)
		{
			game.HandleControls();
			Vector3 currentCameraOffset = Vector3.Lerp(startCameraOffset, cameraOffset, Mathf.SmoothStep(0f, 1f, ratio2));
			float characterAimOffset = Mathf.Lerp(startCameraAimOffset, cameraAimOffset, Mathf.SmoothStep(0f, 1f, ratio2));
			float particleOffset = Mathf.Lerp(0f, 1f, fisso.Evaluate(ratio2));
			character.z += jetpackSpeed * Time.deltaTime;
			Vector3 pivot = track.GetPosition(character.x, character.z) + Vector3.up * (startY + (flyHeight - startY) * Mathf.SmoothStep(0f, 1f, ratio2));
			characterTransform.position = pivot;
			cameraZ += ((!headStart) ? game.currentLevelSpeed : jetpackSpeed) * Time.deltaTime;
			Vector3 cameraPositionEnd = new Vector3(track.GetPosition(character.x, character.z).x, pivot.y, character.z) + currentCameraOffset;
			Vector3 cameraTargetEnd = pivot + Vector3.up * characterAimOffset;
			float warpedRatio = flyAhead.cameraMovement.Evaluate(ratio2);
			Vector3 cameraPositionNew = Vector3.Lerp(cameraPositionStart, cameraPositionEnd, warpedRatio);
			Vector3 cameraTargetNew = Vector3.Lerp(cameraTargetStart, cameraTargetEnd, warpedRatio);
			cameraPositionNew.x = cameraPositionEnd.x;
			cameraTargetNew.x = cameraPositionEnd.x;
			characterCamera.position = cameraPositionNew;
			characterCamera.target = cameraTargetNew;
			Debug.DrawLine(cameraPositionEnd, cameraTargetEnd, Color.red);
			JetpackParticles.transform.rotation = Quaternion.Euler(initRot - new Vector3(particleOffset * 1f, 0f, 0f));
			JetpackParticles.transform.localScale = initScale + new Vector3(0f, 0f, particleOffset * 2f);
			game.UpdateMeters();
			game.LayTrackChunks();
			yield return 0;
			ratio2 = (Time.time - startTime2) / flyAheadDuration;
		}
		character.characterCollider.enabled = true;
		float width = 200f;
		Debug.DrawLine(Vector3.forward * landingZ + Vector3.left * width, Vector3.forward * landingZ + -Vector3.left * width, Color.red, 100f);
		Debug.DrawLine(Vector3.forward * landingZ + Vector3.left * width, Vector3.forward * landingZ + -Vector3.left * width, Color.blue, 100f);
		startTime2 = Time.time;
		ratio2 = (Time.time - startTime2) / extendedFlyDuration;
		while (ratio2 < 1f)
		{
			game.HandleControls();
			character.z += jetpackSpeed * Time.deltaTime;
			character.transform.position = track.GetPosition(character.x, character.z) + Vector3.up * flyHeight;
			Vector3 characterPosition = character.transform.position;
			characterCamera.position = characterPosition + cameraOffset;
			characterCamera.target = characterPosition + Vector3.up * cameraAimOffset;
			game.UpdateMeters();
			game.LayTrackChunks();
			yield return 0;
			ratio2 = (Time.time - startTime2) / extendedFlyDuration;
			fuel = 1f - ratio2;
			Debug.DrawLine(Vector3.forward * character.z + Vector3.left * width, Vector3.forward * character.z + -Vector3.left * width, Color.green);
		}
		if (OnStop != null)
		{
			OnStop();
		}
		characterController.detectCollisions = true;
		TurnOffEffects();
		coinsManager.ReleaseCoins();
		game.Modifiers.Resume();
		game.ChangeState(game.Running);
		character.ChangeAnimations();
	}

	private void TurnOffEffects()
	{
		JetpackParticles.SetActiveRecursively(false);
		powerupMesh.active = false;
		audioStateLoop.ChangeLoop(AudioState.JetpackStop);
	}

	private void ChangeAnimations()
	{
		character.animations.run = "jetpack_forward";
		character.animations.dodgeLeft = "jetpack_forward";
		character.animations.dodgeRight = "jetpack_forward";
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
		}
	}
}
