using UnityEngine;

public class Cloud : MonoBehaviour
{
	private Transform cameraTransform;

	private void Awake()
	{
		cameraTransform = Camera.main.transform;
	}

	private void OnBecameVisible()
	{
		base.enabled = true;
	}

	private void Update()
	{
		base.transform.rotation = cameraTransform.rotation;
	}

	private void OnBecameInvisible()
	{
		base.enabled = false;
	}
}
