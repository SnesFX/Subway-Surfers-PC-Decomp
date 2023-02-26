using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Game/UI/Tooltip")]
public class UITooltip : MonoBehaviour
{
	private static UITooltip mInstance;

	public Camera uiCamera;

	public UILabel text;

	public UISlicedSprite background;

	public float appearSpeed = 10f;

	public bool scalingTransitions = true;

	private Transform mTrans;

	private float mTarget;

	private float mCurrent;

	private Vector3 mPos;

	private Vector3 mSize;

	private UIWidget[] mWidgets;

	private void Awake()
	{
		mInstance = this;
	}

	private void OnDestroy()
	{
		mInstance = null;
	}

	private void Start()
	{
		mTrans = base.transform;
		mWidgets = GetComponentsInChildren<UIWidget>();
		mPos = mTrans.localPosition;
		mSize = mTrans.localScale;
		if (uiCamera == null)
		{
			uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		SetAlpha(0f);
	}

	private void Update()
	{
		if (mCurrent != mTarget)
		{
			mCurrent = Mathf.Lerp(mCurrent, mTarget, Time.deltaTime * appearSpeed);
			if (Mathf.Abs(mCurrent - mTarget) < 0.001f)
			{
				mCurrent = mTarget;
			}
			SetAlpha(mCurrent * mCurrent);
			if (scalingTransitions)
			{
				Vector3 vector = mSize * 0.25f;
				vector.y = 0f - vector.y;
				Vector3 localScale = Vector3.one * (1.5f - mCurrent * 0.5f);
				Vector3 pos = Vector3.Lerp(mPos - vector, mPos, mCurrent);
				pos = NGUIMath.ApplyHalfPixelOffset(pos);
				mTrans.localPosition = pos;
				mTrans.localScale = localScale;
			}
		}
	}

	private void SetAlpha(float val)
	{
		int i = 0;
		for (int num = mWidgets.Length; i < num; i++)
		{
			UIWidget uIWidget = mWidgets[i];
			Color color = uIWidget.color;
			color.a = val;
			uIWidget.color = color;
		}
	}

	private void SetText(string tooltipText)
	{
		if (text != null && !string.IsNullOrEmpty(tooltipText))
		{
			mTarget = 1f;
			if (text != null)
			{
				text.text = tooltipText;
			}
			mPos = Input.mousePosition;
			if (background != null)
			{
				Transform transform = background.transform;
				Transform transform2 = text.transform;
				Vector3 localPosition = transform2.localPosition;
				Vector3 localScale = transform2.localScale;
				mSize = text.relativeSize;
				mSize.x *= localScale.x;
				mSize.y *= localScale.y;
				mSize.x += background.border.x + background.border.z + (localPosition.x - background.border.x) * 2f;
				mSize.y += background.border.y + background.border.w + (0f - localPosition.y - background.border.y) * 2f;
				mSize.z = 1f;
				transform.localScale = mSize;
			}
			if (uiCamera != null)
			{
				mPos.x = Mathf.Clamp01(mPos.x / (float)Screen.width);
				mPos.y = Mathf.Clamp01(mPos.y / (float)Screen.height);
				float num = uiCamera.orthographicSize / mTrans.parent.lossyScale.y;
				float num2 = (float)Screen.height * 0.5f / num;
				Vector2 vector = new Vector2(num2 * mSize.x / (float)Screen.width, num2 * mSize.y / (float)Screen.height);
				mPos.x = Mathf.Min(mPos.x, 1f - vector.x);
				mPos.y = Mathf.Max(mPos.y, vector.y);
				mTrans.position = uiCamera.ViewportToWorldPoint(mPos);
				mPos = mTrans.localPosition;
			}
			else
			{
				if (mPos.x + mSize.x > (float)Screen.width)
				{
					mPos.x = (float)Screen.width - mSize.x;
				}
				if (mPos.y - mSize.y < 0f)
				{
					mPos.y = mSize.y;
				}
				mPos.x -= (float)Screen.width * 0.5f;
				mPos.y -= (float)Screen.height * 0.5f;
			}
			mTrans.localPosition = NGUIMath.ApplyHalfPixelOffset(mPos);
		}
		else
		{
			mTarget = 0f;
		}
	}

	public static void ShowText(string tooltipText)
	{
		if (mInstance != null)
		{
			mInstance.SetText(tooltipText);
		}
	}

	public static void ShowItem(InvGameItem item)
	{
		if (item != null)
		{
			InvBaseItem baseItem = item.baseItem;
			if (baseItem != null)
			{
				string text = "[" + NGUITools.EncodeColor(item.color) + "]" + item.name + "[-]\n";
				string text2 = text;
				text = text2 + "[AFAFAF]Level " + item.itemLevel + " " + baseItem.slot;
				List<InvStat> list = item.CalculateStats();
				int i = 0;
				for (int count = list.Count; i < count; i++)
				{
					InvStat invStat = list[i];
					if (invStat.amount != 0)
					{
						text = ((invStat.amount >= 0) ? (text + "\n[00FF00]+" + invStat.amount) : (text + "\n[FF0000]" + invStat.amount));
						if (invStat.modifier == InvStat.Modifier.Percent)
						{
							text += "%";
						}
						text = text + " " + invStat.id;
						text += "[-]";
					}
				}
				if (!string.IsNullOrEmpty(baseItem.description))
				{
					text = text + "\n[FF9900]" + baseItem.description;
				}
				ShowText(text);
				return;
			}
		}
		if (mInstance != null)
		{
			mInstance.mTarget = 0f;
		}
	}
}
