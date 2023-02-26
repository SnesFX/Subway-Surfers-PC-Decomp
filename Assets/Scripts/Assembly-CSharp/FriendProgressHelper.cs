using UnityEngine;

public class FriendProgressHelper : MonoBehaviour
{
	public UILabel label;

	public UISlider slider;

	public UISprite coinPouch;

	public Vector3 GetCoinPouchGlobalPosition()
	{
		return coinPouch.transform.position;
	}
}
