using UnityEngine;

public class UISocialScreen : MonoBehaviour
{
	public FriendHandlerHighScore _highScoreHandler;

	public FriendHandlerCrew _crewHandler;

	public GameObject FacebookLoginPrefab;

	private GameObject _FacebookLoginButton;

	public void ReloadFriends()
	{
		if (_highScoreHandler != null)
		{
			_highScoreHandler.LoadHighScore();
		}
		else
		{
			if (!(_crewHandler != null))
			{
				return;
			}
			_crewHandler.InitCrew();
			if (SocialManager.instance.facebookIsLoggedIn)
			{
				if (_FacebookLoginButton != null)
				{
					NGUITools.SetActive(_FacebookLoginButton, false);
					Object.Destroy(_FacebookLoginButton);
				}
			}
			else if (_FacebookLoginButton == null)
			{
				_FacebookLoginButton = NGUITools.AddChild(base.gameObject, FacebookLoginPrefab);
			}
		}
	}
}
