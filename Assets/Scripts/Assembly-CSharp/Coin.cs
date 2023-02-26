using System;
using UnityEngine;

[RequireComponent(typeof(Pickup))]
public class Coin : MonoBehaviour
{
	[HideInInspector]
	public Transform pivot;

	private Vector3 initialPivotPosition;

	private Pickup pickup;

	private void Awake()
	{
		pivot = base.transform.GetChild(0);
		initialPivotPosition = pivot.localPosition;
		pickup = GetComponent<Pickup>();
		Pickup obj = pickup;
		obj.OnPickup = (Pickup.OnPickupDelegate)Delegate.Combine(obj.OnPickup, new Pickup.OnPickupDelegate(OnPickup));
		TrackObject component = GetComponent<TrackObject>();
		component.OnActivate = (TrackObject.OnActivateDelegate)Delegate.Combine(component.OnActivate, new TrackObject.OnActivateDelegate(OnActivate));
	}

	private void OnPickup(CharacterPickupParticles pickupParticles)
	{
		GameStats.Instance.coins++;
		pickupParticles.PickedUpCoin(pickup);
	}

	private void OnActivate()
	{
		pivot.localPosition = initialPivotPosition;
	}
}
