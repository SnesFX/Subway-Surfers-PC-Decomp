using System;
using UnityEngine;

public class SuperSneakersPickup : MonoBehaviour
{
	private Game game;

	private void Awake()
	{
		game = Game.Instance;
		Pickup component = GetComponent<Pickup>();
		component.OnPickup = (Pickup.OnPickupDelegate)Delegate.Combine(component.OnPickup, new Pickup.OnPickupDelegate(OnPickup));
	}

	private void OnPickup(CharacterPickupParticles particles)
	{
		game.Modifiers.Add(game.Modifiers.SuperSneakes);
		GameStats.Instance.superSneakerPickups++;
		particles.PickedUpPowerUp();
	}
}
