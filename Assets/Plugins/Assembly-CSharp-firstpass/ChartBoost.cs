using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class ChartBoost : MonoBehaviour
{
	private const string BRIDGE_DELEGATE_GAMEOBJECT_NAME = "ChartBoostBridge";

	private static ChartBoost bridgeDelegate;

	public static Action didFailToLoadInterstitial;

	public static Action didDismissInterstitial;

	public static Action didCloseInterstitial;

	public static Action didClickInterstitial;

	public static Action didFailToLoadMoreApps;

	public static Action didDismissMoreApps;

	public static Action didCloseMoreApps;

	public static Action didClickMoreApps;

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

	private void bridge_didFailToLoadInterstitial()
	{
		invokeHandlerIfNotNull(didFailToLoadInterstitial);
	}

	private void bridge_didDismissInterstitial()
	{
		invokeHandlerIfNotNull(didDismissInterstitial);
	}

	private void bridge_didCloseInterstitial()
	{
		invokeHandlerIfNotNull(didCloseInterstitial);
	}

	private void bridge_didClickInterstitial()
	{
		invokeHandlerIfNotNull(didClickInterstitial);
	}

	private void bridge_didFailToLoadMoreApps()
	{
		invokeHandlerIfNotNull(didFailToLoadMoreApps);
	}

	private void bridge_didDismissMoreApps()
	{
		invokeHandlerIfNotNull(didDismissMoreApps);
	}

	private void bridge_didCloseMoreApps()
	{
		invokeHandlerIfNotNull(didCloseMoreApps);
	}

	private void bridge_didClickMoreApps()
	{
		invokeHandlerIfNotNull(didClickMoreApps);
	}

	public static void InitAndStartSession(string appId, string appSignature)
	{
	}

	public static void CacheInterstitial()
	{
	}

	public static void CacheInterstitial(string location)
	{
	}

	public static void ShowInterstitial()
	{
	}

	public static void ShowInterstitial(string location)
	{
	}

	public static bool HasCachedInterstitial()
	{
		return bridge_hasCachedInterstitial();
	}

	public static bool HasCachedInterstitial(string location)
	{
		return bridge_hasCachedInterstitialLocation(location);
	}

	public static void CacheMoreApps()
	{
	}

	public static void ShowMoreApps()
	{
	}

	public static void SetIdentityHidden(bool hidden)
	{
	}

	public static bool IsIdentityHidden()
	{
		return bridge_isIdentityHidden();
	}

	public static bool GetShouldRequestInterstitial()
	{
		return bridge_getShouldRequestInterstitial();
	}

	public static void SetShouldRequestInterstitial(bool should)
	{
	}

	public static bool GetShouldDisplayInterstitial()
	{
		return bridge_getShouldDisplayInterstitial();
	}

	public static void SetShouldDisplayInterstitial(bool should)
	{
	}

	public static bool GetShouldDisplayLoadingViewForMoreApps()
	{
		return bridge_getShouldDisplayLoadingViewForMoreApps();
	}

	public static void SetShouldDisplayLoadingViewForMoreApps(bool should)
	{
	}

	public static bool GetShouldDisplayMoreApps()
	{
		return bridge_getShouldDisplayMoreApps();
	}

	public static void SetShouldDisplayMoreApps(bool should)
	{
	}

	[DllImport("__Internal")]
	private static extern void bridge_initAndStartSession(string appId, string appSignature);

	[DllImport("__Internal")]
	private static extern void bridge_cacheInterstitial();

	[DllImport("__Internal")]
	private static extern void bridge_cacheInterstitialLocation(string location);

	[DllImport("__Internal")]
	private static extern void bridge_showInterstitial();

	[DllImport("__Internal")]
	private static extern void bridge_showInterstitialLocation(string location);

	[DllImport("__Internal")]
	private static extern bool bridge_hasCachedInterstitial();

	[DllImport("__Internal")]
	private static extern bool bridge_hasCachedInterstitialLocation(string location);

	[DllImport("__Internal")]
	private static extern void bridge_cacheMoreApps();

	[DllImport("__Internal")]
	private static extern void bridge_showMoreApps();

	[DllImport("__Internal")]
	private static extern void bridge_setIdentityHidden(bool hidden);

	[DllImport("__Internal")]
	private static extern bool bridge_isIdentityHidden();

	[DllImport("__Internal")]
	private static extern bool bridge_getShouldRequestInterstitial();

	[DllImport("__Internal")]
	private static extern void bridge_setShouldRequestInterstitial(bool should);

	[DllImport("__Internal")]
	private static extern bool bridge_getShouldDisplayInterstitial();

	[DllImport("__Internal")]
	private static extern void bridge_setShouldDisplayInterstitial(bool should);

	[DllImport("__Internal")]
	private static extern bool bridge_getShouldDisplayLoadingViewForMoreApps();

	[DllImport("__Internal")]
	private static extern void bridge_setShouldDisplayLoadingViewForMoreApps(bool should);

	[DllImport("__Internal")]
	private static extern bool bridge_getShouldDisplayMoreApps();

	[DllImport("__Internal")]
	private static extern void bridge_setShouldDisplayMoreApps(bool should);
}
