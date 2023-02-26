using System;
using System.Collections;
using UnityEngine;

public class SuperSneakers : CharacterModifier
{
	private float duration;

	public GameObject powerupMesh;

	private Animation characterAnimation;

	[HideInInspector]
	public bool isActive;

	public OnTriggerObject coinMagnetCollider;

	public float pullSpeed = 200f;

	private CharacterController characterController;

	private float ratio;

	private Character character;

	private SuperSneakersGroup[] objects;

	private Game game;

	public ActivePowerup Powerup;

	public override bool ShouldPauseInJetpack
	{
		get
		{
			return true;
		}
	}

	public void Awake()
	{
		character = Character.Instance;
		characterAnimation = character.characterAnimation;
		objects = UnityEngine.Object.FindObjectsOfType(typeof(SuperSneakersGroup)) as SuperSneakersGroup[];
		characterController = character.characterController;
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
		character.Stumble = false;
		isActive = true;
		powerupMesh.active = true;
		character.ChangeAnimations();
		Powerup = GameStats.Instance.TriggerPowerup(PowerupType.supersneakers);
		float duration = Powerup.timeLeft;
		coinMagnetCollider.OnEnter = CoinHit;
		coinMagnetCollider.GetComponent<Collider>().enabled = true;
		character.jumpHeight = character.jumpHeightSuperSneakers;
		stop = StopSignal.DONT_STOP;
		SuperSneakersGroup[] array = objects;
		foreach (SuperSneakersGroup o in array)
		{
			o.GroupActive = true;
		}
		while (Powerup.timeLeft > 0f && stop == StopSignal.DONT_STOP)
		{
			yield return 0;
			ratio = Powerup.timeLeft / duration;
		}
		coinMagnetCollider.GetComponent<Collider>().enabled = false;
		OnTriggerObject onTriggerObject = coinMagnetCollider;
		onTriggerObject.OnEnter = (OnTriggerObject.OnEnterDelegate)Delegate.Remove(onTriggerObject.OnEnter, new OnTriggerObject.OnEnterDelegate(CoinHit));
		ratio = 0f;
		SuperSneakersGroup[] array2 = objects;
		foreach (SuperSneakersGroup o2 in array2)
		{
			o2.GroupActive = false;
		}
		character.jumpHeight = character.jumpHeightNormal;
		powerupMesh.active = false;
		isActive = false;
		character.ChangeAnimations();
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
		Pickup pickup = coin.GetComponent<Pickup>();
		character.NotifyPickup(pickup);
	}
}
