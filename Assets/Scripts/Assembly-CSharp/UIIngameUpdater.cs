using System;
using UnityEngine;

public class UIIngameUpdater : IgnoreTimeScale
{
	public UILabel scoreLabel;

	public UILabel multiplierLabel;

	public UILabel coinLabel;

	public UISlicedSprite scoreBG;

	private Transform _cachedScoreBGTransform;

	public UISlicedSprite multiplierBG;

	public UISlicedSprite coinBG;

	private Transform _cachedCoinBGTransform;

	public UIHeadStartHelper headstartHelper;

	public UILabel countdownStartingLabel;

	public UILabel countdownLabel;

	private float countdownSeconds;

	private static bool countingDown;

	private Vector3 cachedCountdownLabelScale = Vector3.zero;

	private int score = -1;

	public static bool isCountingDown()
	{
		return countingDown;
	}

	public void Awake()
	{
		PlayerInfo instance = PlayerInfo.Instance;
		instance.onScoreMultiplierChanged = (Action)Delegate.Combine(instance.onScoreMultiplierChanged, new Action(readMultiplier));
		GameStats instance2 = GameStats.Instance;
		instance2.OnCoinsChanged = (Action)Delegate.Combine(instance2.OnCoinsChanged, new Action(OnCoinsChanged));
		Game instance3 = Game.Instance;
		instance3.OnGameStarted = (Action)Delegate.Combine(instance3.OnGameStarted, new Action(OnGameStarted));
		readMultiplier();
		scoreLabel.text = string.Empty + GameStats.Instance.score;
		_cachedScoreBGTransform = scoreBG.cachedTransform;
		_cachedCoinBGTransform = coinBG.cachedTransform;
		countdownStartingLabel.text = string.Empty;
		countdownLabel.text = string.Empty;
	}

	public void OnDestroy()
	{
		PlayerInfo instance = PlayerInfo.Instance;
		instance.onScoreMultiplierChanged = (Action)Delegate.Remove(instance.onScoreMultiplierChanged, new Action(readMultiplier));
		GameStats instance2 = GameStats.Instance;
		instance2.OnCoinsChanged = (Action)Delegate.Remove(instance2.OnCoinsChanged, new Action(OnCoinsChanged));
		Game instance3 = Game.Instance;
		instance3.OnGameStarted = (Action)Delegate.Remove(instance3.OnGameStarted, new Action(OnGameStarted));
	}

	private void OnDisable()
	{
		countingDown = false;
		countdownStartingLabel.text = string.Empty;
		countdownLabel.text = string.Empty;
	}

	public void TriggerInGameUI()
	{
		if (Game.Instance != null)
		{
			if (Game.Instance.isPaused)
			{
				countdownSeconds = 3f;
				countingDown = true;
			}
		}
		else
		{
			Debug.LogError("You must be running the GUI scene");
		}
	}

	private void readMultiplier()
	{
		multiplierLabel.text = "x" + PlayerInfo.Instance.scoreMultiplier;
	}

	private void Update()
	{
		if (Game.Instance.isReadyForHeadStart && !Game.Instance.track.IsRunningOnTutorialTrack)
		{
			Game.Instance.isReadyForHeadStart = false;
			headstartHelper.ShowHeadStart();
		}
		GameStats.Instance.CalculateScore();
		if (score != GameStats.Instance.score)
		{
			SetScoreLabel();
		}
		if (!countingDown)
		{
			return;
		}
		float num = UpdateRealTimeDelta();
		num *= 1.75f;
		countdownSeconds -= num;
		countdownStartingLabel.text = "Starting in";
		countdownLabel.text = Mathf.CeilToInt(countdownSeconds).ToString();
		if (cachedCountdownLabelScale == Vector3.zero)
		{
			cachedCountdownLabelScale = countdownLabel.cachedTransform.localScale;
		}
		countdownLabel.cachedTransform.localScale = cachedCountdownLabelScale * ((1f - countdownSeconds % 1f) * 0.5f + 1f);
		if (countdownSeconds < 0f)
		{
			countingDown = false;
			countdownStartingLabel.text = string.Empty;
			countdownLabel.text = string.Empty;
			if (Game.Instance != null)
			{
				Game.Instance.TriggerPause(false);
			}
		}
	}

	private void OnCoinsChanged()
	{
		coinLabel.text = string.Empty + GameStats.Instance.coins;
		ResizeCoinBox();
	}

	private void OnGameStarted()
	{
		if (!Game.Instance.isReadyForHeadStart)
		{
			headstartHelper.HideHeadStart(true);
		}
	}

	private void SetScoreLabel()
	{
		score = GameStats.Instance.score;
		string text;
		switch (Utility.NumberOfDigits(score))
		{
		case 1:
			text = "00000";
			break;
		case 2:
			text = "0000";
			break;
		case 3:
			text = "000";
			break;
		case 4:
			text = "00";
			break;
		case 5:
			text = "0";
			break;
		default:
			text = string.Empty;
			break;
		}
		scoreLabel.text = text + score;
		ResizeScoreBox();
	}

	private void ResizeScoreBox()
	{
		int length = scoreLabel.text.Length;
		float num = 99f;
		if (length > 6)
		{
			num += (float)(13 * (length - 6));
		}
		if (_cachedScoreBGTransform.localScale.x != num)
		{
			_cachedScoreBGTransform.localScale = new Vector3(num, _cachedScoreBGTransform.localScale.y, _cachedScoreBGTransform.localScale.z);
		}
	}

	private void ResizeCoinBox()
	{
		int length = coinLabel.text.Length;
		float num = 64f;
		if (length > 1)
		{
			num += (float)(13 * (length - 1));
		}
		if (_cachedCoinBGTransform.localScale.x != num)
		{
			_cachedCoinBGTransform.localScale = new Vector3(num, _cachedCoinBGTransform.localScale.y, _cachedCoinBGTransform.localScale.z);
		}
	}

	private void ResizeMultiplierBox()
	{
		int length = multiplierLabel.text.Length;
		float num = 50f;
		if (length > 2)
		{
			num += (float)(10 * (length - 2));
		}
		if (multiplierBG.transform.localScale.x != num)
		{
			multiplierBG.transform.localScale = new Vector3(num, multiplierBG.transform.localScale.y, multiplierBG.transform.localScale.z);
		}
	}
}
