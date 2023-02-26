using System;
using UnityEngine;

public class AdColonyCallbackHandler : MonoBehaviour
{
	public const string ADCOLONY_APPID = "app2568a30bc18f470288d36d";

	public const string ADCOLONY_ZONEID = "vz714b7567808540889e4a44";

	private const string ADCOLONY_NOVIDEOS_NATIVE_POPUP_TITLE = "No videos available";

	private const string ADCOLONY_NOVIDEOS_NATIVE_POPUP_MESSAGE = "We are currently out of videos to show you. Please try again later.";

	private const string ADCOLONY_NOVIDEOS_NATIVE_POPUP_OKBUTTONTEXT = "OK";

	private bool adColonySoundOnBeforeMute;

	private void Awake()
	{
		AdColony.takeoverBegan = (Action)Delegate.Combine(AdColony.takeoverBegan, new Action(TakeOverBegan));
		AdColony.takeoverEndedWithVC = (Action<bool>)Delegate.Combine(AdColony.takeoverEndedWithVC, new Action<bool>(TakeOverEndedWithVC));
		AdColony.videoAdNotServed = (Action)Delegate.Combine(AdColony.videoAdNotServed, new Action(VideoAdNotServed));
		AdColony.virtualCurrencyAwarded = (Action<string, int>)Delegate.Combine(AdColony.virtualCurrencyAwarded, new Action<string, int>(VirtualCurrencyAwarded));
	}

	private void OnDestroy()
	{
		AdColony.takeoverBegan = (Action)Delegate.Remove(AdColony.takeoverBegan, new Action(TakeOverBegan));
		AdColony.takeoverEndedWithVC = (Action<bool>)Delegate.Remove(AdColony.takeoverEndedWithVC, new Action<bool>(TakeOverEndedWithVC));
		AdColony.videoAdNotServed = (Action)Delegate.Remove(AdColony.videoAdNotServed, new Action(VideoAdNotServed));
	}

	private void TakeOverBegan()
	{
		adColonySoundOnBeforeMute = Settings.optionSound;
		Settings.optionSound = false;
	}

	private void TakeOverEndedWithVC(bool withVirtualCurrency)
	{
		Settings.optionSound = adColonySoundOnBeforeMute;
	}

	private void VideoAdNotServed()
	{
		DeviceUtility.showNativePopup("No videos available", "We are currently out of videos to show you. Please try again later.", "OK");
	}

	private void VirtualCurrencyAwarded(string currency, int amount)
	{
		PlayerInfo.Instance.amountOfCoins += amount;
		PlayerInfo.Instance.Save();
	}
}
