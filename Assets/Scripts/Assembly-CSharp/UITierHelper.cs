using UnityEngine;

public class UITierHelper : MonoBehaviour
{
	public UIAtlas usedAtlas;

	private PowerupType _type;

	public bool ResetTiers()
	{
		foreach (Transform item in base.transform)
		{
			Object.Destroy(item.gameObject);
		}
		SetupTiers(_type);
		if (PlayerInfo.Instance.GetCurrentTier(_type) < Upgrades.upgrades[_type].numberOfTiers - 1)
		{
			return false;
		}
		return true;
	}

	public void SetupTiers(PowerupType type)
	{
		_type = type;
		int numberOfTiers = Upgrades.upgrades[type].numberOfTiers;
		int currentTier = PlayerInfo.Instance.GetCurrentTier(type);
		UISprite uISprite = NGUITools.AddSprite(base.gameObject, usedAtlas, "progressbar_background");
		uISprite.name = "0background";
		uISprite.transform.localScale = new Vector3((float)numberOfTiers * 20f - 10f, 16f, 1f);
		uISprite.pivot = UIWidget.Pivot.BottomLeft;
		uISprite.depth = 11;
		for (int i = 0; i < numberOfTiers - 1; i++)
		{
			UISprite uISprite2 = NGUITools.AddSprite(base.gameObject, usedAtlas, "progressbar_bar_off");
			uISprite2.name = "slot" + (i + 1);
			uISprite2.transform.localPosition = new Vector3(5f + (float)(20 * i), 3f, 0f);
			uISprite2.transform.localScale = new Vector3(16f, 10f, 1f);
			uISprite2.pivot = UIWidget.Pivot.BottomLeft;
			uISprite2.depth = 12;
			uISprite2.MakePixelPerfect();
		}
		for (int j = 0; j < currentTier; j++)
		{
			UISprite uISprite3 = NGUITools.AddSprite(base.gameObject, usedAtlas, "progressbar_bar_on");
			uISprite3.name = "ActiveSlot" + (j + 1);
			uISprite3.transform.localPosition = new Vector3(4f + (float)(20 * j), 3f, 0f);
			uISprite3.transform.localScale = new Vector3(18f, 9f, 1f);
			uISprite3.pivot = UIWidget.Pivot.BottomLeft;
			uISprite3.depth = 13;
			uISprite3.MakePixelPerfect();
		}
	}

	private Color getTierColor(int numberOfActiveTiers)
	{
		switch (numberOfActiveTiers)
		{
		case 1:
			return new Color(1f, 0f, 0f, 1f);
		case 2:
			return new Color(1f, 0f, 0f, 1f);
		case 3:
			return new Color(1f, 0f, 0f, 1f);
		case 4:
			return new Color(1f, 0f, 0f, 1f);
		case 5:
			return new Color(1f, 0f, 0f, 1f);
		default:
			return Color.white;
		}
	}
}
