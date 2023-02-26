using UnityEngine;

public static class Utility
{
	public static void SetLayerRecursively(Transform t, int layer)
	{
		t.gameObject.layer = layer;
		foreach (Transform item in t)
		{
			SetLayerRecursively(item, layer);
		}
	}

	public static int NumberOfDigits(int number)
	{
		int num = 0;
		if (number == 0)
		{
			return 1;
		}
		while (number != 0)
		{
			number /= 10;
			num++;
		}
		return num;
	}
}
