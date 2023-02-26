using UnityEngine;

[AddComponentMenu("NGUI/UI/Anchor")]
[ExecuteInEditMode]
public class UIAnchor : MonoBehaviour
{
	public enum Side
	{
		BottomLeft = 0,
		Left = 1,
		TopLeft = 2,
		Top = 3,
		TopRight = 4,
		Right = 5,
		BottomRight = 6,
		Bottom = 7,
		Center = 8
	}

	public Camera uiCamera;

	public Side side = Side.Center;

	public bool halfPixelOffset = true;

	public float depthOffset;

	public Vector2 relativeOffset = Vector2.zero;

	[SerializeField]
	[HideInInspector]
	private bool stretchToFill;

	private Transform mTrans;

	private bool mIsWindows;

	private void Start()
	{
		if (stretchToFill)
		{
			stretchToFill = false;
			UIStretch uIStretch = base.gameObject.AddComponent<UIStretch>();
			uIStretch.style = UIStretch.Style.Both;
			uIStretch.uiCamera = uiCamera;
		}
	}

	private void OnEnable()
	{
		mTrans = base.transform;
		mIsWindows = Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;
		if (uiCamera == null)
		{
			uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
	}

	private void Update()
	{
		if (!(uiCamera != null))
		{
			return;
		}
		Rect pixelRect = uiCamera.pixelRect;
		float x = (pixelRect.xMin + pixelRect.xMax) * 0.5f;
		float y = (pixelRect.yMin + pixelRect.yMax) * 0.5f;
		Vector3 position = new Vector3(x, y, depthOffset);
		if (side != Side.Center)
		{
			if (side == Side.Right || side == Side.TopRight || side == Side.BottomRight)
			{
				position.x = pixelRect.xMax;
			}
			else if (side == Side.Top || side == Side.Center || side == Side.Bottom)
			{
				position.x = x;
			}
			else
			{
				position.x = pixelRect.xMin;
			}
			if (side == Side.Top || side == Side.TopRight || side == Side.TopLeft)
			{
				position.y = pixelRect.yMax;
			}
			else if (side == Side.Left || side == Side.Center || side == Side.Right)
			{
				position.y = y;
			}
			else
			{
				position.y = pixelRect.yMin;
			}
		}
		float width = pixelRect.width;
		float height = pixelRect.height;
		position.x += relativeOffset.x * width;
		position.y += relativeOffset.y * height;
		if (uiCamera.orthographic)
		{
			position.x = Mathf.RoundToInt(position.x);
			position.y = Mathf.RoundToInt(position.y);
			if (halfPixelOffset && mIsWindows)
			{
				position.x -= 0.5f;
				position.y += 0.5f;
			}
		}
		position = uiCamera.ScreenToWorldPoint(position);
		if (mTrans.position != position)
		{
			mTrans.position = position;
		}
	}
}
