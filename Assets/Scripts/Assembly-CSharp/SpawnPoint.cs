using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : Selector
{
	public GameObject dailyLetter;

	public GameObject doubleScoreMultiplier;

	public GameObject jetpackPickup;

	public GameObject jumpBooster;

	public GameObject magnetBooster;

	public GameObject mysteryBox;

	public override void PerformSelection(List<GameObject> objectsToVisit)
	{
		SpawnPointManager.Instance.PerformSelection(this, objectsToVisit);
	}
}
