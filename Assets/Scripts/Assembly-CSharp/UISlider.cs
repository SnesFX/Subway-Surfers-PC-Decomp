using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Slider")]
public class UISlider : IgnoreTimeScale
{
	public enum Direction
	{
		Horizontal = 0,
		Vertical = 1
	}

	public static UISlider current;

	public Transform foreground;

	public Transform thumb;

	public Direction direction;

	public Vector2 fullSize = Vector2.zero;

	public GameObject eventReceiver;

	public string functionName = "OnSliderChange";

	public int numberOfSteps;

	[SerializeField]
	[HideInInspector]
	private float rawValue = 1f;

	private float mStepValue = 1f;

	private BoxCollider mCol;

	private Transform mTrans;

	private Transform mFGTrans;

	private UIWidget mFGWidget;

	private UIFilledSprite mFGFilled;

	private bool mInitDone;

	public float sliderValue
	{
		get
		{
			return mStepValue;
		}
		set
		{
			Set(value, false);
		}
	}

	private void Init()
	{
		mInitDone = true;
		if (foreground != null)
		{
			mFGWidget = foreground.GetComponent<UIWidget>();
			mFGFilled = ((!(mFGWidget != null)) ? null : (mFGWidget as UIFilledSprite));
			mFGTrans = foreground.transform;
			if (fullSize == Vector2.zero)
			{
				fullSize = foreground.localScale;
			}
		}
		else if (mCol != null)
		{
			if (fullSize == Vector2.zero)
			{
				fullSize = mCol.size;
			}
		}
		else
		{
			Debug.LogWarning("UISlider expected to find a foreground object or a box collider to work with", this);
		}
	}

	private void Awake()
	{
		mTrans = base.transform;
		mCol = base.GetComponent<Collider>() as BoxCollider;
	}

	private void Start()
	{
		Init();
		if (Application.isPlaying && thumb != null && thumb.GetComponent<Collider>() != null)
		{
			UIEventListener uIEventListener = UIEventListener.Get(thumb.gameObject);
			uIEventListener.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener.onPress, new UIEventListener.BoolDelegate(OnPressThumb));
			uIEventListener.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uIEventListener.onDrag, new UIEventListener.VectorDelegate(OnDragThumb));
		}
		Set(rawValue, true);
	}

	private void OnPress(bool pressed)
	{
		if (pressed)
		{
			UpdateDrag();
		}
	}

	private void OnDrag(Vector2 delta)
	{
		UpdateDrag();
	}

	private void OnPressThumb(GameObject go, bool pressed)
	{
		if (pressed)
		{
			UpdateDrag();
		}
	}

	private void OnDragThumb(GameObject go, Vector2 delta)
	{
		UpdateDrag();
	}

	private void OnKey(KeyCode key)
	{
		float num = ((!((float)numberOfSteps > 1f)) ? 0.125f : (1f / (float)(numberOfSteps - 1)));
		if (direction == Direction.Horizontal)
		{
			switch (key)
			{
			case KeyCode.LeftArrow:
				Set(rawValue - num, false);
				break;
			case KeyCode.RightArrow:
				Set(rawValue + num, false);
				break;
			}
		}
		else
		{
			switch (key)
			{
			case KeyCode.DownArrow:
				Set(rawValue - num, false);
				break;
			case KeyCode.UpArrow:
				Set(rawValue + num, false);
				break;
			}
		}
	}

	private void UpdateDrag()
	{
		if (!(mCol == null) && !(UICamera.currentCamera == null) && UICamera.currentTouch != null)
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
			Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
			float enter;
			if (new Plane(mTrans.rotation * Vector3.back, mTrans.position).Raycast(ray, out enter))
			{
				Vector3 vector = mTrans.localPosition + mCol.center - mCol.extents;
				Vector3 vector2 = mTrans.localPosition - vector;
				Vector3 vector3 = mTrans.InverseTransformPoint(ray.GetPoint(enter));
				Vector3 vector4 = vector3 + vector2;
				Set((direction != 0) ? (vector4.y / mCol.size.y) : (vector4.x / mCol.size.x), false);
			}
		}
	}

	private void Set(float input, bool force)
	{
		if (!mInitDone)
		{
			Init();
		}
		float num = Mathf.Clamp01(input);
		if (num < 0.001f)
		{
			num = 0f;
		}
		rawValue = num;
		if (numberOfSteps > 1)
		{
			num = Mathf.Round(num * (float)(numberOfSteps - 1)) / (float)(numberOfSteps - 1);
		}
		if (!force && mStepValue == num)
		{
			return;
		}
		mStepValue = num;
		Vector3 localScale = fullSize;
		if (direction == Direction.Horizontal)
		{
			localScale.x *= mStepValue;
		}
		else
		{
			localScale.y *= mStepValue;
		}
		if (mFGFilled != null)
		{
			mFGFilled.fillAmount = mStepValue;
		}
		else if (foreground != null)
		{
			mFGTrans.localScale = localScale;
			if (mFGWidget != null)
			{
				if (num > 0.001f)
				{
					mFGWidget.enabled = true;
					mFGWidget.MarkAsChanged();
				}
				else
				{
					mFGWidget.enabled = false;
				}
			}
		}
		if (thumb != null)
		{
			Vector3 localPosition = thumb.localPosition;
			if (mFGFilled != null)
			{
				if (mFGFilled.fillDirection == UIFilledSprite.FillDirection.Horizontal)
				{
					localPosition.x = ((!mFGFilled.invert) ? localScale.x : (fullSize.x - localScale.x));
				}
				else if (mFGFilled.fillDirection == UIFilledSprite.FillDirection.Vertical)
				{
					localPosition.y = ((!mFGFilled.invert) ? localScale.y : (fullSize.y - localScale.y));
				}
			}
			else if (direction == Direction.Horizontal)
			{
				localPosition.x = localScale.x;
			}
			else
			{
				localPosition.y = localScale.y;
			}
			thumb.localPosition = localPosition;
		}
		if (eventReceiver != null && !string.IsNullOrEmpty(functionName) && Application.isPlaying)
		{
			current = this;
			eventReceiver.SendMessage(functionName, mStepValue, SendMessageOptions.DontRequireReceiver);
			current = null;
		}
	}

	public void ForceUpdate()
	{
		Set(rawValue, true);
	}
}
