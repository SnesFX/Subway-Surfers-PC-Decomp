using System;
using UnityEngine;

[RequireComponent(typeof(UIPanel))]
[AddComponentMenu("NGUI/Interaction/Draggable Panel")]
[ExecuteInEditMode]
public class UIDraggablePanel : IgnoreTimeScale
{
	public enum DragEffect
	{
		None = 0,
		Momentum = 1,
		MomentumAndSpring = 2
	}

	public enum ShowCondition
	{
		Always = 0,
		OnlyIfNeeded = 1,
		WhenDragging = 2
	}

	public bool restrictWithinPanel = true;

	public bool disableDragIfFits;

	public DragEffect dragEffect = DragEffect.MomentumAndSpring;

	public Vector3 scale = Vector3.one;

	public float scrollWheelFactor;

	public float momentumAmount = 35f;

	public Vector2 relativePositionOnReset = Vector2.zero;

	public bool repositionClipping;

	public UIScrollBar horizontalScrollBar;

	public UIScrollBar verticalScrollBar;

	public ShowCondition showScrollBars = ShowCondition.OnlyIfNeeded;

	private Transform mTrans;

	private UIPanel mPanel;

	private Plane mPlane;

	private Vector3 mLastPos;

	private bool mPressed;

	private Vector3 mMomentum = Vector3.zero;

	private float mScroll;

	private Bounds mBounds;

	private bool mCalculatedBounds;

	private bool mShouldMove;

	private bool mIgnoreCallbacks;

	private int mTouches;

	public Bounds bounds
	{
		get
		{
			if (!mCalculatedBounds)
			{
				mCalculatedBounds = true;
				mBounds = NGUIMath.CalculateRelativeWidgetBounds(mTrans, mTrans);
			}
			return mBounds;
		}
	}

	public bool shouldMoveHorizontally
	{
		get
		{
			float num = bounds.size.x;
			if (mPanel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				num += mPanel.clipSoftness.x * 2f;
			}
			return num > mPanel.clipRange.z;
		}
	}

	public bool shouldMoveVertically
	{
		get
		{
			float num = bounds.size.y;
			if (mPanel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				num += mPanel.clipSoftness.y * 2f;
			}
			return num > mPanel.clipRange.w;
		}
	}

	private bool shouldMove
	{
		get
		{
			if (!disableDragIfFits)
			{
				return true;
			}
			if (mPanel == null)
			{
				mPanel = GetComponent<UIPanel>();
			}
			Vector4 clipRange = mPanel.clipRange;
			Bounds bounds = this.bounds;
			float num = clipRange.z * 0.5f;
			float num2 = clipRange.w * 0.5f;
			if (!Mathf.Approximately(scale.x, 0f))
			{
				if (bounds.min.x < clipRange.x - num)
				{
					return true;
				}
				if (bounds.max.x > clipRange.x + num)
				{
					return true;
				}
			}
			if (!Mathf.Approximately(scale.y, 0f))
			{
				if (bounds.min.y < clipRange.y - num2)
				{
					return true;
				}
				if (bounds.max.y > clipRange.y + num2)
				{
					return true;
				}
			}
			return false;
		}
	}

	public Vector3 currentMomentum
	{
		get
		{
			return mMomentum;
		}
		set
		{
			mMomentum = value;
		}
	}

	private void Awake()
	{
		mTrans = base.transform;
		mPanel = GetComponent<UIPanel>();
	}

	private void Start()
	{
		UpdateScrollbars(true);
		if (horizontalScrollBar != null)
		{
			UIScrollBar uIScrollBar = horizontalScrollBar;
			uIScrollBar.onChange = (UIScrollBar.OnScrollBarChange)Delegate.Combine(uIScrollBar.onChange, new UIScrollBar.OnScrollBarChange(OnHorizontalBar));
			horizontalScrollBar.alpha = ((showScrollBars != 0 && !shouldMoveHorizontally) ? 0f : 1f);
		}
		if (verticalScrollBar != null)
		{
			UIScrollBar uIScrollBar2 = verticalScrollBar;
			uIScrollBar2.onChange = (UIScrollBar.OnScrollBarChange)Delegate.Combine(uIScrollBar2.onChange, new UIScrollBar.OnScrollBarChange(OnVerticalBar));
			verticalScrollBar.alpha = ((showScrollBars != 0 && !shouldMoveVertically) ? 0f : 1f);
		}
	}

	public void RestrictWithinBounds(bool instant)
	{
		Vector3 vector = mPanel.CalculateConstrainOffset(bounds.min, bounds.max);
		if (vector.magnitude > 0.001f)
		{
			if (!instant && dragEffect == DragEffect.MomentumAndSpring)
			{
				SpringPanel.Begin(mPanel.gameObject, mTrans.localPosition + vector, 13f);
				return;
			}
			MoveRelative(vector);
			mMomentum = Vector3.zero;
			mScroll = 0f;
		}
		else
		{
			DisableSpring();
		}
	}

