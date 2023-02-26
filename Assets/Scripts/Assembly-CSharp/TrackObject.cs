using UnityEngine;

public class TrackObject : MonoBehaviour
{
	public delegate void OnActivateDelegate();

	public delegate void OnDeactivateDelegate();

	public OnActivateDelegate OnActivate;

	public OnDeactivateDelegate OnDeactivate;

	public void Activate()
	{
		if (OnActivate != null)
		{
			OnActivate();
		}
	}

	public void Deactivate()
	{
		if (OnDeactivate != null)
		{
			OnDeactivate();
		}
	}
}
