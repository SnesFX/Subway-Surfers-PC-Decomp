using UnityEngine;

public class CharacterCamera : MonoBehaviour
{
	public Vector3 position;

	public Vector3 target;

	private Vector3 shake = Vector3.zero;

	private static CharacterCamera instance;

	public static CharacterCamera Instance
	{
		get
		{
			return instance ?? (instance = Object.FindObjectOfType(typeof(CharacterCamera)) as CharacterCamera);
		}
	}

	public void Shake()
	{
		Vector3 diff = Vector3.zero;
		float amplitude = 100f;
		StartCoroutine(pTween.To(0.3f, delegate(float t)
		{
			diff += Random.insideUnitSphere;
			shake = (1f - t) * diff * amplitude * Time.deltaTime;
		}));
	}

	public void LateUpdate()
	{
		base.transform.position = position + shake;
		base.transform.LookAt(target + shake);
	}

	public void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine(position, target);
	}
}
