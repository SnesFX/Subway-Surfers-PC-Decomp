using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Localization")]
public class Localization : MonoBehaviour
{
	private static Localization mInst;

	public string startingLanguage;

	public TextAsset[] languages;

	private Dictionary<string, string> mDictionary = new Dictionary<string, string>();

	private string mLanguage;

	public static Localization instance
	{
		get
		{
			if (mInst == null)
			{
				mInst = Object.FindObjectOfType(typeof(Localization)) as Localization;
				if (mInst == null)
				{
					GameObject gameObject = new GameObject("_Localization");
					Object.DontDestroyOnLoad(gameObject);
					mInst = gameObject.AddComponent<Localization>();
				}
			}
			return mInst;
		}
	}

	public string currentLanguage
	{
		get
		{
			if (string.IsNullOrEmpty(mLanguage))
			{
				currentLanguage = PlayerPrefs.GetString("Language");
				if (string.IsNullOrEmpty(mLanguage))
				{
					currentLanguage = startingLanguage;
					if (string.IsNullOrEmpty(mLanguage) && languages != null && languages.Length > 0)
					{
						currentLanguage = languages[0].name;
					}
				}
			}
			return mLanguage;
		}
		set
		{
			if (!(mLanguage != value))
			{
				return;
			}
			if (!string.IsNullOrEmpty(value))
			{
				if (languages != null)
				{
					int i = 0;
					for (int num = languages.Length; i < num; i++)
					{
						TextAsset textAsset = languages[i];
						if (textAsset != null && textAsset.name == value)
						{
							Load(textAsset);
							return;
						}
					}
				}
				TextAsset textAsset2 = Resources.Load(value, typeof(TextAsset)) as TextAsset;
				if (textAsset2 != null)
				{
					Load(textAsset2);
					return;
				}
			}
			mDictionary.Clear();
			PlayerPrefs.DeleteKey("Language");
		}
	}

	private void Awake()
	{
		if (mInst == null)
		{
			mInst = this;
			Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void OnEnable()
	{
		if (mInst == null)
		{
			mInst = this;
		}
	}

	private void OnDestroy()
	{
		if (mInst == this)
		{
			mInst = null;
		}
	}

	private void Load(TextAsset asset)
	{
		mLanguage = asset.name;
		PlayerPrefs.SetString("Language", mLanguage);
		ByteReader byteReader = new ByteReader(asset);
		mDictionary = byteReader.ReadDictionary();
		UIRoot.Broadcast("OnLocalize", this);
	}

	public string Get(string key)
	{
		string value;
		return (!mDictionary.TryGetValue(key, out value)) ? key : value;
	}
}
