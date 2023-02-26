using UnityEngine;

public class FacebookInviteButton : MonoBehaviour
{
	private void OnClick()
	{
		if (SocialManager.instance.facebookIsLoggedIn)
		{
			SocialManager.instance.RecommendAppFacebook();
		}
		else
		{
			SocialManager.instance.FacebookLogin(UIScreenController.Instance.FacebookLogIn);
		}
	}
}
