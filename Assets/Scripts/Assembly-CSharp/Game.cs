using System;
using System.Collections;
using Sensors;
using UnityEngine;

public class Game : MonoBehaviour
{
	[Serializable]
	public class SwipeInfo
	{
		public float distanceMin = 0.1f;

		public float doubleTapDuration = 0.3f;
	}

	[Serializable]
	public class SpeedInfo
	{
		public float min = 30f;

		public float max = 70f;

		public float rampUpDuration = 200f;
	}

	public delegate void OnGameOverDelegate(GameStats gameStats);

	public delegate void OnPauseChangeDelegate(bool pause);

	public delegate void OnTopMenuDelegate();

	private const float tiltCooldown = 0.3f;

	private const int lengthOYValues = 200;

	[HideInInspector]
	public bool isDead;

	public bool ingameTouchDetection = true;

	private AccelData accelData;

	private bool acc;

	private float accelReadingX;

	private float currentTiltCooldown;

	private bool tiltCooldownActive;

	[HideInInspector]
	public float currentSpeed;

	public float currentLevelSpeed = 30f;

	public float distancePerMeter = 8f;

	public SwipeInfo swipe;

	public SpeedInfo speed;

	public float backToCheckpointDelayTime = 0.7f;

	public float backToCheckpointZoomTime = 1f;

	private bool goingBackToCheckpoint;

	public Transform introAnimation;

	private IEnumerator currentThread;

	private CharacterState characterState;

	[HideInInspector]
	public CharacterModifierCollection modifiers;

	private Swipe currentSwipe;

	private float lastTapTime = float.MinValue;

	public static bool HasLoaded;

	private static CharacterController characterController;

	public Character character;

	public Animation characterAnimation;

	public Animation guardAnimation;

	public Track track;

	private CharacterCamera characterCamera;

	private Transform characterCameraTransform;

	private Distort distort;

	private FollowingGuard enemies;

	public Running running;

	private Jetpack jetpack;

	private static Game instance;

	private float startTime;

	private float currentRunTime;

	private PlayerInfo player;

	private GameStats stats;

	public Action OnGameStarted;

	public Action OnGameEnded;

	public OnGameOverDelegate OnGameOver;

	public OnPauseChangeDelegate OnPauseChange;

	public OnTopMenuDelegate OnTopMenu;

	public Variable<bool> isInGame;

	private TestStats _testStats;

	public AudioStateLoop audioStateLoop;

	public AudioClipInfo DieSound;

	public bool awakeDone;

	public bool isReadyForHeadStart;

	private bool _paused;

	private float sumAccelDataX;

	private float sumAccelDataY;

	private float oldSumAccelDataX;

	private float ignorTimeStampX;

	private float oldX;

	private float oldY;

	private float oldSumAccelDataY;

	private float ignorTimeStampY;

	private float timeStamp;

	private bool filled;

	private float oneOverLengthOYValues = 0.005f;

	public float[] yValues = new float[200];

	private int yValueCounter;

	public bool isPaused
	{
		get
		{
			return _paused;
		}
	}

	private float Medium
	{
		get
		{
			float num = 0f;
			for (int i = 0; i < 200; i++)
			{
				num += yValues[i];
			}
			return num * oneOverLengthOYValues;
		}
	}

	public Character Character
	{
		get
		{
			return character;
		}
	}

	public CharacterState CharacterState
	{
		get
		{
			return characterState;
		}
	}

	public CharacterModifierCollection Modifiers
	{
		get
		{
			return modifiers;
		}
	}

	public Running Running
	{
		get
		{
			return running;
		}
	}

	public Jetpack Jetpack
	{
		get
		{
			return jetpack;
		}
	}

	public bool IsInJetpackMode
	{
		get
		{
			return characterState == Jetpack;
		}
	}

	public bool HasSuperSneakers
	{
		get
		{
			return modifiers.SuperSneakes.isActive;
		}
	}

	public float NormalizedGameSpeed
	{
		get
		{
			return currentSpeed / speed.min;
		}
	}

