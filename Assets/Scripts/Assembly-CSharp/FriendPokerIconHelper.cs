using UnityEngine;

public class FriendPokerIconHelper : MonoBehaviour
{
	public string highResName;

	private void Start()
	{
		if (DeviceInfo.isHighres)
		{
			UISprite component = GetComponent<UISprite>();
			component.spriteName = highResName;
		}
	}
}
