using System.Runtime.InteropServices;
using UnityEngine;

public static class Flurry
{
	public const string EVENT_UISCREEN_CHANGED_PREFIX = "UI Screen ";

	public const string EVENT_10_SOCIAL_ACTIONS_TAKEN = "10 social actions taken";

	public const string EVENT_FIRST_GAMECENTER_LOGIN = "First GameCenter Login";

	public const string EVENT_FIRST_FACEBOOK_LOGIN = "First Facebook Login";

	public const string EVENT_MYSTERY_BOX_OPENED = "Mystery Box opened";

	public const string EVENT_INAPPPURCHASE_COMPLETED = "InApp purchase completed";

	public const string EVENT_INAPPPURCHASE_COINPACK1 = "InApp Coin Pack 1 purchased";

	public const string EVENT_INAPPPURCHASE_COINPACK2 = "InApp Coin Pack 2 purchased";

	public const string EVENT_INAPPPURCHASE_COINPACK3 = "InApp Coin Pack 3 purchased";

	public const string EVENT_CHARACTER_UNLOCKED = "Character unlocked";

	public const string EVENT_AUTOMESSAGE_TURNED_OFF = "AutoBrag turned off";

	public const string EVENT_MISSIONSET_COMPLETED = "Mission Set completed";

	public const string EVENT_DAILY_CHALLENGE_COMPLETED = "Daily Challenge completed";

	public const string EVENT_BOOST_HEADSTART500_PURCHASED = "Boost Headstart500 purchased";

	public const string EVENT_BOOST_HEADSTART2000_PURCHASED = "Boost Headstart2000 purchased";

	public const string EVENT_BOOST_HOVERBOARD_PURCHASED = "Boost Hoverboard purchased";

	public const string EVENT_BOOST_COINMAGNET_PURCHASED = "Boost Coinmagnet purchased";

	public const string EVENT_BOOST_DOUBLEMULTIPLIER_PURCHASED = "Boost 2x multiplier purchased";

	public const string EVENT_BOOST_JETPACK_PURCHASED = "Boost jetpack purchased";

	public const string EVENT_BOOST_LETTERS_PURCHASED = "Boost letters purchased";

	public const string EVENT_BOOST_SUPERSNEAKERS_PURCHASED = "Boost supersneakers purchased";

	public const string EVENT_BOOST_MYSTERYBOX_PURCHASED = "Boost MysteryBox purchased";

	public const string EVENT_BOOST_MISSION_SKIP_PURCHASED = "Boost Mission Skip purchased";

	public const string EVENT_ARGKEY_ID = "Id";

	public const string EVENT_ARGKEY_TIER = "Tier";

	public const string EVENT_ARGKEY_UI_SCREENNAME = "Screen Name";

	public const string EVENT_ARGKEY_MISSIONSET = "Mission Set";

	public const string EVENT_ARGKEY_MISSIONSET_AND_INDEX = "Mission Set and Index";

	public const string EVENT_ARGKEY_TOTAL = "Total";

	private const bool disable = true;

	private static bool inSession;

	public static void LogGenericSocialAction()
	{
		int @int = PlayerPrefs.GetInt("flurry_social_total", 0);
		int int2 = PlayerPrefs.GetInt("flurry_social_unlogged", 0);
		@int++;
		int2++;
		Debug.Log("LogGenericSocialAction: new unlogged total = " + int2);
		if (int2 == 10)
		{
			int2 = 0;
			LogEventWithAParameter("10 social actions taken", "Total", @int.ToString());
		}
		PlayerPrefs.SetInt("flurry_social_total", @int);
		PlayerPrefs.SetInt("flurry_social_unlogged", int2);
	}

	public static void LogGameCenterLogin()
	{
		if (!PlayerPrefs.HasKey("flurry_has_logged_gc"))
		{
			LogEvent("First GameCenter Login");
			PlayerPrefs.SetInt("flurry_has_logged_gc", 1);
		}
	}

	public static void LogFacebookLogin()
	{
		if (!PlayerPrefs.HasKey("flurry_has_logged_fb"))
		{
			LogEvent("First Facebook Login");
			PlayerPrefs.SetInt("flurry_has_logged_fb", 1);
		}
	}

	[DllImport("__Internal")]
	private static extern void flurryStartSession(string apiKey);

	[DllImport("__Internal")]
	private static extern void flurrySetUserInfo(string userId, int age, int gender);

	[DllImport("__Internal")]
	private static extern void flurryLogEvent(string eventName);

	[DllImport("__Internal")]
	private static extern void flurryLogEventWithAParameter(string eventName, string argKey, string argValue);

	[DllImport("__Internal")]
	private static extern void flurryLogEventWithSeveralParameters(string eventName, string argKeys, string argValues);

	[DllImport("__Internal")]
	private static extern void flurryLogError(string errorName, string message);

	public static void StartSession(string apiKey)
	{
		if (inSession)
		{
		}
	}

	public static void SetUserInfo(string userId)
	{
		SetUserInfo(userId, 0, 0);
	}

	public static void SetUserInfo(string userId, int age, int gender)
	{
		if (inSession)
		{
		}
	}

	public static void LogEvent(string eventName)
	{
		if (inSession)
		{
		}
	}

	public static void LogEventWithAParameter(string eventName, string argKey, string argValue)
	{
		if (inSession)
		{
		}
	}

	public static void LogEventWithSeveralParameters(string eventName, string argKeys, string argValues)
	{
		if (inSession)
		{
		}
	}

	public static void LogError(string errorName, string message)
	{
		if (inSession)
		{
		}
	}
}
