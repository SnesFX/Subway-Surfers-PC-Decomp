using UnityEngine;

public class UINode
{
	private int mVisibleFlag = -1;

	public Transform trans;

	public UIWidget widget;

	public bool lastActive;

	public Vector3 lastPos;

	public Quaternion lastRot;

	public Vector3 lastScale;

	public int changeFlag = -1;

	private GameObject mGo;

	public int visibleFlag
	{
		get
		{
			return (!(widget != null)) ? mVisibleFlag : widget.visibleFlag;
		}
		set
		{
			if (widget != null)
			{
				widget.visibleFlag = value;
			}
			else
			{
				mVisibleFlag = value;
			}
		}
	}

	public UINode(Transform t)
	{
		trans = t;
		lastPos = trans.localPosition;
		lastRot = trans.localRotation;
		lastScale = trans.localScale;
		mGo = t.gameObject;
	}

	public bool HasChanged()
	{
		bool flag = mGo.active && (widget == null || (widget.enabled && widget.color.a > 0.001f));
		if (lastActive != flag || (flag && (lastPos != trans.localPosition || lastRot != trans.localRotation || lastScale != trans.localScale)))
		{
			lastActive = flag;
			lastPos = trans.localPosition;
			lastRot = trans.localRotation;
			lastScale = trans.localScale;
			return true;
		}
		return false;
	}
}
