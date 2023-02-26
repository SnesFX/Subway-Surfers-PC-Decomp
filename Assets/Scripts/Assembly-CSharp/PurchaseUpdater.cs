using UnityEngine;

public class PurchaseUpdater : MonoBehaviour
{
	public UILabel price;

	public UISprite coin;

	public UITierHelper tierHelper;

	public UILabel haveAmount;

	public GameObject buyButton;

	public UILabel buyButtonTitle;

	public UISlicedSprite buyButtonBackground;

	public void UpgradePurchased(PowerupType type)
	{
		Debug.Log("Purchased powerup: " + type);
		switch (type)
		{
		case PowerupType.hoverboard:
		case PowerupType.headstart500:
		case PowerupType.headstart2000:
			if (haveAmount != null)
			{
				haveAmount.text = "You have: " + PlayerInfo.Instance.GetUpgradeAmount(type);
			}
			break;
		case PowerupType.jetpack:
		case PowerupType.supersneakers:
		case PowerupType.coinmagnet:
		case PowerupType.letters:
		case PowerupType.doubleMultiplier:
			if (tierHelper.ResetTiers())
			{
				NGUITools.Destroy(coin);
				price.text = string.Empty;
				buyButtonTitle.text = "Max";
				Object.Destroy(buyButton.GetComponent<BoxCollider>());
			}
			else
			{
				price.text = string.Empty + Upgrades.upgrades[type].getPrice(PlayerInfo.Instance.GetCurrentTier(type) + 1);
			}
			break;
		case PowerupType.mysterybox:
		case PowerupType.coinpouch:
		case PowerupType.skipmission1:
		case PowerupType.skipmission2:
		case PowerupType.skipmission3:
			break;
		}
	}
}
