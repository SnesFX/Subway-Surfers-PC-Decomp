using System.Collections;
using UnityEngine;

public class Hoverboard : CharacterModifier
{
	private float duration;

	public float cooldownDstance = 50f;

	public float slowMotionDistance = 90f;

	public float slowDownToScale = 0.3f;

	public bool isAllowed = true;

	public GameObject powerupMesh;

	public float WaitForParticlesDelay;

	public float RemoveObstaclesDistance = 250f;

	private Game game;

	private Character character;

	private Track track;

	private float lastEndActivationTime;

	[HideInInspector]
	public bool isActive;

	public AudioClipInfo CrashSound;

	public AudioClipInfo StartSound;

	public ActivePowerup Powerup;

	private static Hoverboard instance;

	public override bool ShouldPauseInJetpack
	{
		get
		{
			return true;
		}
	}

	public static Hoverboard Instance
	{
		get
		{
			return instance ?? (instance = Object.FindObjectOfType(typeof(Hoverboard)) as Hoverboard);
		}
	}

	public void Awake()
	{
		character = Character.Instance;
		track = Track.Instance;
	}

	public override void Reset()
	{
		character.immuneToCriticalHit = false;
		character.characterController.enabled = true;
		character.characterCollider.enabled = true;
		powerupMesh.active = false;
		isActive = false;
		Time.timeScale = 1f;
		character.hoverboardCrashParticleSystem.gameObject.SetActiveRecursively(false);
	}

	public override IEnumerator Begin()
	{
		float timeSinceLastActivation = Time.time - lastEndActivationTime;
		if (!isAllowed || timeSinceLastActivation < WaitForParticlesDelay + PlayerInfo.Instance.GetHoverBoardCoolDown())
		{
			yield break;
		}
		GameStats.Instance.usePowerups++;
		PlayerInfo.Instance.UseUpgrade(PowerupType.hoverboard);
		Missions.Instance.PlayerDidThis(Missions.MissionTarget.HoverBoard);
		Paused = false;
		character.Stumble = false;
		isActive = true;
		character.ChangeAnimations();
		character.characterAnimation.CrossFade("h_skate_on", 0.06f);
		character.characterAnimation.CrossFadeQueued("h_run", 0.2f);
		So.Instance.playSound(StartSound);
		character.CharacterPickupParticleSystem.PickedUpDefaultPowerUp();
		character.immuneToCriticalHit = true;
		stop = StopSignal.DONT_STOP;
		Powerup = GameStats.Instance.TriggerPowerup(PowerupType.hoverboard);
		duration = Powerup.timeLeft;
		powerupMesh.active = true;
		while (Powerup.timeLeft > 0f && stop == StopSignal.DONT_STOP)
		{
			yield return 0;
		}
		if (stop == StopSignal.DONT_STOP)
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.HoverBoardExpire);
		}
		powerupMesh.active = false;
		character.immuneToCriticalHit = false;
		isActive = false;
		character.ChangeAnimations();
		lastEndActivationTime = Time.time;
		if (stop != StopSignal.STOP)
		{
			yield break;
		}
		isActive = false;
		character.immuneToCriticalHit = false;
		character.hoverboardCrashParticleSystem.gameObject.SetActiveRecursively(true);
		character.hoverboardCrashParticleSystem.Play();
		PlayCrashSound();
		float timeLeft = WaitForParticlesDelay;
		while (timeLeft > 0f)
		{
			timeLeft -= Time.deltaTime;
			yield return 0;
		}
		track.LayEmptyChunks(character.z, RemoveObstaclesDistance * Game.Instance.NormalizedGameSpeed);
		character.jumping = true;
		character.falling = false;
		character.verticalSpeed = character.CalculateJumpVerticalSpeed(10f);
		character.characterAnimation.CrossFade(character.animations.jump, 0.05f);
		float newSlowMotionDistance = slowMotionDistance * Game.Instance.NormalizedGameSpeed;
		float newCoolDownDist = cooldownDstance * Game.Instance.NormalizedGameSpeed;
		float distanceLeft = newSlowMotionDistance;
		bool didStopCooldown = false;
		while (distanceLeft > 0f)
		{
			distanceLeft -= Game.Instance.currentLevelSpeed * Time.deltaTime;
			newCoolDownDist -= Game.Instance.currentLevelSpeed * Time.deltaTime;
			if (newCoolDownDist < 0f && !didStopCooldown)
			{
				character.immuneToCriticalHit = false;
				didStopCooldown = true;
			}
			yield return 0;
		}
		character.hoverboardCrashParticleSystem.gameObject.SetActiveRecursively(false);
	}

	public void PlayCrashSound()
	{
		So.Instance.playSound(CrashSound);
	}

	public override void Pause()
	{
		powerupMesh.active = false;
	}

	public override void Resume()
	{
		powerupMesh.active = true;
	}
}
