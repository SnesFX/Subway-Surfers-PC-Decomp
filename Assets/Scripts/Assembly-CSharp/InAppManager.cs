using System;
using System.Collections;
using UnityEngine;

public class InAppManager : MonoBehaviour
{
	private enum InAppPurchaseState
	{
		NotStarted = 0,
		Started = 1,
		Failed = 2,
		Complete = 3
	}

	private InAppData inAppData;

	private static InAppManager _instance;

	private InAppPurchaseState _inAppPurchaseState;

	[HideInInspector]
	public bool productRequestSucceeded;

	public Action onProductRequestSuccess;

	public Action onPurchaseSuccess;

	public Action onPurchaseFailure;

	public static InAppManager Instance
	{
		get
		{
			return _instance ?? (_instance = UnityEngine.Object.FindObjectOfType(typeof(InAppManager)) as InAppManager);
		}
	}

	public static bool IsInstanced()
	{
		return _instance != null;
	}

	private void Awake()
	{
		_instance = this;
	}

	private void Start()
	{
		inAppData = new InAppData();
		RetryProductRequest();
		if (!InAppPurchaseHandler.isInitializedForPurchase())
		{
			InAppPurchaseHandler.initPurchase(base.gameObject.name, "PurchaseSuccess", "PurchaseFailure");
		}
	}

	public void RetryProductRequest()
	{
		if (!productRequestSucceeded)
		{
			StartCoroutine(QueryInAppPurchases());
		}
		else
		{
			Debug.Log("Retried product request, but already succeeded");
		}
	}

	private IEnumerator QueryInAppPurchases()
	{
		if (WillQueryInAppPurchases())
		{
			if (!InAppPurchaseHandler.isInitializedForProductRequest())
			{
				InAppPurchaseHandler.initProductRequest(base.gameObject.name, "ProductRequestSuccess", "ProductRequestFailure");
			}
			InAppPurchaseHandler.queryProducts(inAppData.CommaSeparatedProductIds);
		}
		yield break;
	}

	public void BuyInAppNow(GameObject sender)
	{
		string key = sender.GetComponent<CoinButtonHelper>().Key;
		StartPurchase(key);
	}

	public void BuyFromPopup(string purchaseId)
	{
		StartPurchase(purchaseId);
	}

	private void StartPurchase(string inAppPurchaseId)
	{
		UIScreenController.Instance.ShowInAppPurchaseOverlay();
		InAppPurchaseHandler.startPurchase(inAppPurchaseId);
	}

	public void ProductRequestSuccess(string validProductIdsAndPrices)
	{
		string[] array = validProductIdsAndPrices.Split(";"[0]);
		int num = array.Length / 2;
		string[] array2 = new string[num];
		string[] array3 = new string[num];
		for (int i = 0; i < num; i++)
		{
			array2[i] = array[i * 2];
			array3[i] = array[i * 2 + 1];
		}
		for (int j = 0; j < num; j++)
		{
			InAppData.inAppData[array2[j]].price = array3[j];
			InAppData.inAppData[array2[j]].validInApp = true;
		}
		Action action = onProductRequestSuccess;
		if (action != null)
		{
			action();
		}
		_inAppPurchaseState = InAppPurchaseState.Complete;
		productRequestSucceeded = true;
	}

	public void ProductRequestFailure()
	{
		Debug.Log("Inapp product request failure!");
		_inAppPurchaseState = InAppPurchaseState.Failed;
		productRequestSucceeded = false;
	}

	public void PurchaseSuccess(string transactionAndProductId)
	{
		string text = InAppPurchaseHandler.parseProductIdFromCallbackString(transactionAndProductId);
		PlayerInfo.Instance.inAppPurchaseCount++;
		PlayerInfo.Instance.amountOfCoins += InAppData.inAppData[text].amountOfCoins;
		PlayerInfo.Instance.Save();
		Action action = onPurchaseSuccess;
		if (action != null)
		{
			action();
		}
		InAppPurchaseHandler.callbackHasBeenHandled(transactionAndProductId);
		UIScreenController.Instance.HideInAppPurchaseOverlay();
		Flurry.LogEventWithAParameter("InApp purchase completed", "Id", text);
		if (text == InAppData.inAppTier1)
		{
			Flurry.LogEventWithAParameter("InApp Coin Pack 1 purchased", "Mission Set", PlayerInfo.Instance.currentMissionSet.ToString());
		}
		else if (text == InAppData.inAppTier2)
		{
			Flurry.LogEventWithAParameter("InApp Coin Pack 2 purchased", "Mission Set", PlayerInfo.Instance.currentMissionSet.ToString());
		}
		else if (text == InAppData.inAppTier3)
		{
			Flurry.LogEventWithAParameter("InApp Coin Pack 3 purchased", "Mission Set", PlayerInfo.Instance.currentMissionSet.ToString());
		}
	}

	public void PurchaseFailure(string transactionAndProductId)
	{
		string text = InAppPurchaseHandler.parseProductIdFromCallbackString(transactionAndProductId);
		Action action = onPurchaseFailure;
		if (action != null)
		{
			action();
		}
		DeviceUtility.showNativePopup("Purchase Failed", "An error occurred while handling your purchase. Please try again later.", "Ok");
		InAppPurchaseHandler.callbackHasBeenHandled(transactionAndProductId);
		UIScreenController.Instance.HideInAppPurchaseOverlay();
	}

	private void OnDestroy()
	{
		InAppPurchaseHandler.resetForPurchase();
		InAppPurchaseHandler.resetForProductRequest();
	}

	public bool WillQueryInAppPurchases()
	{
		if (_inAppPurchaseState == InAppPurchaseState.NotStarted || _inAppPurchaseState == InAppPurchaseState.Failed)
		{
			_inAppPurchaseState = InAppPurchaseState.Started;
			return true;
		}
		return false;
	}

	public bool HasQueriedInAppPurchases()
	{
		return _inAppPurchaseState == InAppPurchaseState.Complete;
	}

	public bool IsQueryingInAppPurchases()
	{
		return _inAppPurchaseState == InAppPurchaseState.Started;
	}
}
