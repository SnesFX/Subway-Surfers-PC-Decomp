using System;
using System.Collections;
using UnityEngine;

public class FollowingGuard : MonoBehaviour
{
	public float distanceToCharacterMin = 10f;

	public float distanceToCharacterMax = 50f;

	public float catchUpDuration = 0.7f;

	public float resetCatchUpDuration = 1.5f;

	public float lastGroundedSmoothTime = 0.3f;

	public float xSmoothTime = 0.1f;

	public float gravity = 200f;

	public bool isShowing;

	public Animation guardAnimation;

	public Animation dogRightAnimation;

	private Renderer[] enemyRenderers;

	public Transform[] enemies;

	private Vector3[] enemiesStartPos;

	private float y;

	private bool closeToCharacter;

	private float distanceToCharacter;

	private float lastGroundedSmooth;

	private float lastGroundedVelocity;

	private SmoothDampFloat x;

	private Game game;

	private Character character;

	private Transform characterTransform;

	private float verticalSpeed;

	public float guardProximityLoopVolume = 0.9f;

	private static FollowingGuard instance;

	private bool isPaused = true;

	public static FollowingGuard Instance
	{
		get
		{
			return instance ?? (instance = UnityEngine.Object.FindObjectOfType(typeof(FollowingGuard)) as FollowingGuard);
		}
	}

	private void Awake()
	{
		game = Game.Instance;
		character = Character.Instance;
		characterTransform = character.transform;
		enemyRenderers = base.gameObject.GetComponentsInChildren<Renderer>();
		enemiesStartPos = new Vector3[enemies.Length];
		for (int i = 0; i < enemies.Length; i++)
		{
			enemiesStartPos[i] = enemies[i].position;
		}
		x = new SmoothDampFloat(0f, xSmoothTime);
		base.GetComponent<AudioSource>().volume = guardProximityLoopVolume;
		Game obj = game;
		obj.OnPauseChange = (Game.OnPauseChangeDelegate)Delegate.Combine(obj.OnPauseChange, new Game.OnPauseChangeDelegate(HandleOnPauseChange));
	}

	private void HandleOnPauseChange(bool pause)
	{
		if (pause)
		{
			if (base.GetComponent<AudioSource>().isPlaying)
			{
				base.GetComponent<AudioSource>().Pause();
			}
			isPaused = true;
		}
		if (!pause)
		{
			if (isPaused)
			{
				base.GetComponent<AudioSource>().Play();
			}
			isPaused = false;
		}
	}

	public void Restart(bool closeToCharacter)
	{
		StopAllCoroutines();
		this.closeToCharacter = closeToCharacter;
		distanceToCharacter = ((!closeToCharacter) ? distanceToCharacterMax : distanceToCharacterMin);
	}

	public void OnEnable()
	{
		lastGroundedSmooth = character.lastGroundedY;
		lastGroundedVelocity = 0f;
		y = character.lastGroundedY;
		x.Value = character.transform.position.x;
		distanceToCharacter = distanceToCharacterMin;
		closeToCharacter = true;
		verticalSpeed = 0f;
		bool flag = false;
		guardAnimation["Guard_Run"].enabled = flag;
		if (flag)
		{
			guardAnimation.Play("Guard_Run");
			dogRightAnimation.Play("Dog_Fast Run");
		}
		Character obj = character;
		obj.OnJump = (Character.OnJumpDelegate)Delegate.Combine(obj.OnJump, new Character.OnJumpDelegate(Jump));
	}

	public void OnDisable()
	{
		Character obj = character;
		obj.OnJump = (Character.OnJumpDelegate)Delegate.Remove(obj.OnJump, new Character.OnJumpDelegate(Jump));
	}

	public void CatchUp()
	{
		CatchUp(catchUpDuration);
	}

