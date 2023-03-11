using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class AdColony : MonoBehaviour
{
	private const string BRIDGE_DELEGATE_GAMEOBJECT_NAME = "AdColonyBridge";

	private static AdColony bridgeDelegate;

	public static Action noVideoFill;

	public static Action videoAdsReady;

	public static Action videoAdsNotReady;

	public static Action<string, int> virtualCurrencyAwarded;

	public static Action<string, int, string> virtualCurrencyNotAwarded;

	public static Action takeoverBegan;

	public static Action<bool> takeoverEndedWithVC;

	public static Action videoAdNotServed;

	public static bool isInitialized
	{
		get
		{
			return bridgeDelegate != null;
		}
	}

	private void OnDestroy()
	{
		if (bridgeDelegate == this)
		{
			bridgeDelegate = null;
		}
	}

	private void invokeHandlerIfNotNull(Action handler)
	{
		if (handler != null)
		{
			handler();
		}
	}

	private void bridge_noVideoFill()
	{
		invokeHandlerIfNotNull(noVideoFill);
	}

	private void bridge_videoAdsReady()
	{
		invokeHandlerIfNotNull(videoAdsReady);
	}

	private void bridge_videoAdsNotReady()
	{
		invokeHandlerIfNotNull(videoAdsNotReady);
	}

	private void bridge_virtualCurrencyAwarded(string message)
	{
		Action<string, int> action = virtualCurrencyAwarded;
		if (action == null)
		{
			return;
		}
		string[] array = message.Split(';');
		if (array.Length == 2)
		{
			int result = 0;
			if (int.TryParse(array[1], out result))
			{
				string arg = array[0];
				action(arg, result);
			}
			else
			{
				Debug.LogError("bridge_virtualCurrencyAwarded: Failed to parse amount: " + message);
			}
		}
		else
		{
			Debug.LogError("bridge_virtualCurrencyAwarded: Failed to parse message: " + message);
		}
	}

	private void bridge_virtualCurrencyNotAwarded(string message)
	{
		Action<string, int, string> action = virtualCurrencyNotAwarded;
		if (action == null)
		{
			return;
		}
		string[] array = message.Split(';');
		if (array.Length == 3)
		{
			int result = 0;
			if (int.TryParse(array[1], out result))
			{
				string arg = array[0];
				string arg2 = array[2];
				action(arg, result, arg2);
			}
			else
			{
				Debug.LogError("bridge_virtualCurrencyNotAwarded: Failed to parse amount: " + message);
			}
		}
		else
		{
			Debug.LogError("bridge_virtualCurrencyNotAwarded: Failed to parse message: " + message);
		}
	}

	private void bridge_takeoverBegan()
	{
		invokeHandlerIfNotNull(takeoverBegan);
	}

	private void bridge_takeoverEndedWithVC(string message)
	{
		Action<bool> action = takeoverEndedWithVC;
		if (action != null)
		{
			bool obj = message == "1";
			action(obj);
		}
	}

	private void bridge_videoAdNotServed()
	{
		invokeHandlerIfNotNull(videoAdNotServed);
	}

	public static void Init(string appId, string zoneId)
	{
		if (bridgeDelegate == null)
		{
			GameObject gameObject = new GameObject("AdColonyBridge");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			bridgeDelegate = gameObject.AddComponent<AdColony>();
		}
	}

	public static bool VirtualCurrencyAwardAvailable()
	{
		return bridge_virtualCurrencyAwardAvailable();
	}

	public static void PlayVideoAdWithPrePopup(bool prePopup, bool postPopup)
	{
		bridge_playVideoAd(prePopup, postPopup);
	}

	[DllImport("__Internal")]
	private static extern void bridge_initAdColony(string appId, string zoneId);

	[DllImport("__Internal")]
	private static extern bool bridge_virtualCurrencyAwardAvailable();

	[DllImport("__Internal")]
	private static extern void bridge_playVideoAd(bool prePopup, bool postPopup);
}
