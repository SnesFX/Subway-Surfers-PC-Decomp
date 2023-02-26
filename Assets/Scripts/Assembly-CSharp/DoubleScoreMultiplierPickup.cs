using System;
using UnityEngine;

public class DoubleScoreMultiplierPickup : MonoBehaviour
{
	private Game game;

	public void Awake()
	{
		game = Game.Instance;
		Pickup component = GetComponent<Pickup>();
		component.OnPickup = (Pickup.OnPickupDelegate)Delegate.Combine(component.OnPickup, new Pickup.OnPickupDelegate(OnPickup));
	}

	private void OnPickup(CharacterPickupParticles particles)
	{
		game.Modifiers.Add(game.Modifiers.DoubleScoreMultiplier);
		GameStats.Instance.doubleMultiplierPickups++;
		particles.PickedUpPowerUp();
	}
}
