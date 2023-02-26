using System;
using UnityEngine;

public class UpgradeHelper : MonoBehaviour
{
	private enum AnimState
	{
		Closed = 0,
		Opening = 1,
		Open = 2,
		Closing = 3
	}

	public UISprite powerupIcon;

	public UILabel titleLabel;

	public UILabel descLabel;

	public UILabel priceLabel;

	public UILabel amountLabel;

	public BuyButtonIngame buyButton;

	public UILabel buyButtonTitle;

	public UISlicedSprite buyButtonBackground;

	public UITierHelper tierHelper;

	public UISprite coin;

	private PowerupType _type;

	private UpgradeScreenSetup _upgradeScreenSetup;

	public ContentChange contentChanger;

	private AnimState _animState;

	private AnimState animState
	{
		get
		{
			return _animState;
		}
		set
		{
			_animState = value;
		}
	}

	private void OnDestroy()
	{
		PlayerInfo instance = PlayerInfo.Instance;
		instance.onPowerupAmountChanged = (Action)Delegate.Remove(instance.onPowerupAmountChanged, new Action(UpdateLabel));
	}

	public void InitSingle(PowerupType type)
	{
		if (base.gameObject.GetComponent<UIPanel>() != null)
		{
			UnityEngine.Object.Destroy(base.gameObject.GetComponent<UIPanel>());
		}
		_type = type;
		powerupIcon.spriteName = Upgrades.upgrades[type].iconName;
		titleLabel.text = Upgrades.upgrades[type].name.ToUpper();
		descLabel.text = Upgrades.upgrades[type].description;
		priceLabel.text = string.Empty + Upgrades.upgrades[type].getPrice(0);
		animState = AnimState.Closed;
		switch (type)
		{
		case PowerupType.skipmission1:
		{
			MissionInfo missionInfo = Missions.Instance.GetMissionInfo(0);
			string format = ((missionInfo.mission.goal != 1) ? missionInfo.template.ultraShortDescription : missionInfo.template.ultraShortDescriptionSingle);
			amountLabel.text = string.Format(format, missionInfo.mission.goal);
			break;
		}
		case PowerupType.skipmission2:
		{
			MissionInfo missionInfo = Missions.Instance.GetMissionInfo(1);
			string format = ((missionInfo.mission.goal != 1) ? missionInfo.template.ultraShortDescription : missionInfo.template.ultraShortDescriptionSingle);
			amountLabel.text = string.Format(format, missionInfo.mission.goal);
			break;
		}
		case PowerupType.skipmission3:
		{
			MissionInfo missionInfo = Missions.Instance.GetMissionInfo(2);
			string format = ((missionInfo.mission.goal != 1) ? missionInfo.template.ultraShortDescription : missionInfo.template.ultraShortDescriptionSingle);
			amountLabel.text = string.Format(format, missionInfo.mission.goal);
			break;
		}
		case PowerupType.mysterybox:
			amountLabel.text = "Opens immediately";
			break;
		default:
			amountLabel.text = "You have: " + PlayerInfo.Instance.GetUpgradeAmount(type);
			break;
		}
		buyButton.initBuyButton(type);
		PlayerInfo instance = PlayerInfo.Instance;
		instance.onPowerupAmountChanged = (Action)Delegate.Combine(instance.onPowerupAmountChanged, new Action(UpdateLabel));
	}