	public void DisableSpring()
	{
		SpringPanel component = GetComponent<SpringPanel>();
		if (component != null)
		{
			component.enabled = false;
		}
	}

	public void UpdateScrollbars(bool recalculateBounds)
	{
		if (mPanel == null)
		{
			return;
		}
		if (horizontalScrollBar != null || verticalScrollBar != null)
		{
			if (recalculateBounds)
			{
				mCalculatedBounds = false;
				mShouldMove = shouldMove;
			}
			if (horizontalScrollBar != null)
			{
				Bounds bounds = this.bounds;
				Vector3 size = bounds.size;
				if (size.x > 0f)
				{
					Vector4 clipRange = mPanel.clipRange;
					float num = clipRange.z * 0.5f;
					float num2 = clipRange.x - num - bounds.min.x;
					float num3 = bounds.max.x - num - clipRange.x;
					if (mPanel.clipping == UIDrawCall.Clipping.SoftClip)
					{
						num2 += mPanel.clipSoftness.x;
						num3 -= mPanel.clipSoftness.x;
					}
					num2 = Mathf.Clamp01(num2 / size.x);
					num3 = Mathf.Clamp01(num3 / size.x);
					float num4 = num2 + num3;
					mIgnoreCallbacks = true;
					horizontalScrollBar.barSize = 1f - num4;
					horizontalScrollBar.scrollValue = ((!(num4 > 0.001f)) ? 0f : (num2 / num4));
					mIgnoreCallbacks = false;
				}
			}
			if (!(verticalScrollBar != null))
			{
				return;
			}
			Bounds bounds2 = this.bounds;
			Vector3 size2 = bounds2.size;
			if (size2.y > 0f)
			{
				Vector4 clipRange2 = mPanel.clipRange;
				float num5 = clipRange2.w * 0.5f;
				float num6 = clipRange2.y - num5 - bounds2.min.y;
				float num7 = bounds2.max.y - num5 - clipRange2.y;
				if (mPanel.clipping == UIDrawCall.Clipping.SoftClip)
				{
					num6 += mPanel.clipSoftness.y;
					num7 -= mPanel.clipSoftness.y;
				}
				num6 = Mathf.Clamp01(num6 / size2.y);
				num7 = Mathf.Clamp01(num7 / size2.y);
				float num8 = num6 + num7;
				mIgnoreCallbacks = true;
				verticalScrollBar.barSize = 1f - num8;
				verticalScrollBar.scrollValue = ((!(num8 > 0.001f)) ? 0f : (1f - num6 / num8));
				mIgnoreCallbacks = false;
			}
		}
		else if (recalculateBounds)
		{
			mCalculatedBounds = false;
		}
	}

	public void SetDragAmount(float x, float y, bool updateScrollbars)
	{
		DisableSpring();
		Bounds bounds = this.bounds;
		if (bounds.min.x == bounds.max.x || bounds.min.y == bounds.max.x)
		{
			return;
		}
		Vector4 clipRange = mPanel.clipRange;
		float num = clipRange.z * 0.5f;
		float num2 = clipRange.w * 0.5f;
		float num3 = bounds.min.x + num;
		float num4 = bounds.max.x - num;
		float num5 = bounds.min.y + num2;
		float num6 = bounds.max.y - num2;
		if (mPanel.clipping == UIDrawCall.Clipping.SoftClip)
		{
			num3 -= mPanel.clipSoftness.x;
			num4 += mPanel.clipSoftness.x;
			num5 -= mPanel.clipSoftness.y;
			num6 += mPanel.clipSoftness.y;
		}
		float num7 = Mathf.Lerp(num3, num4, x);
		float num8 = Mathf.Lerp(num6, num5, y);
		if (!updateScrollbars)
		{
			Vector3 localPosition = mTrans.localPosition;
			if (scale.x != 0f)
			{
				localPosition.x += clipRange.x - num7;
			}
			if (scale.y != 0f)
			{
				localPosition.y += clipRange.y - num8;
			}
			mTrans.localPosition = localPosition;
		}
		clipRange.x = num7;
		clipRange.y = num8;
		mPanel.clipRange = clipRange;
		if (updateScrollbars)
		{
			UpdateScrollbars(false);
		}
	}

	public void ResetPosition()
	{
		mCalculatedBounds = false;
		SetDragAmount(relativePositionOnReset.x, relativePositionOnReset.y, false);
		SetDragAmount(relativePositionOnReset.x, relativePositionOnReset.y, true);
	}

	private void OnHorizontalBar(UIScrollBar sb)
	{
		if (!mIgnoreCallbacks)
		{
			float x = ((!(horizontalScrollBar != null)) ? 0f : horizontalScrollBar.scrollValue);
			float y = ((!(verticalScrollBar != null)) ? 0f : verticalScrollBar.scrollValue);
			SetDragAmount(x, y, false);
		}
	}

