using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeScreenSetup : MonoBehaviour
{
	public GameObject ConsumablePrefab;

	public GameObject PermanentPrefab;

	public UIFont headerFont;

	private UITable _table;

	private UIDraggablePanel _parentDragPanel;

	private GameObject[] skipMissions = new GameObject[3];

	private string[] skipMissionNames = new string[3];

	public List<UpgradeHelper> cachedUpgradeHelpers = new List<UpgradeHelper>(11);

	private void Awake()
	{
		_table = GetComponent<UITable>();
		_parentDragPanel = NGUITools.FindInParents<UIDraggablePanel>(base.transform.parent.gameObject);
		Missions instance = Missions.Instance;
		instance.onMissionComplete = (Missions.MissionCompleteHandler)Delegate.Combine(instance.onMissionComplete, new Missions.MissionCompleteHandler(OnMissionComplete));
		Missions instance2 = Missions.Instance;
		instance2.onMissionSetComplete = (Missions.MissionSetCompleteHandler)Delegate.Combine(instance2.onMissionSetComplete, new Missions.MissionSetCompleteHandler(OnMissionSetComplete));
	}

	private void OnDestroy()
	{
		Missions instance = Missions.Instance;
		instance.onMissionComplete = (Missions.MissionCompleteHandler)Delegate.Remove(instance.onMissionComplete, new Missions.MissionCompleteHandler(OnMissionComplete));
		Missions instance2 = Missions.Instance;
		instance2.onMissionSetComplete = (Missions.MissionSetCompleteHandler)Delegate.Remove(instance2.onMissionSetComplete, new Missions.MissionSetCompleteHandler(OnMissionSetComplete));
	}

	private void OnEnable()
	{
		_table.repositionNow = true;
	}

	private void Start()
	{
		FillTable();
		if (!AdColony.isInitialized)
		{
			AdColony.Init("app2568a30bc18f470288d36d", "vz714b7567808540889e4a44");
		}
	}

	private void FillTable()
	{
		int num = 0;
		UILabel uILabel = NGUITools.AddWidget<UILabel>(base.gameObject);
		uILabel.font = headerFont;
		uILabel.text = "Single Use";
		uILabel.color = new Color(0f, 0.2901961f, 0.5019608f, 1f);
		uILabel.name = string.Format("{0:000}", num);
		uILabel.supportEncoding = false;
		uILabel.multiLine = false;
		uILabel.MakePixelPerfect();
		if (DeviceInfo.isHighres)
		{
			uILabel.gameObject.transform.localScale = new Vector3(uILabel.gameObject.transform.localScale.x / 2f, uILabel.gameObject.transform.localScale.y / 2f, uILabel.gameObject.transform.localScale.z);
		}
		num++;
		GameObject gameObject = NGUITools.AddChild(base.gameObject, ConsumablePrefab);
		gameObject.name = string.Format("{0:000}", num);
		gameObject.GetComponent<UpgradeHelper>().InitSingle(PowerupType.hoverboard);
		gameObject.GetComponent<UIDragPanelContents>().draggablePanel = _parentDragPanel;
		NGUITools.AddWidgetCollider(gameObject);
		cachedUpgradeHelpers.Add(gameObject.GetComponent<UpgradeHelper>());
		num++;
		gameObject = NGUITools.AddChild(base.gameObject, ConsumablePrefab);
		gameObject.name = string.Format("{0:000}", num);
		gameObject.GetComponent<UpgradeHelper>().InitSingle(PowerupType.mysterybox);
		gameObject.GetComponent<UIDragPanelContents>().draggablePanel = _parentDragPanel;
		NGUITools.AddWidgetCollider(gameObject);
		cachedUpgradeHelpers.Add(gameObject.GetComponent<UpgradeHelper>());
		num++;
		gameObject = NGUITools.AddChild(base.gameObject, ConsumablePrefab);
		gameObject.name = string.Format("{0:000}", num);
		gameObject.GetComponent<UpgradeHelper>().InitSingle(PowerupType.headstart500);
		gameObject.GetComponent<UIDragPanelContents>().draggablePanel = _parentDragPanel;
		NGUITools.AddWidgetCollider(gameObject);
		cachedUpgradeHelpers.Add(gameObject.GetComponent<UpgradeHelper>());
		num++;
		gameObject = NGUITools.AddChild(base.gameObject, ConsumablePrefab);
		gameObject.name = string.Format("{0:000}", num);
		gameObject.GetComponent<UpgradeHelper>().InitSingle(PowerupType.headstart2000);
		gameObject.GetComponent<UIDragPanelContents>().draggablePanel = _parentDragPanel;
		NGUITools.AddWidgetCollider(gameObject);
		cachedUpgradeHelpers.Add(gameObject.GetComponent<UpgradeHelper>());
		num++;
		if (Missions.Instance.HasMoreMissions())
		{
			string text = string.Format("{0:000}", num);
			skipMissionNames[0] = text;
			if (!Missions.Instance.GetMissionInfo(0).complete)
			{
				gameObject = NGUITools.AddChild(base.gameObject, ConsumablePrefab);
				gameObject.name = string.Format("{0:000}", num);
				gameObject.GetComponent<UpgradeHelper>().InitSingle(PowerupType.skipmission1);
				gameObject.GetComponent<UIDragPanelContents>().draggablePanel = _parentDragPanel;
				NGUITools.AddWidgetCollider(gameObject);
				skipMissions[0] = gameObject;
				cachedUpgradeHelpers.Add(gameObject.GetComponent<UpgradeHelper>());
			}
			num++;
			text = string.Format("{0:000}", num);
			skipMissionNames[1] = text;
			if (!Missions.Instance.GetMissionInfo(1).complete)
			{
				gameObject = NGUITools.AddChild(base.gameObject, ConsumablePrefab);
				gameObject.name = string.Format("{0:000}", num);
				gameObject.GetComponent<UpgradeHelper>().InitSingle(PowerupType.skipmission2);
				gameObject.GetComponent<UIDragPanelContents>().draggablePanel = _parentDragPanel;
				NGUITools.AddWidgetCollider(gameObject);
				skipMissions[1] = gameObject;
				cachedUpgradeHelpers.Add(gameObject.GetComponent<UpgradeHelper>());
			}
			num++;
			text = string.Format("{0:000}", num);
			skipMissionNames[2] = text;
			if (!Missions.Instance.GetMissionInfo(2).complete)
			{
				gameObject = NGUITools.AddChild(base.gameObject, ConsumablePrefab);
				gameObject.name = string.Format("{0:000}", num);
				gameObject.GetComponent<UpgradeHelper>().InitSingle(PowerupType.skipmission3);
				gameObject.GetComponent<UIDragPanelContents>().draggablePanel = _parentDragPanel;
				NGUITools.AddWidgetCollider(gameObject);
				skipMissions[2] = gameObject;
				cachedUpgradeHelpers.Add(gameObject.GetComponent<UpgradeHelper>());
			}
			num++;
		}
		UILabel uILabel2 = NGUITools.AddWidget<UILabel>(base.gameObject);
		uILabel2.font = headerFont;
		uILabel2.text = "Upgrades";
		uILabel2.color = new Color(0f, 0.2901961f, 0.5019608f, 1f);
		uILabel2.name = string.Format("{0:000}", num);
		uILabel2.supportEncoding = false;
		uILabel2.multiLine = false;
		uILabel2.MakePixelPerfect();
		if (DeviceInfo.isHighres)
		{
			uILabel2.gameObject.transform.localScale = new Vector3(uILabel2.gameObject.transform.localScale.x / 2f, uILabel2.gameObject.transform.localScale.y / 2f, uILabel2.gameObject.transform.localScale.z);
		}
		num++;
		gameObject = NGUITools.AddChild(base.gameObject, PermanentPrefab);
		gameObject.name = string.Format("{0:000}", num);
		gameObject.GetComponent<UpgradeHelper>().InitPermanent(PowerupType.jetpack);
		gameObject.GetComponent<UIDragPanelContents>().draggablePanel = _parentDragPanel;
		NGUITools.AddWidgetCollider(gameObject);
		cachedUpgradeHelpers.Add(gameObject.GetComponent<UpgradeHelper>());
		num++;
		gameObject = NGUITools.AddChild(base.gameObject, PermanentPrefab);
		gameObject.name = string.Format("{0:000}", num);
		gameObject.GetComponent<UpgradeHelper>().InitPermanent(PowerupType.supersneakers);
		gameObject.GetComponent<UIDragPanelContents>().draggablePanel = _parentDragPanel;
		NGUITools.AddWidgetCollider(gameObject);
		cachedUpgradeHelpers.Add(gameObject.GetComponent<UpgradeHelper>());
		num++;
		gameObject = NGUITools.AddChild(base.gameObject, PermanentPrefab);
		gameObject.name = string.Format("{0:000}", num);
		gameObject.GetComponent<UpgradeHelper>().InitPermanent(PowerupType.coinmagnet);
		gameObject.GetComponent<UIDragPanelContents>().draggablePanel = _parentDragPanel;
		NGUITools.AddWidgetCollider(gameObject);
		cachedUpgradeHelpers.Add(gameObject.GetComponent<UpgradeHelper>());
		num++;
		gameObject = NGUITools.AddChild(base.gameObject, PermanentPrefab);
		gameObject.name = string.Format("{0:000}", num);
		gameObject.GetComponent<UpgradeHelper>().InitPermanent(PowerupType.doubleMultiplier);
		gameObject.GetComponent<UIDragPanelContents>().draggablePanel = _parentDragPanel;
		NGUITools.AddWidgetCollider(gameObject);
		cachedUpgradeHelpers.Add(gameObject.GetComponent<UpgradeHelper>());
		num++;
		base.gameObject.BroadcastMessage("CreatePanel", SendMessageOptions.DontRequireReceiver);
		_table.gameObject.transform.localPosition = new Vector3(_table.gameObject.transform.localPosition.x, 0f, _table.gameObject.transform.localPosition.z);
		_table.sorted = true;
		_table.repositionNow = true;
		_table.Reposition();
		_parentDragPanel.RestrictWithinBounds(true);
		_parentDragPanel.ResetPosition();
	}

	private void OnMissionComplete(string payload)
	{
		for (int i = 0; i < skipMissions.Length; i++)
		{
			if (skipMissions[i] != null && Missions.Instance.GetMissionInfo(i).complete)
			{
				cachedUpgradeHelpers.Remove(skipMissions[i].GetComponent<UpgradeHelper>());
				NGUITools.SetActive(skipMissions[i], false);
				UnityEngine.Object.Destroy(skipMissions[i]);
			}
		}
		_table.repositionNow = true;
	}

	private void OnMissionSetComplete()
	{
		for (int i = 0; i < skipMissions.Length; i++)
		{
			if (skipMissions[i] != null)
			{
				cachedUpgradeHelpers.Remove(skipMissions[i].GetComponent<UpgradeHelper>());
				UnityEngine.Object.Destroy(skipMissions[i]);
			}
		}
		if (Missions.Instance.HasMoreMissions())
		{
			bool state = base.gameObject.active;
			if (!Missions.Instance.GetMissionInfo(0).complete)
			{
				GameObject gameObject = NGUITools.AddChild(base.gameObject, ConsumablePrefab);
				gameObject.name = skipMissionNames[0];
				gameObject.GetComponent<UpgradeHelper>().InitSingle(PowerupType.skipmission1);
				gameObject.GetComponent<UIDragPanelContents>().draggablePanel = _parentDragPanel;
				NGUITools.AddWidgetCollider(gameObject);
				skipMissions[0] = gameObject;
				NGUITools.SetActive(gameObject, state);
				cachedUpgradeHelpers.Add(gameObject.GetComponent<UpgradeHelper>());
			}
			if (!Missions.Instance.GetMissionInfo(1).complete)
			{
				GameObject gameObject2 = NGUITools.AddChild(base.gameObject, ConsumablePrefab);
				gameObject2.name = skipMissionNames[1];
				gameObject2.GetComponent<UpgradeHelper>().InitSingle(PowerupType.skipmission2);
				gameObject2.GetComponent<UIDragPanelContents>().draggablePanel = _parentDragPanel;
				NGUITools.AddWidgetCollider(gameObject2);
				skipMissions[1] = gameObject2;
				NGUITools.SetActive(gameObject2, state);
				cachedUpgradeHelpers.Add(gameObject2.GetComponent<UpgradeHelper>());
			}
			if (!Missions.Instance.GetMissionInfo(2).complete)
			{
				GameObject gameObject3 = NGUITools.AddChild(base.gameObject, ConsumablePrefab);
				gameObject3.name = skipMissionNames[2];
				gameObject3.GetComponent<UpgradeHelper>().InitSingle(PowerupType.skipmission3);
				gameObject3.GetComponent<UIDragPanelContents>().draggablePanel = _parentDragPanel;
				NGUITools.AddWidgetCollider(gameObject3);
				skipMissions[2] = gameObject3;
				NGUITools.SetActive(gameObject3, state);
				cachedUpgradeHelpers.Add(gameObject3.GetComponent<UpgradeHelper>());
			}
		}
		if (base.gameObject.active)
		{
			_table.repositionNow = true;
		}
	}

	private IEnumerator SetStatic()
	{
		yield return null;
		base.transform.parent.GetComponent<UIPanel>().widgetsAreStatic = true;
	}
}
