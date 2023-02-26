using UnityEngine;

public abstract class UIItemSlot : MonoBehaviour
{
	public UISprite icon;

	public UIWidget background;

	public UILabel label;

	public AudioClip grabSound;

	public AudioClip placeSound;

	public AudioClip errorSound;

	private InvGameItem mItem;

	private string mText = string.Empty;

	private static InvGameItem mDraggedItem;

	protected abstract InvGameItem observedItem { get; }

	protected abstract InvGameItem Replace(InvGameItem item);

	private void OnTooltip(bool show)
	{
		UITooltip.ShowItem((!show) ? null : mItem);
	}

	private void OnClick()
	{
		if (mDraggedItem != null)
		{
			OnDrop(null);
		}
		else if (mItem != null)
		{
			mDraggedItem = Replace(null);
			if (mDraggedItem != null)
			{
				NGUITools.PlaySound(grabSound);
			}
			UpdateCursor();
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (mDraggedItem == null && mItem != null)
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			mDraggedItem = Replace(null);
			NGUITools.PlaySound(grabSound);
			UpdateCursor();
		}
	}

	private void OnDrop(GameObject go)
	{
		InvGameItem invGameItem = Replace(mDraggedItem);
		if (mDraggedItem == invGameItem)
		{
			NGUITools.PlaySound(errorSound);
		}
		else if (invGameItem != null)
		{
			NGUITools.PlaySound(grabSound);
		}
		else
		{
			NGUITools.PlaySound(placeSound);
		}
		mDraggedItem = invGameItem;
		UpdateCursor();
	}

	private void UpdateCursor()
	{
		if (mDraggedItem != null && mDraggedItem.baseItem != null)
		{
			UICursor.Set(mDraggedItem.baseItem.iconAtlas, mDraggedItem.baseItem.iconName);
		}
		else
		{
			UICursor.Clear();
		}
	}

	private void Update()
	{
		InvGameItem invGameItem = observedItem;
		if (mItem == invGameItem)
		{
			return;
		}
		mItem = invGameItem;
		InvBaseItem invBaseItem = ((invGameItem == null) ? null : invGameItem.baseItem);
		if (label != null)
		{
			string text = ((invGameItem == null) ? null : invGameItem.name);
			if (string.IsNullOrEmpty(mText))
			{
				mText = label.text;
			}
			label.text = ((text == null) ? mText : text);
		}
		if (icon != null)
		{
			if (invBaseItem == null || invBaseItem.iconAtlas == null)
			{
				icon.enabled = false;
			}
			else
			{
				icon.atlas = invBaseItem.iconAtlas;
				icon.spriteName = invBaseItem.iconName;
				icon.enabled = true;
				icon.MakePixelPerfect();
			}
		}
		if (background != null)
		{
			background.color = ((invGameItem == null) ? Color.white : invGameItem.color);
		}
	}
}