	private void OnVerticalBar(UIScrollBar sb)
	{
		if (!mIgnoreCallbacks)
		{
			float x = ((!(horizontalScrollBar != null)) ? 0f : horizontalScrollBar.scrollValue);
			float y = ((!(verticalScrollBar != null)) ? 0f : verticalScrollBar.scrollValue);
			SetDragAmount(x, y, false);
		}
	}

	private void MoveRelative(Vector3 relative)
	{
		mTrans.localPosition += relative;
		Vector4 clipRange = mPanel.clipRange;
		clipRange.x -= relative.x;
		clipRange.y -= relative.y;
		mPanel.clipRange = clipRange;
		UpdateScrollbars(false);
	}

	private void MoveAbsolute(Vector3 absolute)
	{
		Vector3 vector = mTrans.InverseTransformPoint(absolute);
		Vector3 vector2 = mTrans.InverseTransformPoint(Vector3.zero);
		MoveRelative(vector - vector2);
	}

	public void Press(bool pressed)
	{
		if (!base.enabled || !base.gameObject.active)
		{
			return;
		}
		mTouches += (pressed ? 1 : (-1));
		mCalculatedBounds = false;
		mShouldMove = shouldMove;
		if (mShouldMove)
		{
			mPressed = pressed;
			if (pressed)
			{
				mMomentum = Vector3.zero;
				mScroll = 0f;
				DisableSpring();
				mLastPos = UICamera.lastHit.point;
				mPlane = new Plane(mTrans.rotation * Vector3.back, mLastPos);
			}
			else if (restrictWithinPanel && mPanel.clipping != 0 && dragEffect == DragEffect.MomentumAndSpring)
			{
				RestrictWithinBounds(false);
			}
		}
	}

	public void Drag(Vector2 delta)
	{
		if (!base.enabled || !base.gameObject.active || !mShouldMove)
		{
			return;
		}
		UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
		Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
		float enter = 0f;
		if (mPlane.Raycast(ray, out enter))
		{
			Vector3 point = ray.GetPoint(enter);
			Vector3 vector = point - mLastPos;
			mLastPos = point;
			if (vector.x != 0f || vector.y != 0f)
			{
				vector = mTrans.InverseTransformDirection(vector);
				vector.Scale(scale);
				vector = mTrans.TransformDirection(vector);
			}
			mMomentum = Vector3.Lerp(mMomentum, mMomentum + vector * (0.01f * momentumAmount), 0.67f);
			MoveAbsolute(vector);
			if (restrictWithinPanel && mPanel.clipping != 0 && dragEffect != DragEffect.MomentumAndSpring)
			{
				RestrictWithinBounds(false);
			}
		}
	}

	public void Scroll(float delta)
	{
		if (base.enabled && base.gameObject.active)
		{
			mShouldMove = shouldMove;
			if (Mathf.Sign(mScroll) != Mathf.Sign(delta))
			{
				mScroll = 0f;
			}
			mScroll += delta * scrollWheelFactor;
		}
	}

	private void LateUpdate()
	{
		if (mPanel.changedLastFrame)
		{
			UpdateScrollbars(true);
		}
		if (repositionClipping)
		{
			repositionClipping = false;
			mCalculatedBounds = false;
			SetDragAmount(relativePositionOnReset.x, relativePositionOnReset.y, true);
		}
		if (!Application.isPlaying)
		{
			return;
		}
		float num = UpdateRealTimeDelta();
		if (showScrollBars != 0)
		{
			bool flag = false;
			bool flag2 = false;
			if (showScrollBars != ShowCondition.WhenDragging || mTouches > 0)
			{
				flag = shouldMoveVertically;
				flag2 = shouldMoveHorizontally;
			}
			if ((bool)verticalScrollBar)
			{
				float alpha = verticalScrollBar.alpha;
				alpha += ((!flag) ? ((0f - num) * 3f) : (num * 6f));
				alpha = Mathf.Clamp01(alpha);
				if (verticalScrollBar.alpha != alpha)
				{
					verticalScrollBar.alpha = alpha;
				}
			}
			if ((bool)horizontalScrollBar)
			{
				float alpha2 = horizontalScrollBar.alpha;
				alpha2 += ((!flag2) ? ((0f - num) * 3f) : (num * 6f));
				alpha2 = Mathf.Clamp01(alpha2);
				if (horizontalScrollBar.alpha != alpha2)
				{
					horizontalScrollBar.alpha = alpha2;
				}
			}
		}
		if (mShouldMove && !mPressed)
		{
			mMomentum += scale * ((0f - mScroll) * 0.05f);
			if (mMomentum.magnitude > 0.0001f)
			{
				mScroll = NGUIMath.SpringLerp(mScroll, 0f, 20f, num);
				Vector3 absolute = NGUIMath.SpringDampen(ref mMomentum, 9f, num);
				MoveAbsolute(absolute);
				if (restrictWithinPanel && mPanel.clipping != 0)
				{
					RestrictWithinBounds(false);
				}
				return;
			}
			mScroll = 0f;
		}
		else
		{
			mScroll = 0f;
		}
		NGUIMath.SpringDampen(ref mMomentum, 9f, num);
	}
}
