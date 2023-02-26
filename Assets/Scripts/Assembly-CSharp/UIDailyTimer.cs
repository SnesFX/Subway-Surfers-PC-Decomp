using System;
using UnityEngine;

public class UIDailyTimer : MonoBehaviour
{
	private UILabel _timerLabel;

	private void Start()
	{
		_timerLabel = GetComponent<UILabel>();
		DailyWord.Instance.ForceSync();
	}

	private void Update()
	{
		TimeSpan timeSpan = PlayerInfo.Instance.dailyWordExpireTime - DateTime.UtcNow;
		if (timeSpan.Ticks > 0)
		{
			_timerLabel.text = string.Format("Time: {0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
		}
		else
		{
			_timerLabel.text = "Time: 00:00:00";
		}
	}
}
