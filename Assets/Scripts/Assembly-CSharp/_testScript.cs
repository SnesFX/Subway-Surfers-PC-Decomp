using System;
using UnityEngine;

public class _testScript : MonoBehaviour
{
	private void Start()
	{
	}

	private void OnEnable()
	{
		Debug.Log("Enabled");
		Missions instance = Missions.Instance;
		instance.onMissionComplete = (Missions.MissionCompleteHandler)Delegate.Combine(instance.onMissionComplete, new Missions.MissionCompleteHandler(OnMissionComplete));
		Missions instance2 = Missions.Instance;
		instance2.onMissionSetComplete = (Missions.MissionSetCompleteHandler)Delegate.Combine(instance2.onMissionSetComplete, new Missions.MissionSetCompleteHandler(OnMissionSetComplete));
		PlayerInfo instance3 = PlayerInfo.Instance;
		instance3.onCoinsChanged = (Action)Delegate.Combine(instance3.onCoinsChanged, new Action(OnCoinsChanged));
		PlayerInfo instance4 = PlayerInfo.Instance;
		instance4.onScoreMultiplierChanged = (Action)Delegate.Combine(instance4.onScoreMultiplierChanged, new Action(OnScoreMultiplierChanged));
	}

	private void OnDisable()
	{
		Debug.Log("Disabled");
		Missions instance = Missions.Instance;
		instance.onMissionComplete = (Missions.MissionCompleteHandler)Delegate.Remove(instance.onMissionComplete, new Missions.MissionCompleteHandler(OnMissionComplete));
		Missions instance2 = Missions.Instance;
		instance2.onMissionSetComplete = (Missions.MissionSetCompleteHandler)Delegate.Remove(instance2.onMissionSetComplete, new Missions.MissionSetCompleteHandler(OnMissionSetComplete));
		PlayerInfo instance3 = PlayerInfo.Instance;
		instance3.onCoinsChanged = (Action)Delegate.Remove(instance3.onCoinsChanged, new Action(OnCoinsChanged));
		PlayerInfo instance4 = PlayerInfo.Instance;
		instance4.onScoreMultiplierChanged = (Action)Delegate.Remove(instance4.onScoreMultiplierChanged, new Action(OnScoreMultiplierChanged));
	}

	private void OnMissionComplete(string msg)
	{
		Debug.Log("OnMissionComplete: " + msg);
	}

	private void OnMissionSetComplete()
	{
		Debug.Log("OnMissionSetComplete, new mission set is " + PlayerInfo.Instance.currentMissionSet + ", new missions:");
		MissionInfo[] missionInfo = Missions.Instance.GetMissionInfo();
		MissionInfo[] array = missionInfo;
		foreach (MissionInfo missionInfo2 in array)
		{
			Debug.Log("Mission " + missionInfo2.complete + " " + string.Format(missionInfo2.template.description, missionInfo2.mission.goal, missionInfo2.progress));
		}
	}

	private void OnCoinsChanged()
	{
		Debug.Log("OnCoinsChanged: " + PlayerInfo.Instance.amountOfCoins);
	}

	private void OnScoreMultiplierChanged()
	{
		Debug.Log("OnScoreMultiplierChanged: " + PlayerInfo.Instance.scoreMultiplier);
	}

