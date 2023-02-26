using UnityEngine;

public class MysteryBox
{
	public static MysteryBoxReward Roll()
	{
		MysteryBoxReward mysteryBoxReward = new MysteryBoxReward();
		PlayerInfo.Instance.amountOfMysteryBoxesOpened++;
		PlayerInfo.Instance.Save();
		if (PlayerInfo.Instance.amountOfMysteryBoxesOpened == 1)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.token;
			mysteryBoxReward.amount = 1;
			mysteryBoxReward.tokenType = (CharacterModels.ModelType)(Random.Range(0, 4) + 1);
			return mysteryBoxReward;
		}
		if (PlayerInfo.Instance.amountOfMysteryBoxesOpened == 2)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.powerup;
			mysteryBoxReward.amount = 1;
			mysteryBoxReward.powerupType = PowerupType.headstart500;
			return mysteryBoxReward;
		}
		if (PlayerInfo.Instance.amountOfMysteryBoxesOpened == 3)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.Coins;
			mysteryBoxReward.amount = 600;
			return mysteryBoxReward;
		}
		float num = Random.Range(0f, 1f);
		if (num < 0.4f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.token;
			mysteryBoxReward.amount = 1;
			mysteryBoxReward.tokenType = (CharacterModels.ModelType)(Random.Range(0, 4) + 1);
		}
		else if (num < 0.55f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.Coins;
			mysteryBoxReward.amount = Random.Range(50, 250);
		}
		else if (num < 0.66f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.Coins;
			mysteryBoxReward.amount = Random.Range(100, 500);
		}
		else if (num < 0.7f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.powerup;
			mysteryBoxReward.powerupType = PowerupType.headstart500;
			mysteryBoxReward.amount = 1;
		}
		else if (num < 0.8f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.Coins;
			mysteryBoxReward.amount = 500;
		}
		else if (num < 0.87f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.powerup;
			mysteryBoxReward.powerupType = PowerupType.hoverboard;
			mysteryBoxReward.amount = 2;
		}
		else if (num < 0.92f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.Coins;
			mysteryBoxReward.amount = 1000;
		}
		else if (num < 0.95f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.Coins;
			mysteryBoxReward.amount = 1500;
		}
		else if (num < 0.96f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.powerup;
			mysteryBoxReward.powerupType = PowerupType.hoverboard;
			mysteryBoxReward.amount = Random.Range(0, 7) + 3;
		}
		else if (num < 0.97f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.powerup;
			mysteryBoxReward.powerupType = PowerupType.headstart2000;
			mysteryBoxReward.amount = 1;
		}
		else if (num < 0.98f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.Coins;
			mysteryBoxReward.amount = 2500;
		}
		else if (num < 0.988f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.powerup;
			mysteryBoxReward.powerupType = PowerupType.hoverboard;
			mysteryBoxReward.amount = 10;
		}
		else if (num < 0.993f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.Coins;
			mysteryBoxReward.amount = 5000;
		}
		else if (num < 0.998f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.powerup;
			mysteryBoxReward.powerupType = PowerupType.headstart2000;
			mysteryBoxReward.amount = 3;
		}
		else if (num <= 1f)
		{
			mysteryBoxReward.type = MysteryBoxRewardType.Coins;
			mysteryBoxReward.amount = 10000;
		}
		return mysteryBoxReward;
	}
}
