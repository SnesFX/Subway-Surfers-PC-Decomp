using System;
using System.Text;
using UnityEngine;

public static class EarnCurrencyInfo
{
	public enum Id
	{
		FacebookKiloo = 0,
		FacebookSybo = 1,
		TwitterKiloo = 2,
		TwitterSybo = 3,
		YoutubeKiloo = 4,
		YoutubeSybo = 5,
		Review = 6,
		AdColony = 7
	}

	public enum Type
	{
		OpenURL = 0,
		AdColony = 1
	}

	public enum Repeatability
	{
		Once = 0,
		OncePerVersion = 1,
		Forever = 2
	}

	public class EarnCurrencyProfile
	{
		public Id id;

		public Type type;

		public Repeatability repeatability;

		public int amountOfCoins;

		public string title = string.Empty;

		public string desc = string.Empty;

		public string iconName = string.Empty;

		public string url;
	}

	private const string DATA_PROFILE_MAIN_SPLIT = ";";

	private const string DATA_PROFILE_SUB_SPLIT = "=";

	public static readonly EarnCurrencyProfile[] profiles = new EarnCurrencyProfile[7]
	{
		new EarnCurrencyProfile
		{
			id = Id.AdColony,
			type = Type.AdColony,
			repeatability = Repeatability.Forever,
			amountOfCoins = 50,
			title = "Sponsored video",
			desc = "Watch get {0} coins",
			iconName = "icon_coinPack_1"
		},
		new EarnCurrencyProfile
		{
			amountOfCoins = 300,
			title = "Kiloo Facebook",
			desc = "'Like' get {0} coins",
			iconName = "icon_coinPack_1",
			url = "http://www.facebook.com/kiloogames"
		},
		new EarnCurrencyProfile
		{
			id = Id.FacebookSybo,
			amountOfCoins = 300,
			title = "Sybo Facebook",
			desc = "'Like' get {0} coins",
			iconName = "icon_coinPack_1",
			url = "http://www.facebook.com/sybogames"
		},
		new EarnCurrencyProfile
		{
			id = Id.TwitterKiloo,
			amountOfCoins = 300,
			title = "Kiloo Twitter",
			desc = "Follow get {0} coins",
			iconName = "icon_coinPack_1",
			url = "https://twitter.com/@kiloogames"
		},
		new EarnCurrencyProfile
		{
			id = Id.TwitterSybo,
			amountOfCoins = 300,
			title = "Sybo Twitter",
			desc = "Follow get {0} coins",
			iconName = "icon_coinPack_1",
			url = "https://twitter.com/@sybogames"
		},
		new EarnCurrencyProfile
		{
			id = Id.YoutubeKiloo,
			amountOfCoins = 300,
			title = "Kiloo YouTube",
			desc = "Subscribe get {0} coins",
			iconName = "icon_coinPack_1",
			url = "http://www.youtube.com/kiloomobile"
		},
		new EarnCurrencyProfile
		{
			id = Id.YoutubeSybo,
			amountOfCoins = 300,
			title = "Sybo YouTube",
			desc = "Subscribe get {0} coins",
			iconName = "icon_coinPack_1",
			url = "http://www.youtube.com/sybogames"
		}
	};

	private static readonly string[] DATA_PROFILE_ALL_SPLITS = new string[2] { "=", ";" };

	private static string[] profileData;

	public static bool ShouldShowInGUI(int profileIndex)
	{
		EarnCurrencyProfile earnCurrencyProfile = profiles[profileIndex];
		string text = null;
		if (earnCurrencyProfile.repeatability == Repeatability.Once || earnCurrencyProfile.repeatability == Repeatability.OncePerVersion)
		{
			string text2 = GetProfileData(profileIndex);
			if (string.IsNullOrEmpty(text2))
			{
				return true;
			}
			if (earnCurrencyProfile.repeatability == Repeatability.OncePerVersion)
			{
				if (text == null)
				{
					text = DeviceUtility.GetBundleVersion();
				}
				if (text2 != text)
				{
					return true;
				}
			}
			return false;
		}
		return true;
	}

	public static void Trigger(int profileIndex)
	{
		EarnCurrencyProfile earnCurrencyProfile = profiles[profileIndex];
		if (earnCurrencyProfile.type == Type.OpenURL)
		{
			if (earnCurrencyProfile.amountOfCoins > 0)
			{
				PlayerInfo.Instance.amountOfCoins += earnCurrencyProfile.amountOfCoins;
			}
		}
		else if (earnCurrencyProfile.type != Type.AdColony)
		{
			Debug.LogError("Unhandled earner type: " + earnCurrencyProfile.type);
		}
		if (earnCurrencyProfile.repeatability == Repeatability.Once || earnCurrencyProfile.repeatability == Repeatability.OncePerVersion)
		{
			string bundleVersion = DeviceUtility.GetBundleVersion();
			SetAndSaveProfileData(profileIndex, bundleVersion);
		}
		PlayerInfo.Instance.Save();
		if (earnCurrencyProfile.type == Type.OpenURL)
		{
			Application.OpenURL(earnCurrencyProfile.url);
		}
		else if (earnCurrencyProfile.type == Type.AdColony)
		{
			if (!AdColony.isInitialized)
			{
				AdColony.Init("app2568a30bc18f470288d36d", "vz714b7567808540889e4a44");
			}
			AdColony.PlayVideoAdWithPrePopup(true, true);
		}
		else
		{
			Debug.LogError("Unhandled earner type: " + earnCurrencyProfile.type);
		}
	}

	private static void InitProfileDataArrayIfNeeded()
	{
		if (profileData != null)
		{
			return;
		}
		profileData = new string[profiles.Length];
		string earnCurrenyData = PlayerInfo.Instance.earnCurrenyData;
		Debug.Log("EarnCurrenyInfo: Loaded raw profile data: " + earnCurrenyData);
		if (string.IsNullOrEmpty(earnCurrenyData))
		{
			return;
		}
		string[] array = earnCurrenyData.Split(DATA_PROFILE_ALL_SPLITS, StringSplitOptions.None);
		for (int i = 0; i < array.Length - 1; i++)
		{
			string value = array[i];
			string text = array[i + 1];
			if (!Enum.IsDefined(typeof(Id), value))
			{
				continue;
			}
			Id id = (Id)(int)Enum.Parse(typeof(Id), value, true);
			for (int j = 0; j < profiles.Length; j++)
			{
				if (profiles[j].id == id)
				{
					profileData[j] = text;
					break;
				}
			}
		}
	}

	private static string GetProfileData(int profileIndex)
	{
		InitProfileDataArrayIfNeeded();
		return profileData[profileIndex];
	}

	private static void SetAndSaveProfileData(int profileIndex, string data)
	{
		InitProfileDataArrayIfNeeded();
		profileData[profileIndex] = data;
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < profileData.Length; i++)
		{
			if (profileData[i] != null)
			{
				stringBuilder.Append(profiles[i].id.ToString());
				stringBuilder.Append("=");
				stringBuilder.Append(profileData[i]);
				stringBuilder.Append(";");
			}
		}
		Debug.Log("EarnCurrenyInfo: Saving profile data: " + stringBuilder.ToString());
		PlayerInfo.Instance.earnCurrenyData = stringBuilder.ToString();
		PlayerInfo.Instance.Save();
	}
}
