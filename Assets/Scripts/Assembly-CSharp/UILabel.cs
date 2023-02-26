using UnityEngine;

[AddComponentMenu("NGUI/UI/Label")]
[ExecuteInEditMode]
public class UILabel : UIWidget
{
	public enum Effect
	{
		None = 0,
		Shadow = 1,
		Outline = 2
	}

	[SerializeField]
	[HideInInspector]
	private UIFont mFont;

	[HideInInspector]
	[SerializeField]
	private string mText = string.Empty;

	[HideInInspector]
	[SerializeField]
	private int mMaxLineWidth;

	[HideInInspector]
	[SerializeField]
	private bool mEncoding = true;

	[SerializeField]
	[HideInInspector]
	private bool mMultiline = true;

	[SerializeField]
	[HideInInspector]
	private bool mPassword;

	[HideInInspector]
	[SerializeField]
	private bool mShowLastChar;

	[HideInInspector]
	[SerializeField]
	private Effect mEffectStyle;

	[HideInInspector]
	[SerializeField]
	private Color mEffectColor = Color.black;

	[SerializeField]
	[HideInInspector]
	private float mLineWidth;

	private bool mShouldBeProcessed = true;

	private string mProcessedText;

	private Vector3 mLastScale = Vector3.one;

	private string mLastText = string.Empty;

	private int mLastWidth;

	private bool mLastEncoding = true;

	private bool mLastMulti = true;

	private bool mLastPass;

	private bool mLastShow;

	private Effect mLastEffect;

	private Color mLastColor = Color.black;

	private Vector3 mSize = Vector3.zero;

	private bool hasChanged
	{
		get
		{
			return mShouldBeProcessed || mLastText != text || mLastWidth != mMaxLineWidth || mLastEncoding != mEncoding || mLastMulti != mMultiline || mLastPass != mPassword || mLastShow != mShowLastChar || mLastEffect != mEffectStyle || mLastColor != mEffectColor;
		}
		set
		{
			if (value)
			{
				mChanged = true;
				mShouldBeProcessed = true;
				return;
			}
			mShouldBeProcessed = false;
			mLastText = text;
			mLastWidth = mMaxLineWidth;
			mLastEncoding = mEncoding;
			mLastMulti = mMultiline;
			mLastPass = mPassword;
			mLastShow = mShowLastChar;
			mLastEffect = mEffectStyle;
			mLastColor = mEffectColor;
		}
	}

	public UIFont font
	{
		get
		{
			return mFont;
		}
		set
		{
			if (mFont != value)
			{
				mFont = value;
				material = ((!(mFont != null)) ? null : mFont.material);
				mChanged = true;
				hasChanged = true;
				MarkAsChanged();
			}
		}
	}

	public string text
	{
		get
		{
			return mText;
		}
		set
		{
			if (value != null && mText != value)
			{
				mText = value;
				hasChanged = true;
			}
		}
	}

	public bool supportEncoding
	{
		get
		{
			return mEncoding;
		}
		set
		{
			if (mEncoding != value)
			{
				mEncoding = value;
				hasChanged = true;
				if (value)
				{
					mPassword = false;
				}
			}
		}
	}

	public int lineWidth
	{
		get
		{
			return mMaxLineWidth;
		}
		set
		{
			if (mMaxLineWidth != value)
			{
				mMaxLineWidth = value;
				hasChanged = true;
			}
		}
	}

	public bool multiLine
	{
		get
		{
			return mMultiline;
		}
		set
		{
			if (mMultiline != value)
			{
				mMultiline = value;
				hasChanged = true;
				if (value)
				{
					mPassword = false;
				}
			}
		}
	}

	public bool password
	{
		get
		{
			return mPassword;
		}
		set
		{
			if (mPassword != value)
			{
				mPassword = value;
				mMultiline = false;
				mEncoding = false;
				hasChanged = true;
			}
		}
	}

	public bool showLastPasswordChar
	{
		get
		{
			return mShowLastChar;
		}
		set
		{
			if (mShowLastChar != value)
			{
				mShowLastChar = value;
				hasChanged = true;
			}
		}
	}