	private void Update()
	{
		Missions instance = Missions.Instance;
		PlayerInfo instance2 = PlayerInfo.Instance;
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			Debug.Log("Next mission set");
			if (instance.currentMissionSet + 1 < instance.missionSetCount)
			{
				instance.currentMissionSet++;
				Debug.Log("Current Mission Set set to " + instance.currentMissionSet);
				MissionInfo[] missionInfo = Missions.Instance.GetMissionInfo();
				MissionInfo[] array = missionInfo;
				foreach (MissionInfo missionInfo2 in array)
				{
					Debug.Log("Mission " + missionInfo2.complete + " " + string.Format(missionInfo2.template.description, missionInfo2.mission.goal, Mathf.Max(0, missionInfo2.mission.goal - missionInfo2.progress)));
				}
			}
			else
			{
				Debug.Log("Current mission set is already at max " + instance.currentMissionSet);
			}
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			Debug.Log("Previous mission set");
			if (instance.currentMissionSet > 0)
			{
				instance.currentMissionSet--;
				Debug.Log("Current Mission Set set to " + instance.currentMissionSet);
				MissionInfo[] missionInfo3 = Missions.Instance.GetMissionInfo();
				MissionInfo[] array2 = missionInfo3;
				foreach (MissionInfo missionInfo4 in array2)
				{
					Debug.Log("Mission " + missionInfo4.complete + " " + string.Format(missionInfo4.template.description, missionInfo4.mission.goal, Mathf.Max(0, missionInfo4.mission.goal - missionInfo4.progress)));
				}
			}
			else
			{
				Debug.Log("Current mission set is already at min 0");
			}
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			Debug.Log("Get mission info (menu)");
			MissionInfo[] missionInfo5 = Missions.Instance.GetMissionInfo();
			MissionInfo[] array3 = missionInfo5;
			foreach (MissionInfo missionInfo6 in array3)
			{
				Debug.Log("tempMission " + missionInfo6.complete + " " + string.Format(missionInfo6.template.description, missionInfo6.mission.goal, Mathf.Max(0, missionInfo6.mission.goal - missionInfo6.progress)));
			}
		}
		else if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			Missions.Instance.inRun = true;
			Debug.Log("inRun true");
		}
		else if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			Missions.Instance.inRun = false;
			Debug.Log("inRun false");
		}
		else if (Input.GetKeyDown(KeyCode.Alpha7))
		{
			instance2.amountOfCoins += 5;
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.EarnCoin, 5);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha8))
		{
			instance2.doubleScore = !instance2.doubleScore;
		}
		else if (Input.GetKeyDown(KeyCode.Q))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.BuyMysterybox);
			Debug.Log("Missions.Instance.MissionTarget.BuyMysterybox");
		}
		else if (Input.GetKeyDown(KeyCode.W))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.CollectCoinPouch);
			Debug.Log("Missions.Instance.MissionTarget.CollectCoinPouch");
		}
		else if (Input.GetKeyDown(KeyCode.E))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.BeatFriends);
			Debug.Log("Missions.Instance.MissionTarget.BeatFriends");
		}
		else if (Input.GetKeyDown(KeyCode.R))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.DodgeBarriers);
			Debug.Log("Missions.Instance.MissionTarget.Characters");
		}
		else if (Input.GetKeyDown(KeyCode.T))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.CoinsWithMagnet);
			Debug.Log("Missions.Instance.MissionTarget.CoinsWithMagnet");
		}
		else if (Input.GetKeyDown(KeyCode.Y))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.CrashTrains);
			Debug.Log("Missions.Instance.MissionTarget.CrashTrains");
		}
		else if (Input.GetKeyDown(KeyCode.U))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.DailyQuests);
			Debug.Log("Missions.Instance.MissionTarget.DailyQuests");
		}
		else if (Input.GetKeyDown(KeyCode.I))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.DieToTrain);
			Debug.Log("Missions.Instance.MissionTarget.DieToTrain");
		}
		else if (Input.GetKeyDown(KeyCode.O))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.RollUnderBarriers);
			Debug.Log("Missions.Instance.MissionTarget.DodgeBarriers");
		}
		else if (Input.GetKeyDown(KeyCode.P))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.EarnCoin);
			Debug.Log("Missions.Instance.MissionTarget.EarnCoin");
		}
		else if (Input.GetKeyDown(KeyCode.A))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.Headstart);
			Debug.Log("Missions.Instance.MissionTarget.Headstart");
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.Jetpack);
			Debug.Log("Missions.Instance.MissionTarget.Jetpack");
		}
		else if (Input.GetKeyDown(KeyCode.D))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.Jump);
			Debug.Log("Missions.Instance.MissionTarget.Jump");
		}
		else if (Input.GetKeyDown(KeyCode.F))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.JumpBarriers);
			Debug.Log("Missions.Instance.MissionTarget.JumpBarriers");
		}
		else if (Input.GetKeyDown(KeyCode.G))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.JumpTrain);
			Debug.Log("Missions.Instance.MissionTarget.JumpTrain");
		}
		else if (Input.GetKeyDown(KeyCode.H))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.Letters);
			Debug.Log("Missions.Instance.MissionTarget.Letters");
		}
		else if (Input.GetKeyDown(KeyCode.J))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.Magnets);
			Debug.Log("Missions.Instance.MissionTarget.Magnets");
		}
		else if (Input.GetKeyDown(KeyCode.K))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.MysteryBoxes);
			Debug.Log("Missions.Instance.MissionTarget.MysteryBoxes");
		}
		else if (Input.GetKeyDown(KeyCode.L))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.Powerups);
			Debug.Log("Missions.Instance.MissionTarget.Powerups");
		}
		else if (Input.GetKeyDown(KeyCode.Z))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.Roll);
			Debug.Log("Missions.Instance.MissionTarget.Roll");
		}
		else if (Input.GetKeyDown(KeyCode.X))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.RollCenter);
			Debug.Log("Missions.Instance.MissionTarget.RollCenter");
		}
		else if (Input.GetKeyDown(KeyCode.C))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.RollLeft);
			Debug.Log("Missions.Instance.MissionTarget.RollLeft");
		}
		else if (Input.GetKeyDown(KeyCode.V))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.RollRight);
			Debug.Log("Missions.Instance.MissionTarget.RollRight");
		}
		else if (Input.GetKeyDown(KeyCode.B))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.Score, 3);
			Debug.Log("Missions.Instance.MissionTarget.Score");
		}
		else if (Input.GetKeyDown(KeyCode.N))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.SpendCoin);
			Debug.Log("Missions.Instance.MissionTarget.ScoreBetween");
		}
		else if (Input.GetKeyDown(KeyCode.M))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.SuperSneakers);
			Debug.Log("Missions.Instance.MissionTarget.SuperSneekers");
		}
		else if (Input.GetKeyDown(KeyCode.Comma))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.TimeDeath, (int)Time.time);
			Debug.Log("Missions.Instance.MissionTarget.TimeDeath");
		}
		else if (Input.GetKeyDown(KeyCode.Period))
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.Tokens);
			Debug.Log("Missions.Instance.MissionTarget.Tokens");
		}
	}
}
