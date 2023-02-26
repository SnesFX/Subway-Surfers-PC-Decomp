using UnityEngine;

[ExecuteInEditMode]
public class DailyChallengeColorChanger : MonoBehaviour
{
	public UILabel shadowLabel;

	public Color MyColorActive;

	public Color MyColorInactive;

	public Color shadowColorActive;

	public Color shadowColorInactive;

	private string _MyColorActive;

	private string _MyColorInactive;

	private string _shadowColorActive;

	private string _shadowColorInactive;

	private UILabel _myLabel;

	private string _cachedText = string.Empty;

	private string _cachedDailyWord = string.Empty;

	private IntMask _cachedDailyMask = -1;

	private void Awake()
	{
		_myLabel = base.gameObject.GetComponent<UILabel>();
		_MyColorActive = NGUITools.EncodeColor(MyColorActive);
		_MyColorInactive = NGUITools.EncodeColor(MyColorInactive);
		_shadowColorActive = NGUITools.EncodeColor(shadowColorActive);
		_shadowColorInactive = NGUITools.EncodeColor(shadowColorInactive);
	}

	private void Update()
	{
		UpdateDailyWord();
	}

	private void UpdateDailyWord()
	{
		if (_myLabel == null)
		{
			_myLabel = base.gameObject.GetComponent<UILabel>();
		}
		string text = PlayerInfo.Instance.dailyWord;
		if (string.IsNullOrEmpty(text))
		{
			text = string.Empty;
		}
		int length = text.Length;
		IntMask dailyWordUnlockedMask = PlayerInfo.Instance.dailyWordUnlockedMask;
		if (!(text == _cachedDailyWord) || (int)dailyWordUnlockedMask != (int)_cachedDailyMask)
		{
			_cachedDailyWord = text;
			_cachedDailyMask = dailyWordUnlockedMask;
			string text2 = string.Empty;
			for (int i = 0; i < length; i++)
			{
				text2 = ((!dailyWordUnlockedMask[i]) ? (text2 + "[" + _MyColorInactive + "]") : (text2 + "[" + _MyColorActive + "]"));
				text2 = text2 + text[i] + " ";
			}
			_cachedText = text2;
			_myLabel.text = _cachedText;
			string text3 = string.Empty;
			for (int j = 0; j < length; j++)
			{
				text3 = ((!dailyWordUnlockedMask[j]) ? (text3 + "[" + _shadowColorInactive + "]") : (text3 + "[" + _shadowColorActive + "]"));
				text3 = text3 + text[j] + " ";
			}
			shadowLabel.text = text3;
		}
	}
}
