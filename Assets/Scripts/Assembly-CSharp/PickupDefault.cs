using System;
using UnityEngine;

public class PickupDefault : MonoBehaviour
{
	public MeshRenderer meshRenderer;

	public Glow glow;

	public bool ShouldSpawnParticles;

	private Collider parentCollider;

	private void Awake()
	{
		Pickup component = GetComponent<Pickup>();
		component.OnPickup = (Pickup.OnPickupDelegate)Delegate.Combine(component.OnPickup, new Pickup.OnPickupDelegate(OnPickup));
		TrackObject trackObject = GetComponent<TrackObject>() ?? base.gameObject.AddComponent<TrackObject>();
		trackObject.OnActivate = (TrackObject.OnActivateDelegate)Delegate.Combine(trackObject.OnActivate, new TrackObject.OnActivateDelegate(OnActivate));
		trackObject.OnDeactivate = (TrackObject.OnDeactivateDelegate)Delegate.Combine(trackObject.OnDeactivate, new TrackObject.OnDeactivateDelegate(OnDeactivate));
		parentCollider = FindParentCollider(base.transform);
		if (parentCollider == null)
		{
			Debug.Log("Error: No collider for PickupDefault.");
		}
		SetVisible(false);
	}

	private Collider FindParentCollider(Transform current)
	{
		if (current.GetComponent<Collider>() != null)
		{
			return current.GetComponent<Collider>();
		}
		if (current.parent != null)
		{
			return FindParentCollider(current.parent);
		}
		return null;
	}

	private void SetVisible(bool visible)
	{
		meshRenderer.enabled = visible;
		if (glow != null)
		{
			glow.SetVisible(visible);
		}
	}

	private void OnActivate()
	{
		parentCollider.enabled = true;
		SetVisible(true);
	}

	private void OnDeactivate()
	{
		SetVisible(false);
	}

	private void OnPickup(CharacterPickupParticles particles)
	{
		if (base.gameObject != null)
		{
			parentCollider.enabled = false;
		}
		SetVisible(false);
		if (ShouldSpawnParticles)
		{
			particles.PickedUpDefaultPowerUp();
		}
	}
}
