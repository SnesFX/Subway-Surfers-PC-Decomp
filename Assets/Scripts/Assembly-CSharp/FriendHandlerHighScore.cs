using System.Collections;
using UnityEngine;

public class FriendHandlerHighScore : MonoBehaviour
{
	public GameObject FriendPrefab;

	private Color myColor = new Color(1f / 15f, 0.39607844f, 0.6156863f, 1f);

	private UIGrid _grid;

	private Vector4 defaultPanelClipping = new Vector4(-5.5f, 240f, 277f, 300f);

	private void Awake()
	{
		_grid = GetComponent<UIGrid>();
	}

	public void LoadHighScore()
	{
		LoadFriends();
	}

	private void LoadFriends()
	{
		base.transform.parent.GetComponent<UIPanel>().widgetsAreStatic = false;
		if (_grid == null)
		{
			_grid = GetComponent<UIGrid>();
		}
		foreach (Transform item in base.transform)
		{
			NGUITools.SetActive(item.gameObject, false);
			Object.Destroy(item.gameObject);
		}
		Friend[] array = SocialManager.instance.FriendsSortedByScore();
		Transform transform2 = base.transform;
		bool flag = false;
		int num = 1;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = NGUITools.AddChild(base.gameObject, FriendPrefab);
			gameObject.name = string.Format("{0:000000}{1}", array[i].score, num);
			FriendHelperHighScore component = gameObject.GetComponent<FriendHelperHighScore>();
			if (!flag && PlayerInfo.Instance.highestScore >= array[i].score)
			{
				transform2 = gameObject.transform;
				component.InitLocalUser(num, num % 2 == 0);
				num++;
				flag = true;
				gameObject = null;
				component = null;
				gameObject = NGUITools.AddChild(base.gameObject, FriendPrefab);
				component = gameObject.GetComponent<FriendHelperHighScore>();
				gameObject.name = string.Format("{0:000000}{1}", array[i].score, num);
			}
			component.InitFriend(array[i], num, num % 2 == 0);
			num++;
		}
		if (!flag)
		{
			GameObject gameObject2 = NGUITools.AddChild(base.gameObject, FriendPrefab);
			FriendHelperHighScore component2 = gameObject2.GetComponent<FriendHelperHighScore>();
			transform2 = gameObject2.transform;
			component2.InitLocalUser(num, num % 2 == 0);
			num++;
			flag = true;
		}
		UIPanel component3 = _grid.transform.parent.GetComponent<UIPanel>();
		Vector3 localPosition = _grid.transform.parent.localPosition;
		component3.clipRange = defaultPanelClipping;
		_grid.sorted = false;
		_grid.repositionNow = true;
		_grid.Reposition();
		component3.transform.localPosition = new Vector3(localPosition.x, 0f - transform2.localPosition.y, localPosition.z);
		component3.clipRange = new Vector4(defaultPanelClipping.x, defaultPanelClipping.y + transform2.localPosition.y, defaultPanelClipping.z, defaultPanelClipping.w);
		component3.GetComponent<UIDraggablePanel>().RestrictWithinBounds(true);
		base.gameObject.BroadcastMessage("CreatePanel", SendMessageOptions.DontRequireReceiver);
		StartCoroutine(SetStatic());
	}

	private IEnumerator SetStatic()
	{
		yield return null;
		base.transform.parent.GetComponent<UIPanel>().widgetsAreStatic = true;
	}
}