	public static Game Instance
	{
		get
		{
			return instance ?? (instance = UnityEngine.Object.FindObjectOfType(typeof(Game)) as Game);
		}
	}

	public static CharacterController Charactercontroller
	{
		get
		{
			return characterController ?? (characterController = UnityEngine.Object.FindObjectOfType(typeof(CharacterController)) as CharacterController);
		}
	}

	public Game()
	{
		isInGame = new Variable<bool>(false);
	}

	public void TriggerPause(bool pauseGame)
	{
		_paused = pauseGame;
		if (pauseGame)
		{
			ingameTouchDetection = false;
			Time.timeScale = 0f;
		}
		else
		{
			ingameTouchDetection = true;
			Time.timeScale = 1f;
		}
		if (OnPauseChange != null)
		{
			OnPauseChange(_paused);
		}
	}

	public void StartNewRun()
	{
		isInGame.Value = true;
		ChangeState(null, Intro());
		Action onGameStarted = OnGameStarted;
		if (onGameStarted != null)
		{
			onGameStarted();
		}
	}

	public void Awake()
	{
		HasLoaded = true;
		character = Character.Instance;
		characterAnimation = character.characterAnimation;
		guardAnimation = character.guardAnimation;
		track = Track.Instance;
		characterCamera = CharacterCamera.Instance;
		characterCameraTransform = characterCamera.transform;
		distort = this.FindObject<Distort>();
		running = Running.Instance;
		jetpack = Jetpack.Instance;
		enemies = FollowingGuard.Instance;
		modifiers = new CharacterModifierCollection();
		Character obj = character;
		obj.OnStumble = (Character.OnStumbleDelegate)Delegate.Combine(obj.OnStumble, new Character.OnStumbleDelegate(OnStumble));
		Character obj2 = character;
		obj2.OnCriticalHit = (Character.OnCriticalHitDelegate)Delegate.Combine(obj2.OnCriticalHit, new Character.OnCriticalHitDelegate(OnCriticalHit));
		currentLevelSpeed = Speed(0f);
		player = PlayerInfo.Instance;
		stats = GameStats.Instance;
		character.SetAnimations();
		_testStats = GetComponent<TestStats>();
		awakeDone = true;
	}

	public void Start()
	{
		track.Restart();
		currentThread = GameIntro();
		currentThread.MoveNext();
	}

	public void Update()
	{
		float t = Time.time - startTime;
		currentLevelSpeed = Speed(t);
		currentThread.MoveNext();
		if (characterState != null)
		{
			modifiers.Update();
		}
		GameStats.Instance.UpdatePowerupTimes(Time.deltaTime);
	}

	public void LayTrackChunks()
	{
		track.LayTrackChunks(character.z);
	}