	public void InitPermanent(PowerupType type)
	{
		if (base.gameObject.GetComponent<UIPanel>() != null)
		{
			UnityEngine.Object.Destroy(base.gameObject.GetComponent<UIPanel>());
		}
		_type = type;
		powerupIcon.spriteName = Upgrades.upgrades[type].iconName;
		titleLabel.text = Upgrades.upgrades[type].name.ToUpper();
		descLabel.text = Upgrades.upgrades[type].description;
		tierHelper.SetupTiers(type);
		animState = AnimState.Closed;
		if (PlayerInfo.Instance.GetCurrentTier(type) >= Upgrades.upgrades[type].numberOfTiers - 1)
		{
			priceLabel.text = "Full";
			priceLabel.color = new Color(23f / 85f, 0.2627451f, 0.2627451f, 1f);
			priceLabel.effectColor = new Color(0.85490197f, 0.85490197f, 0.85490197f, 1f);
			priceLabel.effectStyle = UILabel.Effect.Shadow;
			priceLabel.pivot = UIWidget.Pivot.Top;
			priceLabel.transform.localPosition = new Vector3(101f, priceLabel.transform.localPosition.y, priceLabel.transform.localPosition.z);
			UISlicedSprite uISlicedSprite = NGUITools.AddSprite(priceLabel.transform.parent.gameObject, coin.atlas, "background_buy_full") as UISlicedSprite;
			uISlicedSprite.depth = 5;
			uISlicedSprite.transform.localPosition = new Vector3(101f, 28f, 0f);
			uISlicedSprite.transform.localScale = new Vector3(52f, 20f, 1f);
			uISlicedSprite.pivot = UIWidget.Pivot.Center;
			uISlicedSprite.color = new Color(52f / 85f, 52f / 85f, 52f / 85f, 1f);
			uISlicedSprite.name = "4FullBG";
			uISlicedSprite.fillCenter = true;
			uISlicedSprite.MakePixelPerfect();
			NGUITools.Destroy(coin);
			UnityEngine.Object.Destroy(buyButton.gameObject);
		}
		else
		{
			priceLabel.text = string.Empty + Upgrades.upgrades[type].getPrice(PlayerInfo.Instance.GetCurrentTier(type) + 1);
		}
		if (buyButton != null)
		{
			buyButton.initBuyButton(type);
		}
	}

	public void UpgradePurchased(PowerupType type)
	{
		Debug.Log("Purchased powerup: " + type);
		switch (type)
		{
		case PowerupType.hoverboard:
		case PowerupType.headstart500:
		case PowerupType.headstart2000:
			if (amountLabel != null)
			{
				amountLabel.text = "You have: " + PlayerInfo.Instance.GetUpgradeAmount(type);
			}
			break;
		case PowerupType.jetpack:
		case PowerupType.supersneakers:
		case PowerupType.coinmagnet:
		case PowerupType.letters:
		case PowerupType.doubleMultiplier:
			if (tierHelper.ResetTiers())
			{
				priceLabel.text = "Full";
				priceLabel.color = new Color(23f / 85f, 0.2627451f, 0.2627451f, 1f);
				priceLabel.effectColor = new Color(0.85490197f, 0.85490197f, 0.85490197f, 1f);
				priceLabel.effectStyle = UILabel.Effect.Shadow;
				priceLabel.pivot = UIWidget.Pivot.Top;
				priceLabel.transform.localPosition = new Vector3(101f, priceLabel.transform.localPosition.y, priceLabel.transform.localPosition.z);
				UISlicedSprite uISlicedSprite = NGUITools.AddSprite(priceLabel.transform.parent.gameObject, coin.atlas, "background_buy_full") as UISlicedSprite;
				uISlicedSprite.depth = 5;
				uISlicedSprite.transform.localPosition = new Vector3(101f, 29f, 0f);
				uISlicedSprite.transform.localScale = new Vector3(52f, 20f, 1f);
				uISlicedSprite.pivot = UIWidget.Pivot.Center;
				uISlicedSprite.color = new Color(52f / 85f, 52f / 85f, 52f / 85f, 1f);
				uISlicedSprite.name = "4FullBG";
				uISlicedSprite.fillCenter = true;
				uISlicedSprite.MakePixelPerfect();
				NGUITools.Destroy(coin);
				UnityEngine.Object.Destroy(buyButton.gameObject);
			}
			else
			{
				priceLabel.text = string.Empty + Upgrades.upgrades[type].getPrice(PlayerInfo.Instance.GetCurrentTier(type) + 1);
			}
			break;
		case PowerupType.mysterybox:
			PlayerInfo.Instance.mysteryBoxesToUnlock++;
			UIScreenController.Instance.QueueMysteryBox();
			break;
		case PowerupType.skipmission1:
			Flurry.LogEventWithAParameter("Boost Mission Skip purchased", "Mission Set and Index", Missions.Instance.currentMissionSet + "-0");
			Missions.Instance.SkipMission(0);
			break;
		case PowerupType.skipmission2:
			Flurry.LogEventWithAParameter("Boost Mission Skip purchased", "Mission Set and Index", Missions.Instance.currentMissionSet + "-1");
			Missions.Instance.SkipMission(1);
			break;
		case PowerupType.skipmission3:
			Flurry.LogEventWithAParameter("Boost Mission Skip purchased", "Mission Set and Index", Missions.Instance.currentMissionSet + "-2");
			Missions.Instance.SkipMission(2);
			break;
		}
		switch (type)
		{
		case PowerupType.headstart500:
			Flurry.LogEvent("Boost Headstart500 purchased");
			break;
		case PowerupType.headstart2000:
			Flurry.LogEvent("Boost Headstart2000 purchased");
			break;
		case PowerupType.hoverboard:
			Flurry.LogEvent("Boost Hoverboard purchased");
			break;
		case PowerupType.coinmagnet:
			Flurry.LogEventWithAParameter("Boost Coinmagnet purchased", "Tier", PlayerInfo.Instance.GetCurrentTier(type).ToString());
			break;
		case PowerupType.doubleMultiplier:
			Flurry.LogEventWithAParameter("Boost 2x multiplier purchased", "Tier", PlayerInfo.Instance.GetCurrentTier(type).ToString());
			break;
		case PowerupType.jetpack:
			Flurry.LogEventWithAParameter("Boost jetpack purchased", "Tier", PlayerInfo.Instance.GetCurrentTier(type).ToString());
			break;
		case PowerupType.letters:
			Flurry.LogEventWithAParameter("Boost letters purchased", "Tier", PlayerInfo.Instance.GetCurrentTier(type).ToString());
			break;
		case PowerupType.supersneakers:
			Flurry.LogEventWithAParameter("Boost supersneakers purchased", "Tier", PlayerInfo.Instance.GetCurrentTier(type).ToString());
			break;
		case PowerupType.mysterybox:
			Flurry.LogEvent("Boost MysteryBox purchased");
			break;
		}
	}

