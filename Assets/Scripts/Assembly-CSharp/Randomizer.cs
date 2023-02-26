using System.Collections.Generic;
using UnityEngine;

public class Randomizer : Selector
{
	public override void PerformSelection(List<GameObject> objectsToVisit)
	{
		int num = Random.Range(0, base.transform.childCount);
		for (int i = 0; i < base.transform.childCount; i++)
		{
			GameObject gameObject = base.transform.GetChild(i).gameObject;
			if (i == num)
			{
				objectsToVisit.Add(gameObject);
			}
			else
			{
				gameObject.SetActiveRecursively(false);
			}
		}
	}
}
