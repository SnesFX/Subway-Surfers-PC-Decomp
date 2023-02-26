using UnityEngine;

public class VisibleObject : MonoBehaviour
{
	public delegate void OnVisibleChangeDelegate(bool isVisible);

	public OnVisibleChangeDelegate OnVisibleChange;

	public void OnBecameVisible()
	{
		if (OnVisibleChange != null)
		{
			OnVisibleChange(true);
		}
	}

	public void OnBecameInvisible()
	{
		if (OnVisibleChange != null)
		{
			OnVisibleChange(false);
		}
	}
}
