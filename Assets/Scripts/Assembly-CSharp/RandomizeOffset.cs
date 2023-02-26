using System;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeOffset : MonoBehaviour
{
	[Serializable]
	public class RandomOffsets
	{
		public bool left = true;

		public bool mid = true;

		public bool right = true;
	}

	public RandomOffsets randomOffsets;

	public void ChooseRandomOffset()
	{
		List<float> list = new List<float>();
		if (randomOffsets.left)
		{
			list.Add(-20f);
		}
		if (randomOffsets.mid)
		{
			list.Add(0f);
		}
		if (randomOffsets.right)
		{
			list.Add(20f);
		}
		float[] array = list.ToArray();
		if (array.Length > 0)
		{
			Vector3 localPosition = base.transform.localPosition;
			localPosition.x = array[UnityEngine.Random.Range(0, array.Length)];
			base.transform.localPosition = localPosition;
		}
	}
}
