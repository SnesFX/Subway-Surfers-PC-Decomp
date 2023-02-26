using UnityEngine;

public class CameraCulling : MonoBehaviour
{
	private float[] distances;

	public float distance;

	private void Awake()
	{
		distances = new float[32];
		distances[LayerMask.NameToLayer("TransparentFX")] = distance;
		base.GetComponent<Camera>().layerCullDistances = distances;
	}
}
