using System;
using UnityEngine;

public class CoinBoxSizer : MonoBehaviour
{
	public GameObject AddFundsIcon;

	public GameObject GrayFG;

	public GameObject AmountLabel;

	public GameObject CoinIcon;

	public GameObject Shadow;

	private UILabel cachedAmountLabel;

	private float cachedWidth;

	private bool _updateAutomatically = true;

	public bool updateAutomatically
	{
		get
		{
			return _updateAutomatically;
		}
		set
		{
			_updateAutomatically = value;
			if (value)
			{
				OnCoinsChanged();
			}
		}
	}

	private void Awake()
	{
		cachedAmountLabel = AmountLabel.GetComponent<UILabel>();
		PlayerInfo instance = PlayerInfo.Instance;
		instance.onCoinsChanged = (Action)Delegate.Combine(instance.onCoinsChanged, new Action(OnCoinsChanged));
		OnCoinsChanged();
	}

	private void OnDestroy()
	{
		PlayerInfo instance = PlayerInfo.Instance;
		instance.onCoinsChanged = (Action)Delegate.Remove(instance.onCoinsChanged, new Action(OnCoinsChanged));
	}

	private void Start()
	{
		_AdjustSize();
	}

	private void OnCoinsChanged()
	{
		if (updateAutomatically)
		{
			cachedAmountLabel.text = string.Empty + PlayerInfo.Instance.amountOfCoins;
			_AdjustSize();
		}
	}

	private void _AdjustSize()
	{
		if (AddFundsIcon != null)
		{
			float num = cachedAmountLabel.relativeSize.x;
			if (num < 1f)
			{
				num = 1f;
			}
			if (num != cachedWidth)
			{
				GrayFG.transform.localScale = new Vector3(num * 17f + 20f, GrayFG.transform.localScale.y, GrayFG.transform.localScale.z);
				CoinIcon.transform.localPosition = new Vector3(-1f * (num * 17f + 3f), CoinIcon.transform.localPosition.y, CoinIcon.transform.localPosition.z);
				AddFundsIcon.transform.localScale = new Vector3(GrayFG.transform.localScale.x + 16f, GrayFG.transform.localScale.y, GrayFG.transform.localScale.z);
				if (Shadow != null)
				{
					Shadow.transform.localScale = AddFundsIcon.transform.localScale;
				}
				cachedWidth = num;
			}
			return;
		}
		float num2 = cachedAmountLabel.relativeSize.x;
		if (num2 < 1f)
		{
			num2 = 1f;
		}
		if (num2 != cachedWidth)
		{
			GrayFG.transform.localScale = new Vector3(num2 * 17f + 20f, GrayFG.transform.localScale.y, GrayFG.transform.localScale.z);
			CoinIcon.transform.localPosition = new Vector3(-1f * (num2 * 17f + 3f), CoinIcon.transform.localPosition.y, CoinIcon.transform.localPosition.z);
			if (Shadow != null)
			{
				Shadow.transform.localScale = GrayFG.transform.localScale;
			}
			cachedWidth = num2;
		}
	}
}
