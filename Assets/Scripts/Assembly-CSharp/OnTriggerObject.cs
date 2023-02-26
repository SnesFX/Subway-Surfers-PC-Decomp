using UnityEngine;

public class OnTriggerObject : MonoBehaviour
{
	public delegate void OnEnterDelegate(Collider collider);

	public delegate void OnExitDelegate(Collider collider);

	public OnEnterDelegate OnEnter;

	public OnExitDelegate OnExit;

	public void OnTriggerEnter(Collider collider)
	{
		if (OnEnter != null)
		{
			OnEnter(collider);
		}
	}

	public void OnTriggerExit(Collider collider)
	{
		if (OnExit != null)
		{
			OnExit(collider);
		}
	}
}