	private void HandleDebugControls()
	{
		DebugTimeControl();
		if (Input.GetKeyDown(KeyCode.S))
		{
			modifiers.Add(modifiers.SuperSneakes);
		}
		if (Input.GetKeyDown(KeyCode.Q))
		{
			ActivateJetpack();
		}
		if (Input.GetKeyDown(KeyCode.P))
		{
			modifiers.Hoverboard.Begin();
		}
		if (Input.GetKeyDown(KeyCode.M))
		{
			modifiers.Add(modifiers.CoinMagnet);
		}
		if (Input.GetKeyDown(KeyCode.H))
		{
		}
		if (characterState != null)
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				characterState.HandleSwipe(SwipeDir.Up);
			}
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				characterState.HandleSwipe(SwipeDir.Down);
			}
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				characterState.HandleSwipe(SwipeDir.Left);
			}
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				characterState.HandleSwipe(SwipeDir.Right);
			}
		}
	}

	public void UpdateMeters()
	{
		stats.meters = Mathf.RoundToInt(character.z / distancePerMeter);
	}

	public float CalcTime(float z)
	{
		if (z <= Position(speed.rampUpDuration))
		{
			float f = speed.min * speed.min + 2f * ((speed.max - speed.min) / speed.rampUpDuration) * z;
			return (0f - speed.min + Mathf.Sqrt(f)) / ((speed.max - speed.min) / speed.rampUpDuration);
		}
		return (z - Position(speed.rampUpDuration)) * 1f / speed.max + speed.rampUpDuration;
	}

	public void ChangeState(CharacterState state)
	{
		characterState = state;
		if (state != null)
		{
			currentThread = state.Begin();
		}
	}

	public void ChangeState(CharacterState state, IEnumerator thread)
	{
		characterState = state;
		currentThread = thread;
	}

	public void ActivateJetpack()
	{
		if (characterState != Jetpack)
		{
			ChangeState(Jetpack);
		}
	}

	private float Speed(float t)
	{
		if (t < speed.rampUpDuration)
		{
			return t * (speed.max - speed.min) / speed.rampUpDuration + speed.min;
		}
		return speed.max;
	}

	private float Position(float t)
	{
		if (t < speed.rampUpDuration)
		{
			return 0.5f * ((speed.max - speed.min) / speed.rampUpDuration) * t * t + speed.min * t;
		}
		return (t - speed.rampUpDuration) * speed.max + 0.5f * (speed.max - speed.min) * speed.rampUpDuration + speed.min * speed.rampUpDuration;
	}

	public void Die()
	{
		if (modifiers.IsActive(modifiers.Hoverboard))
		{
			enemies.MuteProximityLoop();
			enemies.ResetCatchUp();
			character.stumble = false;
			enemies.Restart(false);
			modifiers.Hoverboard.Stop = CharacterModifier.StopSignal.STOP;
			GameStats.Instance.RemoveHoverBoardPowerup();
			return;
		}
		if (track.IsRunningOnTutorialTrack)
		{
			if (!goingBackToCheckpoint)
			{
				StartCoroutine(BackToCheckPointSequence());
			}
			return;
		}
		GameStats.Instance.ClearPowerups();
		isDead = true;
		MovingTrain.ActivateAutoPilot();
		MovingCoin.ActivateAutoPilot();
		if (enemies.isShowing)
		{
			if (characterAnimation["death_movingTrain"].enabled)
			{
				enemies.HitByTrainSequence();
			}
			else
			{
				enemies.CatchPlayer(character.x - character.GetTrackX());
			}
		}
		stats.duration = GetDuration();
		Missions.Instance.PlayerDidThis(Missions.MissionTarget.TimeDeath, Mathf.FloorToInt(GameStats.Instance.duration));
		enemies.enabled = false;
		if (OnGameOver != null)
		{
			OnGameOver(stats);
		}
		Action onGameEnded = OnGameEnded;
		if (onGameEnded != null)
		{
			onGameEnded();
		}
		StopAllCoroutines();
		ChangeState(null, SwitchToDieStateWhenGrounded());
	}

	public float GetDuration()
	{
		return Time.time - startTime;
	}

	private IEnumerator SwitchToDieStateWhenGrounded()
	{
		while (!character.characterController.isGrounded)
		{
			character.MoveWithGravity();
			yield return 0;
		}
		ChangeState(null, DieSequence());
	}

	public void OnCriticalHit()
	{
		if (characterState != null)
		{
			So.Instance.playSound(DieSound);
			characterState.HandleCriticalHit();
		}
	}

	private IEnumerator StumbleDeathSequence()
	{
		currentSpeed = speed.min;
		yield return new WaitForSeconds(0.2f);
		if (characterState != Jetpack)
		{
			characterAnimation.CrossFade("stumbleFall", 0.2f);
			characterState.HandleCriticalHit();
		}
	}

	public void OnStumble()
	{
		if (character.Stumble && characterState != null)
		{
			StartCoroutine(StumbleDeathSequence());
		}
	}

	public void StartJetpack()
	{
		Jetpack.headStart = false;
		Jetpack.powerType = PowerupType.jetpack;
		ChangeState(Jetpack);
	}

	public void PickupJetpack()
	{
		Instance.StartJetpack();
		GameStats.Instance.jetpackPickups++;
	}

	public void StartTopMenu()
	{
		ChangeState(null, TopMenu());
	}

	public void StartHeadStart2000()
	{
		float powerupDuration = PlayerInfo.Instance.GetPowerupDuration(PowerupType.headstart2000);
		Jetpack.headStart = true;
		Jetpack.powerType = PowerupType.headstart2000;
		Jetpack.headStartDistance = powerupDuration * distancePerMeter;
		Jetpack.headStartSpeed = 1000f;
		ChangeState(Jetpack);
		PlayerInfo.Instance.UseUpgrade(PowerupType.headstart2000);
		Missions.Instance.PlayerDidThis(Missions.MissionTarget.Headstart);
	}

	public void StartHeadStart500()
	{
		float powerupDuration = PlayerInfo.Instance.GetPowerupDuration(PowerupType.headstart500);
		Jetpack.headStart = true;
		Jetpack.powerType = PowerupType.headstart500;
		Jetpack.headStartDistance = powerupDuration * distancePerMeter;
		Jetpack.headStartSpeed = 1000f;
		ChangeState(Jetpack);
		PlayerInfo.Instance.UseUpgrade(PowerupType.headstart500);
		Missions.Instance.PlayerDidThis(Missions.MissionTarget.Headstart);
	}

	private IEnumerator DieSequence()
	{
		float wait = Time.time + 2f;
		while (Time.time < wait - 1.5f)
		{
			yield return 0;
		}
		while (Time.time < wait && !Input.GetMouseButtonUp(0))
		{
			if (Input.touchCount > 0)
			{
				Touch touch = Input.touches[0];
				if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
				{
					break;
				}
			}
			yield return 0;
		}
		ingameTouchDetection = false;
		UIScreenController.Instance.GameOverTriggered();
		ChangeState(null, TopMenu());
	}

	private void StageMenuSequence()
	{
		characterAnimation[character.animations.stumbleDeath].enabled = false;
		enemies.enabled = false;
		enemies.ShowEnemies(false);
		enemies.StopAllCoroutines();
		character.StopAllCoroutines();
		character.transform.position = Vector3.zero + new Vector3(0f, 0.8f, 0f);
		characterAnimation.transform.rotation = Quaternion.identity;
		character.sprayCanModel.GetComponent<Renderer>().enabled = true;
		ParticleSystem[] componentsInChildren = character.sprayCanModel.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem particleSystem in componentsInChildren)
		{
			particleSystem.enableEmission = false;
		}
		characterCamera.enabled = false;
		characterCamera.GetComponent<Camera>().fieldOfView = Running.cameraFOV;
		characterCameraTransform.localPosition = character.transform.position + Running.cameraOffset + Vector3.up * 0.8f;
		characterCameraTransform.localRotation = Quaternion.Euler(21.50143f, 0f, 0f);
		characterAnimation.Play("idlePaint");
	}

	private IEnumerator GameIntro()
	{
		ingameTouchDetection = true;
		StageMenuSequence();
		characterCamera.transform.parent.GetComponent<Animation>().Play("introPan");
		float stopTime = Time.time + introAnimation.GetComponent<Animation>()["introPan"].length;
		while (Time.time < stopTime && !Input.GetMouseButtonUp(0))
		{
			if (Input.touchCount > 0)
			{
				Touch touch = Input.touches[0];
				if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
				{
					break;
				}
			}
			yield return 0;
		}
		ingameTouchDetection = false;
		UIScreenController.Instance.ShowMainMenu();
		ChangeState(null, TopMenu());
	}

	private IEnumerator TopMenu()
	{
		isInGame.Value = false;
		audioStateLoop.ChangeLoop(AudioState.Menu);
		enemies.MuteProximityLoop();
		track.DeactivateTrackChunks();
		modifiers.StopWithNoEnding();
		modifiers.Update();
		GameStats.Instance.ClearPowerups();
		jetpack.coinsManager.ReleaseCoins();
		distort.Reset();
		enemies.ShowEnemies(false);
		StageMenuSequence();
		characterCamera.transform.parent.GetComponent<Animation>().CrossFade("menuIdle", 0.1f);
		if (OnTopMenu != null)
		{
			OnTopMenu();
		}
		yield return null;
	}

	private IEnumerator Intro()
	{
		stats.Reset();
		audioStateLoop.ChangeLoop(AudioState.Ingame);
		enemies.MuteProximityLoop();
		isDead = false;
		ingameTouchDetection = true;
		character.CharacterPickupParticleSystem.CoinEFX.transform.localPosition = Vector3.zero;
		ParticleSystem[] componentsInChildren = character.sprayCanModel.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem ps in componentsInChildren)
		{
			ps.enableEmission = false;
		}
		StageMenuSequence();
		enemies.ShowEnemies(true);
		enemies.PlayIntro();
		currentLevelSpeed = Speed(0f);
		startTime = Time.time;
		character.Restart();
		SpawnPointManager.Instance.Restart();
		track.Restart();
		track.LayTrackChunks(0f);
		characterCamera.transform.parent.GetComponent<Animation>().CrossFade("startPan", 0.2f);
		characterAnimation.CrossFade("introRun", 0.2f);
		IEnumerator cameraMovement = pTween.To(characterAnimation.GetComponent<Animation>()["introRun"].length, delegate
		{
		});
		while (cameraMovement.MoveNext())
		{
			yield return 0;
		}
		stats.Reset();
		character.sprayCanModel.GetComponent<Renderer>().enabled = false;
		enemies.enabled = true;
		if (track.IsRunningOnTutorialTrack)
		{
			enemies.ResetCatchUp();
			character.stumble = false;
		}
		isReadyForHeadStart = true;
		ChangeState(Running);
		yield return 0;
	}

	private bool HandleTap()
	{
		bool result = false;
		if (Time.time < lastTapTime + swipe.doubleTapDuration && characterState != null)
		{
			characterState.HandleDoubleTap();
			result = true;
		}
		lastTapTime = Time.time;
		return result;
	}

	public void HandleControls()
	{
		if (_paused)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			currentSwipe = new Swipe();
			currentSwipe.start = Input.mousePosition;
			currentSwipe.startTime = Time.time;
		}
		if ((Input.GetKey(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse0)) && currentSwipe != null)
		{
			currentSwipe.end = Input.mousePosition;
			currentSwipe.endTime = Time.time;
			SwipeDir swipeDir = AnalyzeSwipe(currentSwipe);
			if (swipeDir != SwipeDir.None)
			{
				if (characterState != null)
				{
					characterState.HandleSwipe(swipeDir);
				}
				currentSwipe = null;
			}
		}
		if (Input.GetKeyUp(KeyCode.Mouse0) && currentSwipe != null)
		{
			currentSwipe.end = Input.mousePosition;
			currentSwipe.endTime = Time.time;
			SwipeDir swipeDir2 = AnalyzeSwipe(currentSwipe);
			if (swipeDir2 == SwipeDir.None && characterState != null)
			{
				HandleTap();
			}
		}
		if (Input.touchCount > 0)
		{
			Touch touch = Input.touches[0];
			if (touch.phase == TouchPhase.Began)
			{
				currentSwipe = new Swipe();
				currentSwipe.start = touch.position;
				currentSwipe.startTime = Time.time;
			}
			if ((touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && currentSwipe != null)
			{
				currentSwipe.endTime = Time.time;
				currentSwipe.end = touch.position;
				SwipeDir swipeDir3 = AnalyzeSwipe(currentSwipe);
				if (swipeDir3 != SwipeDir.None)
				{
					if (characterState != null)
					{
						characterState.HandleSwipe(swipeDir3);
					}
					currentSwipe = null;
				}
			}
			if (touch.phase == TouchPhase.Ended && currentSwipe != null)
			{
				currentSwipe.endTime = Time.time;
				currentSwipe.end = touch.position;
				SwipeDir swipeDir4 = AnalyzeSwipe(currentSwipe);
				if (swipeDir4 == SwipeDir.None && characterState != null)
				{
					HandleTap();
				}
			}
		}
		FillYValues();
		AnalyzeTilt();
	}

	private void DebugTimeControl()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			Time.timeScale = 0f;
			PrintTimeScale();
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			Time.timeScale = Mathf.Clamp01(Time.timeScale - 0.1f);
			PrintTimeScale();
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			Time.timeScale += 0.1f;
			PrintTimeScale();
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			Time.timeScale = 1f;
			PrintTimeScale();
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			Time.timeScale = Mathf.Clamp01(Time.timeScale * 0.9f);
			PrintTimeScale();
		}
		if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			Time.timeScale *= 1.1111112f;
			PrintTimeScale();
		}
	}

	private SwipeDir AnalyzeTilt()
	{
		if (!(ignorTimeStampX > Time.time))
		{
			float num = oldX - (float)accelData.x;
			if (Mathf.Abs(num) > 0.1f)
			{
				if (num < 0f)
				{
					if ((float)accelData.x >= 0f)
					{
						characterState.HandleSwipe(SwipeDir.Right);
					}
				}
				else if ((float)accelData.x <= 0f)
				{
					characterState.HandleSwipe(SwipeDir.Left);
				}
				ignorTimeStampX = Time.time + 0.2f + 0f * Mathf.Abs(num);
			}
		}
		oldX = (float)accelData.x;
		oldY = (float)accelData.y;
		sumAccelDataX = (float)accelData.x;
		sumAccelDataY = (float)accelData.y;
		if (accelData.y < (double)Medium - 0.1)
		{
			Debug.Log("Moving Up: " + accelData.y);
			characterState.HandleSwipe(SwipeDir.Up);
			return SwipeDir.Up;
		}
		if (accelData.y > (double)Medium + 0.1)
		{
			Debug.Log("Moving Down: " + accelData.y);
			characterState.HandleSwipe(SwipeDir.Down);
			return SwipeDir.Down;
		}
		return SwipeDir.None;
	}

	private void FixedUpdate()
	{
		yValues[yValueCounter] = (float)accelData.y;
		yValueCounter++;
		if (yValueCounter >= 200)
		{
			yValueCounter = 0;
		}
	}

	private void FillYValues()
	{
		if (!filled)
		{
			filled = true;
			for (int i = 0; i < 200; i++)
			{
				yValues[i] = (float)accelData.y;
			}
			filled = true;
		}
	}

	private void PrintTimeScale()
	{
		Debug.Log("Time scale = " + Time.timeScale);
	}

	private SwipeDir AnalyzeSwipe(Swipe swipe)
	{
		Vector3 b = Camera.main.ScreenToWorldPoint(new Vector3(swipe.start.x, swipe.start.y, 2f));
		Vector3 a = Camera.main.ScreenToWorldPoint(new Vector3(swipe.end.x, swipe.end.y, 2f));
		float num = Vector3.Distance(a, b);
		if (num < this.swipe.distanceMin)
		{
			return SwipeDir.None;
		}
		Vector3 lhs = swipe.end - swipe.start;
		SwipeDir result = SwipeDir.None;
		float num2 = 0f;
		float num3 = Vector3.Dot(lhs, Vector3.up);
		if (num3 > num2)
		{
			num2 = num3;
			result = SwipeDir.Up;
		}
		num3 = Vector3.Dot(lhs, Vector3.down);
		if (num3 > num2)
		{
			num2 = num3;
			result = SwipeDir.Down;
		}
		num3 = Vector3.Dot(lhs, Vector3.left);
		if (num3 > num2)
		{
			num2 = num3;
			result = SwipeDir.Left;
		}
		num3 = Vector3.Dot(lhs, Vector3.right);
		if (num3 > num2)
		{
			num2 = num3;
			result = SwipeDir.Right;
		}
		return result;
	}

	private IEnumerator BackToCheckPointSequence()
	{
		goingBackToCheckpoint = true;
		ChangeState(null);
		yield return new WaitForSeconds(backToCheckpointDelayTime);
		character.SetBackToCheckPoint(backToCheckpointZoomTime);
		yield return new WaitForSeconds(backToCheckpointZoomTime);
		goingBackToCheckpoint = false;
	}
}