	public Effect effectStyle
	{
		get
		{
			return mEffectStyle;
		}
		set
		{
			if (mEffectStyle != value)
			{
				mEffectStyle = value;
				hasChanged = true;
			}
		}
	}

	public Color effectColor
	{
		get
		{
			return mEffectColor;
		}
		set
		{
			if (mEffectColor != value)
			{
				mEffectColor = value;
				if (mEffectStyle != 0)
				{
					hasChanged = true;
				}
			}
		}
	}

	public string processedText
	{
		get
		{
			if (mLastScale != base.cachedTransform.localScale)
			{
				mLastScale = base.cachedTransform.localScale;
				mShouldBeProcessed = true;
			}
			if (hasChanged)
			{
				ProcessText();
			}
			return mProcessedText;
		}
	}

	public override Material material
	{
		get
		{
			Material material = base.material;
			if (material == null)
			{
				material = (this.material = ((!(mFont != null)) ? null : mFont.material));
			}
			return material;
		}
	}

	public override Vector2 relativeSize
	{
		get
		{
			if (mFont == null)
			{
				return Vector3.one;
			}
			if (hasChanged)
			{
				ProcessText();
			}
			return mSize;
		}
	}

	protected override void OnStart()
	{
		if (mLineWidth > 0f)
		{
			mMaxLineWidth = Mathf.RoundToInt(mLineWidth);
			mLineWidth = 0f;
		}
	}

	public override void MarkAsChanged()
	{
		hasChanged = true;
		base.MarkAsChanged();
	}

	private void ProcessText()
	{
		mChanged = true;
		hasChanged = false;
		mLastText = mText;
		mProcessedText = mText.Replace("\\n", "\n");
		if (mPassword)
		{
			mProcessedText = mFont.WrapText(mProcessedText, 100000f, false, false);
			string text = string.Empty;
			if (mShowLastChar)
			{
				int i = 1;
				for (int length = mProcessedText.Length; i < length; i++)
				{
					text += "*";
				}
				if (mProcessedText.Length > 0)
				{
					text += mProcessedText[mProcessedText.Length - 1];
				}
			}
			else
			{
				int j = 0;
				for (int length2 = mProcessedText.Length; j < length2; j++)
				{
					text += "*";
				}
			}
			mProcessedText = text;
		}
		else if (mMaxLineWidth > 0)
		{
			mProcessedText = mFont.WrapText(mProcessedText, (float)mMaxLineWidth / base.cachedTransform.localScale.x, mMultiline, mEncoding);
		}
		else if (!mMultiline)
		{
			mProcessedText = mFont.WrapText(mProcessedText, 100000f, false, mEncoding);
		}
		mSize = (string.IsNullOrEmpty(mProcessedText) ? Vector2.one : mFont.CalculatePrintedSize(mProcessedText, mEncoding));
		float x = base.cachedTransform.localScale.x;
		mSize.x = Mathf.Max(mSize.x, (!(mFont != null) || !(x > 1f)) ? 1f : ((float)lineWidth / x));
		mSize.y = Mathf.Max(mSize.y, 1f);
	}

	public void MakePositionPerfect()
	{
		float num = ((!(font.atlas != null)) ? 1f : font.atlas.pixelSize);
		Vector3 localScale = base.cachedTransform.localScale;
		if (mFont.size == Mathf.RoundToInt(localScale.x / num) && mFont.size == Mathf.RoundToInt(localScale.y / num) && base.cachedTransform.localRotation == Quaternion.identity)
		{
			Vector2 vector = relativeSize * localScale.x;
			int num2 = Mathf.RoundToInt(vector.x / num);
			int num3 = Mathf.RoundToInt(vector.y / num);
			Vector3 localPosition = base.cachedTransform.localPosition;
			localPosition.x = Mathf.FloorToInt(localPosition.x / num);
			localPosition.y = Mathf.CeilToInt(localPosition.y / num);
			localPosition.z = Mathf.RoundToInt(localPosition.z);
			if (num2 % 2 == 1 && (base.pivot == Pivot.Top || base.pivot == Pivot.Center || base.pivot == Pivot.Bottom))
			{
				localPosition.x += 0.5f;
			}
			if (num3 % 2 == 1 && (base.pivot == Pivot.Left || base.pivot == Pivot.Center || base.pivot == Pivot.Right))
			{
				localPosition.y -= 0.5f;
			}
			localPosition.x *= num;
			localPosition.y *= num;
			if (base.cachedTransform.localPosition != localPosition)
			{
				base.cachedTransform.localPosition = localPosition;
			}
		}
	}

