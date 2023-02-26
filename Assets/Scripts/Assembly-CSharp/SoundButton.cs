using UnityEngine;

public class SoundButton : MonoBehaviour
{
	public UISprite soundIcon;

	private string _iconOn = "icon_soundOn";

	private string _iconOff = "icon_soundOff";

	public UILabel soundLabel;

	private string _labelOff = "Sound OFF";

	private string _labelOn = "Sound ON";

	private void Awake()
	{
		_SetupButton();
	}

	public void Click()
	{
		Settings.optionSound = !Settings.optionSound;
		_SetupButton();
	}

	private void _SetupButton()
	{
		if (Settings.optionSound)
		{
			soundLabel.text = _labelOn;
			soundIcon.spriteName = _iconOn;
		}
		else
		{
			soundLabel.text = _labelOff;
			soundIcon.spriteName = _iconOff;
		}
	}
}
