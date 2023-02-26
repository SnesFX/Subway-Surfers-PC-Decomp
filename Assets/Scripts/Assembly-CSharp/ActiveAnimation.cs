using AnimationOrTween;
using UnityEngine;

[RequireComponent(typeof(Animation))]
[AddComponentMenu("NGUI/Internal/Active Animation")]
public class ActiveAnimation : IgnoreTimeScale
{
	public GameObject eventReceiver;

	public string callWhenFinished;

	private Animation mAnim;

	private Direction mLastDirection;

	private Direction mDisableDirection;

	private bool mNotify;

	public void Reset()
	{
		if (!(mAnim != null))
		{
			return;
		}
		foreach (AnimationState item in mAnim)
		{
			if (mLastDirection == Direction.Reverse)
			{
				item.time = item.length;
			}
			else if (mLastDirection == Direction.Forward)
			{
				item.time = 0f;
			}
		}
	}

	private void Update()
	{
		float num = UpdateRealTimeDelta();
		if (mAnim != null)
		{
			bool flag = false;
			foreach (AnimationState item in mAnim)
			{
				float num2 = item.speed * num;
				item.time += num2;
				if (num2 < 0f)
				{
					if (item.time > 0f)
					{
						flag = true;
					}
					else
					{
						item.time = 0f;
					}
				}
				else if (item.time < item.length)
				{
					flag = true;
				}
				else
				{
					item.time = item.length;
				}
			}
			mAnim.enabled = true;
			mAnim.Sample();
			mAnim.enabled = false;
			if (flag)
			{
				return;
			}
			if (mNotify)
			{
				mNotify = false;
				if (eventReceiver != null && !string.IsNullOrEmpty(callWhenFinished))
				{
					eventReceiver.SendMessage(callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
				}
				if (mDisableDirection != 0 && mLastDirection == mDisableDirection)
				{
					NGUITools.SetActive(base.gameObject, false);
				}
			}
		}
		base.enabled = false;
	}

	private void Play(string clipName, Direction playDirection)
	{
		if (!(mAnim != null))
		{
			return;
		}
		mAnim.enabled = false;
		if (playDirection == Direction.Toggle)
		{
			playDirection = ((mLastDirection != Direction.Forward) ? Direction.Forward : Direction.Reverse);
		}
		if (string.IsNullOrEmpty(clipName))
		{
			if (!mAnim.isPlaying)
			{
				mAnim.Play();
			}
		}
		else if (!mAnim.IsPlaying(clipName))
		{
			mAnim.Play(clipName);
		}
		foreach (AnimationState item in mAnim)
		{
			if (string.IsNullOrEmpty(clipName) || item.name == clipName)
			{
				float num = Mathf.Abs(item.speed);
				item.speed = num * (float)playDirection;
				if (playDirection == Direction.Reverse && item.time == 0f)
				{
					item.time = item.length;
				}
				else if (playDirection == Direction.Forward && item.time == item.length)
				{
					item.time = 0f;
				}
			}
		}
		mLastDirection = playDirection;
		mNotify = true;
	}

	public static ActiveAnimation Play(Animation anim, string clipName, Direction playDirection, EnableCondition enableBeforePlay, DisableCondition disableCondition)
	{
		if (!anim.gameObject.active)
		{
			if (enableBeforePlay != EnableCondition.EnableThenPlay)
			{
				return null;
			}
			NGUITools.SetActive(anim.gameObject, true);
		}
		ActiveAnimation activeAnimation = anim.GetComponent<ActiveAnimation>();
		if (activeAnimation != null)
		{
			activeAnimation.enabled = true;
		}
		else
		{
			activeAnimation = anim.gameObject.AddComponent<ActiveAnimation>();
		}
		activeAnimation.mAnim = anim;
		activeAnimation.mDisableDirection = (Direction)disableCondition;
		activeAnimation.Play(clipName, playDirection);
		return activeAnimation;
	}

	public static ActiveAnimation Play(Animation anim, string clipName, Direction playDirection)
	{
		return Play(anim, clipName, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable);
	}

	public static ActiveAnimation Play(Animation anim, Direction playDirection)
	{
		return Play(anim, null, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable);
	}
}
