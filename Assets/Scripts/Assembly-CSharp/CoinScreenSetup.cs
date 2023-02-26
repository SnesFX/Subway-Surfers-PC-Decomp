using System.Collections;
using UnityEngine;

public class CoinScreenSetup : MonoBehaviour
{
	public GameObject coinPrefab;

	public GameObject coinEarnerPrefab;

	public UIFont headerFont;

	private UITable _table;

	private UIDraggablePanel _parentDragPanel;

	private void Awake()
	{
		_table = GetComponent<UITable>();
		_parentDragPanel = NGUITools.FindInParents<UIDraggablePanel>(base.transform.parent.gameObject);
	}

	private void Start()
	{
		FillTable();
	}

	public void RefreshCurrencyEarners()
	{
		FillTable();
	}

	private void FillTable()
	{
		base.transform.parent.GetComponent<UIPanel>().widgetsAreStatic = false;
		foreach (Transform item in base.transform)
		{
			NGUITools.SetActive(item.gameObject, false);
			Object.Destroy(item.gameObject);
		}
		int num = 0;
		UILabel uILabel = NGUITools.AddWidget<UILabel>(base.gameObject);
		uILabel.font = headerFont;
		uILabel.text = "Coin Shop";
		uILabel.color = new Color(0f, 0.2901961f, 0.5019608f, 1f);
		uILabel.name = string.Format("{0:000}", num);
		uILabel.MakePixelPerfect();
		if (DeviceInfo.isHighres)
		{
			uILabel.gameObject.transform.localScale = new Vector3(uILabel.gameObject.transform.localScale.x / 2f, uILabel.gameObject.transform.localScale.y / 2f, uILabel.gameObject.transform.localScale.z);
		}
		num++;
		GameObject gameObject = NGUITools.AddChild(base.gameObject, coinPrefab);
		gameObject.name = string.Format("{0:000}", num);
		gameObject.GetComponent<CoinButtonHelper>().Init(InAppData.inAppTier1);
		gameObject.GetComponent<UIDragPanelContents>().draggablePanel = _parentDragPanel;
		NGUITools.AddWidgetCollider(gameObject);
		num++;
		gameObject = NGUITools.AddChild(base.gameObject, coinPrefab);
		gameObject.name = string.Format("{0:000}", num);
		gameObject.GetComponent<CoinButtonHelper>().Init(InAppData.inAppTier2);
		gameObject.GetComponent<UIDragPanelContents>().draggablePanel = _parentDragPanel;
		NGUITools.AddWidgetCollider(gameObject);
		num++;
		gameObject = NGUITools.AddChild(base.gameObject, coinPrefab);
		gameObject.name = string.Format("{0:000}", num);
		gameObject.GetComponent<CoinButtonHelper>().Init(InAppData.inAppTier3);
		gameObject.GetComponent<UIDragPanelContents>().draggablePanel = _parentDragPanel;
		NGUITools.AddWidgetCollider(gameObject);
		num++;
		UILabel uILabel2 = NGUITools.AddWidget<UILabel>(base.gameObject);
		uILabel2.font = headerFont;
		uILabel2.text = "Earn Coins";
		uILabel2.color = new Color(0f, 0.2901961f, 0.5019608f, 1f);
		uILabel2.name = string.Format("{0:000}", num);
		uILabel2.MakePixelPerfect();
		if (DeviceInfo.isHighres)
		{
			uILabel2.gameObject.transform.localScale = new Vector3(uILabel2.gameObject.transform.localScale.x / 2f, uILabel2.gameObject.transform.localScale.y / 2f, uILabel2.gameObject.transform.localScale.z);
		}
		num++;
		for (int i = 0; i < EarnCurrencyInfo.profiles.Length; i++)
		{
			if (EarnCurrencyInfo.ShouldShowInGUI(i))
			{
				EarnCurrencyInfo.EarnCurrencyProfile earnCurrencyProfile = EarnCurrencyInfo.profiles[i];
				gameObject = NGUITools.AddChild(base.gameObject, coinEarnerPrefab);
				gameObject.name = string.Format("{0:000}", num);
				string desc = string.Format(earnCurrencyProfile.desc, earnCurrencyProfile.amountOfCoins);
				gameObject.GetComponent<CoinEarnerButtonHelper>().Init(i, earnCurrencyProfile.title, desc, earnCurrencyProfile.iconName);
				gameObject.GetComponent<UIDragPanelContents>().draggablePanel = _parentDragPanel;
				NGUITools.AddWidgetCollider(gameObject);
				num++;
			}
		}
		base.gameObject.BroadcastMessage("CreatePanel", SendMessageOptions.DontRequireReceiver);
		_table.gameObject.transform.localPosition = new Vector3(_table.gameObject.transform.localPosition.x, 0f, _table.gameObject.transform.localPosition.z);
		_table.sorted = true;
		_table.repositionNow = true;
		_table.Reposition();
		_parentDragPanel.RestrictWithinBounds(true);
		StartCoroutine(SetStatic());
	}

	private IEnumerator SetStatic()
	{
		yield return null;
		base.transform.parent.GetComponent<UIPanel>().widgetsAreStatic = true;
	}
}
