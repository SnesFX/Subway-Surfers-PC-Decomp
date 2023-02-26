using UnityEngine;

public class Clouds : MonoBehaviour
{
	public float skyLength = 1700f;

	public float cloudDistance = 200f;

	public int numberOfClouds = 10;

	public float cloudSize = 50f;

	public GameObject cloudPrefab;

	private void Start()
	{
		for (int i = 0; i < numberOfClouds; i++)
		{
			float num = skyLength * (float)i / (float)numberOfClouds;
			GameObject gameObject = Object.Instantiate(cloudPrefab) as GameObject;
			Vector3 position = Quaternion.Euler(0f, 0f, Random.Range(-45f, 45f)) * (Vector3.up * cloudDistance + Vector3.forward * num);
			gameObject.transform.position = position;
			gameObject.transform.localScale = Vector3.one * cloudSize;
		}
	}
}
