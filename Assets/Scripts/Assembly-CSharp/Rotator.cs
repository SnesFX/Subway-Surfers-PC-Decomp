using UnityEngine;

public class Rotator : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		base.transform.Rotate(new Vector3(0.5f, 0f, 0f));
	}
}
