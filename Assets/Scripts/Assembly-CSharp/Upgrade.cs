using UnityEngine;

public class Upgrade
{
	public string name;

	public string description;

	public int numberOfTiers;

	public float[] durations;

	public float spawnProbability;

	public int minimumMeters;

	public int coinmagnetRange = 1;

	public int[] pricesRaw;

	public int levelPriceMultiplyer;

	public string iconName;

	public int getPrice(int tier)
	{
		if (pricesRaw == null)
		{
			Debug.LogWarning("Prices is not initialized");
			return -1;
		}
		return pricesRaw[tier] + levelPriceMultiplyer * Missions.Instance.currentMissionSet;
	}
}
