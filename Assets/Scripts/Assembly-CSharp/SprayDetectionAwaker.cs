using System;
using UnityEngine;

public class SprayDetectionAwaker : MonoBehaviour
{
	public RunnerAnimPlayer runnerAnimPlayer;

	private void Awake()
	{
		UIScreenController instance = UIScreenController.Instance;
		instance.OnChangedScreen = (Action<bool>)Delegate.Combine(instance.OnChangedScreen, new Action<bool>(runnerAnimPlayer.PlayOrMutePaintSound));
	}
}
