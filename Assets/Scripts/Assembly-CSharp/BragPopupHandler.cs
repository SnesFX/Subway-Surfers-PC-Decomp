using UnityEngine;

public class BragPopupHandler : MonoBehaviour
{
	public BragSendHelper SendMessageButton;

	public BragSendHelper FacebookBragButton;

	private FriendHandlerBrag _bragHandler;

	private Color localPlayerColor = new Color(1f, 73f / 85f, 0f, 1f);

	public void SetupBragPopup()
	{
		if (_bragHandler == null)
		{
			_bragHandler = UIScreenController.Instance.overlayAnchor.GetComponentInChildren(typeof(FriendHandlerBrag)) as FriendHandlerBrag;
		}
		if (_bragHandler.bragNotifyDone)
		{
			if (SendMessageButton.gameObject.GetComponent<Collider>() != null)
			{
				Object.Destroy(SendMessageButton.gameObject.GetComponent<Collider>());
			}
			SendMessageButton.icon.alpha = 0.5f;
			SendMessageButton.GetComponent<UIButtonColor>().defaultColor = new Color(1f, 1f, 1f, 0.5f);
			SendMessageButton.line1.text = "Message auto-sent";
			SendMessageButton.line2.text = "to friends you passed";
			SendMessageButton.line3.text = "Disable in settings";
		}
		else
		{
			NGUITools.AddWidgetCollider(SendMessageButton.gameObject);
			SendMessageButton.GetComponent<UIButtonColor>().defaultColor = new Color(1f, 1f, 1f, 1f);
			SendMessageButton.icon.alpha = 1f;
			SendMessageButton.line1.text = "Send message";
			SendMessageButton.line2.text = "to selected friends (" + _bragHandler.bragList.Count + ")";
			SendMessageButton.line3.text = "Tap friend's name to deselect";
		}
		NGUITools.AddWidgetCollider(FacebookBragButton.gameObject);
		FacebookBragButton.icon.alpha = 1f;
		SetupFacebookButtonTexts();
	}

	private void SendMessageButtonClicked()
	{
		if (SocialManager.instance != null)
		{
			SocialManager.instance.BragNotify(PlayerInfo.Instance.oldHighestScore, _bragHandler.bragList);
		}
		if (SendMessageButton.gameObject.GetComponent<Collider>() != null)
		{
			Object.Destroy(SendMessageButton.gameObject.GetComponent<Collider>());
		}
		SendMessageButton.GetComponent<UIButtonColor>().defaultColor = new Color(1f, 1f, 1f, 0.5f);
		SendMessageButton.icon.alpha = 0.5f;
		SendMessageButton.line1.text = "Message sent";
		_bragHandler.bragNotifyDone = true;
		CheckIfCompleted();
	}

	private void FacebookBragButtonClicked()
	{
		if (SocialManager.instance.facebookIsLoggedIn)
		{
			if (SocialManager.instance != null)
			{
				SocialManager.instance.BragFacebook(_bragHandler.bragList);
			}
			if (FacebookBragButton.gameObject.GetComponent<Collider>() != null)
			{
				Object.Destroy(FacebookBragButton.gameObject.GetComponent<Collider>());
			}
			FacebookBragButton.icon.alpha = 0.5f;
			SendMessageButton.GetComponent<UIButtonColor>().defaultColor = new Color(1f, 1f, 1f, 0.5f);
			_bragHandler.bragFacebookDone = true;
		}
		else
		{
			SocialManager.instance.FacebookLogin(FacebookLoggedIn);
		}
		SetupFacebookButtonTexts();
		CheckIfCompleted();
	}

	private void CheckIfCompleted()
	{
		if (_bragHandler.bragFacebookDone && _bragHandler.bragNotifyDone)
		{
			UIScreenController.Instance.ClosePopup();
			_bragHandler.CompletedBrag();
		}
	}

	private void FacebookLoggedIn(bool loggedIn)
	{
		if (loggedIn)
		{
			SetupFacebookButtonTexts();
		}
	}

	private void SetupFacebookButtonTexts()
	{
		if (SocialManager.instance.facebookIsLoggedIn)
		{
			if (_bragHandler.bragFacebookDone)
			{
				FacebookBragButton.line1.text = "Posted to your wall";
				FacebookBragButton.line2.text = "told your friends about Subway Surfers!";
			}
			else
			{
				FacebookBragButton.line1.text = "Post to your wall";
				FacebookBragButton.line2.text = "tell your friends about Subway Surfers!";
			}
		}
		else
		{
			FacebookBragButton.line1.text = "Log in to Facebook";
			FacebookBragButton.line2.text = "to tell your friends about Subway Surfers!";
		}
	}
}