	private void UpdateLabel()
	{
		switch (_type)
		{
		case PowerupType.hoverboard:
		case PowerupType.headstart500:
		case PowerupType.headstart2000:
			if (amountLabel != null)
			{
				amountLabel.text = "You have: " + PlayerInfo.Instance.GetUpgradeAmount(_type);
			}
			break;
		}
	}

	private void OnClick()
	{
		AnimationStarted();
	}

	public void AnimationStarted()
	{
		if (animState == AnimState.Closed || animState == AnimState.Closing)
		{
			animState = AnimState.Opening;
			if (_upgradeScreenSetup == null)
			{
				_upgradeScreenSetup = base.transform.parent.GetComponent<UpgradeScreenSetup>();
			}
			foreach (UpgradeHelper cachedUpgradeHelper in _upgradeScreenSetup.cachedUpgradeHelpers)
			{
				if (cachedUpgradeHelper != this && cachedUpgradeHelper.animState == AnimState.Open)
				{
					cachedUpgradeHelper.SendMessage("OnClick");
				}
			}
		}
		else
		{
			animState = AnimState.Closing;
		}
		contentChanger.FoldClicked();
		Collider collider = base.gameObject.GetComponent<Collider>();
		if (collider != null)
		{
			collider.enabled = false;
			UnityEngine.Object.Destroy(collider);
		}
	}

	public void AnimationEnded()
	{
		if (animState == AnimState.Closing || animState == AnimState.Opening)
		{
			NGUITools.AddWidgetCollider(base.gameObject);
			animState = ((animState != AnimState.Closing) ? AnimState.Open : AnimState.Closed);
			contentChanger.TriggerContent();
		}
		else
		{
			Debug.LogError("Unexpected anim state: " + animState, this);
		}
	}

	private void ColliderFixer()
	{
	}
}
