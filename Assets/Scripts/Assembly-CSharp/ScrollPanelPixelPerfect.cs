using UnityEngine;

public class ScrollPanelPixelPerfect : MonoBehaviour
{
	private Transform _transform;

	private void Start()
	{
		_transform = base.transform;
	}

	private void Update()
	{
		_transform.localPosition = new Vector3(_transform.localPosition.x, Mathf.Round(_transform.localPosition.y), _transform.localPosition.z);
	}
}
