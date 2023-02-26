using UnityEngine;

public class GuardSwitchLane : MonoBehaviour
{
	private SmoothDampFloat smoothGuardX;

	public float guardXSmoothTime = 1f;

	public GameObject character;

	private Vector3 initPos;

	private void Start()
	{
	}

	private void Update()
	{
		Vector3 position = character.transform.position;
		base.gameObject.transform.position = new Vector3(position.x, position.y, base.gameObject.transform.position.z);
	}
}
