using UnityEngine;

public class Settings : MonoBehaviour
{
	private const string OPTION_SOUND_KEY = "OPTION_SOUND";

	private const int OPTION_SOUND_DEFAULT = 1;

	private const string OPTION_AUTOMESSAGE_KEY = "OPTION_AUTOMESSAGE";

	private const int OPTION_AUTOMESSAGE_DEFAULT = 0;

	private static bool _optionsLoaded;

	private static bool _optionSound;

	private static bool _optionAutoMessage;

	public static bool optionAutoMessage
	{
		get
		{
			LoadOptionsIfNeeded();
			return _optionAutoMessage;
		}
		set
		{
			if (!value && !PlayerPrefs.HasKey("OPTION_AUTOMESSAGE"))
			{
				Flurry.LogEvent("AutoBrag turned off");
			}
			_optionAutoMessage = value;
			PlayerPrefs.SetInt("OPTION_AUTOMESSAGE", _optionAutoMessage ? 1 : 0);
		}
	}

	public static bool optionSound
	{
		get
		{
			LoadOptionsIfNeeded();
			return _optionSound;
		}
		set
		{
			_optionSound = value;
			PlayerPrefs.SetInt("OPTION_SOUND", _optionSound ? 1 : 0);
			AudioListener.volume = ((!_optionSound) ? 0f : 1f);
		}
	}

	private void Awake()
	{
		LoadOptionsIfNeeded();
		SocialManager instance = SocialManager.instance;
	}

	private static void LoadOptionsIfNeeded()
	{
		if (!_optionsLoaded)
		{
			_optionSound = PlayerPrefs.GetInt("OPTION_SOUND", 1) != 0;
			AudioListener.volume = ((!_optionSound) ? 0f : 1f);
			_optionAutoMessage = PlayerPrefs.GetInt("OPTION_AUTOMESSAGE", 0) != 0;
			_optionsLoaded = true;
		}
	}
}
