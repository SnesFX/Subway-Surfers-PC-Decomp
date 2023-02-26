using UnityEngine;

public class CoinEarnerButtonHelper : MonoBehaviour
{
	public UISprite icon;

	public UILabel title;

	public UILabel description;

	private int earnCurrencyProfileIndex;

	public void Init(int earnCurrencyProfileIndex, string title, string desc, string iconName)
	{
		this.earnCurrencyProfileIndex = earnCurrencyProfileIndex;
		this.title.text = title;
		icon.spriteName = iconName;
		description.text = desc;
	}

	private void OnClick()
	{
		EarnCurrencyInfo.Trigger(earnCurrencyProfileIndex);
		NGUITools.FindInParents<CoinScreenSetup>(base.gameObject).RefreshCurrencyEarners();
	}
}
