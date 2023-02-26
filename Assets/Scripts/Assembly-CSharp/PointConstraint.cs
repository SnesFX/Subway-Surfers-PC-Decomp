using UnityEngine;

public class PointConstraint : MonoBehaviour
{
	public Transform master;

	private Transform transformCached;

	private void Awake()
	{
		transformCached = base.transform;
	}

	private void LateUpdate()
	{
		transformCached.position = new Vector3(master.position.x, 0f, master.position.z);
		transformCached.localPosition = new Vector3(transformCached.localPosition.x, 0f, transformCached.localPosition.z);
	}
}
