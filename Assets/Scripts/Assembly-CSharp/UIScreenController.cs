using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreenController : MonoBehaviour
{
	public enum SlideInType
	{
		Mission = 0,
		MissionSet = 1,
		Letters = 2,
		Character = 3,
		LettersComplete = 4
	}

	private class SlideIn
	{
		public SlideInType type;

		public string payload = string.Empty;
	}

	private const string CHARTBOOST_APPID = "4fa7c657f77659a92b000006";

	private const string CHARTBOOST_APPSIGNATURE = "e96e642c8ff398581d031532eda34c4617ba11fe";

	private const string CHARTBOOST_ALLOWNEXT_TICKS_KEY = "cb_alwnxt_ticks";

	private const int CHARTBOOST_FIRSTTIME_DELAY_HOURS = 24;

	private const int CHARTBOOST_FIRSTTIME_DELAY_MINUTES = 0;

	private const int CHARTBOOST_DELAY_HOURS = 0;

	private const int CHARTBOOST_DELAY_MINUTES = 0;

	private const string INGAMEUI_SCREENNAME = "IngameUI";

	private static UIScreenController _instance;

	public GameObject backgroundAnchor;

	public GameObject overlayAnchor;

	public GameObject popupAnchor;

	public GameObject superPopupAnchor;

	public GameObject MenuElements3D;

	public bool LoadMenuOnStart;

	public UIFont FloatingTextFont;

	private Camera mainCamera;

	public Action<bool> OnChangedScreen;

	private static readonly List<string> CHARTBOOST_ALLOWED_SCREENS = new List<string> { "FrontUI" };

	private Dictionary<string, GameObject> _cachedScreens = new Dictionary<string, GameObject>();

	private Stack<string> _screenStack = new Stack<string>(15);

	private Queue<string> _popupQueue = new Queue<string>(15);

	private bool _popupActive;

	private List<string> _screenNamesWithoutBackground = new List<string> { "FrontUI", "IngameUI", "MysteryBoxUI" };

	private List<string> _screenNamesWithOnlineVersion = new List<string> { "LeaderboardUI", "FriendsUI" };

	public UISlideInMissionHelper missionSlideIn;

	public UISlideInMissionSetHelper missionSetSlideIn;

	public UISlideInLettersHelper lettersSlideIn;

	public UISlideInCharacterUnlock characterSlideIn;

	public UISlideIn lettersCompleteSlideIn;

	private Queue<SlideIn> _slideInQueue = new Queue<SlideIn>(15);

	private bool slideInActive;

	public AudioClipInfo slideInSound;

	public AudioClipInfo slideInFanfare;

	public UIMessageHelper messageHelper;

	private Queue<string> _messageQueue = new Queue<string>();

	private bool messageShowing;

	public GameObject inAppPurchaseOverlay;

	public static UIScreenController Instance
	{
		get
		{
			return _instance ?? (_instance = UnityEngine.Object.FindObjectOfType(typeof(UIScreenController)) as UIScreenController);
		}
	}

	private void Awake()
	{
		Missions instance = Missions.Instance;
		instance.onMissionComplete = (Missions.MissionCompleteHandler)Delegate.Combine(instance.onMissionComplete, new Missions.MissionCompleteHandler(OnMissionCompleted));
		Missions instance2 = Missions.Instance;
		instance2.onMissionSetComplete = (Missions.MissionSetCompleteHandler)Delegate.Combine(instance2.onMissionSetComplete, new Missions.MissionSetCompleteHandler(OnMissionSetCompleted));
		PlayerInfo instance3 = PlayerInfo.Instance;
		instance3.OnPickedUpLetter = (Action)Delegate.Combine(instance3.OnPickedUpLetter, new Action(OnLetterPickedUp));
		PlayerInfo instance4 = PlayerInfo.Instance;
		instance4.OnTokenCollected = (Action<CharacterModels.ModelType>)Delegate.Combine(instance4.OnTokenCollected, new Action<CharacterModels.ModelType>(OnTokenPickUp));
		ChartBoost.didDismissInterstitial = (Action)Delegate.Combine(ChartBoost.didDismissInterstitial, new Action(OnChartBoostDidDismissInterstitial));
	}

	private void onDestroy()
	{
		Missions instance = Missions.Instance;
		instance.onMissionComplete = (Missions.MissionCompleteHandler)Delegate.Remove(instance.onMissionComplete, new Missions.MissionCompleteHandler(OnMissionCompleted));
		Missions instance2 = Missions.Instance;
		instance2.onMissionSetComplete = (Missions.MissionSetCompleteHandler)Delegate.Remove(instance2.onMissionSetComplete, new Missions.MissionSetCompleteHandler(OnMissionSetCompleted));
		PlayerInfo instance3 = PlayerInfo.Instance;
		instance3.OnPickedUpLetter = (Action)Delegate.Remove(instance3.OnPickedUpLetter, new Action(OnLetterPickedUp));
		PlayerInfo instance4 = PlayerInfo.Instance;
		instance4.OnTokenCollected = (Action<CharacterModels.ModelType>)Delegate.Remove(instance4.OnTokenCollected, new Action<CharacterModels.ModelType>(OnTokenPickUp));
		ChartBoost.didDismissInterstitial = (Action)Delegate.Remove(ChartBoost.didDismissInterstitial, new Action(OnChartBoostDidDismissInterstitial));
	}

	private void Start()
	{
		HideInAppPurchaseOverlay();
		if (LoadMenuOnStart)
		{
			ShowMainMenu();
		}
	}

	private void OnApplicationPause(bool paused)
	{
		if (paused)
		{
			if (_screenStack.Count > 0 && _screenStack.Peek() == "IngameUI")
			{
				PushScreen(null, "PauseUI");
			}
			PlayerInfo.Instance.Save();
		}
	}

	public void FacebookLogIn(bool loggedIn)
	{
		if (_screenStack.Count <= 0)
		{
			return;
		}
		if (_screenStack.Peek() == "FriendsUI_offline")
		{
			if (loggedIn)
			{
				_SwitchScreen("FriendsUI_online");
			}
		}
		else if (_screenStack.Peek() == "FriendsUI_online")
		{
			if (loggedIn)
			{
				_SwitchScreen("FriendsUI_online");
			}
		}
		else if (_screenStack.Peek() == "LeaderboardUI_online")
		{
			if (loggedIn)
			{
				_SwitchScreen("LeaderboardUI_online");
			}
		}
		else if (_screenStack.Peek() == "LeaderboardUI_offline")
		{
			if (loggedIn)
			{
				_SwitchScreen("LeaderboardUI_online");
			}
		}
		else if (_screenStack.Peek() == "GameoverUI" && loggedIn)
		{
			_cachedScreens["GameoverUI"].GetComponent<UIGameOverHelper>().FacebookLoggedIn();
		}
	}

	public void ShowMainMenu()
	{
		string screenName = "FrontUI";
		_ActivateScreen(screenName);
	}

	public void GameOverTriggered()
	{
		Missions.Instance.inRun = false;
		string screenName = "GameoverUI";
		_ActivateScreen(screenName);
	}

	public void QueueMessage(string message)
	{
		Debug.Log("Showing message: " + message);
		_QueueMessage(message);
	}

	public void GoToMainMenuFromGame(GameObject sender)
	{
		if (Game.Instance != null)
		{
			Missions.Instance.inRun = false;
			Game.Instance.StartTopMenu();
			Game.Instance.TriggerPause(false);
		}
		_ActivateScreen("FrontUI");
	}

	public void PushScreen(GameObject sender)
	{
		PushScreen(sender, string.Empty);
	}

	public void PushScreen(GameObject sender, string screenOverride = "")
	{
		string empty = string.Empty;
		empty = ((!(screenOverride != string.Empty)) ? sender.GetComponent<UIButtonChangeScreen>().ScreenNameToOpen : screenOverride);
		if (_screenNamesWithOnlineVersion.Contains(empty))
		{
			empty = ((!SocialManager.instance.consolidatedFriendsCompleted) ? (empty + "_offline") : (empty + "_online"));
		}
		_ActivateScreen(empty);
		if (empty == "IngameUI")
		{
			_cachedScreens[empty].GetComponent<UIIngameUpdater>().TriggerInGameUI();
			if (!UIIngameUpdater.isCountingDown())
			{
				Missions.Instance.inRun = true;
			}
		}
		else if (empty == "PauseUI" && Game.Instance != null)
		{
			Game.Instance.TriggerPause(true);
		}
	}

	public void SwitchScreen(GameObject sender)
	{
		SwitchScreen(sender, string.Empty);
	}

	public void SwitchScreen(GameObject sender, string screenOverride = "")
	{
		string empty = string.Empty;
		empty = ((!(screenOverride != string.Empty)) ? sender.GetComponent<UIButtonChangeScreen>().ScreenNameToOpen : screenOverride);
		if (_screenNamesWithOnlineVersion.Contains(empty) && _screenNamesWithOnlineVersion.Contains(empty))
		{
			empty = ((!SocialManager.instance.consolidatedFriendsCompleted) ? (empty + "_offline") : (empty + "_online"));
		}
		_SwitchScreen(empty);
	}

	public void BackToPrevious()
	{
		_BackToPreviousScreen();
	}

	public void QueuePopup(GameObject sender)
	{
		string screenNameToOpen = sender.GetComponent<UIButtonChangeScreen>().ScreenNameToOpen;
		_QueuePopup(screenNameToOpen);
	}

	public void QueueMysteryBox()
	{
		string text = string.Empty;
		if (_popupQueue.Count > 0)
		{
			Debug.Log("Peeking at popup queue: " + _popupQueue.Peek());
			text = _popupQueue.Peek();
			_RemovePopup();
		}
		_QueuePopup("MysteryBoxPopup");
		if (text != string.Empty)
		{
			_QueuePopup(text);
			Debug.Log("Queueing " + text);
		}
	}

	public void ClosePopup()
	{
		_RemovePopup();
	}

	public void SpawnCollectText(Vector3 startPosition, string text)
	{
		UILabel uILabel = NGUITools.AddWidget<UILabel>(superPopupAnchor);
		uILabel.text = text;
		uILabel.transform.position = new Vector3(startPosition.x, startPosition.y, uILabel.cachedTransform.position.z);
		uILabel.font = FloatingTextFont;
		uILabel.color = new Color(50f / 51f, 66f / 85f, 0.23529412f, 0f);
		uILabel.cachedTransform.localScale = new Vector3(17f, 17f, 1f);
		StartCoroutine(AnimateCollectText(uILabel));
	}

	private IEnumerator AnimateCollectText(UILabel collectText)
	{
		Vector3 fromLocalPosition = collectText.transform.localPosition;
		Vector3 toLocalPosition = new Vector3(fromLocalPosition.x, fromLocalPosition.y + 50f, fromLocalPosition.z);
		yield return StartCoroutine(AnimateAlpha(collectText, 0.1f, 1f));
		StartCoroutine(MoveTransform(collectText.cachedTransform, 1f, toLocalPosition));
		yield return new WaitForSeconds(0.8f);
		StartCoroutine(AnimateAlpha(collectText, 0.2f, 0f));
		yield return new WaitForSeconds(0.25f);
		UnityEngine.Object.Destroy(collectText.gameObject);
	}

	private IEnumerator AnimateAlpha(UILabel label, float duration, float toAlpha)
	{
		float fromAlpha = label.alpha;
		float factor2 = 0f;
		while (factor2 < 1f)
		{
			factor2 += Time.deltaTime / duration;
			factor2 = Mathf.Clamp01(factor2);
			label.alpha = Mathf.Lerp(fromAlpha, toAlpha, factor2);
			yield return null;
		}
	}

	private IEnumerator MoveTransform(Transform trans, float duration, Vector3 toPos)
	{
		Vector3 fromPos = trans.localPosition;
		float factor2 = 0f;
		while (factor2 < 1f)
		{
			factor2 += Time.deltaTime / duration;
			factor2 = Mathf.Clamp01(factor2);
			trans.localPosition = Vector3.Lerp(fromPos, toPos, factor2);
			yield return null;
		}
	}

	private void _ActivateScreen(string screenName)
	{
		if (_cachedScreens.ContainsKey(screenName))
		{
			GameObject gameObject = _cachedScreens[screenName];
			gameObject.SetActiveRecursively(true);
			gameObject.BroadcastMessage("CreatePanel", SendMessageOptions.DontRequireReceiver);
			if (_screenStack.Contains(screenName))
			{
				while (_screenStack.Contains(screenName) && _screenStack.Peek() != screenName)
				{
					string key = _screenStack.Pop();
					_cachedScreens[key].SetActiveRecursively(false);
				}
			}
			else
			{
				_cachedScreens[_screenStack.Peek()].SetActiveRecursively(false);
				_screenStack.Push(screenName);
			}
		}
		else
		{
			GameObject prefab = Resources.Load("Prefabs/Screens/" + screenName, typeof(GameObject)) as GameObject;
			GameObject value = NGUITools.AddChild(overlayAnchor, prefab);
			_cachedScreens.Add(screenName, value);
			if (_screenStack.Count > 0)
			{
				_cachedScreens[_screenStack.Peek()].SetActiveRecursively(false);
			}
			_screenStack.Push(screenName);
		}
		UIModelController.Instance.ClearModels();
		switch (screenName)
		{
		case "GameoverUI":
			UIModelController.Instance.ActivateGameOverModel();
			_cachedScreens[screenName].GetComponent<UIGameOverHelper>().SetupBeforeMysteryBox();
			if (PlayerInfo.Instance.mysteryBoxesToUnlock != 0)
			{
				_QueuePopup("MysteryBoxPopup");
			}
			else
			{
				_cachedScreens[screenName].GetComponent<UIGameOverHelper>().SetupAfterMysteryBox();
			}
			break;
		case "CharacterUI":
			UIModelController.Instance.ActivateCharacterModel();
			break;
		case "FriendsUI_online":
		case "LeaderboardUI_online":
			Debug.Log("Getting an online screen!");
			_cachedScreens[screenName].GetComponent<UISocialScreen>().ReloadFriends();
			break;
		}
		SetBackground(!_screenNamesWithoutBackground.Contains(screenName));
		Action<bool> onChangedScreen = OnChangedScreen;
		if (onChangedScreen != null)
		{
			onChangedScreen(screenName == "FrontUI");
		}
		ScreenDidChange(screenName);
	}

	private void _SwitchScreen(string screenName)
	{
		string key = _screenStack.Pop();
		_cachedScreens[key].SetActiveRecursively(false);
		_ActivateScreen(screenName);
	}

	private void _BackToPreviousScreen()
	{
		if (_screenStack.Count > 1)
		{
			string key = _screenStack.Pop();
			_cachedScreens[key].SetActiveRecursively(false);
			key = _screenStack.Peek();
			_cachedScreens[key].SetActiveRecursively(true);
			SetBackground(!_screenNamesWithoutBackground.Contains(key));
			ScreenDidChange(key);
		}
		else
		{
			Debug.LogError("Tried to remove the only screen in the stack. You dun goofed.", this);
		}
	}

	private void ScreenDidChange(string newScreenName)
	{
		messageHelper.SetTemporaryHidden(newScreenName != "IngameUI");
		Flurry.LogEvent("UI Screen " + newScreenName);
		if (PlayerInfo.Instance.inAppPurchaseCount <= 0)
		{
			if (CHARTBOOST_ALLOWED_SCREENS.Contains(newScreenName))
			{
				DateTime dateTime;
				if (PlayerPrefs.HasKey("cb_alwnxt_ticks"))
				{
					string @string = PlayerPrefs.GetString("cb_alwnxt_ticks");
					long result;
					if (!long.TryParse(@string, out result))
					{
						result = DateTime.Now.Ticks;
					}
					dateTime = new DateTime(result);
				}
				else
				{
					dateTime = DateTime.Now + new TimeSpan(24, 0, 0);
					PlayerPrefs.SetString("cb_alwnxt_ticks", dateTime.Ticks.ToString());
				}
				DateTime now = DateTime.Now;
				bool flag = false;
				if (now >= dateTime)
				{
					flag = true;
				}
				if (flag || ChartBoost.isInitialized)
				{
					if (!ChartBoost.isInitialized)
					{
						ChartBoost.InitAndStartSession("4fa7c657f77659a92b000006", "e96e642c8ff398581d031532eda34c4617ba11fe");
					}
					ChartBoost.SetShouldRequestInterstitial(flag);
					ChartBoost.SetShouldDisplayInterstitial(true);
					ChartBoost.ShowInterstitial();
				}
			}
			else if (ChartBoost.isInitialized)
			{
				ChartBoost.SetShouldRequestInterstitial(false);
				ChartBoost.SetShouldDisplayInterstitial(false);
			}
		}
		else if (ChartBoost.isInitialized)
		{
			ChartBoost.SetShouldRequestInterstitial(false);
			ChartBoost.SetShouldDisplayInterstitial(false);
		}
	}

	private void OnChartBoostDidDismissInterstitial()
	{
		PlayerPrefs.SetString("cb_alwnxt_ticks", (DateTime.Now + new TimeSpan(0, 0, 0)).Ticks.ToString());
	}

	private void _QueuePopup(string name)
	{
		_popupQueue.Enqueue(name);
		if (!_popupActive)
		{
			_ActivateNextPopup();
		}
	}

	private void _ActivateNextPopup()
	{
		if (_popupQueue.Count > 0)
		{
			_PauseAnimations(true, MenuElements3D.transform);
			NGUITools.SetActive(MenuElements3D, false);
			string text = _popupQueue.Peek();
			if (!_cachedScreens.ContainsKey(text))
			{
				GameObject prefab = Resources.Load("Prefabs/Popups/" + text, typeof(GameObject)) as GameObject;
				GameObject value = NGUITools.AddChild(popupAnchor, prefab);
				_cachedScreens.Add(text, value);
			}
			_cachedScreens[text].SetActiveRecursively(true);
			if (text == "MysteryBoxPopup")
			{
				if (mainCamera == null)
				{
					mainCamera = Camera.main;
				}
				mainCamera.enabled = false;
				_cachedScreens[text].GetComponent<MysteryBoxHandler>().SetupMysteryBoxScreen();
			}
			else if (text == "BragPopup")
			{
				_cachedScreens[text].GetComponent<BragPopupHandler>().SetupBragPopup();
			}
			_cachedScreens[text].BroadcastMessage("CreatePanel", SendMessageOptions.DontRequireReceiver);
			_popupActive = true;
			Action<bool> onChangedScreen = OnChangedScreen;
			if (onChangedScreen != null)
			{
				onChangedScreen(false);
			}
		}
		else
		{
			NGUITools.SetActive(MenuElements3D, true);
			_PauseAnimations(false, MenuElements3D.transform);
		}
	}

	private void _PauseAnimations(bool pause, Transform trans)
	{
		foreach (Transform tran in trans)
		{
			_PauseAnimations(pause, tran);
		}
		if (trans.GetComponent<CharacterModel>() != null)
		{
			if (pause)
			{
				trans.GetComponent<CharacterModel>().StopIdleAnimations();
			}
			else
			{
				trans.GetComponent<CharacterModel>().StartIdleAnimations();
			}
		}
	}

	private void _RemovePopup()
	{
		if (_popupQueue.Count < 1)
		{
			return;
		}
		string text = _popupQueue.Dequeue();
		_cachedScreens[text].SetActiveRecursively(false);
		_popupActive = false;
		Action<bool> onChangedScreen = OnChangedScreen;
		if (onChangedScreen != null)
		{
			onChangedScreen(true);
		}
		_ActivateNextPopup();
		if (text == "MysteryBoxPopup")
		{
			if (mainCamera != null)
			{
				mainCamera.enabled = true;
			}
			if (_screenStack.Peek() == "GameoverUI")
			{
				_cachedScreens[_screenStack.Peek()].GetComponent<UIGameOverHelper>().SetupAfterMysteryBox();
			}
		}
	}

	private void SetBackground(bool state)
	{
		string text = "NotebookPanel";
		if (state)
		{
			if (!_cachedScreens.ContainsKey(text))
			{
				GameObject prefab = Resources.Load("Prefabs/Screens/" + text, typeof(GameObject)) as GameObject;
				GameObject value = NGUITools.AddChild(backgroundAnchor, prefab);
				_cachedScreens.Add(text, value);
			}
			_cachedScreens[text].SetActiveRecursively(true);
			_cachedScreens[text].BroadcastMessage("CreatePanel", SendMessageOptions.DontRequireReceiver);
		}
		else if (_cachedScreens.ContainsKey(text))
		{
			_cachedScreens[text].SetActiveRecursively(false);
		}
	}

	private void OnMissionCompleted(string message)
	{
		Debug.Log(message);
		QueueSlideIn(SlideInType.Mission, message);
	}

	private void OnMissionSetCompleted()
	{
		Debug.Log("Mission Set Completed, increase multiplier");
		QueueSlideIn(SlideInType.MissionSet, string.Empty);
	}

	private void OnLetterPickedUp()
	{
		QueueSlideIn(SlideInType.Letters, string.Empty);
	}

	private void OnTokenPickUp(CharacterModels.ModelType type)
	{
		CharacterModels.Model model = CharacterModels.modelData[type];
		if (PlayerInfo.Instance.GetCollectedTokens(type) == model.Price)
		{
			QueueSlideIn(SlideInType.Character, model.ModelName);
			Debug.Log("Queue character slidein");
			Flurry.LogEventWithAParameter("Character unlocked", "Id", type.ToString());
		}
	}

	public void QueueSlideIn(SlideInType type, string payload = "")
	{
		SlideIn slideIn = new SlideIn();
		slideIn.type = type;
		slideIn.payload = payload;
		_slideInQueue.Enqueue(slideIn);
		if (!slideInActive)
		{
			_ShowSlideIn();
		}
	}

	public void ReadyForNextSlide()
	{
		slideInActive = false;
		if (!slideInActive)
		{
			_ShowSlideIn();
		}
	}

	private void _ShowSlideIn()
	{
		if (_slideInQueue.Count > 0)
		{
			SlideIn slideIn = _slideInQueue.Dequeue();
			if (slideIn.type == SlideInType.Mission)
			{
				So.Instance.playSound(slideInFanfare);
				missionSlideIn.SetupSlideInMission(slideIn.payload);
			}
			else if (slideIn.type == SlideInType.MissionSet)
			{
				So.Instance.playSound(slideInFanfare);
				missionSetSlideIn.SetupSlideInMissionSet(PlayerInfo.Instance.rawMultiplier);
			}
			else if (slideIn.type == SlideInType.Letters)
			{
				So.Instance.playSound(slideInSound);
				lettersSlideIn.SetupLetters();
			}
			else if (slideIn.type == SlideInType.Character)
			{
				So.Instance.playSound(slideInSound);
				characterSlideIn.SetupSlideInCharacter(slideIn.payload);
			}
			else if (slideIn.type == SlideInType.LettersComplete)
			{
				So.Instance.playSound(slideInFanfare);
				lettersCompleteSlideIn.SetupSlideIn();
			}
			slideInActive = true;
		}
	}

	public void ReadyForNextMessage()
	{
		messageShowing = false;
		_ShowNextMessage();
	}

	private void _QueueMessage(string message)
	{
		_messageQueue.Enqueue(message);
		if (!messageShowing)
		{
			_ShowNextMessage();
		}
		if (!slideInActive)
		{
			_ShowSlideIn();
		}
	}

	private void _ShowNextMessage()
	{
		if (_messageQueue.Count > 0)
		{
			string message = _messageQueue.Dequeue();
			messageHelper.ShowMessage(message);
			messageShowing = true;
		}
	}

	public void ShowInAppPurchaseOverlay()
	{
		inAppPurchaseOverlay.SetActiveRecursively(true);
	}

	public void HideInAppPurchaseOverlay()
	{
		inAppPurchaseOverlay.SetActiveRecursively(false);
	}
}
