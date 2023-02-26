using System;
using System.Collections;
using UnityEngine;

public class SocialNetworkingManager : MonoBehaviour
{
	public static event Action twitterLogin;

	public static event Action<string> twitterLoginFailed;

	public static event Action twitterPost;

	public static event Action<string> twitterPostFailed;

	public static event Action<ArrayList> twitterHomeTimelineReceived;

	public static event Action<string> twitterHomeTimelineFailed;

	public static event Action<object> twitterRequestDidFinishEvent;

	public static event Action<string> twitterRequestDidFailEvent;

	public static event Action facebookLogin;

	public static event Action<string> facebookLoginFailed;

	public static event Action facebookDidLogoutEvent;

	public static event Action<DateTime> facebookDidExtendTokenEvent;

	public static event Action facebookSessionInvalidatedEvent;

	public static event Action<string> facebookReceivedUsername;

	public static event Action<string> facebookUsernameRequestFailed;

	public static event Action facebookPost;

	public static event Action<string> facebookPostFailed;

	public static event Action<ArrayList> facebookReceivedFriends;

	public static event Action<string> facebookFriendRequestFailed;

	public static event Action facebookDialogCompleted;

	public static event Action<string> facebookDialogFailed;

	public static event Action facebookDialogDidntComplete;

	public static event Action<string> facebookDialogCompletedWithUrl;

	public static event Action<object> facebookReceivedCustomRequest;

	public static event Action<string> facebookCustomRequestFailed;

	private void Awake()
	{
		base.gameObject.name = GetType().ToString();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public void twitterLoginSucceeded(string empty)
	{
		if (SocialNetworkingManager.twitterLogin != null)
		{
			SocialNetworkingManager.twitterLogin();
		}
	}

	public void twitterLoginDidFail(string error)
	{
		if (SocialNetworkingManager.twitterLoginFailed != null)
		{
			SocialNetworkingManager.twitterLoginFailed(error);
		}
	}

	public void twitterPostSucceeded(string empty)
	{
		if (SocialNetworkingManager.twitterPost != null)
		{
			SocialNetworkingManager.twitterPost();
		}
	}

	public void twitterPostDidFail(string error)
	{
		if (SocialNetworkingManager.twitterPostFailed != null)
		{
			SocialNetworkingManager.twitterPostFailed(error);
		}
	}

	public void twitterHomeTimelineDidFail(string error)
	{
		if (SocialNetworkingManager.twitterHomeTimelineFailed != null)
		{
			SocialNetworkingManager.twitterHomeTimelineFailed(error);
		}
	}

	public void twitterHomeTimelineDidFinish(string results)
	{
		if (SocialNetworkingManager.twitterHomeTimelineReceived != null)
		{
			ArrayList obj = (ArrayList)MiniJSON.jsonDecode(results);
			SocialNetworkingManager.twitterHomeTimelineReceived(obj);
		}
	}

	public void twitterRequestDidFinish(string results)
	{
		if (SocialNetworkingManager.twitterRequestDidFinishEvent != null)
		{
			SocialNetworkingManager.twitterRequestDidFinishEvent(MiniJSON.jsonDecode(results));
		}
	}

	public void twitterRequestDidFail(string error)
	{
		if (SocialNetworkingManager.twitterRequestDidFailEvent != null)
		{
			SocialNetworkingManager.twitterRequestDidFailEvent(error);
		}
	}

	public void facebookLoginSucceeded(string empty)
	{
		if (SocialNetworkingManager.facebookLogin != null)
		{
			SocialNetworkingManager.facebookLogin();
		}
	}

	public void facebookLoginDidFail(string error)
	{
		if (SocialNetworkingManager.facebookLoginFailed != null)
		{
			SocialNetworkingManager.facebookLoginFailed(error);
		}
	}

	public void facebookDidLogout(string empty)
	{
		if (SocialNetworkingManager.facebookDidLogoutEvent != null)
		{
			SocialNetworkingManager.facebookDidLogoutEvent();
		}
	}

	public void facebookDidExtendToken(string secondsSinceEpoch)
	{
		if (SocialNetworkingManager.facebookDidExtendTokenEvent != null)
		{
			double value = double.Parse(secondsSinceEpoch);
			DateTime obj = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(value);
			SocialNetworkingManager.facebookDidExtendTokenEvent(obj);
		}
	}

	public void facebookSessionInvalidated(string empty)
	{
		if (SocialNetworkingManager.facebookSessionInvalidatedEvent != null)
		{
			SocialNetworkingManager.facebookSessionInvalidatedEvent();
		}
	}

	public void facebookDidReceiveUsername(string username)
	{
		if (SocialNetworkingManager.facebookReceivedUsername != null)
		{
			SocialNetworkingManager.facebookReceivedUsername(username);
		}
	}

	public void facebookUsernameRequestDidFail(string error)
	{
		if (SocialNetworkingManager.facebookUsernameRequestFailed != null)
		{
			SocialNetworkingManager.facebookUsernameRequestFailed(error);
		}
	}

	public void facebookPostSucceeded(string empty)
	{
		if (SocialNetworkingManager.facebookPost != null)
		{
			SocialNetworkingManager.facebookPost();
		}
	}

	public void facebookPostDidFail(string error)
	{
		if (SocialNetworkingManager.facebookPostFailed != null)
		{
			SocialNetworkingManager.facebookPostFailed(error);
		}
	}

	public void facebookDidReceiveFriends(string jsonResult)
	{
		if (SocialNetworkingManager.facebookReceivedFriends != null)
		{
			Hashtable hashtable = (Hashtable)MiniJSON.jsonDecode(jsonResult);
			if (hashtable.Contains("data"))
			{
				SocialNetworkingManager.facebookReceivedFriends((ArrayList)hashtable["data"]);
			}
			else
			{
				SocialNetworkingManager.facebookReceivedFriends(new ArrayList());
			}
		}
	}

	public void facebookFriendRequestDidFail(string error)
	{
		if (SocialNetworkingManager.facebookFriendRequestFailed != null)
		{
			SocialNetworkingManager.facebookFriendRequestFailed(error);
		}
	}

	public void facebookDialogDidComplete(string empty)
	{
		if (SocialNetworkingManager.facebookDialogCompleted != null)
		{
			SocialNetworkingManager.facebookDialogCompleted();
		}
	}

	public void facebookDialogDidCompleteWithUrl(string url)
	{
		if (SocialNetworkingManager.facebookDialogCompletedWithUrl != null)
		{
			SocialNetworkingManager.facebookDialogCompletedWithUrl(url);
		}
	}

	public void facebookDialogDidNotComplete(string empty)
	{
		if (SocialNetworkingManager.facebookDialogDidntComplete != null)
		{
			SocialNetworkingManager.facebookDialogDidntComplete();
		}
	}

	public void facebookDialogDidFailWithError(string error)
	{
		if (SocialNetworkingManager.facebookDialogFailed != null)
		{
			SocialNetworkingManager.facebookDialogFailed(error);
		}
	}

	public void facebookDidReceiveCustomRequest(string result)
	{
		if (SocialNetworkingManager.facebookReceivedCustomRequest != null)
		{
			object obj = MiniJSON.jsonDecode(result);
			SocialNetworkingManager.facebookReceivedCustomRequest(obj);
		}
	}

	public void facebookCustomRequestDidFail(string error)
	{
		if (SocialNetworkingManager.facebookCustomRequestFailed != null)
		{
			SocialNetworkingManager.facebookCustomRequestFailed(error);
		}
	}
}
