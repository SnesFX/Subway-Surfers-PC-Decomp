using UnityEngine;

public class BuyButtonIngame : MonoBehaviour
{
	public UpgradeHelper updater;

	private PowerupType _type;

	private bool _purchaseInProgress;

	public PowerupType type
	{
		get
		{
			return _type;
		}
	}

	private void OnClick()
	{
		if (!_purchaseInProgress)
		{
			Debug.Log("Buy: " + _type);
			PurchaseHandler.Instance.PurchaseUpgrade(_type, this);
		}
	}

	public void initBuyButton(PowerupType type)
	{
		_type = type;
	}

	public void PurchaseSuccessful()
	{
		updater.UpgradePurchased(_type);
		_purchaseInProgress = false;
	}

	public void PurchaseFailure()
	{
		_purchaseInProgress = false;
	}
}
