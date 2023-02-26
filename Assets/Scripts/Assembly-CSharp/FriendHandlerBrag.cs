using System.Collections.Generic;
using UnityEngine;

public class FriendHandlerBrag : MonoBehaviour
{
	public GameObject friendBragPrefab;

	public UILabel gettingLabel;

	public BragButtonHelper bragButton;

	private FriendHelperBrag playerHelper;

	private Color myColor = new Color(1f / 15f, 0.39607844f, 0.6156863f, 1f);

	private UIGrid _grid;

	private List<Friend> _bragList = new List<Friend>();

	private Vector4 defaultPanelClipping = new Vector4(0f, 152f, 295.5f, 121f);

	[HideInInspector]
	public bool bragNotifyDone;

	[HideInInspector]
	public bool bragFacebookDone;

	public List<Friend> bragList
	{
		get
		{
			if (_bragList != null)
			{
				return _bragList;
			}
			return null;
		}
	}

	private void Awake()
	{
		_grid = GetComponent<UIGrid>();
		NGUITools.SetActive(gettingLabel.gameObject, false);
	}

	public void ShowGettingReadyLabel()
	{
		NGUITools.SetActive(gettingLabel.gameObject, true);
		foreach (Transform item in base.transform)
		{
			NGUITools.SetActive(item.gameObject, false);
			Object.Destroy(item.gameObject);
		}
	}

	public void ShowBragList()
	{
		Transform parent = base.transform.parent;
		if (_grid == null)
		{
			_grid = GetComponent<UIGrid>();
		}
		foreach (Transform item in _grid.transform)
		{
			NGUITools.SetActive(item.gameObject, false);
			Object.Destroy(item.gameObject);
		}
		NGUITools.SetActive(gettingLabel.gameObject, false);
		Friend[] array = SocialManager.instance.FriendsSortedByScore();
		bool flag = false;
		int num = 1;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = NGUITools.AddChild(base.gameObject, friendBragPrefab);
			gameObject.name = string.Format("{0:000000}{1}", array[i].score, num);
			FriendHelperBrag component = gameObject.GetComponent<FriendHelperBrag>();
			if (!flag && PlayerInfo.Instance.highestScore >= array[i].score)
			{
				parent = gameObject.transform;
				component.InitLocalUser(num, num % 2 == 0);
				num++;
				flag = true;
				playerHelper = component;
				gameObject = NGUITools.AddChild(base.gameObject, friendBragPrefab);
				gameObject.name = string.Format("{0:000000}", array[i].score);
				component = gameObject.GetComponent<FriendHelperBrag>();
			}
			bool flag2 = array[i].score <= PlayerInfo.Instance.highestScore && array[i].score > PlayerInfo.Instance.oldHighestScore;
			component.InitFriend(array[i], num, flag2, num % 2 == 0);
			if (flag2)
			{
				AddBragFriend(array[i]);
			}
			num++;
		}
		if (!flag)
		{
			GameObject gameObject2 = NGUITools.AddChild(base.gameObject, friendBragPrefab);
			FriendHelperBrag friendHelperBrag = (playerHelper = gameObject2.GetComponent<FriendHelperBrag>());
			parent = gameObject2.transform;
			friendHelperBrag.InitLocalUser(num, num % 2 == 0);
			num++;
			flag = true;
		}
		if (bragList.Count == 0)
		{
			bragButton.DisableButton();
		}
		else
		{
			bragButton.EnableButton();
			playerHelper.SetRankMovement(true);
		}
		UIPanel component2 = _grid.transform.parent.GetComponent<UIPanel>();
		Vector3 zero = Vector3.zero;
		component2.transform.localPosition = zero;
		Vector3 vector = zero;
		component2.clipRange = defaultPanelClipping;
		_grid.sorted = false;
		_grid.repositionNow = true;
		_grid.Reposition();
		component2.transform.localPosition = new Vector3(vector.x, 0f - parent.localPosition.y, vector.z);
		component2.clipRange = new Vector4(defaultPanelClipping.x, defaultPanelClipping.y + parent.localPosition.y, defaultPanelClipping.z, defaultPanelClipping.w);
		component2.GetComponent<UIDraggablePanel>().RestrictWithinBounds(true);
		base.gameObject.BroadcastMessage("CreatePanel", SendMessageOptions.DontRequireReceiver);
		if (Settings.optionAutoMessage)
		{
			SocialManager.instance.BragNotify(PlayerInfo.Instance.oldHighestScore, bragList);
			bragNotifyDone = true;
		}
	}

	public void AddBragFriend(Friend friend)
	{
		if (!_bragList.Contains(friend))
		{
			_bragList.Add(friend);
			if (!bragButton.buttonEnabled)
			{
				bragButton.EnableButton();
			}
		}
	}

	public void RemoveBragFriend(Friend friend)
	{
		if (_bragList.Contains(friend))
		{
			_bragList.Remove(friend);
			if (_bragList.Count == 0)
			{
				bragButton.DisableButton();
			}
		}
	}

	public void CompletedBrag()
	{
		bragButton.DisableButton();
		base.gameObject.BroadcastMessage("CompletedBragging", SendMessageOptions.DontRequireReceiver);
		_bragList.Clear();
	}
}
