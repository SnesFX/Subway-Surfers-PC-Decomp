using System;
using System.Collections;
using UnityEngine;

public class Distort : MonoBehaviour
{
	[Serializable]
	public class EnviromentSettings
	{
		public Color backgroundColor;

		public Color materialColor = new Color(1f, 1f, 1f, 1f);
	}

	private float xMin = -0.2f;

	private float xMax = 0.2f;

	private float yMin = -0.1f;

	private float yMax;

	public float partLength = 700f;

	public Material[] materials;

	public Vector3 distortion = Vector3.zero;

	private Vector3 distortionTarget = Vector3.zero;

	private Vector3 distortionVelocity = Vector3.zero;

	private float lastZ;

	private float nextPartZ;

	private float environmentT;

	private Character character;

	private Vector3 distortionStart = new Vector3(-0.05f, -0.03f, 0f);

	public EnviromentSettings day;

	public EnviromentSettings night;

	private void Awake()
	{
		character = Character.Instance;
		distortionStart = distortion;
	}

	private void Start()
	{
		StartDestortion();
	}

	public void Reset()
	{
		distortion = distortionStart;
		StartDestortion();
	}

	private void StartDestortion()
	{
		StopAllCoroutines();
		distortionTarget = distortion;
		lastZ = character.z;
		nextPartZ = character.z + partLength;
		StartCoroutine(RandomDirs());
	}

	private IEnumerator RandomDirs()
	{
		while (true)
		{
			yield return new WaitForSeconds(1f);
			if (character.z > nextPartZ)
			{
				distortionTarget = new Vector3(UnityEngine.Random.Range(xMin, xMax), UnityEngine.Random.Range(yMin, yMax), 0f);
				nextPartZ = character.z + partLength;
			}
		}
	}

	private void Update()
	{
		float deltaTime = character.z - lastZ;
		lastZ = character.z;
		distortion = Vector3.SmoothDamp(distortion, distortionTarget, ref distortionVelocity, partLength, float.MaxValue, deltaTime);
		Vector3 vector = distortion / 100f;
		Material[] array = materials;
		foreach (Material material in array)
		{
			material.SetVector("_Distort", vector);
		}
	}
}
