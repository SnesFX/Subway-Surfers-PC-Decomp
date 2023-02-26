using System.Collections.Generic;
using UnityEngine;

public class InAppHelper : MonoBehaviour
{
	private static InAppHelper _instance;

	private string inAppPurchaseKey = string.Empty;

	public static InAppHelper Instance
	{
		get
		{
			return _instance ?? (_instance = Object.FindObjectOfType(typeof(InAppHelper)) as InAppHelper);
		}
	}

	public void SetupNativePopup(int cost)
	{
		int num = 0;
		num = cost - PlayerInfo.Instance.amountOfCoins;
		string text = string.Empty;
		if (InAppManager.Instance.productRequestSucceeded)
		{
			foreach (KeyValuePair<string, InAppProfile> inAppDatum in InAppData.inAppData)
			{
				if (!inAppDatum.Value.validInApp || inAppDatum.Value.amountOfCoins <= num)
				{
					continue;
				}
				if (!string.IsNullOrEmpty(text))
				{
					if (InAppData.inAppData[text].amountOfCoins > InAppData.inAppData[inAppDatum.Key].amountOfCoins)
					{
						text = inAppDatum.Key;
					}
				}
				else
				{
					text = inAppDatum.Key;
				}
			}
		}
		inAppPurchaseKey = text;
		string title = "Not enough coins!";
		if (!string.IsNullOrEmpty(inAppPurchaseKey))
		{
			string message = string.Format("You need {0} more Coins to complete your purchase. Buy {1} for {2}?", num, InAppData.inAppData[text].amountOfCoins, InAppData.inAppData[text].price);
			DeviceUtility.showNativePopupWithCallback("3InAppController", "NativePurchaseInappPack", title, message, "Cancel", "Buy", null);
		}
		else
		{
			string message2 = string.Format("You need {0} more Coins to complete your purchase. Buy more in the store", num);
			DeviceUtility.showNativePopup(title, message2, "Ok");
		}
	}

	public void NativePurchaseInappPack(string message)
	{
		if (message == "0")
		{
			inAppPurchaseKey = string.Empty;
		}
		else if (InAppManager.Instance.productRequestSucceeded)
		{
			InAppManager.Instance.BuyFromPopup(inAppPurchaseKey);
		}
		else
		{
			inAppPurchaseKey = string.Empty;
		}
	}
}
