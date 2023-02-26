using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class SocialManager : MonoBehaviour
{
	private enum FacebookCurrentRequest
	{
		None = 0,
		Error = 1,
		LoggingIn = 2,
		GettingUserInfo = 3,
		GettingFriends = 4
	}

	private enum TwitterCurrentRequest
	{
		None = 0,
		Error = 1,
		LoggingIn = 2
	}

	private enum WWWRequestResult
	{
		Success = 0,
		Error = 1
	}

	private delegate void WWWComplete(WWWRequestResult result, string output, object cookie);

	private const byte VERSION = 1;

	private const string FACEBOOK_APPID = "254616967963463";

	private const string TWITTER_CONSUMER_KEY = "VKV2NMbj7YIEGblD97ZFSw";

	private const string TWITTER_CONSUMER_SECRET = "z1Wy3GXYL4XS9z9a2YbE4KWF3T0ynAFBwwwxZSYDI";

	private const bool DEBUG_SET_DEBUG_POST_FIELD = false;

	private const string BASE_URL = "http://hoodrunner.kiloo.com";

	private const string REGISTER_DEVICE_URL = "/register.php";

	private const string REPORT_SCORE_URL = "/report.php";

	private const string CONSOLIDATE_FRIENDS_URL = "/friends.php";

	private const string UPDATE_FRIEND_SCORES_URL = "/scores.php";

	private const string POKE_URL = "/poke.php";

	private const string BRAG_URL = "/brag.php";

	private const string SECRET = "resxrctrv7tgv7gb8h9h9u0909kllfmolkjnhghgjjkhjghg";

	private static SocialManager _instance;

	private int _userid;

	private FacebookProfile _fbProfile;

	private List<Friend> _friends;

	private Dictionary<string, Hashtable> _fbFriends;

	private Dictionary<string, IUserProfile> _gcFriends;

	private bool _gameCenterAuthenticationComplete;

	private bool _gameCenterFriendListRequestComplete;

	private bool _fbReady;

	private bool _twitterReady;

	private FacebookCurrentRequest _fbCurrentRequest;

	private TwitterCurrentRequest _twitterCurrentRequest;

	private bool _doneLoggingIn;

	private bool _consolidatedFriendsCompleted;

	private Dictionary<string, Friend.Status> _friendStatus;

	private bool _dirty;

	private Dictionary<string, FacebookProfile> _fbProfiles;

	public static bool debugGUI;

	private Dictionary<string, FacebookProfile> _debugFacebookFriends;

	private Vector2 _debugGCScrollPosition = new Vector2(0f, 0f);

	private Vector2 _debugFBScrollPosition = new Vector2(0f, 0f);

	public FacebookProfile facebookProfile
	{
		get
		{
			return _fbProfile;
		}
	}

	public Texture2D localUserImage
	{
		get
		{
			if (facebookProfile != null)
			{
				return facebookProfile.image;
			}
			if (Social.localUser != null)
			{
				return Social.localUser.image;
			}
			Debug.LogError("Local user not initialized");
			return null;
		}
	}

	public string localUserName
	{
		get
		{
			if (facebookProfile != null)
			{
				return facebookProfile.name;
			}
			if (Social.localUser != null)
			{
				return Social.localUser.userName;
			}
			Debug.LogError("Local user not initialized");
			return null;
		}
	}

	public static SocialManager instance
	{
		get
		{
			if (_instance == null)
			{
				GameObject gameObject = new GameObject();
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				gameObject.AddComponent<SocialNetworkingManager>();
				_instance = gameObject.AddComponent<SocialManager>();
			}
			return _instance;
		}
	}

	public bool facebookIsLoggedIn
	{
		get
		{
			return FacebookBinding.isLoggedIn();
		}
	}

	public bool twitterIsLoggedIn
	{
		get
		{
			return TwitterBinding.isLoggedIn();
		}
	}

	public bool doneLoggingIn
	{
		get
		{
			return _doneLoggingIn;
		}
	}

	public bool consolidatedFriendsCompleted
	{
		get
		{
			return _consolidatedFriendsCompleted;
		}
	}

	public bool dirty
	{
		get
		{
			return _dirty;
		}
	}

	public Friend[] FriendsSortedByScore()
	{
		if (_friends != null)
		{
			Friend[] array = _friends.ToArray();
			Array.Sort(array, (Friend x, Friend y) => y.score - x.score);
			return array;
		}
		return new Friend[0];
	}

	public Friend[] FriendsSortedByCash()
	{
		if (_friends != null)
		{
			Debug.Log("Friends was NOT null");
			Friend[] array = _friends.ToArray();
			Array.Sort(array, (Friend x, Friend y) => y.gamesToCashIn - x.gamesToCashIn);
			return array;
		}
		Debug.Log("Friends was null");
		return new Friend[0];
	}

	private void InitPushNotifications()
	{
	}

	private void InitGameCenter()
	{
		_gameCenterAuthenticationComplete = false;
		Social.localUser.Authenticate(delegate(bool authenticated)
		{
			if (authenticated)
			{
				Social.localUser.LoadFriends(delegate(bool friendsLoaded)
				{
					if (friendsLoaded)
					{
						IUserProfile[] friends = Social.localUser.friends;
						_gcFriends = new Dictionary<string, IUserProfile>(friends.Length);
						IUserProfile[] array = friends;
						foreach (IUserProfile userProfile in array)
						{
							_gcFriends[userProfile.id] = userProfile;
						}
					}
					else
					{
						_gcFriends = null;
					}
					_gameCenterFriendListRequestComplete = true;
					Invalidate();
				});
				Flurry.LogGameCenterLogin();
			}
			else if (_friends != null)
			{
				_friends.RemoveAll((Friend item) => item.gcProfile != null && item.fbProfile == null);
				_friends.ForEach(delegate(Friend item)
				{
					item.gcProfile = null;
				});
			}
			_gameCenterAuthenticationComplete = true;
		});
	}

	public void FacebookLogin(Action<bool> onComplete)
	{
		StartCoroutine(FacebookLoginCoroutine(onComplete));
	}

	public void FacebookLogout()
	{
		FacebookBinding.logout();
	}

	private IEnumerator FacebookLoginCoroutine(Action<bool> onComplete)
	{
		if (!FacebookBinding.isLoggedIn())
		{
			_fbCurrentRequest = FacebookCurrentRequest.LoggingIn;
			FacebookBinding.login();
			while (_fbCurrentRequest != 0)
			{
				if (_fbCurrentRequest == FacebookCurrentRequest.Error)
				{
					if (onComplete != null)
					{
						onComplete(false);
					}
					yield break;
				}
				yield return null;
			}
		}
		_fbCurrentRequest = FacebookCurrentRequest.GettingUserInfo;
		Hashtable args2 = new Hashtable();
		args2["fields"] = "id,name,first_name";
		FacebookBinding.graphRequest("me", "GET", args2);
		while (_fbCurrentRequest != 0)
		{
			if (_fbCurrentRequest == FacebookCurrentRequest.Error)
			{
				if (onComplete != null)
				{
					onComplete(false);
				}
				yield break;
			}
			yield return null;
		}
		StartCoroutine(DownloadFacebookPicture(_fbProfile));
		_fbCurrentRequest = FacebookCurrentRequest.GettingFriends;
		Hashtable args = new Hashtable();
		args["fields"] = "id,name,first_name";
		FacebookBinding.graphRequest("me/friends", "GET", args);
		while (_fbCurrentRequest != 0)
		{
			if (_fbCurrentRequest == FacebookCurrentRequest.Error)
			{
				if (onComplete != null)
				{
					onComplete(false);
				}
				yield break;
			}
			yield return null;
		}
		if (debugGUI)
		{
			_debugFacebookFriends = new Dictionary<string, FacebookProfile>(_fbFriends.Count);
			foreach (Hashtable entry in _fbFriends.Values)
			{
				FacebookProfile p = new FacebookProfile
				{
					id = (string)entry["id"],
					name = (string)entry["first_name"],
					fullName = (string)entry["name"]
				};
				_debugFacebookFriends[p.id] = p;
			}
			StartCoroutine(DownloadFacebookPictures(_debugFacebookFriends));
		}
		_fbReady = true;
		Invalidate();
		if (onComplete != null)
		{
			onComplete(true);
		}
	}

	private void TwitterLogin(Action<bool> onComplete)
	{
		StartCoroutine(TwitterLoginCoroutine(onComplete));
	}

	public void TwitterLogout()
	{
		TwitterBinding.logout();
	}

	private IEnumerator TwitterLoginCoroutine(Action<bool> onComplete)
	{
		if (!TwitterBinding.isLoggedIn())
		{
			_twitterCurrentRequest = TwitterCurrentRequest.LoggingIn;
			TwitterBinding.showOauthLoginDialog();
			while (_twitterCurrentRequest != 0)
			{
				if (_twitterCurrentRequest == TwitterCurrentRequest.Error)
				{
					if (onComplete != null)
					{
						onComplete(false);
					}
					yield break;
				}
				yield return null;
			}
		}
		_twitterReady = true;
		if (onComplete != null)
		{
			onComplete(true);
		}
	}

	private void Invalidate()
	{
		if (_gameCenterAuthenticationComplete && (!Social.localUser.authenticated || _gameCenterFriendListRequestComplete) && (!facebookIsLoggedIn || _fbReady))
		{
			_doneLoggingIn = true;
			RegisterUser(delegate(bool success)
			{
				Debug.Log((!success) ? "Register user failed" : "Register user succeeded");
			});
			ConsolidateFriends(delegate(bool success)
			{
				Debug.Log((!success) ? "Consolidate friends failed" : "Consolidate friends succeeded");
				_consolidatedFriendsCompleted = true;
			});
		}
	}

	public void CollectFriendReward(Friend friend)
	{
		friend.status.gamesCashedIn = friend.games;
		_dirty = true;
	}

	public int CashIn(Friend friend, int max)
	{
		int num = friend.games - friend.status.gamesCashedIn;
		if (num > 0)
		{
			friend.status.gamesCashedIn = friend.games;
			_dirty = true;
			return Mathf.Max(num, max);
		}
		return 0;
	}

	public int CashInAll(int maxPerFriend)
	{
		int num = 0;
		foreach (Friend friend in _friends)
		{
			num += CashIn(friend, maxPerFriend);
		}
		return num;
	}

	public void WriteTo(Stream stream)
	{
		BinaryWriter binaryWriter = new BinaryWriter(stream);
		binaryWriter.Write((byte)1);
		if (_friendStatus != null)
		{
			binaryWriter.Write(_friendStatus.Count);
			{
				foreach (KeyValuePair<string, Friend.Status> item in _friendStatus)
				{
					binaryWriter.Write(item.Key);
					binaryWriter.Write(item.Value.gamesCashedIn);
					binaryWriter.Write(item.Value.lastPokeTime.ToBinary());
				}
				return;
			}
		}
		binaryWriter.Write(0);
	}

	public void ReadFrom(Stream stream)
	{
		BinaryReader binaryReader = new BinaryReader(stream);
		byte b = binaryReader.ReadByte();
		if (b == 1)
		{
			int num = binaryReader.ReadInt32();
			_friendStatus = new Dictionary<string, Friend.Status>(num);
			for (int i = 0; i < num; i++)
			{
				string text = binaryReader.ReadString();
				if (!string.IsNullOrEmpty(text))
				{
					Friend.Status status = new Friend.Status();
					status.gamesCashedIn = binaryReader.ReadInt32();
					status.lastPokeTime = DateTime.FromBinary(binaryReader.ReadInt64());
					_friendStatus[text] = status;
				}
			}
			return;
		}
		throw new IOException("Unsupported playerdata file version");
	}

	private static string GetSaveDataPath()
	{
		return Application.persistentDataPath + "/socialdata";
	}

	private static bool ArraysAreEqual<T>(T[] a, T[] b)
	{
		if (a == null && b == null)
		{
			return true;
		}
		if (a.Length != b.Length)
		{
			return false;
		}
		for (int i = 0; i < a.Length; i++)
		{
			if (!object.Equals(a[i], b[i]))
			{
				return false;
			}
		}
		return true;
	}

	public void Load()
	{
		try
		{
			string saveDataPath = GetSaveDataPath();
			byte[] buffer = FileUtil.Load(GetSaveDataPath(), "resxrctrv7tgv7gb8h9h9u0909kllfmolkjnhghgjjkhjghg");
			MemoryStream memoryStream = new MemoryStream(buffer);
			ReadFrom(memoryStream);
			memoryStream.Close();
			_dirty = false;
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Could not load data: " + ex.Message);
		}
	}

	public bool Save()
	{
		//Discarded unreachable code: IL_0045
		try
		{
			MemoryStream memoryStream = new MemoryStream(8192);
			WriteTo(memoryStream);
			byte[] buffer = memoryStream.GetBuffer();
			FileUtil.Save(GetSaveDataPath(), "resxrctrv7tgv7gb8h9h9u0909kllfmolkjnhghgjjkhjghg", buffer, 0, (int)memoryStream.Length);
			memoryStream.Close();
			_dirty = false;
			return true;
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Error saving social data: " + ex.GetType().Name + ": " + ex.Message + "\n" + ex.StackTrace);
		}
		return false;
	}

	private void Awake()
	{
		Load();
		FacebookBinding.init("254616967963463");
		InitPushNotifications();
		if (facebookIsLoggedIn)
		{
			FacebookLogin(null);
		}
		InitGameCenter();
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			Save();
			return;
		}
		if (facebookIsLoggedIn)
		{
			FacebookLogin(null);
		}
		InitGameCenter();
	}

	private void OnEnable()
	{
		SocialNetworkingManager.twitterLogin += twitterLogin;
		SocialNetworkingManager.twitterLoginFailed += twitterLoginFailed;
		SocialNetworkingManager.twitterPost += twitterPost;
		SocialNetworkingManager.twitterPostFailed += twitterPostFailed;
		SocialNetworkingManager.twitterHomeTimelineReceived += twitterHomeTimelineReceived;
		SocialNetworkingManager.twitterHomeTimelineFailed += twitterHomeTimelineFailed;
		SocialNetworkingManager.twitterRequestDidFinishEvent += twitterRequestDidFinishEvent;
		SocialNetworkingManager.twitterRequestDidFailEvent += twitterRequestDidFailEvent;
		SocialNetworkingManager.facebookLogin += facebookLogin;
		SocialNetworkingManager.facebookLoginFailed += facebookLoginFailed;
		SocialNetworkingManager.facebookDidLogoutEvent += facebookDidLogoutEvent;
		SocialNetworkingManager.facebookDidExtendTokenEvent += facebookDidExtendTokenEvent;
		SocialNetworkingManager.facebookSessionInvalidatedEvent += facebookSessionInvalidatedEvent;
		SocialNetworkingManager.facebookReceivedUsername += facebookReceivedUsername;
		SocialNetworkingManager.facebookUsernameRequestFailed += facebookUsernameRequestFailed;
		SocialNetworkingManager.facebookPost += facebookPost;
		SocialNetworkingManager.facebookPostFailed += facebookPostFailed;
		SocialNetworkingManager.facebookReceivedFriends += facebookReceivedFriends;
		SocialNetworkingManager.facebookFriendRequestFailed += facebookFriendRequestFailed;
		SocialNetworkingManager.facebookDialogCompleted += facebokDialogCompleted;
		SocialNetworkingManager.facebookDialogCompletedWithUrl += facebookDialogCompletedWithUrl;
		SocialNetworkingManager.facebookDialogDidntComplete += facebookDialogDidntComplete;
		SocialNetworkingManager.facebookDialogFailed += facebookDialogFailed;
		SocialNetworkingManager.facebookReceivedCustomRequest += facebookReceivedCustomRequest;
		SocialNetworkingManager.facebookCustomRequestFailed += facebookCustomRequestFailed;
	}

	private void OnDisable()
	{
		SocialNetworkingManager.twitterLogin -= twitterLogin;
		SocialNetworkingManager.twitterLoginFailed -= twitterLoginFailed;
		SocialNetworkingManager.twitterPost -= twitterPost;
		SocialNetworkingManager.twitterPostFailed -= twitterPostFailed;
		SocialNetworkingManager.twitterHomeTimelineReceived -= twitterHomeTimelineReceived;
		SocialNetworkingManager.twitterHomeTimelineFailed -= twitterHomeTimelineFailed;
		SocialNetworkingManager.twitterRequestDidFinishEvent -= twitterRequestDidFinishEvent;
		SocialNetworkingManager.twitterRequestDidFailEvent -= twitterRequestDidFailEvent;
		SocialNetworkingManager.facebookLogin -= facebookLogin;
		SocialNetworkingManager.facebookLoginFailed -= facebookLoginFailed;
		SocialNetworkingManager.facebookDidLogoutEvent -= facebookDidLogoutEvent;
		SocialNetworkingManager.facebookDidExtendTokenEvent -= facebookDidExtendTokenEvent;
		SocialNetworkingManager.facebookSessionInvalidatedEvent -= facebookSessionInvalidatedEvent;
		SocialNetworkingManager.facebookReceivedUsername -= facebookReceivedUsername;
		SocialNetworkingManager.facebookUsernameRequestFailed -= facebookUsernameRequestFailed;
		SocialNetworkingManager.facebookPost -= facebookPost;
		SocialNetworkingManager.facebookPostFailed -= facebookPostFailed;
		SocialNetworkingManager.facebookReceivedFriends -= facebookReceivedFriends;
		SocialNetworkingManager.facebookFriendRequestFailed += facebookFriendRequestFailed;
		SocialNetworkingManager.facebookDialogCompleted -= facebokDialogCompleted;
		SocialNetworkingManager.facebookDialogCompletedWithUrl -= facebookDialogCompletedWithUrl;
		SocialNetworkingManager.facebookDialogDidntComplete -= facebookDialogDidntComplete;
		SocialNetworkingManager.facebookDialogFailed -= facebookDialogFailed;
		SocialNetworkingManager.facebookReceivedCustomRequest -= facebookReceivedCustomRequest;
		SocialNetworkingManager.facebookCustomRequestFailed -= facebookCustomRequestFailed;
	}

	private void twitterLogin()
	{
		Debug.Log("Successfully logged in to Twitter");
		if (_twitterCurrentRequest == TwitterCurrentRequest.LoggingIn)
		{
			_twitterCurrentRequest = TwitterCurrentRequest.None;
		}
		else
		{
			Debug.LogWarning("Received twitter login message, but we are not in that state");
		}
	}

	private void twitterLoginFailed(string error)
	{
		Debug.Log("Twitter login failed: " + error);
		if (_twitterCurrentRequest == TwitterCurrentRequest.LoggingIn)
		{
			_twitterCurrentRequest = TwitterCurrentRequest.Error;
		}
		else
		{
			Debug.LogWarning("Received twitter login failed message, but we are not in that state");
		}
	}

	private void twitterPost()
	{
		Debug.Log("Successfully posted to Twitter");
	}

	private void twitterPostFailed(string error)
	{
		Debug.Log("Twitter post failed: " + error);
	}

	private void twitterHomeTimelineFailed(string error)
	{
		Debug.Log("Twitter HomeTimeline failed: " + error);
	}

	private void twitterHomeTimelineReceived(ArrayList result)
	{
		Debug.Log("received home timeline with tweet count: " + result.Count);
	}

	private void twitterRequestDidFailEvent(string error)
	{
		Debug.Log("twitterRequestDidFailEvent: " + error);
	}

	private void twitterRequestDidFinishEvent(object result)
	{
		if (result != null)
		{
			Debug.Log("twitterRequestDidFinishEvent: " + result.GetType().ToString());
		}
		else
		{
			Debug.Log("twitterRequestDidFinishEvent with no data");
		}
	}

	private void facebookLogin()
	{
		Debug.Log("Successfully logged in to Facebook");
		if (_fbCurrentRequest == FacebookCurrentRequest.LoggingIn)
		{
			_fbCurrentRequest = FacebookCurrentRequest.None;
			Flurry.LogFacebookLogin();
		}
		else
		{
			Debug.LogWarning("Received facebook login message, but we are not in that state");
		}
	}

	private void facebookLoginFailed(string error)
	{
		Debug.Log("Facebook login failed: " + error);
		if (_fbCurrentRequest == FacebookCurrentRequest.LoggingIn)
		{
			_fbCurrentRequest = FacebookCurrentRequest.Error;
		}
		else
		{
			Debug.LogWarning("Received facebook login failed message, but we are not in that state");
		}
	}

	private void facebookDidLogoutEvent()
	{
		Debug.Log("facebookDidLogoutEvent");
		if (_friends != null)
		{
			_friends.RemoveAll((Friend item) => item.gcProfile == null && item.fbProfile != null);
			_friends.ForEach(delegate(Friend item)
			{
				item.fbProfile = null;
			});
		}
		_fbFriends = null;
	}

	private void facebookDidExtendTokenEvent(DateTime newExpiry)
	{
		Debug.Log("facebookDidExtendTokenEvent: " + newExpiry);
	}

	private void facebookSessionInvalidatedEvent()
	{
		Debug.Log("facebookSessionInvalidatedEvent");
	}

	private void facebookReceivedUsername(string username)
	{
		Debug.Log("Facebook logged in users name: " + username);
	}

	private void facebookUsernameRequestFailed(string error)
	{
		Debug.Log("Facebook failed to receive username: " + error);
	}

	private void facebookPost()
	{
		Debug.Log("Successfully posted to Facebook");
	}

	private void facebookPostFailed(string error)
	{
		Debug.Log("Facebook post failed: " + error);
	}

	private void facebookReceivedFriends(ArrayList result)
	{
		Debug.Log("received total friends: " + result.Count);
		if (_fbCurrentRequest == FacebookCurrentRequest.GettingFriends)
		{
			_fbCurrentRequest = FacebookCurrentRequest.None;
		}
		_fbFriends = new Dictionary<string, Hashtable>(result.Count);
		foreach (Hashtable item in result)
		{
			if (item.ContainsKey("id"))
			{
				_fbFriends[(string)item["id"]] = item;
			}
			else
			{
				Debug.LogError("Unexpected format of FaceBook Friend");
			}
		}
	}

	private void facebookFriendRequestFailed(string error)
	{
		Debug.Log("FacebookFriendRequestFailed: " + error);
		_fbFriends = null;
		if (_fbCurrentRequest == FacebookCurrentRequest.GettingFriends)
		{
			_fbCurrentRequest = FacebookCurrentRequest.Error;
		}
	}

	private void facebokDialogCompleted()
	{
		Debug.Log("facebokDialogCompleted");
	}

	private void facebookDialogCompletedWithUrl(string url)
	{
		Debug.Log("facebookDialogCompletedWithUrl: " + url);
	}

	private void facebookDialogDidntComplete()
	{
		Debug.Log("facebookDialogDidntComplete");
	}

	private void facebookDialogFailed(string error)
	{
		Debug.Log("facebookDialogFailed: " + error);
	}

	private void facebookReceivedCustomRequest(object obj)
	{
		Debug.Log("facebookReceivedCustomRequest");
		if (_fbCurrentRequest == FacebookCurrentRequest.GettingUserInfo)
		{
			_fbProfile = new FacebookProfile();
			Hashtable hashtable = (Hashtable)obj;
			_fbProfile.id = (string)hashtable["id"];
			_fbProfile.name = (string)hashtable["first_name"];
			_fbProfile.fullName = (string)hashtable["name"];
			_fbCurrentRequest = FacebookCurrentRequest.None;
		}
	}

	private void facebookCustomRequestFailed(string error)
	{
		Debug.Log("facebookCustomRequestFailed failed: " + error);
		if (_fbCurrentRequest == FacebookCurrentRequest.GettingUserInfo)
		{
			_fbCurrentRequest = FacebookCurrentRequest.Error;
		}
	}

	private static string GetRandomIdentifier()
	{
		string text = ((!Application.isEditor) ? SystemInfo.deviceUniqueIdentifier : "0000000000000000000000000000000000000000");
		return text + UnityEngine.Random.Range(0, int.MaxValue);
	}

	private static string GetChecksum(string data)
	{
		return GetSHA1Hash(data + "resxrctrv7tgv7gb8h9h9u0909kllfmolkjnhghgjjkhjghg");
	}

	private static string GetChecksum(params string[] data)
	{
		return GetChecksum(string.Join(null, data));
	}

	private static string GetSHA1Hash(string unhashed)
	{
		SHA1 sHA = SHA1.Create();
		byte[] array = sHA.ComputeHash(Encoding.Default.GetBytes(unhashed));
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("x2"));
		}
		return stringBuilder.ToString();
	}

	private static IEnumerator WWWRequestCoroutine(WWWComplete onWWWComplete, string relativeUrl, object cookie, params string[] postItems)
	{
		string url = "http://hoodrunner.kiloo.com" + relativeUrl;
		string identifier = GetRandomIdentifier();
		StringBuilder checksumSB = new StringBuilder();
		for (int j = 1; j < postItems.Length; j += 2)
		{
			checksumSB.Append(postItems[j]);
		}
		string checksum = GetChecksum(identifier + checksumSB.ToString());
		WWWForm postData = new WWWForm();
		postData.AddField("identifier", identifier);
		postData.AddField("checksum", checksum);
		StringBuilder sb = new StringBuilder();
		sb.Append("WWWRequest(").Append(url).Append(")\n");
		for (int i = 0; i < postItems.Length; i += 2)
		{
			sb.Append("Adding post data: \"").Append(postItems[i]).Append("\" = \"")
				.Append(postItems[i + 1])
				.Append("\"\n");
			postData.AddField(postItems[i], postItems[i + 1]);
		}
		WWW www = new WWW(url, postData);
		yield return www;
		if (www.text != null)
		{
			sb.Append("Text: \"").Append(www.text).Append("\"\n");
		}
		if (www.error != null)
		{
			sb.Append("Error: \"").Append(www.error).Append("\"\n");
		}
		Debug.Log(sb.ToString());
		if (onWWWComplete == null)
		{
			yield break;
		}
		if (www.error != null)
		{
			onWWWComplete(WWWRequestResult.Error, null, cookie);
			yield break;
		}
		string result = null;
		int resultStart2 = www.text.IndexOf("<result>", StringComparison.OrdinalIgnoreCase);
		if (resultStart2 >= 0)
		{
			resultStart2 += 8;
			int resultEnd = www.text.IndexOf("</result>", resultStart2, StringComparison.OrdinalIgnoreCase);
			if (resultEnd > resultStart2)
			{
				result = www.text.Substring(resultStart2, resultEnd - resultStart2);
			}
			else if (resultEnd == resultStart2)
			{
				result = string.Empty;
			}
		}
		onWWWComplete((result == null) ? WWWRequestResult.Error : WWWRequestResult.Success, result, cookie);
	}

	private static string ByteArrayToHex(byte[] barray)
	{
		char[] array = new char[barray.Length * 2];
		for (int i = 0; i < barray.Length; i++)
		{
			byte b = (byte)(barray[i] >> 4);
			array[i * 2] = (char)((b <= 9) ? (b + 48) : (b + 55));
			b = (byte)(barray[i] & 0xFu);
			array[i * 2 + 1] = (char)((b <= 9) ? (b + 48) : (b + 55));
		}
		return new string(array);
	}

	private static string GetBundleVersion()
	{
		return DeviceUtility.GetBundleVersion();
	}

	private void RegisterUser(Action<bool> registerUserCompleted)
	{
	}

	private void WWWRegisterUserCompleted(WWWRequestResult result, string output, object cookie)
	{
		bool obj = false;
		if (result == WWWRequestResult.Success)
		{
			Dictionary<string, string> dictionary = StringUtility.ParseProperties(output);
			if (dictionary.ContainsKey("userid"))
			{
				string text = dictionary["userid"];
				string text2 = dictionary["score"];
				string text3 = dictionary["meters"];
				string text4 = dictionary["games"];
				string strA = dictionary["checksum"];
				string checksum = GetChecksum(text, text2, text3, text4);
				if (string.Compare(strA, checksum, true) == 0)
				{
					try
					{
						int userid = int.Parse(text);
						int highestScore = int.Parse(text2);
						int highestMeters = int.Parse(text3);
						_userid = userid;
						PlayerInfo.Instance.highestScore = highestScore;
						PlayerInfo.Instance.highestMeters = highestMeters;
						obj = true;
					}
					catch (Exception)
					{
						Debug.LogError("Error parsing output data from register user");
					}
				}
				else
				{
					Debug.LogError("Output data from register user corrupted or tampered with");
				}
			}
		}
		if (cookie != null)
		{
			((Action<bool>)cookie)(obj);
		}
	}

	private void ConsolidateFriends(Action<bool> consolidateFriendsCompleted)
	{
		string text;
		if (_fbFriends != null)
		{
			string[] array = new string[_fbFriends.Count];
			_fbFriends.Keys.CopyTo(array, 0);
			text = string.Join(";", array);
		}
		else
		{
			text = string.Empty;
		}
		string text2;
		if (_gcFriends != null)
		{
			string[] array2 = new string[_gcFriends.Count];
			_gcFriends.Keys.CopyTo(array2, 0);
			text2 = string.Join(";", array2);
		}
		else
		{
			text2 = string.Empty;
		}
		if (string.IsNullOrEmpty(text) && string.IsNullOrEmpty(text2))
		{
			consolidateFriendsCompleted(true);
			return;
		}
		StartCoroutine(WWWRequestCoroutine(WWWConsolidateFriendsCompleted, "/friends.php", consolidateFriendsCompleted, "fblist", text, "gclist", text2));
	}

	private static string[][] ParseSets(string setsString)
	{
		string[] separator = new string[1] { ");(" };
		string[] array = setsString.Split(separator, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length > 0)
		{
			if (array[0][0] == '(')
			{
				array[0] = array[0].Substring(1);
			}
			int num = array.Length - 1;
			int num2 = array[num].Length - 1;
			if (array[num][num2] == ')')
			{
				array[num] = array[num].Remove(num2);
			}
			string[][] array2 = new string[array.Length][];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = array[i].Split(';');
			}
			return array2;
		}
		return new string[0][];
	}

	private void WWWConsolidateFriendsCompleted(WWWRequestResult result, string output, object cookie)
	{
		bool obj = false;
		if (result == WWWRequestResult.Success)
		{
			Dictionary<string, string> dictionary = StringUtility.ParseProperties(output);
			Debug.Log("Parse properties");
			if (dictionary.ContainsKey("friendslist"))
			{
				Debug.Log("props contain friendslist");
				string text = dictionary["friendslist"];
				string strA = dictionary["checksum"];
				string checksum = GetChecksum(text);
				if (string.Compare(strA, checksum, true) == 0)
				{
					Debug.Log("Checksum fits");
					if (string.IsNullOrEmpty(text))
					{
						_friends = null;
						Debug.Log("no friends");
					}
					else
					{
						Debug.Log("Friends exist");
						string[][] array = ParseSets(text);
						_friends = new List<Friend>(array.Length);
						string[][] array2 = array;
						foreach (string[] array3 in array2)
						{
							Debug.Log("foreach friend");
							if (array3.Length == 6 && (array3[1].Length > 0 || array3[2].Length > 0))
							{
								try
								{
									Debug.Log("Trying to create friend");
									Friend friend = new Friend();
									friend.userid = int.Parse(array3[0]);
									string text2 = array3[1];
									if (text2.Length > 0)
									{
										Debug.Log("gcid length > 0: " + text2);
										friend.gcProfile = _gcFriends[text2];
									}
									string text3 = array3[2];
									if (text3.Length > 0)
									{
										Debug.Log("fbid > 0");
										if (_fbProfiles == null)
										{
											_fbProfiles = new Dictionary<string, FacebookProfile>();
										}
										FacebookProfile facebookProfile;
										if (_fbProfiles.ContainsKey(text3))
										{
											facebookProfile = _fbProfiles[text3];
										}
										else
										{
											Hashtable hashtable = _fbFriends[text3];
											facebookProfile = new FacebookProfile();
											facebookProfile.id = text3;
											facebookProfile.name = (string)hashtable["first_name"];
											facebookProfile.fullName = (string)hashtable["name"];
											_fbProfiles[text3] = facebookProfile;
										}
										friend.fbProfile = facebookProfile;
									}
									friend.score = int.Parse(array3[3]);
									friend.meters = int.Parse(array3[4]);
									friend.games = int.Parse(array3[5]);
									Friend.Status status = null;
									if (_friendStatus == null)
									{
										_friendStatus = new Dictionary<string, Friend.Status>();
									}
									if (friend.fbProfile != null && _friendStatus.ContainsKey(friend.fbProfile.id))
									{
										Debug.Log("found fb");
										status = _friendStatus[friend.fbProfile.id];
									}
									else if (friend.gcProfile != null && _friendStatus.ContainsKey(friend.gcProfile.id))
									{
										Debug.Log("found gc");
										status = _friendStatus[friend.gcProfile.id];
									}
									else
									{
										Debug.Log("creating new");
										status = new Friend.Status();
										status.gamesCashedIn = friend.games;
										string key = ((friend.fbProfile == null) ? friend.gcProfile.id : friend.fbProfile.id);
										_friendStatus[key] = status;
										_dirty = true;
									}
									friend.status = status;
									_friends.Add(friend);
								}
								catch (Exception ex)
								{
									Debug.LogError("Friend parse error " + ex.ToString());
								}
							}
							else
							{
								Debug.LogError("Malformed friend: (" + string.Join(";", array3) + ")");
							}
						}
						if (_fbProfiles != null)
						{
							StartCoroutine(DownloadFacebookPictures(_fbProfiles));
						}
					}
					Debug.Log("success");
					obj = true;
				}
				else
				{
					Debug.LogError("Consolidated friend data corrupted");
				}
			}
		}
		if (cookie != null)
		{
			((Action<bool>)cookie)(obj);
		}
	}

	public void ReportScore(int newScore, int newMeters)
	{
		if (_userid > 0)
		{
			StartCoroutine(WWWRequestCoroutine(null, "/report.php", null, "userid", _userid.ToString(), "score", newScore.ToString(), "meters", newMeters.ToString()));
			if (Social.localUser.authenticated)
			{
				Social.ReportScore(newScore, "com.kiloo.subwaysurfers.ScoreLeaderboard", GameCenterCallBack);
			}
			else
			{
				Debug.Log("Game Center localuser was not authenticated");
			}
		}
	}

	private void GameCenterCallBack(bool success)
	{
		Debug.Log("Game center score report " + ((!success) ? "failure" : "success"));
	}

	public void UpdateFriendScores(Action<bool> updateFriendsScoresCompleted)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (Friend friend in _friends)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(';');
			}
			stringBuilder.Append(friend.userid);
		}
		string text = stringBuilder.ToString();
		StartCoroutine(WWWRequestCoroutine(WWWUpdateFriendScoresCompleted, "/scores.php", updateFriendsScoresCompleted, "idlist", text));
	}

	private void WWWUpdateFriendScoresCompleted(WWWRequestResult result, string output, object cookie)
	{
		bool obj = false;
		if (result == WWWRequestResult.Success)
		{
			Dictionary<string, string> dictionary = StringUtility.ParseProperties(output);
			if (dictionary.ContainsKey("scores"))
			{
				string text = dictionary["scores"];
				string strA = dictionary["checksum"];
				string checksum = GetChecksum(text);
				if (string.Compare(strA, checksum, true) == 0)
				{
					try
					{
						string[][] array = ParseSets(text);
						string[][] array2 = array;
						foreach (string[] array3 in array2)
						{
							if (array3.Length == 4)
							{
								int userid = int.Parse(array3[0]);
								Friend friend = _friends.Find((Friend f) => f.userid == userid);
								if (friend != null)
								{
									int score = int.Parse(array3[1]);
									int meters = int.Parse(array3[2]);
									int games = int.Parse(array3[3]);
									friend.score = score;
									friend.meters = meters;
									friend.games = games;
								}
								else
								{
									Debug.LogWarning("UpdateFriendScores: Unexpected friend user id");
								}
								continue;
							}
							Debug.LogError("UpdateFriendScores: Malformed score (" + string.Join(";", array3) + ")");
							throw new Exception();
						}
						obj = true;
					}
					catch (Exception)
					{
						Debug.LogError("UpdateFriendScores: Error parsing output data");
					}
				}
				else
				{
					Debug.LogError("UpdateFriendScores: Output data corrupt");
				}
			}
		}
		if (cookie != null)
		{
			((Action<bool>)cookie)(obj);
		}
	}

	public void Poke(Friend friend)
	{
		string text = ((friend.fbProfile != null) ? _fbProfile.fullName : ((!Social.localUser.authenticated) ? string.Empty : Social.localUser.userName));
		StartCoroutine(WWWRequestCoroutine(null, "/poke.php", null, "friend", friend.userid.ToString(), "name", text));
		friend.status.lastPokeTime = DateTime.UtcNow;
		_dirty = true;
		Flurry.LogGenericSocialAction();
	}

	public void SetPokeFirstTime(Friend friend)
	{
		friend.status.lastPokeTime = DateTime.UtcNow;
		_dirty = true;
	}

	public void BragNotify(int oldScore, List<Friend> friends)
	{
		if (friends == null)
		{
			return;
		}
		int count = friends.Count;
		StringBuilder stringBuilder = new StringBuilder(count * 8);
		StringBuilder stringBuilder2 = new StringBuilder(count * 2);
		foreach (Friend friend in friends)
		{
			int relation = friend.relation;
			int userid = friend.userid;
			if (relation != 0 && userid != 0)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(';');
					stringBuilder2.Append(';');
				}
				stringBuilder.Append(userid);
				stringBuilder2.Append(relation);
			}
		}
		if (stringBuilder.Length > 0)
		{
			StartCoroutine(WWWRequestCoroutine(null, "/brag.php", null, "oldscore", oldScore.ToString(), "newscore", PlayerInfo.Instance.highestScore.ToString(), "useridlist", stringBuilder.ToString(), "relationlist", stringBuilder2.ToString(), "fbname", (_fbProfile == null) ? string.Empty : _fbProfile.name, "gcname", (!Social.localUser.authenticated) ? string.Empty : Social.localUser.userName));
			Flurry.LogGenericSocialAction();
		}
	}

	private static string GetDeviceTypeString()
	{
		return "iDevice";
	}

	public void RecommendAppFacebook()
	{
		if (facebookIsLoggedIn)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("link", "http://redirect.kiloo.com/subwayapp.php");
			hashtable.Add("picture", "http://hoodrunner.kiloo.com/fblogo.png");
			hashtable.Add("name", "Subway Surfers");
			hashtable.Add("caption", "Dodge the trains! Help Jake, Tricky and Fresh escape.");
			hashtable.Add("description", "Try out Subway Surfers for free on iOS!");
			FacebookBinding.showPostMessageDialogWithOptions(hashtable);
		}
		else
		{
			Debug.LogError("Not logged in to facebook");
		}
	}

	public void BragFacebook(List<Friend> friends)
	{
		if (facebookIsLoggedIn)
		{
			List<Friend> list = null;
			if (friends != null)
			{
				list = new List<Friend>(friends.Count);
				foreach (Friend friend in friends)
				{
					if (friend.fbProfile != null && friend.score < PlayerInfo.Instance.highestScore)
					{
						list.Add(friend);
					}
				}
				list.Sort((Friend x, Friend y) => y.score - x.score);
			}
			string value = ((list == null || list.Count == 0) ? ("I just scored " + PlayerInfo.Instance.highestScore + " points dodging trains in Subway Surfers on my " + GetDeviceTypeString() + ". Check it out!") : ((list.Count == 1) ? ("I just scored " + PlayerInfo.Instance.highestScore + " points in Subway Surfers on my " + GetDeviceTypeString() + " and beat " + list[0].fbProfile.fullName) : ((list.Count == 2) ? ("I just scored " + PlayerInfo.Instance.highestScore + " points in Subway Surfers on my " + GetDeviceTypeString() + " and beat " + list[0].fbProfile.fullName + " and " + list[1].fbProfile.fullName) : ((list.Count != 3) ? ("I just scored " + PlayerInfo.Instance.highestScore + " points in Subway Surfers on my " + GetDeviceTypeString() + " and beat " + list[0].fbProfile.fullName + ", " + list[1].fbProfile.fullName + " and " + (list.Count - 2) + " others") : ("I just scored " + PlayerInfo.Instance.highestScore + " points in Subway Surfers on my " + GetDeviceTypeString() + " and beat " + list[0].fbProfile.fullName + ", " + list[1].fbProfile.fullName + " and " + list[2].fbProfile.fullName)))));
			Hashtable hashtable = new Hashtable();
			hashtable.Add("link", "http://redirect.kiloo.com/subwayapp.php");
			hashtable.Add("picture", "http://hoodrunner.kiloo.com/fblogo.png");
			hashtable.Add("name", "Subway Surfers");
			hashtable.Add("caption", "New Subway Surfers High Score");
			hashtable.Add("description", value);
			FacebookBinding.showPostMessageDialogWithOptions(hashtable);
			Flurry.LogGenericSocialAction();
		}
		else
		{
			Debug.LogError("Not logged in to facebook");
		}
	}

	public void BragTwitter(int oldScore)
	{
		if (TwitterBinding.isLoggedIn())
		{
			string status = "I just scored " + PlayerInfo.Instance.highestScore + " points dodging trains in Subway Surfers on my " + GetDeviceTypeString() + ". Check it out: http://redirect.kiloo.com/subwayapp.php";
			TwitterBinding.showTweetComposer(status, null);
			Flurry.LogGenericSocialAction();
		}
	}

	private IEnumerator DownloadFacebookPicture(FacebookProfile profile)
	{
		if (profile == null)
		{
			Debug.LogError("facebook profile was null in DownloadFacebookPictures!");
			yield break;
		}
		string url = "http://graph.facebook.com/" + profile.id + "/picture?type=square";
		Debug.Log("www getting facebook image for " + profile.name + " at \"" + url + "\"");
		WWW www = new WWW(url);
		yield return www;
		if (www.error != null)
		{
			Debug.LogWarning("www failed getting image for " + profile.name + ": " + www.error);
		}
		Texture2D image = www.texture;
		if (image == null || (image.width == 8 && image.height == 8))
		{
			Debug.LogWarning("www done but no image for " + profile.name);
			yield break;
		}
		profile.image = image;
		Debug.Log("www done, got image for " + profile.name + " (width=" + profile.image.width + ", height=" + profile.image.height + ")");
	}

	private IEnumerator DownloadFacebookPictures(Dictionary<string, FacebookProfile> fbProfiles)
	{
		List<FacebookProfile> profiles = new List<FacebookProfile>(fbProfiles.Count);
		foreach (FacebookProfile profile2 in fbProfiles.Values)
		{
			if (profile2.image == null)
			{
				profiles.Add(profile2);
			}
		}
		foreach (FacebookProfile profile in profiles)
		{
			yield return StartCoroutine(DownloadFacebookPicture(profile));
		}
	}
}
