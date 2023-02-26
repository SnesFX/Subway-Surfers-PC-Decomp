using AnimationOrTween;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Play Animation")]
public class UIButtonPlayAnimation : MonoBehaviour
{
	public Animation target;

	public string clipName;

	public Trigger trigger;

	public Direction playDirection = Direction.Forward;

	public bool resetOnPlay;

	public EnableCondition ifDisabledOnPlay;

	public DisableCondition disableWhenFinished;

	public GameObject eventReceiver;

	public string callWhenFinished;

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

	private void OnHover(bool isOver)
	{
		if (base.enabled)
		{
			if (trigger == Trigger.OnHover || (trigger == Trigger.OnHoverTrue && isOver) || (trigger == Trigger.OnHoverFalse && !isOver))
			{
				Play(isOver);
			}
			mHighlighted = isOver;
		}
	}

	private void OnPress(bool isPressed)
	{
		if (base.enabled && (trigger == Trigger.OnPress || (trigger == Trigger.OnPressTrue && isPressed) || (trigger == Trigger.OnPressFalse && !isPressed)))
		{
			Play(isPressed);
		}
	}

	private void OnClick()
	{
		if (base.enabled && trigger == Trigger.OnClick)
		{
			Play(true);
		}
	}

	private void Play(bool forward)
	{
		if (target == null)
		{
			target = GetComponentInChildren<Animation>();
		}
		if (target != null)
		{
			int num = 0 - playDirection;
			Direction direction = ((!forward) ? ((Direction)num) : playDirection);
			ActiveAnimation activeAnimation = ActiveAnimation.Play(target, clipName, direction, ifDisabledOnPlay, disableWhenFinished);
			if (resetOnPlay)
			{
				activeAnimation.Reset();
			}
			if (eventReceiver != null && !string.IsNullOrEmpty(callWhenFinished))
			{
				activeAnimation.eventReceiver = eventReceiver;
				activeAnimation.callWhenFinished = callWhenFinished;
			}
		}
	}
}
