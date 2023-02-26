using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrackObject))]
public class MovingTrain : MonoBehaviour
{
	public float speed = 1f;

	private Transform train;

	private BoxCollider trainCollider;

	public float trainCount = 3f;

	private Game game;

	private static CharacterController characterController;

	private static List<MovingTrain> activeTrains = new List<MovingTrain>();

	private bool autoPilot;

	public static float autoPilotActivationDistance = 200f;

	public AudioClip trianPassClip;

	private AudioSource trainPassSource;

	private bool isInitialized;

	private bool startSound;

	public void Awake()
	{
		game = Game.Instance;
		if (!(game == null))
		{
			if (game.awakeDone)
			{
				Init();
			}
			if (base.transform.childCount == 0)
			{
				Debug.Log("No train child");
			}
			train = base.transform.GetChild(0);
			train.localPosition = -Vector3.up * 200f;
			trainCollider = GetComponent<BoxCollider>();
			Vector3 size = trainCollider.size;
			trainCollider.size = new Vector3(size.x, size.y, size.z / (1f + speed));
			trainCollider.center = new Vector3(0f, 15.2f, (30f * trainCount + 1f) / (1f + speed));
			base.enabled = false;
			TrackObject component = GetComponent<TrackObject>();
			component.OnActivate = (TrackObject.OnActivateDelegate)Delegate.Combine(component.OnActivate, new TrackObject.OnActivateDelegate(OnActivate));
			component.OnDeactivate = (TrackObject.OnDeactivateDelegate)Delegate.Combine(component.OnDeactivate, new TrackObject.OnDeactivateDelegate(OnDeactivate));
			trainPassSource = base.gameObject.AddComponent<AudioSource>();
			trainPassSource.minDistance = 20f;
			trainPassSource.maxDistance = 50f;
			trainPassSource.playOnAwake = false;
			trainPassSource.loop = true;
			trainPassSource.clip = trianPassClip;
		}
	}

	private void Init()
	{
		if (!isInitialized)
		{
			characterController = game.character.characterController;
			isInitialized = true;
		}
	}

	public void OnActivate()
	{
		activeTrains.Add(this);
		base.enabled = true;
		autoPilot = false;
		train.localPosition = new Vector3(0f, 0f, (base.transform.position.z - characterController.transform.position.z) * speed);
		startSound = true;
	}

	public void Update()
	{
		Init();
		if (startSound)
		{
			trainPassSource.pitch = UnityEngine.Random.Range(0.8f, 1.1f);
			trainPassSource.volume = UnityEngine.Random.Range(0.1f, 0.6f);
			trainPassSource.timeSamples = UnityEngine.Random.Range(0, trainPassSource.timeSamples);
			trainPassSource.Play();
			startSound = false;
		}
		if (autoPilot)
		{
			train.position -= Vector3.forward * Time.deltaTime * game.currentSpeed * speed;
			return;
		}
		Vector3 position = new Vector3(0f, 0f, (base.transform.position.z - characterController.transform.position.z) * speed);
		Vector3 position2 = base.transform.TransformPoint(position);
		train.position = position2;
	}

	public void OnDeactivate()
	{
		trainPassSource.Stop();
		activeTrains.Remove(this);
		base.enabled = false;
		train.transform.localPosition = -100f * Vector3.up;
	}

	public void OnDrawGizmos()
	{
		if (train != null)
		{
			Gizmos.color = Color.white;
			Gizmos.DrawLine(train.position, base.transform.position);
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(base.transform.position, 5f);
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(train.position, 5f);
		}
	}

	public static void ActivateAutoPilot()
	{
		foreach (MovingTrain activeTrain in activeTrains)
		{
			if (activeTrain.GetComponent<Collider>().bounds.min.z - characterController.transform.position.z < autoPilotActivationDistance)
			{
				activeTrain.autoPilot = true;
			}
		}
	}
}
