using UnityEngine;

public class AutoMessageButton : MonoBehaviour
{
	public UISprite Icon;

	private string _iconOn = "icon_autoMessage_on";

	private string _iconOff = "icon_autoMessage_off";

	public UILabel Label;

	private string _labelOff = "Auto Message OFF";

	private string _labelOn = "Auto Message ON";

	private void Awake()
	{
		_SetupButton();
	}

	public void Click()
	{
		Settings.optionAutoMessage = !Settings.optionAutoMessage;
		_SetupButton();
	}

	private void _SetupButton()
	{
		if (Settings.optionAutoMessage)
		{
			Label.text = _labelOn;
			Icon.spriteName = _iconOn;
		}
		else
		{
			Label.text = _labelOff;
			Icon.spriteName = _iconOff;
		}
	}
}
