using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Track : MonoBehaviour
{
	private struct SpawnPointWrapper
	{
		public SpawnPoint SpawnPoint;

		public float Z;

		public SpawnPointWrapper(SpawnPoint spawnPoint)
		{
			SpawnPoint = spawnPoint;
			Z = spawnPoint.transform.position.z;
		}
	}

	private const int ActiveTruePerFrame = 4;

	private const int ActivatePerFrame = 1;

	public Transform trackLeft;

	public Transform trackRight;

	public int numberOfTracks = 3;

	public float cleanUpDistance = 2000f;

	public float trackAheadDistance = 700f;

	public Transform levelChunksParent;

	public TrackChunk jetpackLandingChunk;

	public bool tutorial;

	public TrackChunk tutorialTrackChunk;

	private bool firstTrackChunk = true;

	private TrackChunkCollection trackChunks;

	private float trackSpacing;

	private float trackChunkZ;

	private List<TrackChunk> activeTrackChunks = new List<TrackChunk>(5);

	private List<TrackChunk> trackChunksForDeactivation = new List<TrackChunk>(5);

	private Hoverboard hoverboard;

	private static Track instance;

	public bool IsRunningOnTutorialTrack { get; set; }

	public static Track Instance
	{
		get
		{
			return instance ?? (instance = UnityEngine.Object.FindObjectOfType(typeof(Track)) as Track);
		}
	}

	public void Awake()
	{
		trackSpacing = (trackRight.position - trackLeft.position).magnitude / (float)(numberOfTracks - 1);
		trackChunks = new TrackChunkCollection();
		hoverboard = Hoverboard.Instance;
		tutorial = ((!PlayerInfo.Instance.tutorialCompleted) ? true : false);
	}

	public Vector3 GetPosition(float x, float z)
	{
		return Vector3.forward * z + trackLeft.position + x * Vector3.right;
	}

	public float GetTrackX(int trackIndex)
	{
		return trackSpacing * (float)trackIndex;
	}

	public float LayJetpackChunks(float characterZ, float flyLength)
	{
		LayTracksUpTo(characterZ, flyLength, true);
		float result = trackChunkZ - characterZ;
		LayTrackChunk(jetpackLandingChunk);
		return result;
	}

	public void LayEmptyChunks(float characterZ, float removeDistance)
	{
		RemoveChunkObstacles(characterZ + removeDistance);
	}

	public void RemoveChunkObstacles(float removeDistance)
	{
		foreach (TrackChunk activeTrackChunk in activeTrackChunks)
		{
			activeTrackChunk.DeactivateObstacles(removeDistance);
		}
	}

	public void Initialize(float characterZ)
	{
		trackChunks.Initialize(characterZ);
	}

	public void LayTrackChunks(float characterZ)
	{
		LayTracksUpTo(characterZ, trackAheadDistance, false);
	}

	public void LayTracksUpTo(float characterZ, float trackAheadDistance, bool isJetpack)
	{
		if (!trackChunks.CanDeliver())
		{
			return;
		}
		float num = characterZ + trackAheadDistance;
		float num2 = 200f;
		Debug.DrawLine(Vector3.forward * num + Vector3.left * num2, Vector3.forward * num + -Vector3.left * num2, Color.white);
		if (trackChunkZ < num)
		{
			CleanupTrackChunks(characterZ);
		}
		int num3 = 0;
		while (trackChunkZ < num)
		{
			trackChunks.MoveForward(trackChunkZ);
			TrackChunk trackChunk;
			if (firstTrackChunk && tutorial)
			{
				trackChunk = tutorialTrackChunk;
				firstTrackChunk = false;
				if (trackChunk.CheckPoints.Count > 0)
				{
					IsRunningOnTutorialTrack = true;
				}
				hoverboard.isAllowed = false;
			}
			else if (isJetpack)
			{
				trackChunk = trackChunks.GetJetPakChunk(num3);
				num3++;
			}
			else
			{
				trackChunk = trackChunks.GetRandomActive();
				int num4 = 0;
				while (activeTrackChunks.Contains(trackChunk) && num4 < 1000)
				{
					trackChunk = trackChunks.GetRandomActive();
					num4++;
				}
				if (num4 == 1000)
				{
					Debug.Log("active track chunks");
					Debug.Log("active: " + string.Join(", ", activeTrackChunks.ConvertAll((TrackChunk chunk) => chunk.gameObject.name).ToArray()));
					Debug.LogError("infinite loop. not track chunks to select.");
				}
			}
			LayTrackChunk(trackChunk);
		}
	}

	private void LayTrackChunk(TrackChunk trackChunk)
	{
		StartCoroutine(LayTrackChunkAsync(trackChunk));
	}

	private IEnumerator LayTrackChunkAsync(TrackChunk trackChunk)
	{
		trackChunk.gameObject.transform.position = Vector3.forward * trackChunkZ;
		trackChunkZ += trackChunk.zSize;
		activeTrackChunks.Add(trackChunk);
		trackChunk.RestoreHiddenObstacles();
		yield return StartCoroutine(PerformRecursiveSelection(trackChunk.gameObject));
		int i = 0;
		Array.Sort(trackChunk.objects, (TrackObject g1, TrackObject g2) => g1.transform.position.z.CompareTo(g2.transform.position.z));
		TrackObject[] objects = trackChunk.objects;
		foreach (TrackObject o in objects)
		{
			if (o.gameObject.active)
			{
				o.Activate();
				i++;
			}
			if (i == 1)
			{
				yield return null;
				i = 0;
			}
		}
	}

	private IEnumerator ActivateGameObjects(List<GameObject> objects)
	{
		objects.Sort((GameObject g1, GameObject g2) => g1.transform.position.z.CompareTo(g2.transform.position.z));
		int i = 0;
		foreach (GameObject gameObject in objects)
		{
			gameObject.active = true;
			i++;
			if (i == 4)
			{
				yield return null;
				i = 0;
			}
		}
	}

	private IEnumerator PerformRecursiveSelection(GameObject parent, bool sortSpawnPoints = true)
	{
		List<GameObject> objectsToActivate = new List<GameObject>();
		List<GameObject> objectsToVisit = new List<GameObject>();
		List<SpawnPointWrapper> spawnPoints = new List<SpawnPointWrapper>();
		objectsToVisit.Add(parent);
		while (objectsToVisit.Count > 0)
		{
			GameObject gameObject = objectsToVisit[0];
			objectsToVisit.RemoveAt(0);
			if (sortSpawnPoints)
			{
				SpawnPoint spawnPoint = gameObject.GetComponent<SpawnPoint>();
				if (spawnPoint != null)
				{
					spawnPoints.Add(new SpawnPointWrapper(spawnPoint));
					continue;
				}
			}
			RandomizeOffset randomizeOffset = gameObject.GetComponent<RandomizeOffset>();
			if (randomizeOffset != null)
			{
				randomizeOffset.ChooseRandomOffset();
			}
			Transform gameObjectTransform = gameObject.transform;
			objectsToActivate.Add(gameObject);
			Selector selector = gameObject.GetComponent<Selector>();
			if (selector != null)
			{
				selector.PerformSelection(objectsToVisit);
			}
			else if (gameObject.GetComponent<Group>() == null)
			{
				for (int i = 0; i < gameObjectTransform.childCount; i++)
				{
					GameObject child = gameObjectTransform.GetChild(i).gameObject;
					objectsToVisit.Add(child);
				}
			}
		}
		List<GameObject> LowPriority = new List<GameObject>();
		LowPriority = objectsToActivate.Where((GameObject x) => IsLowPriority(x)).ToList();
		objectsToActivate = objectsToActivate.Where((GameObject x) => !IsLowPriority(x)).ToList();
		objectsToActivate.Sort((GameObject g1, GameObject g2) => g1.transform.position.z.CompareTo(g2.transform.position.z));
		LowPriority.Sort((GameObject g1, GameObject g2) => g1.transform.position.z.CompareTo(g2.transform.position.z));
		objectsToActivate.AddRange(LowPriority);
		int j = 0;
		foreach (GameObject gameObject2 in objectsToActivate)
		{
			gameObject2.active = true;
			j++;
			if (j == 4)
			{
				yield return null;
				j = 0;
			}
		}
		if (spawnPoints.Count <= 0)
		{
			yield break;
		}
		spawnPoints.Sort((SpawnPointWrapper x, SpawnPointWrapper y) => x.Z.CompareTo(y.Z));
		foreach (SpawnPoint spawnPoint2 in spawnPoints.ConvertAll((SpawnPointWrapper wrapper) => wrapper.SpawnPoint))
		{
			yield return StartCoroutine(PerformRecursiveSelection(spawnPoint2.gameObject, false));
		}
	}

	private bool IsLowPriority(GameObject g)
	{
		return g.layer != 16;
	}

	public void CleanupTrackChunks(float characterZ)
	{
		float num = characterZ - cleanUpDistance;
		foreach (TrackChunk activeTrackChunk in activeTrackChunks)
		{
			if (activeTrackChunk.transform.position.z + activeTrackChunk.zSize < num)
			{
				trackChunksForDeactivation.Add(activeTrackChunk);
			}
		}
		foreach (TrackChunk item in trackChunksForDeactivation)
		{
			if (!item.isTutorial)
			{
				item.Deactivate();
			}
			activeTrackChunks.Remove(item);
		}
		trackChunksForDeactivation.Clear();
	}

	public void DeactivateTrackChunks()
	{
		StopAllCoroutines();
		foreach (TrackChunk activeTrackChunk in activeTrackChunks)
		{
			activeTrackChunk.Deactivate();
		}
	}

	public void Restart()
	{
		TrackChunk[] array = trackChunks.TrackChunks;
		foreach (TrackChunk trackChunk in array)
		{
			Vector3 position = trackChunk.transform.position;
			position.y = -1000f;
			trackChunk.transform.position = position;
		}
		trackChunkZ = 0f;
		trackChunks.Initialize(0f);
		foreach (TrackChunk activeTrackChunk in activeTrackChunks)
		{
			activeTrackChunk.Deactivate();
		}
		activeTrackChunks.Clear();
		firstTrackChunk = true;
	}

	private void TrackPositionGizmos()
	{
		for (int i = 0; i < numberOfTracks; i++)
		{
			Vector3 vector = Vector3.Lerp(trackLeft.position, trackRight.position, (float)i / (float)(numberOfTracks - 1));
			Gizmos.DrawLine(vector, vector + Vector3.forward * 5f);
		}
	}

	public void OnDrawGizmos()
	{
		TrackPositionGizmos();
	}

	public float GetLastCheckPoint(float characterZ)
	{
		foreach (TrackChunk activeTrackChunk in activeTrackChunks)
		{
			if (IsRunningOnTutorialTrack && activeTrackChunk == tutorialTrackChunk)
			{
				return activeTrackChunk.GetLastCheckPoint(characterZ);
			}
		}
		Debug.Log("No checkpoints in track");
		return 0f;
	}
}
