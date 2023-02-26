using System.Collections;
using UnityEngine;

public class CoinMagnet : CharacterModifier
{
	private float duration;

	public OnTriggerObject coinMagnetCollider;

	public float pullSpeed = 150f;

	public GameObject powerupMesh;

	private CharacterController characterController;

	private Animation characterAnimation;

	private Character character;

	private Transform coinEFX;

	public Transform shoulder;

	private float ratio;

	private Game game;

	public AudioStateLoop audioStateLoop;

	public ActivePowerup Powerup;

	private void Awake()
	{
		character = Character.Instance;
		characterController = character.characterController;
		coinEFX = character.CharacterPickupParticleSystem.CoinEFX.transform;
		characterAnimation = character.characterAnimation;
		characterAnimation["hold_magnet"].AddMixingTransform(shoulder);
		characterAnimation["hold_magnet"].layer = 3;
		characterAnimation["hold_magnet"].weight = 0.9f;
		characterAnimation["hold_magnet"].enabled = false;
		game = Game.Instance;
	}

	public override void Reset()
	{
		ratio = 0f;
		Paused = false;
	}

	public override IEnumerator Begin()
	{
		GameStats.Instance.usePowerups++;
		Paused = false;
		audioStateLoop.ChangeLoop(AudioState.Magnet);
		character.Stumble = false;
		powerupMesh.active = true;
		characterAnimation["hold_magnet"].enabled = true;
		characterAnimation.Play("hold_magnet");
		Powerup = GameStats.Instance.TriggerPowerup(PowerupType.coinmagnet);
		duration = Powerup.timeLeft;
		coinMagnetCollider.OnEnter = CoinHit;
		coinMagnetCollider.GetComponent<Collider>().enabled = true;
		base.enabled = true;
		stop = StopSignal.DONT_STOP;
		while (Powerup.timeLeft > 0f && stop == StopSignal.DONT_STOP)
		{
			yield return 0;
			ratio = Powerup.timeLeft / duration;
		}
		coinMagnetCollider.GetComponent<Collider>().enabled = false;
		base.enabled = false;
		powerupMesh.active = false;
		coinEFX.localPosition = Vector3.zero;
		characterAnimation["hold_magnet"].enabled = false;
		audioStateLoop.ChangeLoop(AudioState.MagnetStop);
	}

	public void CoinHit(Collider collider)
	{
		Coin component = collider.GetComponent<Coin>();
		if (component != null)
		{
			component.GetComponent<Collider>().enabled = false;
			StartCoroutine(Pull(component));
		}
	}

	private IEnumerator Pull(Coin coin)
	{
		Transform pivot = coin.pivot.transform;
		Vector3 position = pivot.position;
		float distance = (position - characterController.transform.position).magnitude;
		yield return StartCoroutine(pTween.To(distance / (pullSpeed * game.NormalizedGameSpeed), delegate(float t)
		{
			pivot.position = Vector3.Lerp(position, powerupMesh.transform.position, t * t);
		}));
		coinEFX.position = powerupMesh.transform.position;
		Pickup pickup = coin.GetComponent<Pickup>();
		character.NotifyPickup(pickup);
		GameStats.Instance.coinsCoinMagnet++;
	}
}
