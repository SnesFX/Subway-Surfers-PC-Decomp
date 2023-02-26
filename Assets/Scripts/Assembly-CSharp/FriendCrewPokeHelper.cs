using UnityEngine;

public class FriendCrewPokeHelper : MonoBehaviour
{
	public UISprite zzzIcon;

	public UISprite pokeIcon;

	private Friend _friend;

	public void ActivatePoke(Friend friend)
	{
		_friend = friend;
		NGUITools.AddWidgetCollider(base.gameObject);
	}

	public void DeactivatePoke()
	{
		Object.Destroy(base.gameObject.GetComponent<Collider>());
		NGUITools.SetActive(zzzIcon.gameObject, false);
		NGUITools.SetActive(pokeIcon.gameObject, false);
	}

	private void OnClick()
	{
		DeactivatePoke();
		SocialManager.instance.Poke(_friend);
	}
}
