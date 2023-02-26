using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStats
{
	public delegate void CoinsChangedIngame();

	public float duration;

	public Action OnCoinsChanged;

	private int _coins;

	private int _coinsCoinMagnet;

	private int _score;

	private float _metersLastUsedForScore;

	private float _meterScore;

	private List<ActivePowerup> _listOfActivePowerups = new List<ActivePowerup>();

	public float meters;

	public float metersRunLeftTrack;

	public float metersRunCenterTrack;

	public float metersRunRightTrack;

	public float metersFly;

	public float metersRunGround;

	public float metersRunTrain;

	public float metersRunStation;

	private int _jumps;

	private int _jumpsOverTrains;

	private int _rolls;

	private int _rollsLeftTrack;

	private int _rollsCenterTrack;

	private int _rollsRightTrack;

	public int trackChanges;

	private int _dodgeBarrier;

	private int _jumpBarrier;

	private int _trainHit;

	private int _movingTrainHit;

	private int _barrierHit;

	private int _jetpackPickups;

	private int _superSneakerPickups;

	private int _letterPickups;

	private int _coinMagnetsPickups;

	private int _mysteryBoxPickups;

	public int doubleMultiplierPickups;

	private int _usePowerups;

	public CoinsChangedIngame OnChoinsChangedIngame;

	public List<KeyValuePair<int, int>> coinsSummerized = new List<KeyValuePair<int, int>>();

	private static GameStats instance;

	public int coins
	{
		get
		{
			return _coins;
		}
		set
		{
			_coins = value;
			Action onCoinsChanged = OnCoinsChanged;
			if (onCoinsChanged != null)
			{
				onCoinsChanged();
			}
			if (value != 0)
			{
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.EarnCoin);
			}
		}
	}

	public int coinsCoinMagnet
	{
		get
		{
			return _coinsCoinMagnet;
		}
		set
		{
			_coinsCoinMagnet = value;
			if (value != 0)
			{
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.CoinsWithMagnet);
			}
		}
	}

	public int score
	{
		get
		{
			return _score;
		}
	}

	public int jumps
	{
		get
		{
			return _jumps;
		}
		set
		{
			_jumps = value;
			if (value != 0)
			{
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.Jump);
			}
		}
	}

	public int jumpsOverTrains
	{
		get
		{
			return _jumpsOverTrains;
		}
		set
		{
			_jumpsOverTrains = value;
			if (value != 0)
			{
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.JumpTrain);
			}
		}
	}

	public int rolls
	{
		get
		{
			return _rolls;
		}
		set
		{
			_rolls = value;
			if (value != 0)
			{
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.Roll);
			}
		}
	}

	public int rollsLeftTrack
	{
		get
		{
			return _rollsLeftTrack;
		}
		set
		{
			_rollsLeftTrack = value;
			if (value != 0)
			{
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.RollLeft);
			}
		}
	}

	public int rollsCenterTrack
	{
		get
		{
			return _rollsCenterTrack;
		}
		set
		{
			_rollsCenterTrack = value;
			if (value != 0)
			{
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.RollCenter);
			}
		}
	}

	public int rollsRightTrack
	{
		get
		{
			return _rollsRightTrack;
		}
		set
		{
			_rollsRightTrack = value;
			if (value != 0)
			{
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.RollRight);
			}
		}
	}

	public int dodgeBarrier
	{
		get
		{
			return _dodgeBarrier;
		}
		set
		{
			_dodgeBarrier = value;
			if (value != 0)
			{
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.RollUnderBarriers);
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.DodgeBarriers);
			}
		}
	}

	public int jumpBarrier
	{
		get
		{
			return _jumpBarrier;
		}
		set
		{
			_jumpBarrier = value;
			if (value != 0)
			{
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.JumpBarriers);
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.DodgeBarriers);
			}
		}
	}

	public int trainHit
	{
		get
		{
			return _trainHit;
		}
		set
		{
			_trainHit = value;
			if (value != 0)
			{
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.CrashTrains);
			}
		}
	}

	public int movingTrainHit
	{
		get
		{
			return _movingTrainHit;
		}
		set
		{
			_movingTrainHit = value;
			if (value != 0)
			{
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.DieToTrain);
			}
		}
	}

	public int barrierHit
	{
		get
		{
			return _barrierHit;
		}
		set
		{
			_barrierHit = value;
			if (value != 0)
			{
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.CrashBarriers);
			}
		}
	}

	public int jetpackPickups
	{
		get
		{
			return _jetpackPickups;
		}
		set
		{
			_jetpackPickups = value;
			if (value != 0)
			{
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.Jetpack);
			}
		}
	}

	public int superSneakerPickups
	{
		get
		{
			return _superSneakerPickups;
		}
		set
		{
			_superSneakerPickups = value;
			if (value != 0)
			{
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.SuperSneakers);
			}
		}
	}

	public int letterPickups
	{
		get
		{
			return _letterPickups;
		}
		set
		{
			_letterPickups = value;
			if (value != 0)
			{
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.Letters);
			}
		}
	}

	public int coinMagnetsPickups
	{
		get
		{
			return _coinMagnetsPickups;
		}
		set
		{
			_coinMagnetsPickups = value;
			if (value != 0)
			{
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.Magnets);
			}
		}
	}

	public int mysteryBoxPickups
	{
		get
		{
			return _mysteryBoxPickups;
		}
		set
		{
			_mysteryBoxPickups = value;
			if (value != 0)
			{
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.MysteryBoxes);
			}
		}
	}

	public int usePowerups
	{
		get
		{
			return _usePowerups;
		}
		set
		{
			_usePowerups = value;
			if (value != 0)
			{
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.Powerups);
			}
		}
	}

	public static GameStats Instance
	{
		get
		{
			return instance ?? (instance = new GameStats());
		}
	}

	public static int CoinToScoreConversion(int coins)
	{
		return coins * 2 * PlayerInfo.Instance.rawMultiplier;
	}

	public void CalculateScore()
	{
		if (_metersLastUsedForScore < meters)
		{
			_meterScore = meters - _metersLastUsedForScore;
			_metersLastUsedForScore = meters;
			int num = (int)(_meterScore * (float)PlayerInfo.Instance.scoreMultiplier);
			_score += num;
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.Score, num);
			if (coins <= 0)
			{
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.NoCoinsBeforeScore, num);
			}
		}
	}

	public void AddScoreForPickup(PowerupType type)
	{
		switch (type)
		{
		case PowerupType.mysterybox:
		case PowerupType.jetpack:
		case PowerupType.supersneakers:
		case PowerupType.coinmagnet:
		case PowerupType.letters:
		case PowerupType.doubleMultiplier:
		{
			int num = PlayerInfo.Instance.scoreMultiplier * 50;
			_score += num;
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.Score, num);
			if (coins <= 0)
			{
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.NoCoinsBeforeScore, num);
			}
			break;
		}
		}
	}

	public void ResetScore()
	{
		_score = 0;
		_metersLastUsedForScore = 0f;
		_meterScore = 0f;
	}

	public ActivePowerup TriggerPowerup(PowerupType type)
	{
		ActivePowerup activePowerup = new ActivePowerup();
		activePowerup.type = type;
		activePowerup.timeActivated = Time.time;
		activePowerup.timeLeft = PlayerInfo.Instance.GetPowerupDuration(type);
		if (type == PowerupType.headstart2000 || type == PowerupType.headstart500)
		{
			activePowerup.timeLeft = 0f;
		}
		for (int num = _listOfActivePowerups.Count - 1; num >= 0; num--)
		{
			if (_listOfActivePowerups[num].type == activePowerup.type)
			{
				_listOfActivePowerups.RemoveAt(num);
				Debug.Log("Removing existing powerup: " + type);
			}
		}
		AddScoreForPickup(type);
		_listOfActivePowerups.Add(activePowerup);
		return activePowerup;
	}

	public List<ActivePowerup> GetActivePowerups()
	{
		return _listOfActivePowerups;
	}

	public void UpdatePowerupTimes(float deltaTime)
	{
		for (int num = _listOfActivePowerups.Count - 1; num >= 0; num--)
		{
			if (Game.Instance.IsInJetpackMode && (_listOfActivePowerups[num].type == PowerupType.hoverboard || _listOfActivePowerups[num].type == PowerupType.supersneakers))
			{
				continue;
			}
			_listOfActivePowerups[num].timeLeft -= deltaTime;
			if (!(_listOfActivePowerups[num].timeLeft < 0f) || (Game.Instance.IsInJetpackMode && _listOfActivePowerups[num].type == PowerupType.jetpack))
			{
				continue;
			}
			if (_listOfActivePowerups[num].type == PowerupType.hoverboard)
			{
				float num2 = Hoverboard.Instance.WaitForParticlesDelay + PlayerInfo.Instance.GetHoverBoardCoolDown();
				if (_listOfActivePowerups[num].timeLeft > 0f - num2)
				{
					continue;
				}
			}
			_listOfActivePowerups.RemoveAt(num);
		}
	}

	public void ClearPowerups()
	{
		_listOfActivePowerups.Clear();
	}

	public void RemoveHoverBoardPowerup()
	{
		for (int num = _listOfActivePowerups.Count - 1; num >= 0; num--)
		{
			if (_listOfActivePowerups[num].type == PowerupType.hoverboard)
			{
				_listOfActivePowerups[num].timeLeft = 0f;
			}
		}
	}

	public void Reset()
	{
		duration = 0f;
		ResetScore();
		coins = 0;
		coinsCoinMagnet = 0;
		meters = 0f;
		metersRunLeftTrack = 0f;
		metersRunCenterTrack = 0f;
		metersRunRightTrack = 0f;
		metersRunGround = 0f;
		metersRunTrain = 0f;
		metersRunStation = 0f;
		metersFly = 0f;
		jumps = 0;
		jumpsOverTrains = 0;
		rolls = 0;
		rollsLeftTrack = 0;
		rollsCenterTrack = 0;
		rollsRightTrack = 0;
		trackChanges = 0;
		dodgeBarrier = 0;
		jumpBarrier = 0;
		trainHit = 0;
		barrierHit = 0;
		jetpackPickups = 0;
		superSneakerPickups = 0;
		letterPickups = 0;
		coinMagnetsPickups = 0;
		mysteryBoxPickups = 0;
		usePowerups = 0;
		doubleMultiplierPickups = 0;
		coinsSummerized = new List<KeyValuePair<int, int>>();
	}
}
