using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
	public struct Animations
	{
		public string[] runAnimations;

		public string[] landAnimations;

		public string[] jumpAnimations;

		public string[] hangtimeAnimations;

		public string[] grindAnimations;

		public string[] grindLandAnimations;

		public string[] hoverAnimations;

		public string[] hoverLandAnimations;

		public string[] hoverJumpAnimations;

		public string[] hoverHangtimeAnimations;

		public string jump;

		public string run;

		public string landing;

		public string hangtime;

		public string roll;

		public string dodgeLeft;

		public string dodgeRight;

		public string hitMid;

		public string hitUpper;

		public string hitLower;

		public string hitMoving;

		public string stumble;

		public string stumbleDeath;

		public string stumbleLeftSide;

		public string stumbleRightSide;

		public string stumbleLeftCorner;

		public string stumbleRightCorner;

		public void SetRandomGrind()
		{
			if (grindAnimations.Length == 0 || grindLandAnimations.Length != grindAnimations.Length)
			{
				Debug.Log("animation arrays should be same length if paired; also not null");
				return;
			}
			int num = UnityEngine.Random.Range(0, grindAnimations.Length);
			run = grindAnimations[num];
			landing = grindLandAnimations[num];
		}

		public void SetRandomRun()
		{
			if (runAnimations.Length == 0 || runAnimations.Length != landAnimations.Length)
			{
				Debug.Log("animation arrays should be same length if paired; also not null");
				return;
			}
			int num = UnityEngine.Random.Range(0, runAnimations.Length);
			run = runAnimations[num];
			landing = landAnimations[num];
		}

		public void SetRandomHover()
		{
			if (hoverAnimations.Length == 0 || hoverAnimations.Length != hoverLandAnimations.Length)
			{
				Debug.Log("animation arrays should be same length if paired; also not null");
				return;
			}
			int num = UnityEngine.Random.Range(0, hoverAnimations.Length);
			run = hoverAnimations[num];
			landing = hoverLandAnimations[num];
		}

		public void SetRandomJump()
		{
			if (jumpAnimations.Length == 0 || hangtimeAnimations.Length == 0)
			{
				Debug.Log("animation array is null");
				return;
			}
			int num = UnityEngine.Random.Range(0, jumpAnimations.Length);
			int num2 = UnityEngine.Random.Range(0, hangtimeAnimations.Length);
			jump = jumpAnimations[num];
			hangtime = hangtimeAnimations[num2];
		}

		public void SetRandomHoverJump()
		{
			if (hoverJumpAnimations.Length == 0 || hoverJumpAnimations.Length != hoverHangtimeAnimations.Length)
			{
				Debug.Log("animation arrays should be same length if paired; also not null");
				return;
			}
			int num = UnityEngine.Random.Range(0, hoverJumpAnimations.Length);
			jump = hoverJumpAnimations[num];
			hangtime = hoverHangtimeAnimations[num];
		}
	}

	private enum ObstacleTypes
	{
		jumpTrain = 0,
		rollBarrier = 1,
		jumpBarrier = 2,
		none = 3
	}

	private enum ImpactX
	{
		Left = 0,
		Middle = 1,
		Right = 2
	}

	private enum ImpactY
	{
		Upper = 0,
		Middle = 1,
		Lower = 2
	}

	private enum ImpactZ
	{
		Before = 0,
		Middle = 1,
		After = 2
	}

	public delegate void OnStumbleDelegate();

	public delegate void OnCriticalHitDelegate();

	public delegate void OnJumpDelegate();

	public delegate void OnGroundedDelegate();

	public int initialTrackIndex = 1;

	public CapsuleCollider characterCollider;

	public OnTriggerObject coinMagnetCollider;

	public OnStumbleDelegate OnStumble;

	public OnCriticalHitDelegate OnCriticalHit;

	public OnJumpDelegate OnJump;

	public Transform characterRoot;

	public float characterAngle = 45f;

	public Animation characterAnimation;

	public GameObject shadow;

	public ParticleSystem hoverboardCrashParticleSystem;

	public bool fallAnim;

	private Vector3 characterControllerCenter;

	private float characterControllerHeight;

	private Vector3 characterColliderCenter;

	private float characterColliderHeight;

	public CharacterPickupParticles CharacterPickupParticleSystem;

	public float ColliderTrackWidth = 17f;

	public Animation guardAnimation;

	[HideInInspector]
	public CharacterController characterController;

	[HideInInspector]
	public OnTriggerObject characterColliderTrigger;

	[HideInInspector]
	public CharacterModel characterModel;

	[HideInInspector]
	public CharacterCamera characterCamera;

	[HideInInspector]
	public Hoverboard hoverboard;

	[HideInInspector]
	public SuperSneakers superSneakers;

	[HideInInspector]
	public Running running;

	public GameObject sprayCanModel;

	[HideInInspector]
	public bool immuneToCriticalHit;

	[HideInInspector]
	public int trackIndex;

	[HideInInspector]
	public float x;

	public float z;

	public float verticalSpeed;

	[HideInInspector]
	public float lastGroundedY;

	private int trackMovement;

	private int trackMovementNext;

	private float characterRotation;

	private int trackIndexTarget;

	private float trackIndexPosition;

	private Game game;

	private Track track;

	[HideInInspector]
	public Animations animations;

	[HideInInspector]
	public float jumpHeight;

	public float gravity = 200f;

	public float jumpHeightNormal = 20f;

	public float jumpHeightSuperSneakers = 40f;

	public float verticalFallSpeedLimit = -1f;

	public float stumbleCornerTolerance = 15f;

	[HideInInspector]
	public bool stumble;

	public float stumbleDecayTime = 5f;

	private IEnumerator roll;

	[HideInInspector]
	public bool jumping;

	[HideInInspector]
	public bool falling;

	private bool inAirJump;

	private string lastHitTag;

	[HideInInspector]
	public bool stopColliding;

	private GameStats stats;

	private FollowingGuard guard;

	private AnimationState runState;

	private int grindSwitch = 1;

	private static Character instance;

	private bool startedJumpFromGround;

	private float trainJumpSampleZ;

	private float trainJumpSampleLength = 10f;

	private bool trainJump;

	private float verticalSpeed_jumpTolerance = -30f;

	private ObstacleTypes lastObstacleTriggerType;

	private int lastObstacleTriggerTrackInex;

	public AudioClipInfo DodgeLeft;

	public AudioClipInfo DodgeRight;

	public AudioClipInfo H_Left;

	public AudioClipInfo H_Right;

	public AudioClipInfo StumbleSound;

	public AudioClipInfo StumbleBushSound;

	public AudioClipInfo StumbleSideSound;

	public bool Stumble
	{
		get
		{
			return stumble;
		}
		set
		{
			if (value)
			{
				StartStumble();
			}
			else
			{
				StopStumble();
			}
			stumble = value;
		}
	}

	public static Character Instance
	{
		get
		{
			return instance ?? (instance = UnityEngine.Object.FindObjectOfType(typeof(Character)) as Character);
		}
	}

	public event OnGroundedDelegate OnGrounded;

	public void Awake()
	{
		game = Game.Instance;
		Variable<bool> isInGame2 = game.isInGame;
		isInGame2.OnChange = (Variable<bool>.OnChangeDelegate)Delegate.Combine(isInGame2.OnChange, (Variable<bool>.OnChangeDelegate)delegate(bool isInGame)
		{
			if (!isInGame)
			{
				StopAllCoroutines();
				immuneToCriticalHit = false;
				characterController.enabled = true;
				stopColliding = false;
			}
		});
		track = Track.Instance;
		characterController = Game.Charactercontroller;
		hoverboard = Hoverboard.Instance;
		running = Running.Instance;
		superSneakers = this.FindObject<SuperSneakers>();
		characterModel = GetComponentInChildren<CharacterModel>();
		characterCamera = CharacterCamera.Instance;
		guard = FollowingGuard.Instance;
		CharacterPickupParticleSystem = GetComponentInChildren<CharacterPickupParticles>();
		characterColliderTrigger = characterCollider.GetComponent<OnTriggerObject>();
		OnTriggerObject onTriggerObject = characterColliderTrigger;
		onTriggerObject.OnEnter = (OnTriggerObject.OnEnterDelegate)Delegate.Combine(onTriggerObject.OnEnter, new OnTriggerObject.OnEnterDelegate(OnCharacterColliderEnter));
		OnTriggerObject onTriggerObject2 = characterColliderTrigger;
		onTriggerObject2.OnExit = (OnTriggerObject.OnExitDelegate)Delegate.Combine(onTriggerObject2.OnExit, new OnTriggerObject.OnExitDelegate(OnCharacterColliderExit));
		characterAnimation["caught"].layer = 4;
		characterAnimation["caught"].enabled = false;
		characterAnimation["caught2"].layer = 4;
		characterAnimation["caught2"].enabled = false;
		characterControllerCenter = characterController.center;
		characterControllerHeight = characterController.height;
		characterColliderCenter = characterCollider.center;
		characterColliderHeight = characterCollider.height;
		stats = GameStats.Instance;
	}

	public void Restart()
	{
		trackIndex = initialTrackIndex;
		trackIndexTarget = initialTrackIndex;
		x = track.GetTrackX(trackIndex);
		trackIndexPosition = trackIndex;
		characterModel.ResetBlink();
		z = 0f;
		trackMovement = 0;
		trackMovementNext = 0;
		characterController.transform.position = track.GetPosition(x, z) + Vector3.up * 5f;
		characterController.Move(-5f * Vector3.up);
		verticalSpeed = 0f;
		jumpHeight = jumpHeightNormal;
		inAirJump = false;
		lastGroundedY = 0f;
		guard.Restart(false);
		Stumble = false;
		startedJumpFromGround = false;
	}

	public void ChangeTrack(int movement, float duration)
	{
		stats.trackChanges++;
		if (trackMovement != movement)
		{
			ForceChangeTrack(movement, duration);
		}
		else
		{
			trackMovementNext = movement;
		}
	}

	public void ForceChangeTrack(int movement, float duration)
	{
		trackMovement = movement;
		trackMovementNext = 0;
		StopAllCoroutines();
		StartCoroutine(ChangeTrackCoroutine(movement, duration));
	}

	private IEnumerator ChangeTrackCoroutine(int move, float duration)
	{
		trackMovement = move;
		trackMovementNext = 0;
		int newTrackIndex = trackIndexTarget + move;
		float trackChangeIndexDistance = Mathf.Abs((float)newTrackIndex - trackIndexPosition);
		float trackIndexPositionBegin = trackIndexPosition;
		float startX = x;
		float endX = track.GetTrackX(newTrackIndex);
		float dir = Mathf.Sign(newTrackIndex - trackIndexTarget);
		float startRotation = characterRotation;
		if (characterController.isGrounded)
		{
			string dodgeAnimation = ((!(dir < 0f)) ? animations.dodgeRight : animations.dodgeLeft);
			characterAnimation["dodgeRight"].speed = Game.Instance.NormalizedGameSpeed;
			characterAnimation["dodgeLeft"].speed = Game.Instance.NormalizedGameSpeed;
			characterAnimation.CrossFade(dodgeAnimation, 0.02f);
		}
		characterAnimation.CrossFadeQueued(animations.run, (!game.Modifiers.IsActive(game.Modifiers.Hoverboard)) ? 0.02f : 0.4f);
		if (newTrackIndex < 0 || newTrackIndex >= track.numberOfTracks)
		{
			NotifyStumble(StumbleSideSound, "side");
			if (!game.Modifiers.IsActive(game.Modifiers.Hoverboard) && !game.IsInJetpackMode)
			{
				characterAnimation.CrossFade((!(dir < 0f)) ? "stumbleOffRight" : "stumbleOffLeft", 0.2f);
			}
			characterAnimation.CrossFadeQueued(animations.run, (!game.Modifiers.IsActive(game.Modifiers.Hoverboard)) ? 0.02f : 0.4f);
			yield break;
		}
		if (move < 0)
		{
			if (game.Modifiers.IsActive(game.Modifiers.Hoverboard))
			{
				So.Instance.playSound(H_Left);
			}
			else
			{
				So.Instance.playSound(DodgeLeft);
			}
		}
		else if (game.Modifiers.IsActive(game.Modifiers.Hoverboard))
		{
			So.Instance.playSound(H_Right);
		}
		else
		{
			So.Instance.playSound(DodgeRight);
		}
		trackIndexTarget = newTrackIndex;
		yield return StartCoroutine(pTween.To(trackChangeIndexDistance * duration, delegate(float t)
		{
			trackIndexPosition = Mathf.Lerp(trackIndexPositionBegin, newTrackIndex, t);
			x = Mathf.Lerp(startX, endX, t);
			characterRotation = pMath.Bell(t) * dir * characterAngle + Mathf.Lerp(startRotation, 0f, t);
			characterRoot.localRotation = Quaternion.Euler(0f, characterRotation, 0f);
		}));
		trackIndex = newTrackIndex;
		trackMovement = 0;
		if (trackMovementNext != 0)
		{
			StartCoroutine(ChangeTrackCoroutine(trackMovementNext, duration));
		}
	}

	public void SetBackToCheckPoint(float zoomTime)
	{
		float lastCheckPoint = track.GetLastCheckPoint(z);
		trackIndex = initialTrackIndex;
		trackIndexTarget = initialTrackIndex;
		float trackX = track.GetTrackX(trackIndex);
		trackIndexPosition = trackIndex;
		trackMovement = 0;
		trackMovementNext = 0;
		StartCoroutine(MoveCharacterToPosition(trackX, lastCheckPoint, zoomTime));
	}

	private IEnumerator MoveCharacterToPosition(float newX, float newZ, float time)
	{
		float oldX = x;
		float oldZ = z;
		game.ChangeState(null);
		immuneToCriticalHit = true;
		stopColliding = true;
		characterController.enabled = false;
		characterAnimation.CrossFade(animations.run, time);
		float newX2 = default(float);
		float newZ2 = default(float);
		yield return StartCoroutine(pTween.To(time, delegate(float t)
		{
			x = Mathf.SmoothStep(oldX, newX2, t);
			z = Mathf.SmoothStep(oldZ, newZ2, t);
		}));
		immuneToCriticalHit = false;
		characterController.enabled = true;
		characterAnimation.Play(animations.run);
		stopColliding = false;
		game.ChangeState(game.Running);
	}

	private ObstacleTypes ObstacleTagToType(string tag)
	{
		switch (tag)
		{
		case "JumpTrain":
			return ObstacleTypes.jumpTrain;
		case "RollBarrier":
			return ObstacleTypes.rollBarrier;
		case "JumpBarrier":
			return ObstacleTypes.jumpBarrier;
		default:
			return ObstacleTypes.none;
		}
	}

	private void OnCharacterColliderExit(Collider collider)
	{
		ObstacleTypes obstacleTypes = ObstacleTagToType(collider.tag);
		if (obstacleTypes == lastObstacleTriggerType && lastObstacleTriggerTrackInex == trackIndex)
		{
			switch (obstacleTypes)
			{
			case ObstacleTypes.jumpBarrier:
				stats.jumpBarrier++;
				break;
			case ObstacleTypes.jumpTrain:
				stats.jumpsOverTrains++;
				break;
			case ObstacleTypes.rollBarrier:
				stats.dodgeBarrier++;
				break;
			}
		}
	}

	private void OnCharacterColliderEnter(Collider collider)
	{
		if (stopColliding || collider.gameObject.layer == 16)
		{
			return;
		}
		Pickup componentInChildren = collider.GetComponentInChildren<Pickup>();
		if (componentInChildren != null)
		{
			NotifyPickup(componentInChildren);
			return;
		}
		if (collider.gameObject.layer == 0)
		{
			if (collider.isTrigger && characterController.isGrounded && this.OnGrounded != null)
			{
				this.OnGrounded();
			}
			if (collider.isTrigger)
			{
				ObstacleTypes obstacleTypes = ObstacleTagToType(collider.tag);
				if (obstacleTypes != ObstacleTypes.none)
				{
					lastObstacleTriggerType = obstacleTypes;
					lastObstacleTriggerTrackInex = trackIndex;
				}
			}
			return;
		}
		if (collider.isTrigger)
		{
			characterAnimation.CrossFade(animations.stumble, 0.05f);
			characterAnimation.CrossFadeQueued(animations.run, 0.5f);
			if (collider.name == "bush")
			{
				NotifyStumble(StumbleBushSound, collider.name);
			}
			else
			{
				NotifyStumble(StumbleSound, collider.name);
			}
			return;
		}
		lastHitTag = collider.tag;
		ImpactX impactX = GetImpactX(collider);
		ImpactY impactY = GetImpactY(collider);
		ImpactZ impactZ = GetImpactZ(collider);
		int num = (((collider.bounds.min.x + collider.bounds.max.x) / 2f > base.transform.position.x) ? 1 : (-1));
		bool flag = trackMovement == num;
		bool flag2 = characterCollider.bounds.center.z < collider.bounds.min.z;
		bool flag3 = impactZ == ImpactZ.Before && !flag2 && flag;
		if (impactZ == ImpactZ.Middle || flag3)
		{
			if (trackMovement != 0)
			{
				float duration = 0.5f;
				if (track.IsRunningOnTutorialTrack)
				{
					duration = 0.2f;
				}
				ChangeTrack(-trackMovement, duration);
			}
			switch (impactX)
			{
			case ImpactX.Left:
				characterAnimation.Play(animations.stumbleLeftSide);
				characterAnimation.PlayQueued(animations.run);
				NotifyStumble(StumbleSound, collider.name);
				break;
			case ImpactX.Right:
				characterAnimation.Play(animations.stumbleRightSide);
				characterAnimation.PlayQueued(animations.run);
				NotifyStumble(StumbleSound, collider.name);
				break;
			}
			return;
		}
		if (impactX == ImpactX.Middle)
		{
			bool flag4 = true;
			if (!immuneToCriticalHit)
			{
				if (impactY == ImpactY.Lower)
				{
					characterAnimation.CrossFade(animations.stumble, 0.05f);
					characterAnimation.CrossFadeQueued(animations.run, 0.5f);
					flag4 = false;
					verticalSpeed = CalculateJumpVerticalSpeed(8f);
					NotifyStumble(StumbleSound, collider.name);
				}
				else if (collider.gameObject.CompareTag("HitMovingTrain"))
				{
					HitByTrainSequence();
				}
				else if (impactY == ImpactY.Middle)
				{
					characterAnimation.CrossFade(animations.hitMid, 0.07f);
				}
				else
				{
					characterAnimation.CrossFade(animations.hitUpper, 0.07f);
				}
			}
			if (flag4)
			{
				NotifyCriticalHit();
			}
			return;
		}
		if (impactZ == ImpactZ.Before && flag)
		{
			if (collider.gameObject.CompareTag("HitMovingTrain"))
			{
				HitByTrainSequence();
				NotifyCriticalHit();
			}
			else if (collider.gameObject.layer == 13)
			{
				characterAnimation.CrossFade(animations.stumble, 0.05f);
				characterAnimation.CrossFadeQueued(animations.run, 0.5f);
			}
			else
			{
				ForceChangeTrack(-trackMovement, 0.5f);
			}
		}
		else if (collider.gameObject.layer == 13)
		{
			ForceChangeTrack(-trackMovement, 0.5f);
		}
		switch (impactX)
		{
		case ImpactX.Left:
			characterAnimation.Play(animations.stumbleLeftCorner);
			characterAnimation.PlayQueued(animations.run);
			break;
		case ImpactX.Right:
			characterAnimation.Play(animations.stumbleRightCorner);
			characterAnimation.PlayQueued(animations.run);
			break;
		}
		NotifyStumble(StumbleSound, collider.name);
	}

	private void HitByTrainSequence()
	{
		if (!hoverboard.isActive)
		{
			characterAnimation.Play(animations.hitMoving);
			Vector3 currentPos = base.transform.position;
			Vector3 camPos = characterCamera.transform.position;
			StartCoroutine(pTween.To(0.5f, delegate(float t)
			{
				base.transform.position = Vector3.Lerp(currentPos, new Vector3(camPos.x, camPos.y - 33f, currentPos.z), t);
			}));
		}
	}

	private ImpactX GetImpactX(Collider collider)
	{
		Bounds bounds = characterCollider.bounds;
		Bounds bounds2 = collider.bounds;
		float num = Mathf.Max(bounds.min.x, bounds2.min.x);
		float num2 = Mathf.Min(bounds.max.x, bounds2.max.x);
		float num3 = (num + num2) * 0.5f;
		float num4 = (num3 - bounds2.min.x) / bounds2.size.x;
		float num5 = num3 - bounds2.min.x;
		if ((double)num5 > (double)bounds2.size.x - (double)ColliderTrackWidth * 0.33)
		{
			return ImpactX.Right;
		}
		if ((double)num5 < (double)ColliderTrackWidth * 0.33)
		{
			return ImpactX.Left;
		}
		return ImpactX.Middle;
	}

	private ImpactZ GetImpactZ(Collider collider)
	{
		Vector3 position = base.transform.position;
		Bounds bounds = collider.bounds;
		if (position.z > bounds.max.z - ((!(bounds.max.z - bounds.min.z > 30f)) ? ((bounds.max.z - bounds.min.z) * 0.5f) : stumbleCornerTolerance))
		{
			return ImpactZ.After;
		}
		if (position.z < bounds.min.z + stumbleCornerTolerance)
		{
			return ImpactZ.Before;
		}
		return ImpactZ.Middle;
	}

	private ImpactY GetImpactY(Collider collider)
	{
		Bounds bounds = characterCollider.bounds;
		Bounds bounds2 = collider.bounds;
		float num = Mathf.Max(bounds.min.y, bounds2.min.y);
		float num2 = Mathf.Min(bounds.max.y, bounds2.max.y);
		float num3 = (num + num2) * 0.5f;
		float num4 = (num3 - bounds.min.y) / bounds.size.y;
		if (num4 < 0.33f)
		{
			return ImpactY.Lower;
		}
		if (num4 < 0.66f)
		{
			return ImpactY.Middle;
		}
		return ImpactY.Upper;
	}

	public void Update()
	{
		if (roll != null)
		{
			roll.MoveNext();
		}
		Vector3 position = base.transform.position;
		if (position.y < 0f)
		{
			position.y = 1f;
			base.transform.position = position;
			Debug.Log("Character y-position has been clamped to avoid fallthrough.");
		}
	}

	public float GetTrackX()
	{
		return track.GetPosition(track.GetTrackX(trackIndex), 0f).x;
	}

	public void Jump()
	{
		fallAnim = true;
		if (hoverboard.isActive)
		{
			animations.SetRandomHoverJump();
		}
		else
		{
			animations.SetRandomJump();
		}
		bool flag = falling && !jumping && verticalSpeed < 0f && verticalSpeed > verticalSpeed_jumpTolerance;
		if (characterController.isGrounded || flag)
		{
			jumping = true;
			falling = false;
			shadow.active = false;
			verticalSpeed = CalculateJumpVerticalSpeed(jumpHeight);
			characterAnimation.CrossFade(animations.jump, 0.05f);
			if (IsRunningOnGround())
			{
				startedJumpFromGround = true;
				trainJump = false;
				trainJumpSampleZ = z + trainJumpSampleLength;
			}
			if (OnJump != null)
			{
				OnJump();
			}
			stats.jumps++;
		}
		else if (verticalSpeed < 0f)
		{
			inAirJump = true;
		}
	}

	private bool IsRunningOnGround()
	{
		return running.currentRunPosition == Running.RunPositions.ground;
	}

	public void CheckInAirJump()
	{
		if (characterController.isGrounded && inAirJump)
		{
			Jump();
			inAirJump = false;
		}
	}

	public void Roll()
	{
		if (roll == null)
		{
			roll = BeginRoll();
			stats.rolls++;
			if (trackIndex == 0)
			{
				stats.rollsLeftTrack++;
			}
			if (trackIndex == 1)
			{
				stats.rollsCenterTrack++;
			}
			if (trackIndex == 2)
			{
				stats.rollsRightTrack++;
			}
		}
	}

	public void ApplyGravity()
	{
		if (verticalSpeed < 0f && characterController.isGrounded)
		{
			if (startedJumpFromGround && trainJump && IsRunningOnGround())
			{
				stats.jumpsOverTrains++;
			}
			if (running.currentRunPosition != Running.RunPositions.air)
			{
				startedJumpFromGround = false;
			}
			verticalSpeed = 0f;
			if (jumping || falling)
			{
				jumping = false;
				falling = false;
				if (this.OnGrounded != null)
				{
					this.OnGrounded();
				}
				if (roll == null)
				{
					shadow.active = true;
					SetRunAnim();
					if (fallAnim)
					{
						characterAnimation.CrossFade(animations.landing, 0.05f);
						characterAnimation.CrossFadeQueued(animations.run, 0.1f);
					}
					else
					{
						fallAnim = true;
						characterAnimation.CrossFade(animations.run, 0.1f);
					}
				}
			}
		}
		else if (startedJumpFromGround && trainJumpSampleZ < z)
		{
			RaycastHit hitInfo;
			if (Physics.Raycast(new Ray(base.transform.position, -Vector3.up), out hitInfo))
			{
				Debug.DrawRay(base.transform.position, -Vector3.up * hitInfo.distance, Color.red, 1000f);
				if (hitInfo.collider.CompareTag("HitMovingTrain") || hitInfo.collider.CompareTag("HitTrain"))
				{
					trainJump = true;
				}
			}
			trainJumpSampleZ += trainJumpSampleLength;
		}
		verticalSpeed -= gravity * Time.deltaTime;
		if (characterController.isGrounded)
		{
			return;
		}
		shadow.active = false;
		if (!falling && verticalSpeed < verticalFallSpeedLimit && roll == null)
		{
			falling = true;
			if (fallAnim)
			{
				characterAnimation.CrossFade(animations.hangtime, 0.2f);
			}
		}
	}

	public void MoveWithGravity()
	{
		if (characterController.enabled)
		{
			verticalSpeed -= gravity * Time.deltaTime;
			if (verticalSpeed > 0f)
			{
				verticalSpeed = 0f;
			}
			Vector3 motion = verticalSpeed * Time.deltaTime * Vector3.up;
			characterController.Move(motion);
		}
	}

	public void MoveForward()
	{
		Vector3 position = base.transform.position;
		float num = z + game.currentSpeed * Time.deltaTime;
		Vector3 vector = verticalSpeed * Time.deltaTime * Vector3.up;
		Vector3 position2 = track.GetPosition(x, num);
		Vector3 vector2 = new Vector3(position.x, 0f, position.z);
		Vector3 vector3 = position2 - vector2;
		if (characterController.enabled)
		{
			characterController.Move(vector + vector3);
		}
		else
		{
			characterController.transform.position = characterController.transform.position + vector3;
		}
		z = base.transform.position.z;
		if (characterController.isGrounded)
		{
			lastGroundedY = position.y;
		}
	}

	private IEnumerator BeginRoll()
	{
		characterAnimation.CrossFade(animations.roll, 0.1f);
		SetRunAnim();
		fallAnim = false;
		characterAnimation.CrossFadeQueued(animations.run, (!game.Modifiers.IsActive(game.Modifiers.Hoverboard)) ? 0f : 0.2f);
		characterController.height = 4f;
		characterController.center = new Vector3(0f, 2f, characterControllerCenter.z);
		characterCollider.height = 4f;
		characterCollider.center = new Vector3(0f, 4f, characterColliderCenter.z);
		verticalSpeed = 0f - CalculateJumpVerticalSpeed(jumpHeight);
		float endTime = Time.time + characterAnimation[animations.roll].length;
		while (Time.time < endTime)
		{
			yield return 0;
			if (!characterAnimation[animations.roll].enabled)
			{
				break;
			}
		}
		if (characterController.enabled)
		{
			characterController.Move(Vector3.up * 2f);
		}
		characterController.center = characterControllerCenter;
		characterController.height = characterControllerHeight;
		characterCollider.center = characterColliderCenter;
		characterCollider.height = characterColliderHeight;
		if (characterController.enabled)
		{
			characterController.Move(Vector3.down * 2f);
		}
		roll = null;
	}

	public float CalculateJumpVerticalSpeed(float jumpHeight)
	{
		return Mathf.Sqrt(2f * jumpHeight * gravity);
	}

	public float CalculateJumpVerticalSpeed()
	{
		return CalculateJumpVerticalSpeed(jumpHeight);
	}

	public float JumpLength(float speed, float jumpHeight)
	{
		return speed * 2f * CalculateJumpVerticalSpeed(jumpHeight) / gravity;
	}

	private void StartStumble()
	{
		guard.CatchUp();
		guard.StartCoroutine(StumbleDecay());
	}

	private void StopStumble()
	{
		guard.ResetCatchUp();
	}

	private IEnumerator StumbleDecay()
	{
		yield return new WaitForSeconds(stumbleDecayTime);
		stumble = false;
		StopStumble();
	}

	private void NotifyStumble(AudioClipInfo sound, string nameOfCollider)
	{
		if (game.IsInJetpackMode)
		{
			return;
		}
		So.Instance.playSound(sound);
		if (track.IsRunningOnTutorialTrack)
		{
			return;
		}
		if (OnStumble != null)
		{
			OnStumble();
			switch (nameOfCollider)
			{
			case "lightSignal":
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.BumpLightSignal);
				break;
			case "bush":
			case "powerbox":
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.BumpBush);
				break;
			case "collider stumble":
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.BumpTrain);
				break;
			case "blocker_jump":
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.BumpBarrier);
				break;
			case "blocker_roll":
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.BumpBarrier);
				break;
			case "blocker_standard":
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.BumpBarrier);
				break;
			default:
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.BumpTrain);
				break;
			case "side":
			case "collider":
				break;
			}
		}
		Stumble = false;
	}

	private void NotifyCriticalHit()
	{
		if (OnCriticalHit != null)
		{
			OnCriticalHit();
			switch (lastHitTag)
			{
			case "HitTrain":
				stats.trainHit++;
				break;
			case "HitBarrier":
				stats.barrierHit++;
				break;
			case "HitMovingTrain":
				stats.movingTrainHit++;
				break;
			}
		}
	}

	public void NotifyPickup(Pickup pickup)
	{
		pickup.NotifyPickup(CharacterPickupParticleSystem);
	}

	public void ChangeAnimations()
	{
		if (game.isDead)
		{
			return;
		}
		if (hoverboard.isActive)
		{
			animations.run = "h_run";
			animations.roll = "h_roll";
			animations.dodgeLeft = "h_left";
			animations.dodgeRight = "h_right";
		}
		else
		{
			if (superSneakers.isActive)
			{
				animations.run = "superRun";
				animations.landing = "landing";
			}
			else
			{
				animations.SetRandomRun();
			}
			animations.roll = "roll";
			animations.dodgeLeft = "dodgeLeft";
			animations.dodgeRight = "dodgeRight";
			animations.SetRandomJump();
		}
		if (characterController.isGrounded)
		{
			characterAnimation.CrossFade(animations.run);
		}
	}

	public void SetAnimations()
	{
		animations.run = "run";
		animations.runAnimations = new string[4] { "run", "run2", "run3", "run4_long" };
		animations.landAnimations = new string[4] { "landing", "landing", "landing", "landing3" };
		animations.jumpAnimations = new string[5] { "jump", "jump", "jump_salto", "jump2", "jump3" };
		animations.hangtimeAnimations = new string[4] { "hangtime", "hangtime", "hangtime2", "hangtime3" };
		animations.grindAnimations = new string[3] { "h_Grind1", "h_Grind2", "h_Grind3" };
		animations.grindLandAnimations = new string[3] { "landing_grind1", "landing_grind2", "landing_grind3" };
		animations.hoverAnimations = new string[1] { "h_run" };
		animations.hoverLandAnimations = new string[1] { "h_landing" };
		animations.hoverJumpAnimations = new string[11]
		{
			"h_jump", "h_jump2_kickflip", "h_jump3_180", "h_jump4_360flip", "h_jump5_Impossible", "h_jump6_nollie", "h_jump7_heelflip", "h_jump8_pop shuvit", "h_jump9_fs360", "h_jump10_heel360",
			"h_jump11_fs salto"
		};
		animations.hoverHangtimeAnimations = new string[11]
		{
			"h_hangtime", "h_jump2_kickflip", "h_jump3_180", "h_jump4_360flip", "h_jump5_Impossible", "h_jump6_nollie", "h_jump7_heelflip", "h_jump8_pop shuvit", "h_jump9_fs360", "h_jump10_heel360",
			"h_jump11_fs salto"
		};
		animations.run = "run";
		animations.jump = "jump";
		animations.hangtime = "hangtime";
		animations.landing = "landing";
		animations.roll = "roll";
		animations.dodgeLeft = "dodgeLeft";
		animations.dodgeRight = "dodgeRight";
		animations.hitMid = "death_bounce";
		animations.hitUpper = "death_upper";
		animations.hitLower = "death_lower";
		animations.hitMoving = "death_movingTrain";
		animations.stumble = "stumble_low";
		animations.stumbleDeath = "caught";
		animations.stumbleLeftSide = "stumbleSideLeft";
		animations.stumbleRightSide = "stumbleSideRight";
		animations.stumbleLeftCorner = "stumbleCornerLeft";
		animations.stumbleRightCorner = "stumbleCornerRight";
	}

	private void SetRunAnim()
	{
		if (hoverboard.isActive)
		{
			if (base.transform.position.y > 20f)
			{
				animations.SetRandomGrind();
			}
			else
			{
				animations.SetRandomHover();
			}
		}
		else if (!superSneakers.isActive)
		{
			animations.SetRandomRun();
		}
	}
}
