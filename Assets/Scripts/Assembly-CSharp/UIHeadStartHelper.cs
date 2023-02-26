using System;
using UnityEngine;

public class UIHeadStartHelper : MonoBehaviour
{
	public GameObject headStart1;

	public GameObject headStart2;

	public UILabel hs1AmountLabel;

	public UILabel hs2AmountLabel;

	private SpringPosition hs1Spring;

	private SpringPosition hs2Spring;

	private Vector3 hs1PositionOff = new Vector3(-100f, 160f, 0f);

	private Vector3 hs2PositionOff = new Vector3(-100f, 60f, 0f);

	private Vector3 hs1PositionOn = new Vector3(50f, 160f, 0f);

	private Vector3 hs2PositionOn = new Vector3(50f, 60f, 0f);

	private bool hasInited;

	private void Start()
	{
		if (!hasInited)
		{
			InitHelper();
		}
	}

	private void InitHelper()
	{
		headStart1.GetComponent<Collider>().enabled = false;
		headStart2.GetComponent<Collider>().enabled = false;
		headStart1.transform.localPosition = hs1PositionOff;
		headStart2.transform.localPosition = hs2PositionOff;
		PlayerInfo instance = PlayerInfo.Instance;
		instance.onPowerupAmountChanged = (Action)Delegate.Combine(instance.onPowerupAmountChanged, new Action(UpdateHeadstartLabels));
		UpdateHeadstartLabels();
		hasInited = true;
	}

	private void OnDestroy()
	{
		PlayerInfo instance = PlayerInfo.Instance;
		instance.onPowerupAmountChanged = (Action)Delegate.Remove(instance.onPowerupAmountChanged, new Action(UpdateHeadstartLabels));
	}

	private void UpdateHeadstartLabels()
	{
		hs1AmountLabel.text = string.Empty + PlayerInfo.Instance.GetUpgradeAmount(PowerupType.headstart500);
		hs2AmountLabel.text = string.Empty + PlayerInfo.Instance.GetUpgradeAmount(PowerupType.headstart2000);
	}

	public void ShowHeadStart()
	{
		if (!hasInited)
		{
			InitHelper();
		}
		if (PlayerInfo.Instance.GetUpgradeAmount(PowerupType.headstart500) > 0)
		{
			SpringPosition.Begin(headStart1, hs1PositionOn, 10f);
			headStart1.GetComponent<Collider>().enabled = true;
		}
		if (PlayerInfo.Instance.GetUpgradeAmount(PowerupType.headstart2000) > 0)
		{
			SpringPosition.Begin(headStart2, hs2PositionOn, 10f);
			headStart2.GetComponent<Collider>().enabled = true;
		}
		Invoke("HideHeadStart", 5f);
	}

	public void HideHeadStart()
	{
		HideHeadStart(false);
	}

	public void HideHeadStart(bool instant)
	{
		if (!hasInited)
		{
			InitHelper();
		}
		if (instant)
		{
			headStart1.transform.position = hs1PositionOff;
			headStart2.transform.position = hs2PositionOff;
		}
		else
		{
			SpringPosition.Begin(headStart1, hs1PositionOff, 10f);
			SpringPosition.Begin(headStart2, hs2PositionOff, 10f);
		}
		headStart1.GetComponent<Collider>().enabled = false;
		headStart2.GetComponent<Collider>().enabled = false;
	}
}
