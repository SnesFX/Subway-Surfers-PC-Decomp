using UnityEngine;

public class Rotate : MonoBehaviour
{
	private Transform m_trans;

	public float Rotate_X;

	public float Rotate_Y;

	public float Rotate_Z;

	private void Start()
	{
		m_trans = base.gameObject.transform;
	}

	private void Update()
	{
		if (Rotate_X != 0f || Rotate_Y != 0f || Rotate_Z != 0f)
		{
			m_trans.Rotate(new Vector3(Rotate_X * Time.deltaTime, Rotate_Y * Time.deltaTime, Rotate_Z * Time.deltaTime));
		}
	}
}
