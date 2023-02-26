using System;
using UnityEngine;

public class UIPowerupHelper : MonoBehaviour
{
	public UISlider slider;

	public UISprite icon;

	public UILabel amountLabel;

	public void SetPowerup(ActivePowerup powerup)
	{
		icon.spriteName = Upgrades.upgrades[powerup.type].iconName;
		float sliderValue = powerup.timeLeft / PlayerInfo.Instance.GetPowerupDuration(powerup.type);
		slider.sliderValue = sliderValue;
		if (powerup.type == PowerupType.hoverboard)
		{
			amountLabel.text = PlayerInfo.Instance.GetUpgradeAmount(powerup.type).ToString();
			amountLabel.gameObject.active = true;
		}
		else
		{
			amountLabel.gameObject.active = false;
		}
		if (powerup.timeLeft < 0f)
		{
			if (slider.gameObject.active)
			{
				NGUITools.SetActive(slider.gameObject, false);
			}
			icon.color = Color.Lerp(Color.grey, Color.white, 0.5f + 0.5f * Mathf.Cos(powerup.timeLeft * (float)Math.PI * 4f));
		}
		else
		{
			if (!slider.gameObject.active)
			{
				NGUITools.SetActive(slider.gameObject, true);
			}
			icon.color = Color.white;
		}
	}
}