	public void CatchUp(float duration)
	{
		if (!closeToCharacter)
		{
			float distanceFrom = distanceToCharacter;
			ShowEnemies(true);
			StopAllCoroutines();
			guardAnimation.Play("Guard_grap after");
			guardAnimation.PlayQueued("Guard_Run");
			base.GetComponent<AudioSource>().timeSamples = UnityEngine.Random.Range(0, base.GetComponent<AudioSource>().timeSamples);
			base.GetComponent<AudioSource>().Play();
			base.GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.9f, 1.05f);
			StartCoroutine(pTween.To(duration, delegate(float t)
			{
				distanceToCharacter = Mathf.SmoothStep(distanceFrom, distanceToCharacterMin, t);
			}));
			StartCoroutine(pTween.To(duration, delegate(float t)
			{
				base.GetComponent<AudioSource>().volume = Mathf.SmoothStep(0f, guardProximityLoopVolume, t);
			}));
			closeToCharacter = true;
		}
	}

	public void ResetCatchUp()
	{
		ResetCatchUp(resetCatchUpDuration);
	}

	public void ResetCatchUp(float duration)
	{
		StartCoroutine(ResetCatchUpCoroutine(duration));
	}

	public IEnumerator ResetCatchUpCoroutine(float duration)
	{
		if (closeToCharacter)
		{
			float distanceFrom = distanceToCharacter;
			closeToCharacter = false;
			StartCoroutine(pTween.To(duration, delegate(float t)
			{
				distanceToCharacter = Mathf.SmoothStep(distanceFrom, distanceToCharacterMax, t);
			}));
			yield return StartCoroutine(pTween.To(duration * 2f, delegate(float t)
			{
				base.GetComponent<AudioSource>().volume = Mathf.SmoothStep(guardProximityLoopVolume, 0f, t);
			}));
			base.GetComponent<AudioSource>().Stop();
			if (!game.isDead)
			{
				ShowEnemies(false);
			}
		}
	}

	public void MuteProximityLoop()
	{
		base.GetComponent<AudioSource>().Stop();
	}

	public void PlayIntro()
	{
		base.gameObject.transform.position = new Vector3(0f, 0f, -10f);
		for (int i = 0; i < enemies.Length; i++)
		{
			enemies[i].position = enemiesStartPos[i];
			enemies[i].rotation = Quaternion.Euler(0f, 0f, 0f);
		}
		guardAnimation.Play("playIntro");
		dogRightAnimation.Play("playIntro");
		guardAnimation.CrossFadeQueued("Guard_Run", 0.2f);
		dogRightAnimation.CrossFadeQueued("Dog_Fast Run", 0.2f);
	}

	public void CatchPlayer(float pos)
	{
		base.GetComponent<AudioSource>().Stop();
		StopAllCoroutines();
		character.characterAnimation.Stop("caught");
		character.characterAnimation.Stop("caught2");
		if (pos < 20f)
		{
			guardAnimation.CrossFade("catch2", 0.2f);
			dogRightAnimation.CrossFade("catch2", 0.2f);
			character.animations.stumbleDeath = "caught2";
		}
		else
		{
			guardAnimation.CrossFade("catch", 0.2f);
			dogRightAnimation.CrossFade("catch", 0.2f);
			character.animations.stumbleDeath = "caught";
		}
		character.characterAnimation[character.animations.stumbleDeath].weight = 0f;
		character.characterAnimation[character.animations.stumbleDeath].enabled = true;
		float num = 0.68f;
		StartCoroutine(pTween.To(num, delegate(float t)
		{
			for (int i = 0; i < enemies.Length; i++)
			{
				enemies[i].position = Vector3.Lerp(enemies[i].position, character.transform.position, t);
			}
		}));
		StartCoroutine(CatchPlayerAnimStarter(num));
	}

	private IEnumerator CatchPlayerAnimStarter(float delay)
	{
		yield return new WaitForSeconds(delay);
		StartCoroutine(pTween.To(0.2f, delegate(float t)
		{
			character.characterAnimation[character.animations.stumbleDeath].weight = Mathf.Lerp(0f, 1f, t);
		}));
	}

	public void HitByTrainSequence()
	{
		base.GetComponent<AudioSource>().Stop();
		StartCoroutine(HitByTrainSequenceCoroutine());
	}

	public IEnumerator HitByTrainSequenceCoroutine()
	{
		float catchUpTime = 0.2f;
		yield return StartCoroutine(pTween.To(catchUpTime, delegate(float t)
		{
			for (int i = 0; i < enemies.Length; i++)
			{
				enemies[i].position = Vector3.Lerp(enemies[i].position, character.transform.position, t);
			}
		}));
		dogRightAnimation.Play("Dog_death_movingTrain");
		yield return new WaitForSeconds(0.4f);
		Vector3 charPos = characterTransform.position;
		StartCoroutine(pTween.To(1f, delegate(float t)
		{
			characterTransform.position = Vector3.Lerp(charPos, new Vector3(charPos.x, -5f, charPos.z), t);
		}));
		yield return new WaitForSeconds(0.2f);
		guardAnimation.Play("Guard_death_movingTrain");
	}

	public void ShowEnemies(bool vis)
	{
		isShowing = vis;
		Renderer[] array = enemyRenderers;
		foreach (Renderer renderer in array)
		{
			renderer.gameObject.active = vis;
		}
	}

	public void LateUpdate()
	{
		x.Target = character.transform.position.x;
		x.Update();
		lastGroundedSmooth = Mathf.SmoothDamp(lastGroundedSmooth, character.lastGroundedY, ref lastGroundedVelocity, lastGroundedSmoothTime);
		if (y > lastGroundedSmooth)
		{
			verticalSpeed -= gravity * Time.deltaTime;
		}
		y += verticalSpeed * Time.deltaTime;
		y = Mathf.Max(y, lastGroundedSmooth);
		Vector3 position = characterTransform.position - Vector3.forward * distanceToCharacter;
		position.y = y;
		position.x = x.Value;
		base.transform.position = position;
	}

	private void Jump()
	{
		Jump(distanceToCharacter / game.currentSpeed);
	}

	public void Jump(float delay)
	{
		StartCoroutine(JumpCoroutine(delay));
	}

	private IEnumerator JumpCoroutine(float delay)
	{
		yield return new WaitForSeconds(delay);
		guardAnimation.Play("Guard_jump");
		guardAnimation.CrossFadeQueued("Guard_Run", 0.2f);
		dogRightAnimation.Play("Dog_jump");
		dogRightAnimation.CrossFadeQueued("Dog_Fast Run", 0.2f);
		verticalSpeed = character.CalculateJumpVerticalSpeed() * 0.6f;
	}
}