	public override void MakePixelPerfect()
	{
		if (mFont != null)
		{
			float num = ((!(font.atlas != null)) ? 1f : font.atlas.pixelSize);
			Vector3 localScale = base.cachedTransform.localScale;
			localScale.x = (float)mFont.size * num;
			localScale.y = localScale.x;
			localScale.z = 1f;
			Vector2 vector = relativeSize * localScale.x;
			int num2 = Mathf.RoundToInt(vector.x / num);
			int num3 = Mathf.RoundToInt(vector.y / num);
			Vector3 localPosition = base.cachedTransform.localPosition;
			localPosition.x = Mathf.FloorToInt(localPosition.x / num);
			localPosition.y = Mathf.CeilToInt(localPosition.y / num);
			localPosition.z = Mathf.RoundToInt(localPosition.z);
			if (base.cachedTransform.localRotation == Quaternion.identity)
			{
				if (num2 % 2 == 1 && (base.pivot == Pivot.Top || base.pivot == Pivot.Center || base.pivot == Pivot.Bottom))
				{
					localPosition.x += 0.5f;
				}
				if (num3 % 2 == 1 && (base.pivot == Pivot.Left || base.pivot == Pivot.Center || base.pivot == Pivot.Right))
				{
					localPosition.y -= 0.5f;
				}
			}
			localPosition.x *= num;
			localPosition.y *= num;
			base.cachedTransform.localPosition = localPosition;
			base.cachedTransform.localScale = localScale;
		}
		else
		{
			base.MakePixelPerfect();
		}
	}

	private void ApplyShadow(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color> cols, int start, int end, float x, float y)
	{
		Color color = mEffectColor;
		color.a *= base.color.a;
		for (int i = start; i < end; i++)
		{
			verts.Add(verts.buffer[i]);
			uvs.Add(uvs.buffer[i]);
			cols.Add(cols.buffer[i]);
			Vector3 vector = verts.buffer[i];
			vector.x += x;
			vector.y += y;
			verts.buffer[i] = vector;
			cols.buffer[i] = color;
		}
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color> cols)
	{
		if (mFont == null)
		{
			return;
		}
		MakePositionPerfect();
		Pivot pivot = base.pivot;
		int size = verts.size;
		switch (pivot)
		{
		case Pivot.TopLeft:
		case Pivot.Left:
		case Pivot.BottomLeft:
			mFont.Print(processedText, base.color, verts, uvs, cols, mEncoding, UIFont.Alignment.Left, 0);
			break;
		case Pivot.TopRight:
		case Pivot.Right:
		case Pivot.BottomRight:
			mFont.Print(processedText, base.color, verts, uvs, cols, mEncoding, UIFont.Alignment.Right, Mathf.RoundToInt(relativeSize.x * (float)mFont.size));
			break;
		default:
			mFont.Print(processedText, base.color, verts, uvs, cols, mEncoding, UIFont.Alignment.Center, Mathf.RoundToInt(relativeSize.x * (float)mFont.size));
			break;
		}
		if (effectStyle == Effect.None)
		{
			return;
		}
		Vector3 localScale = base.cachedTransform.localScale;
		if (localScale.x != 0f && localScale.y != 0f)
		{
			int size2 = verts.size;
			float num = 1f / (float)mFont.size;
			ApplyShadow(verts, uvs, cols, size, size2, num, 0f - num);
			if (effectStyle == Effect.Outline)
			{
				size = size2;
				size2 = verts.size;
				ApplyShadow(verts, uvs, cols, size, size2, 0f - num, num);
				size = size2;
				size2 = verts.size;
				ApplyShadow(verts, uvs, cols, size, size2, num, num);
				size = size2;
				size2 = verts.size;
				ApplyShadow(verts, uvs, cols, size, size2, 0f - num, 0f - num);
			}
		}
	}
}
