using System;
using System.Collections;
using UnityEngine;

public class UIGameOverHelper : MonoBehaviour
{
	public UILabel scoreLabel;

	public UILabel collectedCoinLabel;

	public UILabel coinboxLabel;

	public CoinBoxSizer coinBoxSizer;

	public int coinsToAnimateDebug = 1000;

	private Friend[] _friends;

	private int scoreFrom;

	private int scoreTo;

	private int coinboxFrom;

	private int coinboxTo;

	private int collectedCoinsFrom;

	private int collectedCoinsTo;

	private ScoreCounterSoundPlayer scoreCounterSoundPlayer;

	public FriendHandlerBrag bragHandler;

	public GameObject OfflineParent;

	public GameObject OnlineParent;

	public GameObject newUpgradesIcon;

	public UILabel newUpgradesText;

	[HideInInspector]
	public bool HasBeenSetupAfterAGame;

	private void Awake()
	{
		scoreCounterSoundPlayer = GetComponent<ScoreCounterSoundPlayer>();
	}

	private void OnEnable()
	{
		PlayerInfo instance = PlayerInfo.Instance;
		instance.onCoinsChanged = (Action)Delegate.Combine(instance.onCoinsChanged, new Action(OnCoinsChanged));
	}

	private void OnDisable()
	{
		PlayerInfo instance = PlayerInfo.Instance;
		instance.onCoinsChanged = (Action)Delegate.Remove(instance.onCoinsChanged, new Action(OnCoinsChanged));
	}

	public void SetupBeforeMysteryBox()
	{
		HasBeenSetupAfterAGame = true;
		scoreLabel.text = string.Empty + GameStats.Instance.score;
		collectedCoinLabel.text = string.Empty + GameStats.Instance.coins;
		scoreFrom = GameStats.Instance.score;
		UpdateNewUpgradesLabel();
	}

	private void UpdateNewUpgradesLabel()
	{
		int numberOfAffordableUpgrades = PlayerInfo.Instance.GetNumberOfAffordableUpgrades();
		if (numberOfAffordableUpgrades > 1)
		{
			newUpgradesIcon.active = true;
			newUpgradesText.gameObject.active = true;
			newUpgradesText.text = numberOfAffordableUpgrades.ToString();
		}
		else
		{
			newUpgradesIcon.active = false;
			newUpgradesText.gameObject.active = false;
		}
	}

	private void OnCoinsChanged()
	{
		UpdateNewUpgradesLabel();
	}

	public void SetupAfterMysteryBox()
	{
		if (HasBeenSetupAfterAGame)
		{
			coinBoxSizer.updateAutomatically = false;
			collectedCoinsFrom = GameStats.Instance.coins;
			collectedCoinsTo = 0;
			scoreTo = scoreFrom + GameStats.CoinToScoreConversion(collectedCoinsFrom);
			coinboxFrom = PlayerInfo.Instance.amountOfCoins;
			coinboxTo = coinboxFrom + GameStats.Instance.coins;
			PlayerInfo.Instance.amountOfCoins = coinboxTo;
			PlayerInfo.Instance.highestScore = scoreTo;
			if (SocialManager.instance != null)
			{
				SocialManager.instance.ReportScore(PlayerInfo.Instance.highestScore, Mathf.Max(1, Mathf.RoundToInt(GameStats.Instance.meters)));
			}
			PlayerInfo.Instance.Save();
			StartCoroutine(CountUpCoins());
			HasBeenSetupAfterAGame = false;
		}
	}

	private void CountUpCompleted()
	{
	}

	private IEnumerator CountUpCoins()
	{
		float countFactor = 0f;
		float countTime = Mathf.Lerp(0.3f, 2f, (float)collectedCoinsFrom / 100f);
		yield return new WaitForSeconds(0.5f);
		while (countFactor < 1f)
		{
			scoreCounterSoundPlayer.PlayCoinSound(countFactor);
			countFactor += Time.deltaTime / countTime;
			scoreLabel.text = string.Empty + Mathf.Round(Mathf.SmoothStep(scoreFrom, scoreTo, countFactor));
			coinboxLabel.text = string.Empty + Mathf.Round(Mathf.SmoothStep(coinboxFrom, coinboxTo, countFactor));
			collectedCoinLabel.text = string.Empty + Mathf.Round(Mathf.SmoothStep(collectedCoinsFrom, collectedCoinsTo, countFactor));
			yield return null;
		}
		scoreCounterSoundPlayer.StopScoreSound();
		scoreLabel.text = string.Empty + scoreTo;
		coinboxLabel.text = string.Empty + coinboxTo;
		collectedCoinLabel.text = string.Empty + collectedCoinsTo;
		coinBoxSizer.updateAutomatically = true;
		CountUpCompleted();
	}

	public void FacebookLoggedIn()
	{
	}
}
