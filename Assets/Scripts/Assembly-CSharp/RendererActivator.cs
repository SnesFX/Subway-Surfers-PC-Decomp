using System;
using UnityEngine;

public class RendererActivator : MonoBehaviour
{
	public void Awake()
	{
		TrackObject trackObject = GetComponent<TrackObject>() ?? base.gameObject.AddComponent<TrackObject>();
		trackObject.OnActivate = (TrackObject.OnActivateDelegate)Delegate.Combine(trackObject.OnActivate, new TrackObject.OnActivateDelegate(OnActivate));
		trackObject.OnDeactivate = (TrackObject.OnDeactivateDelegate)Delegate.Combine(trackObject.OnDeactivate, new TrackObject.OnDeactivateDelegate(OnDeactivate));
		base.GetComponent<Renderer>().enabled = false;
	}

	public void OnActivate()
	{
		base.GetComponent<Renderer>().enabled = true;
	}

	public void OnDeactivate()
	{
		base.GetComponent<Renderer>().enabled = false;
	}
}
