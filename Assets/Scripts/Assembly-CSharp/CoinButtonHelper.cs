using System;
using UnityEngine;

public class CoinButtonHelper : MonoBehaviour
{
	public UISprite icon;

	public UILabel title;

	public UILabel price;

	public UILabel description;

	private string _inAppKey;

	public string Key
	{
		get
		{
			return _inAppKey;
		}
	}

	public void Init(string key)
	{
		icon.spriteName = InAppData.inAppData[key].iconName;
		title.text = InAppData.inAppData[key].title;
		price.text = InAppData.inAppData[key].price;
		description.text = InAppData.inAppData[key].amountOfCoins + " Coins";
		_inAppKey = key;
		InAppManager instance = InAppManager.Instance;
		instance.onProductRequestSuccess = (Action)Delegate.Combine(instance.onProductRequestSuccess, new Action(UpdatePrice));
	}

	private void UpdatePrice()
	{
		price.text = InAppData.inAppData[_inAppKey].price;
	}
}
