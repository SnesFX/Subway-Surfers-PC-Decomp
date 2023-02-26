using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Rotation")]
public class UIButtonRotation : MonoBehaviour
{
	public Transform tweenTarget;

	public Vector3 hover = Vector3.zero;

	public Vector3 pressed = Vector3.zero;

	public float duration = 0.2f;

	private Quaternion mRot;

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
			TweenRotation component = tweenTarget.GetComponent<TweenRotation>();
			if (component != null)
			{
				component.rotation = mRot;
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
		mRot = tweenTarget.localRotation;
	}

	private void OnPress(bool isPressed)
	{
		if (base.enabled)
		{
			if (!mInitDone)
			{
				Init();
			}
			TweenRotation.Begin(tweenTarget.gameObject, duration, (!isPressed) ? mRot : (mRot * Quaternion.Euler(pressed))).method = UITweener.Method.EaseInOut;
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
			TweenRotation.Begin(tweenTarget.gameObject, duration, (!isOver) ? mRot : (mRot * Quaternion.Euler(hover))).method = UITweener.Method.EaseInOut;
			mHighlighted = isOver;
		}
	}
}
