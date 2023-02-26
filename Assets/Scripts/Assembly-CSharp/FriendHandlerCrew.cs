using UnityEngine;

public class FriendHandlerCrew : MonoBehaviour
{
	public GameObject FriendPrefab;

	public GameObject InvitePrefab;

	public UILabel CrewHeader;

	public UILabel NoFriends;

	private UIGrid _grid;

	private void Awake()
	{
		_grid = GetComponent<UIGrid>();
	}

	public void InitCrew()
	{
		if (_grid == null)
		{
			_grid = GetComponent<UIGrid>();
		}
		foreach (Transform item in base.transform)
		{
			NGUITools.SetActive(item.gameObject, false);
			Object.Destroy(item.gameObject);
		}
		Friend[] array = SocialManager.instance.FriendsSortedByCash();
		Debug.Log("number of friends: " + array.Length);
		int num = -1;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = NGUITools.AddChild(base.gameObject, FriendPrefab);
			gameObject.name = string.Format("{0:000000}", i);
			FriendHelperCrew component = gameObject.GetComponent<FriendHelperCrew>();
			component.InitFriend(array[i], i % 2 == 0);
			num = i;
		}
		if (SocialManager.instance.facebookIsLoggedIn)
		{
			GameObject gameObject2 = NGUITools.AddChild(base.gameObject, InvitePrefab);
			gameObject2.name = "invite";
		}
		if (num == -1 && !SocialManager.instance.facebookIsLoggedIn)
		{
			NoFriends.alpha = 1f;
			NoFriends.gameObject.active = true;
		}
		else
		{
			NoFriends.alpha = 0f;
			NoFriends.gameObject.active = false;
		}
		CrewHeader.text = "Friends (" + (num + 1) + ")";
		_grid.sorted = false;
		_grid.repositionNow = true;
		_grid.Reposition();
		base.gameObject.BroadcastMessage("CreatePanel", SendMessageOptions.DontRequireReceiver);
	}
}
