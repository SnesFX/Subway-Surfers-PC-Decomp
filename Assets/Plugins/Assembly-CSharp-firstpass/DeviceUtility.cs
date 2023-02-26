using System.Runtime.InteropServices;
using UnityEngine;

public class DeviceUtility
{
	public static bool DISABLE_ALL_PLUGINS = Application.isEditor;

	private static bool disable = DISABLE_ALL_PLUGINS;

	[DllImport("__Internal")]
	private static extern string utilityGetBundleVersion();

	[DllImport("__Internal")]
	private static extern bool utilityOpenUrl(string url);

	[DllImport("__Internal")]
	private static extern bool utilityIsOtherAudioPlaying();

	[DllImport("__Internal")]
	private static extern void utilityShowNativePopup(string title, string message, string cancelButtonTitle);

	[DllImport("__Internal")]
	private static extern void utilityShowNativePopupWithCallback(string callbackGameObjectName, string callbackDidCloseFunctionName, string title, string message, string cancelButtonTitle, string optionalButton2, string optionalButton3);

	public static string GetBundleVersion()
	{
		if (disable)
		{
			return "n/a";
		}
		return utilityGetBundleVersion();
	}

	public static void showNativePopup(string title, string message, string cancelButtonTitle)
	{
		utilityShowNativePopup(title, message, cancelButtonTitle);
	}

	public static void showNativePopupWithCallback(string callbackGameObjectName, string callbackDidCloseFunctionName, string title, string message, string cancelButtonTitle, string optionalButton2, string optionalButton3)
	{
		utilityShowNativePopupWithCallback(callbackGameObjectName, callbackDidCloseFunctionName, title, message, cancelButtonTitle, optionalButton2, optionalButton3);
	}
}
