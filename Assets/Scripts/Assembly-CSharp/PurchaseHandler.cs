using UnityEngine;

public class PurchaseHandler
{
	private static PurchaseHandler _instance;

	public static PurchaseHandler Instance
	{
		get
		{
			return _instance ?? (_instance = new PurchaseHandler());
		}
	}

	public void PurchaseCharacter(CharacterModels.ModelType modelType, UICharacterBuyButton sender)
	{
		CharacterModels.Model model = CharacterModels.modelData[modelType];
		if (model.UnlockType != CharacterModels.UnlockType.coins)
		{
			Debug.Log("Cannot buy character with unlocktype: " + model.UnlockType);
			sender.PurchaseFailure();
			return;
		}
		int price = model.Price;
		if (PlayerInfo.Instance.amountOfCoins >= price)
		{
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.SpendCoin, price);
			PlayerInfo.Instance.CollectToken(modelType, price);
			PlayerInfo.Instance.amountOfCoins -= price;
			sender.PurchaseSuccessful();
			PlayerInfo.Instance.Save();
		}
		else
		{
			InAppHelper.Instance.SetupNativePopup(price);
			sender.PurchaseFailure();
		}
	}

	public void PurchaseUpgrade(PowerupType type, BuyButtonIngame sender)
	{
		int num = ((Upgrades.upgrades[type].numberOfTiers != 0) ? Upgrades.upgrades[type].getPrice(PlayerInfo.Instance.GetCurrentTier(type) + 1) : Upgrades.upgrades[type].getPrice(0));
		if (PlayerInfo.Instance.amountOfCoins >= num)
		{
			switch (type)
			{
			case PowerupType.hoverboard:
			case PowerupType.headstart500:
			case PowerupType.headstart2000:
				PlayerInfo.Instance.IncreaseUpgradeAmount(type);
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.SpendCoin, num);
				break;
			case PowerupType.jetpack:
			case PowerupType.supersneakers:
			case PowerupType.coinmagnet:
			case PowerupType.letters:
			case PowerupType.doubleMultiplier:
				PlayerInfo.Instance.IncreasePowerupTier(type);
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.SpendCoin, num);
				break;
			case PowerupType.mysterybox:
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.SpendCoin, num);
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.BuyMysterybox);
				break;
			case PowerupType.skipmission1:
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.SpendCoin, num);
				break;
			case PowerupType.skipmission2:
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.SpendCoin, num);
				break;
			case PowerupType.skipmission3:
				Missions.Instance.PlayerDidThis(Missions.MissionTarget.SpendCoin, num);
				break;
			}
			PlayerInfo.Instance.amountOfCoins -= num;
			sender.PurchaseSuccessful();
			PlayerInfo.Instance.Save();
		}
		else
		{
			InAppHelper.Instance.SetupNativePopup(num);
			sender.PurchaseFailure();
		}
	}
}
