using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Camera")]
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class UICamera : MonoBehaviour
{
	public enum ClickNotification
	{
		None = 0,
		Always = 1,
		BasedOnDelta = 2
	}

	public class MouseOrTouch
	{
		public Vector2 pos;

		public Vector2 delta;

		public Vector2 totalDelta;

		public Camera pressedCam;

		public GameObject current;

		public GameObject pressed;

		public float clickTime;

		public ClickNotification clickNotification = ClickNotification.Always;
	}

	private class Highlighted
	{
		public GameObject go;

		public int counter;
	}

	public bool useMouse = true;

	public bool useTouch = true;

	public bool useKeyboard = true;

	public bool useController = true;

	public LayerMask eventReceiverMask = -1;

	public float tooltipDelay = 1f;

	public float mouseClickThreshold = 10f;

	public float touchClickThreshold = 40f;

	public float rangeDistance = -1f;

	public string scrollAxisName = "Mouse ScrollWheel";

	public string verticalAxisName = "Vertical";

	public string horizontalAxisName = "Horizontal";

	public static Vector2 lastTouchPosition = Vector2.zero;

	public static RaycastHit lastHit;

	public static Camera currentCamera = null;

	public static int currentTouchID = -1;

	public static MouseOrTouch currentTouch = null;

	public static GameObject fallThrough;

	private static List<UICamera> mList = new List<UICamera>();

	private static List<Highlighted> mHighlighted = new List<Highlighted>();

	private static GameObject mSel = null;

	private static MouseOrTouch[] mMouse = new MouseOrTouch[3]
	{
		new MouseOrTouch(),
		new MouseOrTouch(),
		new MouseOrTouch()
	};

	private static GameObject mHover;

	private static MouseOrTouch mController = new MouseOrTouch();

	private static float mNextEvent = 0f;

	private Dictionary<int, MouseOrTouch> mTouches = new Dictionary<int, MouseOrTouch>();

	private GameObject mTooltip;

	private Camera mCam;

	private LayerMask mLayerMask;

	private float mTooltipTime;

	private bool mIsEditor;

	[Obsolete("Use UICamera.currentCamera instead")]
	public static Camera lastCamera
	{
		get
		{
			return currentCamera;
		}
	}

	[Obsolete("Use UICamera.currentTouchID instead")]
	public static int lastTouchID
	{
		get
		{
			return currentTouchID;
		}
	}

	private bool handlesEvents
	{
		get
		{
			return eventHandler == this;
		}
	}

	public Camera cachedCamera
	{
		get
		{
			if (mCam == null)
			{
				mCam = base.GetComponent<Camera>();
			}
			return mCam;
		}
	}

	public static GameObject hoveredObject
	{
		get
		{
			return mHover;
		}
	}

	public static GameObject selectedObject
	{
		get
		{
			return mSel;
		}
		set
		{
			if (!(mSel != value))
			{
				return;
			}
			if (mSel != null)
			{
				UICamera uICamera = FindCameraForLayer(mSel.layer);
				if (uICamera != null)
				{
					currentCamera = uICamera.mCam;
					mSel.SendMessage("OnSelect", false, SendMessageOptions.DontRequireReceiver);
					if (uICamera.useController || uICamera.useKeyboard)
					{
						Highlight(mSel, false);
					}
				}
			}
			mSel = value;
			if (!(mSel != null))
			{
				return;
			}
			UICamera uICamera2 = FindCameraForLayer(mSel.layer);
			if (uICamera2 != null)
			{
				currentCamera = uICamera2.mCam;
				if (uICamera2.useController || uICamera2.useKeyboard)
				{
					Highlight(mSel, true);
				}
				mSel.SendMessage("OnSelect", true, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public static Camera mainCamera
	{
		get
		{
			UICamera uICamera = eventHandler;
			return (!(uICamera != null)) ? null : uICamera.cachedCamera;
		}
	}

	public static UICamera eventHandler
	{
		get
		{
			for (int i = 0; i < mList.Count; i++)
			{
				UICamera uICamera = mList[i];
				if (!(uICamera == null) && uICamera.enabled && uICamera.gameObject.active)
				{
					return uICamera;
				}
			}
			return null;
		}
	}

	private void OnApplicationQuit()
	{
		mHighlighted.Clear();
	}

	private static int CompareFunc(UICamera a, UICamera b)
	{
		if (a.cachedCamera.depth < b.cachedCamera.depth)
		{
			return 1;
		}
		if (a.cachedCamera.depth > b.cachedCamera.depth)
		{
			return -1;
		}
		return 0;
	}

	private static bool Raycast(Vector3 inPos, ref RaycastHit hit)
	{
		for (int i = 0; i < mList.Count; i++)
		{
			UICamera uICamera = mList[i];
			if (!uICamera.enabled || !uICamera.gameObject.active)
			{
				continue;
			}
			currentCamera = uICamera.cachedCamera;
			Vector3 vector = currentCamera.ScreenToViewportPoint(inPos);
			if (!(vector.x < 0f) && !(vector.x > 1f) && !(vector.y < 0f) && !(vector.y > 1f))
			{
				Ray ray = currentCamera.ScreenPointToRay(inPos);
				int layerMask = currentCamera.cullingMask & (int)uICamera.eventReceiverMask;
				float distance = ((!(uICamera.rangeDistance > 0f)) ? (currentCamera.farClipPlane - currentCamera.nearClipPlane) : uICamera.rangeDistance);
				if (Physics.Raycast(ray, out hit, distance, layerMask))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static UICamera FindCameraForLayer(int layer)
	{
		int num = 1 << layer;
		for (int i = 0; i < mList.Count; i++)
		{
			UICamera uICamera = mList[i];
			Camera camera = uICamera.cachedCamera;
			if (camera != null && (camera.cullingMask & num) != 0)
			{
				return uICamera;
			}
		}
		return null;
	}

	private static int GetDirection(KeyCode up, KeyCode down)
	{
		if (Input.GetKeyDown(up))
		{
			return 1;
		}
		if (Input.GetKeyDown(down))
		{
			return -1;
		}
		return 0;
	}

	private static int GetDirection(KeyCode up0, KeyCode up1, KeyCode down0, KeyCode down1)
	{
		if (Input.GetKeyDown(up0) || Input.GetKeyDown(up1))
		{
			return 1;
		}
		if (Input.GetKeyDown(down0) || Input.GetKeyDown(down1))
		{
			return -1;
		}
		return 0;
	}

	private static int GetDirection(string axis)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (mNextEvent < realtimeSinceStartup)
		{
			float axis2 = Input.GetAxis(axis);
			if (axis2 > 0.75f)
			{
				mNextEvent = realtimeSinceStartup + 0.25f;
				return 1;
			}
			if (axis2 < -0.75f)
			{
				mNextEvent = realtimeSinceStartup + 0.25f;
				return -1;
			}
		}
		return 0;
	}

	public static bool IsHighlighted(GameObject go)
	{
		int num = mHighlighted.Count;
		while (num > 0)
		{
			Highlighted highlighted = mHighlighted[--num];
			if (highlighted.go == go)
			{
				return true;
			}
		}
		return false;
	}

	private static void Highlight(GameObject go, bool highlighted)
	{
		if (!(go != null))
		{
			return;
		}
		int num = mHighlighted.Count;
		while (num > 0)
		{
			Highlighted highlighted2 = mHighlighted[--num];
			if (highlighted2 == null || highlighted2.go == null)
			{
				mHighlighted.RemoveAt(num);
			}
			else if (highlighted2.go == go)
			{
				if (highlighted)
				{
					highlighted2.counter++;
				}
				else if (--highlighted2.counter < 1)
				{
					mHighlighted.Remove(highlighted2);
					go.SendMessage("OnHover", false, SendMessageOptions.DontRequireReceiver);
				}
				return;
			}
		}
		if (highlighted)
		{
			Highlighted highlighted3 = new Highlighted();
			highlighted3.go = go;
			highlighted3.counter = 1;
			mHighlighted.Add(highlighted3);
			go.SendMessage("OnHover", true, SendMessageOptions.DontRequireReceiver);
		}
	}

	private MouseOrTouch GetTouch(int id)
	{
		MouseOrTouch value;
		if (!mTouches.TryGetValue(id, out value))
		{
			value = new MouseOrTouch();
			mTouches.Add(id, value);
		}
		return value;
	}

	private void RemoveTouch(int id)
	{
		mTouches.Remove(id);
	}

	private void Awake()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			useMouse = false;
			useTouch = true;
			useKeyboard = false;
			useController = false;
		}
		else if (Application.platform == RuntimePlatform.PS3 || Application.platform == RuntimePlatform.XBOX360)
		{
			useMouse = false;
			useTouch = false;
			useKeyboard = false;
			useController = true;
		}
		else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
		{
			mIsEditor = true;
		}
		mMouse[0].pos.x = Input.mousePosition.x;
		mMouse[0].pos.y = Input.mousePosition.y;
		lastTouchPosition = mMouse[0].pos;
		mList.Add(this);
		mList.Sort(CompareFunc);
		if ((int)eventReceiverMask == -1)
		{
			eventReceiverMask = base.GetComponent<Camera>().cullingMask;
		}
	}

	private void OnDestroy()
	{
		mList.Remove(this);
	}

	private void FixedUpdate()
	{
		if (useMouse && Application.isPlaying && handlesEvents)
		{
			GameObject current = ((!Raycast(Input.mousePosition, ref lastHit)) ? fallThrough : lastHit.collider.gameObject);
			for (int i = 0; i < 3; i++)
			{
				mMouse[i].current = current;
			}
		}
	}

	private void Update()
	{
		if (!Application.isPlaying || !handlesEvents)
		{
			return;
		}
		if (useMouse || (useTouch && mIsEditor))
		{
			ProcessMouse();
		}
		if (useTouch)
		{
			ProcessTouches();
		}
		if (useKeyboard && mSel != null && Input.GetKeyDown(KeyCode.Escape))
		{
			selectedObject = null;
		}
		if (mSel != null)
		{
			string text = Input.inputString;
			if (useKeyboard && Input.GetKeyDown(KeyCode.Delete))
			{
				text += "\b";
			}
			if (text.Length > 0)
			{
				if (mTooltip != null)
				{
					ShowTooltip(false);
				}
				mSel.SendMessage("OnInput", text, SendMessageOptions.DontRequireReceiver);
			}
			ProcessOthers();
		}
		if (useMouse && mHover != null)
		{
			float axis = Input.GetAxis(scrollAxisName);
			if (axis != 0f)
			{
				mHover.SendMessage("OnScroll", axis, SendMessageOptions.DontRequireReceiver);
			}
			if (mTooltipTime != 0f && mTooltipTime < Time.realtimeSinceStartup)
			{
				mTooltip = mHover;
				ShowTooltip(true);
			}
		}
	}

	private void ProcessMouse()
	{
		bool flag = Time.timeScale < 0.9f;
		if (!flag)
		{
			for (int i = 0; i < 3; i++)
			{
				if (Input.GetMouseButton(i) || Input.GetMouseButtonUp(i))
				{
					flag = true;
					break;
				}
			}
		}
		mMouse[0].pos = Input.mousePosition;
		mMouse[0].delta = mMouse[0].pos - lastTouchPosition;
		bool flag2 = mMouse[0].pos != lastTouchPosition;
		lastTouchPosition = mMouse[0].pos;
		if (flag)
		{
			mMouse[0].current = ((!Raycast(Input.mousePosition, ref lastHit)) ? fallThrough : lastHit.collider.gameObject);
		}
		for (int j = 1; j < 3; j++)
		{
			mMouse[j].pos = mMouse[0].pos;
			mMouse[j].delta = mMouse[0].delta;
			mMouse[j].current = mMouse[0].current;
		}
		bool flag3 = false;
		for (int k = 0; k < 3; k++)
		{
			if (Input.GetMouseButton(k))
			{
				flag3 = true;
				break;
			}
		}
		if (flag3)
		{
			mTooltipTime = 0f;
		}
		else if (flag2)
		{
			if (mTooltipTime != 0f)
			{
				mTooltipTime = Time.realtimeSinceStartup + tooltipDelay;
			}
			else if (mTooltip != null)
			{
				ShowTooltip(false);
			}
		}
		if (!flag3 && mHover != null && mHover != mMouse[0].current)
		{
			if (mTooltip != null)
			{
				ShowTooltip(false);
			}
			Highlight(mHover, false);
			mHover = null;
		}
		for (int l = 0; l < 3; l++)
		{
			bool mouseButtonDown = Input.GetMouseButtonDown(l);
			bool mouseButtonUp = Input.GetMouseButtonUp(l);
			currentTouch = mMouse[l];
			currentTouchID = -1 - l;
			if (mouseButtonDown)
			{
				currentTouch.pressedCam = currentCamera;
			}
			else if (currentTouch.pressed != null)
			{
				currentCamera = currentTouch.pressedCam;
			}
			ProcessTouch(mouseButtonDown, mouseButtonUp);
		}
		currentTouch = null;
		if (!flag3 && mHover != mMouse[0].current)
		{
			mTooltipTime = Time.realtimeSinceStartup + tooltipDelay;
			mHover = mMouse[0].current;
			Highlight(mHover, true);
		}
	}

	private void ProcessTouches()
	{
		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			currentTouchID = touch.fingerId;
			currentTouch = GetTouch(currentTouchID);
			bool flag = touch.phase == TouchPhase.Began;
			bool flag2 = touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended;
			if (flag)
			{
				currentTouch.delta = Vector2.zero;
			}
			else
			{
				currentTouch.delta = touch.position - currentTouch.pos;
			}
			currentTouch.pos = touch.position;
			currentTouch.current = ((!Raycast(currentTouch.pos, ref lastHit)) ? fallThrough : lastHit.collider.gameObject);
			lastTouchPosition = currentTouch.pos;
			if (flag)
			{
				currentTouch.pressedCam = currentCamera;
			}
			else if (currentTouch.pressed != null)
			{
				currentCamera = currentTouch.pressedCam;
			}
			ProcessTouch(flag, flag2);
			if (flag2)
			{
				RemoveTouch(currentTouchID);
			}
			currentTouch = null;
		}
	}

	private void ProcessOthers()
	{
		currentTouchID = -100;
		currentTouch = mController;
		bool flag = mSel != null && mSel.GetComponent<UIInput>() != null;
		bool flag2 = useKeyboard && (Input.GetKeyDown(KeyCode.Return) || (!flag && Input.GetKeyDown(KeyCode.Space)));
		bool flag3 = useController && Input.GetKeyDown(KeyCode.JoystickButton0);
		bool flag4 = useKeyboard && (Input.GetKeyUp(KeyCode.Return) || (!flag && Input.GetKeyUp(KeyCode.Space)));
		bool flag5 = useController && Input.GetKeyUp(KeyCode.JoystickButton0);
		bool flag6 = flag2 || flag3;
		bool flag7 = flag4 || flag5;
		if (flag6 || flag7)
		{
			currentTouch.current = mSel;
			ProcessTouch(flag6, flag7);
		}
		int num = 0;
		int num2 = 0;
		if (useKeyboard)
		{
			if (flag)
			{
				num += GetDirection(KeyCode.UpArrow, KeyCode.DownArrow);
				num2 += GetDirection(KeyCode.RightArrow, KeyCode.LeftArrow);
			}
			else
			{
				num += GetDirection(KeyCode.W, KeyCode.UpArrow, KeyCode.S, KeyCode.DownArrow);
				num2 += GetDirection(KeyCode.D, KeyCode.RightArrow, KeyCode.A, KeyCode.LeftArrow);
			}
		}
		if (useController)
		{
			if (!string.IsNullOrEmpty(verticalAxisName))
			{
				num += GetDirection(verticalAxisName);
			}
			if (!string.IsNullOrEmpty(horizontalAxisName))
			{
				num2 += GetDirection(horizontalAxisName);
			}
		}
		if (num != 0)
		{
			mSel.SendMessage("OnKey", (num <= 0) ? KeyCode.DownArrow : KeyCode.UpArrow, SendMessageOptions.DontRequireReceiver);
		}
		if (num2 != 0)
		{
			mSel.SendMessage("OnKey", (num2 <= 0) ? KeyCode.LeftArrow : KeyCode.RightArrow, SendMessageOptions.DontRequireReceiver);
		}
		if (useKeyboard && Input.GetKeyDown(KeyCode.Tab))
		{
			mSel.SendMessage("OnKey", KeyCode.Tab, SendMessageOptions.DontRequireReceiver);
		}
		if (useController && Input.GetKeyUp(KeyCode.JoystickButton1))
		{
			mSel.SendMessage("OnKey", KeyCode.Escape, SendMessageOptions.DontRequireReceiver);
		}
		currentTouch = null;
	}

	private void ProcessTouch(bool pressed, bool unpressed)
	{
		if (pressed)
		{
			if (mTooltip != null)
			{
				ShowTooltip(false);
			}
			currentTouch.pressed = currentTouch.current;
			currentTouch.clickNotification = ClickNotification.Always;
			currentTouch.totalDelta = Vector2.zero;
			if (currentTouch.pressed != null)
			{
				currentTouch.pressed.SendMessage("OnPress", true, SendMessageOptions.DontRequireReceiver);
			}
			if (currentTouch.pressed != mSel)
			{
				if (mTooltip != null)
				{
					ShowTooltip(false);
				}
				selectedObject = null;
			}
		}
		else if (currentTouch.pressed != null && currentTouch.delta.magnitude != 0f)
		{
			if (mTooltip != null)
			{
				ShowTooltip(false);
			}
			currentTouch.totalDelta += currentTouch.delta;
			bool flag = currentTouch.clickNotification == ClickNotification.None;
			currentTouch.pressed.SendMessage("OnDrag", currentTouch.delta, SendMessageOptions.DontRequireReceiver);
			if (flag)
			{
				currentTouch.clickNotification = ClickNotification.None;
			}
			else if (currentTouch.clickNotification == ClickNotification.BasedOnDelta)
			{
				float num = ((currentTouch != mMouse[0]) ? Mathf.Max(touchClickThreshold, (float)Screen.height * 0.1f) : mouseClickThreshold);
				if (currentTouch.totalDelta.magnitude > num)
				{
					currentTouch.clickNotification = ClickNotification.None;
				}
			}
		}
		if (!unpressed)
		{
			return;
		}
		if (mTooltip != null)
		{
			ShowTooltip(false);
		}
		if (currentTouch.pressed != null)
		{
			currentTouch.pressed.SendMessage("OnPress", false, SendMessageOptions.DontRequireReceiver);
			if (currentTouch.pressed == mHover)
			{
				currentTouch.pressed.SendMessage("OnHover", true, SendMessageOptions.DontRequireReceiver);
			}
			if (currentTouch.pressed == currentTouch.current)
			{
				if (currentTouch.pressed != mSel)
				{
					mSel = currentTouch.pressed;
					currentTouch.pressed.SendMessage("OnSelect", true, SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					mSel = currentTouch.pressed;
				}
				if (currentTouch.clickNotification != 0)
				{
					float realtimeSinceStartup = Time.realtimeSinceStartup;
					currentTouch.pressed.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
					if (currentTouch.clickTime + 0.25f > realtimeSinceStartup)
					{
						currentTouch.pressed.SendMessage("OnDoubleClick", SendMessageOptions.DontRequireReceiver);
					}
					currentTouch.clickTime = realtimeSinceStartup;
				}
			}
			else if (currentTouch.current != null)
			{
				currentTouch.current.SendMessage("OnDrop", currentTouch.pressed, SendMessageOptions.DontRequireReceiver);
			}
		}
		currentTouch.pressed = null;
	}

	private void ShowTooltip(bool val)
	{
		mTooltipTime = 0f;
		if (mTooltip != null)
		{
			mTooltip.SendMessage("OnTooltip", val, SendMessageOptions.DontRequireReceiver);
		}
		if (!val)
		{
			mTooltip = null;
		}
	}
}
