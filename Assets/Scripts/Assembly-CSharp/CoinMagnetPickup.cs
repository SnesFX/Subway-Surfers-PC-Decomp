using System;
using UnityEngine;

[RequireComponent(typeof(Pickup))]
public class CoinMagnetPickup : MonoBehaviour
{
	private Game game;

	private Pickup pickup;

	private void Awake()
	{
		game = Game.Instance;
		pickup = GetComponent<Pickup>();
		Pickup obj = pickup;
		obj.OnPickup = (Pickup.OnPickupDelegate)Delegate.Combine(obj.OnPickup, new Pickup.OnPickupDelegate(OnPickup));
	}

	private void OnPickup(CharacterPickupParticles particles)
	{
		game.Modifiers.Add(game.Modifiers.CoinMagnet);
		GameStats.Instance.coinMagnetsPickups++;
		particles.PickedUpPowerUp();
	}
}
