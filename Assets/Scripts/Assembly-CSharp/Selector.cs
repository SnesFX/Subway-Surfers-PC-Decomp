using System.Collections.Generic;
using UnityEngine;

public abstract class Selector : MonoBehaviour
{
	public abstract void PerformSelection(List<GameObject> objectsToVisit);

	public void Awake()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			base.transform.GetChild(i).gameObject.SetActiveRecursively(false);
		}
	}
}
