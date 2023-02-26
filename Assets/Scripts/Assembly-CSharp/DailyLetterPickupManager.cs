using System.Collections.Generic;
using UnityEngine;

public class DailyLetterPickupManager : MonoBehaviour
{
	public const char NO_LETTER = '\0';

	private char letter;

	private HashSet<DailyLetterPickup> pickups = new HashSet<DailyLetterPickup>();

	private static DailyLetterPickupManager instance;

	public static DailyLetterPickupManager Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject gameObject = new GameObject("DailyLetterPickupManager");
				instance = gameObject.AddComponent<DailyLetterPickupManager>();
			}
			return instance;
		}
	}

	private void NotifyPickups()
	{
		foreach (DailyLetterPickup pickup in pickups)
		{
			pickup.Letter = letter;
		}
	}

	public void InitializePickup(DailyLetterPickup pickup)
	{
		pickups.Add(pickup);
		pickup.Letter = letter;
	}

	public void UpdateLetter()
	{
		letter = PlayerInfo.Instance.GetNewDailyLetter();
		NotifyPickups();
	}
}
