using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager
{
	private class PickupType
	{
		public Func<SpawnPoint, GameObject> ExtractGameObject;

		public float spawnProbability;

		public float spawnDistanceMin;

		public float spawnZ;
	}

	private static SpawnPointManager instance;

	private PickupType dailyLetter;

	private PickupType doubleScoreMultiplier;

	private PickupType jetpackPickup;

	private PickupType jumpBooster;

	private PickupType magnetBooster;

	private PickupType mysteryBox;

	private PickupType[] pickups;

	private float spawnZ;

	private float spawnSpacing;

	private float totalProbability;

	private float[] accumulatedProbability;

	public static SpawnPointManager Instance
	{
		get
		{
			return instance ?? (instance = new SpawnPointManager());
		}
	}

	public SpawnPointManager()
	{
		float distancePerMeter = Game.Instance.distancePerMeter;
		Upgrade upgrade = Upgrades.upgrades[PowerupType.letters];
		dailyLetter = new PickupType();
		dailyLetter.spawnDistanceMin *= distancePerMeter;
		dailyLetter.spawnProbability = upgrade.spawnProbability;
		dailyLetter.ExtractGameObject = (SpawnPoint spawnPoint) => spawnPoint.dailyLetter;
		Upgrade upgrade2 = Upgrades.upgrades[PowerupType.doubleMultiplier];
		doubleScoreMultiplier = new PickupType();
		doubleScoreMultiplier.spawnDistanceMin = (float)upgrade2.minimumMeters * distancePerMeter;
		doubleScoreMultiplier.spawnProbability = upgrade2.spawnProbability;
		doubleScoreMultiplier.ExtractGameObject = (SpawnPoint spawnPoint) => spawnPoint.doubleScoreMultiplier;
		Upgrade upgrade3 = Upgrades.upgrades[PowerupType.jetpack];
		jetpackPickup = new PickupType();
		jetpackPickup.spawnDistanceMin = (float)upgrade3.minimumMeters * distancePerMeter;
		jetpackPickup.spawnProbability = upgrade3.spawnProbability;
		jetpackPickup.ExtractGameObject = (SpawnPoint spawnPoint) => spawnPoint.jetpackPickup;
		Upgrade upgrade4 = Upgrades.upgrades[PowerupType.supersneakers];
		jumpBooster = new PickupType();
		jumpBooster.spawnDistanceMin = (float)upgrade4.minimumMeters * distancePerMeter;
		jumpBooster.spawnProbability = upgrade4.spawnProbability;
		jumpBooster.ExtractGameObject = (SpawnPoint spawnPoint) => spawnPoint.jumpBooster;
		Upgrade upgrade5 = Upgrades.upgrades[PowerupType.coinmagnet];
		magnetBooster = new PickupType();
		magnetBooster.spawnDistanceMin = (float)upgrade5.minimumMeters * distancePerMeter;
		magnetBooster.spawnProbability = upgrade5.spawnProbability;
		magnetBooster.ExtractGameObject = (SpawnPoint spawnPoint) => spawnPoint.magnetBooster;
		Upgrade upgrade6 = Upgrades.upgrades[PowerupType.mysterybox];
		mysteryBox = new PickupType();
		mysteryBox.spawnDistanceMin = (float)upgrade6.minimumMeters * distancePerMeter;
		mysteryBox.spawnProbability = upgrade6.spawnProbability;
		mysteryBox.ExtractGameObject = (SpawnPoint spawnPoint) => spawnPoint.mysteryBox;
		pickups = new PickupType[6] { dailyLetter, doubleScoreMultiplier, jetpackPickup, jumpBooster, magnetBooster, mysteryBox };
	}

	public void PerformSelection(SpawnPoint spawnPoint, List<GameObject> objectsToVisit)
	{
		float z = spawnPoint.transform.position.z;
		PickupType pickupType = null;
		if (z > spawnZ)
		{
			List<PickupType> list = new List<PickupType>(pickups).FindAll((PickupType p) => p.spawnZ < z);
			if (list.Count > 0)
			{
				float[] array = new float[list.Count];
				float num = 0f;
				for (int i = 0; i < list.Count; i++)
				{
					num = (array[i] = num + list[i].spawnProbability);
				}
				float num2 = UnityEngine.Random.Range(0f, num);
				for (int j = 0; j < array.Length; j++)
				{
					if (num2 < array[j])
					{
						pickupType = list[j];
						pickupType.spawnZ = z + pickupType.spawnDistanceMin;
						break;
					}
				}
				spawnZ = z + spawnSpacing;
			}
		}
		for (int k = 0; k < pickups.Length; k++)
		{
			PickupType pickupType2 = pickups[k];
			GameObject gameObject = pickupType2.ExtractGameObject(spawnPoint);
			if (pickupType2 == pickupType)
			{
				objectsToVisit.Add(gameObject);
			}
			else
			{
				gameObject.SetActiveRecursively(false);
			}
		}
	}

	public void Restart()
	{
		float distancePerMeter = Game.Instance.distancePerMeter;
		spawnZ = Upgrades.UpgradeFirstSpawnMeters * distancePerMeter;
		spawnSpacing = Upgrades.UpgradeSpawnSpacingMeters * distancePerMeter;
		PickupType[] array = pickups;
		foreach (PickupType pickupType in array)
		{
			pickupType.spawnZ = float.MinValue;
		}
	}
}
