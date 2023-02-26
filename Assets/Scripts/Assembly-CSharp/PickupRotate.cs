using System;
using UnityEngine;

public class PickupRotate : MonoBehaviour
{
	public Transform target;

	public float speed = 180f;

	public float rotatePhase = 0.9f;

	private float z;

	public void Awake()
	{
		TrackObject trackObject = GetComponent<TrackObject>() ?? base.gameObject.AddComponent<TrackObject>();
		trackObject.OnActivate = (TrackObject.OnActivateDelegate)Delegate.Combine(trackObject.OnActivate, new TrackObject.OnActivateDelegate(OnActivate));
		VisibleObject componentInChildren = GetComponentInChildren<VisibleObject>();
		if (componentInChildren != null)
		{
			componentInChildren.OnVisibleChange = (VisibleObject.OnVisibleChangeDelegate)Delegate.Combine(componentInChildren.OnVisibleChange, (VisibleObject.OnVisibleChangeDelegate)delegate(bool visible)
			{
				base.enabled = visible;
			});
		}
		base.enabled = false;
	}

	private void OnActivate()
	{
		z = base.transform.position.z;
		base.enabled = true;
	}

	private void Update()
	{
		target.localRotation = Quaternion.Euler(0f, Time.time * speed + z * rotatePhase, 0f);
	}
}
