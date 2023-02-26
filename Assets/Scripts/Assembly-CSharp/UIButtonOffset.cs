using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Offset")]
public class UIButtonOffset : MonoBehaviour
{
	public Transform tweenTarget;

	public Vector3 hover = Vector3.zero;

	public Vector3 pressed = new Vector3(2f, -2f);

	public float duration = 0.2f;

	private Vector3 mPos;

	private bool mInitDone;

	private bool mStarted;

	private bool mHighlighted;

	private void Start()
	{
		mStarted = true;
	}

	private void OnEnable()
	{
		if (mStarted && mHighlighted)
		{
			OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	private void OnDisable()
	{
		if (tweenTarget != null)
		{
			TweenPosition component = tweenTarget.GetComponent<TweenPosition>();
			if (component != null)
			{
				component.position = mPos;
				component.enabled = false;
			}
		}
	}

	private void Init()
	{
		mInitDone = true;
		if (tweenTarget == null)
		{
			tweenTarget = base.transform;
		}
		mPos = tweenTarget.localPosition;
	}

	private void OnPress(bool isPressed)
	{
		if (base.enabled)
		{
			if (!mInitDone)
			{
				Init();
			}
			TweenPosition.Begin(tweenTarget.gameObject, duration, (!isPressed) ? mPos : (mPos + pressed)).method = UITweener.Method.EaseInOut;
		}
	}

	private void OnHover(bool isOver)
	{
		if (base.enabled)
		{
			if (!mInitDone)
			{
				Init();
			}
			TweenPosition.Begin(tweenTarget.gameObject, duration, (!isOver) ? mPos : (mPos + hover)).method = UITweener.Method.EaseInOut;
			mHighlighted = isOver;
		}
	}
}
