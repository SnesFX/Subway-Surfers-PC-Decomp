using UnityEngine;

public class FacebookLoginButton : MonoBehaviour
{
	private void OnClick()
	{
		Debug.Log("Facebook login clicked");
		SocialManager.instance.FacebookLogin(UIScreenController.Instance.FacebookLogIn);
	}
}
