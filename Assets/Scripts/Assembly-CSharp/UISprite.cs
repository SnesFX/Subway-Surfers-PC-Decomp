using UnityEngine;

[AddComponentMenu("NGUI/UI/Sprite (Basic)")]
[ExecuteInEditMode]
public class UISprite : UIWidget
{
	[SerializeField]
	[HideInInspector]
	private UIAtlas mAtlas;

	[HideInInspector]
	[SerializeField]
	private string mSpriteName;

	protected UIAtlas.Sprite mSprite;

	protected Rect mOuter;

	protected Rect mOuterUV;

	private bool mSpriteSet;

	private string mLastName = string.Empty;

	public Rect outerUV
	{
		get
		{
			UpdateUVs(false);
			return mOuterUV;
		}
	}

	public UIAtlas atlas
	{
		get
		{
			return mAtlas;
		}
		set
		{
			if (mAtlas != value)
			{
				mAtlas = value;
				mSpriteSet = false;
				mSprite = null;
				material = ((!(mAtlas != null)) ? null : mAtlas.spriteMaterial);
				if (string.IsNullOrEmpty(mSpriteName) && mAtlas != null && mAtlas.spriteList.Count > 0)
				{
					sprite = mAtlas.spriteList[0];
					mSpriteName = mSprite.name;
				}
				if (!string.IsNullOrEmpty(mSpriteName))
				{
					string text = mSpriteName;
					mSpriteName = string.Empty;
					spriteName = text;
					mChanged = true;
					UpdateUVs(true);
				}
			}
		}
	}

	public string spriteName
	{
		get
		{
			return mSpriteName;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				if (!string.IsNullOrEmpty(mSpriteName))
				{
					mSpriteName = string.Empty;
					mSprite = null;
					mChanged = true;
				}
			}
			else if (mSpriteName != value)
			{
				mSpriteName = value;
				mSprite = null;
				mChanged = true;
				if (mSprite != null)
				{
					UpdateUVs(true);
				}
			}
		}
	}

	public UIAtlas.Sprite sprite
	{
		get
		{
			if (!mSpriteSet)
			{
				mSprite = null;
			}
			if (mSprite == null && mAtlas != null)
			{
				if (!string.IsNullOrEmpty(mSpriteName))
				{
					sprite = mAtlas.GetSprite(mSpriteName);
				}
				if (mSprite == null && mAtlas.spriteList.Count > 0)
				{
					sprite = mAtlas.spriteList[0];
					mSpriteName = mSprite.name;
				}
				if (mSprite != null)
				{
					material = mAtlas.spriteMaterial;
				}
			}
			return mSprite;
		}
		set
		{
			mSprite = value;
			mSpriteSet = true;
			material = ((mSprite == null || !(mAtlas != null)) ? null : mAtlas.spriteMaterial);
		}
	}

	public override Vector2 pivotOffset
	{
		get
		{
			Vector2 zero = Vector2.zero;
			if (sprite != null)
			{
				Pivot pivot = base.pivot;
				switch (pivot)
				{
				case Pivot.Top:
				case Pivot.Center:
				case Pivot.Bottom:
					zero.x = (-1f - mSprite.paddingRight + mSprite.paddingLeft) * 0.5f;
					break;
				case Pivot.TopRight:
				case Pivot.Right:
				case Pivot.BottomRight:
					zero.x = -1f - mSprite.paddingRight;
					break;
				default:
					zero.x = mSprite.paddingLeft;
					break;
				}
				switch (pivot)
				{
				case Pivot.Left:
				case Pivot.Center:
				case Pivot.Right:
					zero.y = (1f + mSprite.paddingBottom - mSprite.paddingTop) * 0.5f;
					break;
				case Pivot.BottomLeft:
				case Pivot.Bottom:
				case Pivot.BottomRight:
					zero.y = 1f + mSprite.paddingBottom;
					break;
				default:
					zero.y = 0f - mSprite.paddingTop;
					break;
				}
			}
			return zero;
		}
	}

	public override Material material
	{
		get
		{
			Material material = base.material;
			if (material == null)
			{
				material = ((!(mAtlas != null)) ? null : mAtlas.spriteMaterial);
				mSprite = null;
				this.material = material;
				if (material != null)
				{
					UpdateUVs(true);
				}
			}
			return material;
		}
	}

	public virtual Vector4 border
	{
		get
		{
			return Vector4.zero;
		}
	}

	public virtual void UpdateUVs(bool force)
	{
		if (sprite == null || (!force && !(mOuter != mSprite.outer)))
		{
			return;
		}
		Texture texture = base.mainTexture;
		if (texture != null)
		{
			mOuter = mSprite.outer;
			mOuterUV = mOuter;
			if (mAtlas.coordinates == UIAtlas.Coordinates.Pixels)
			{
				mOuterUV = NGUIMath.ConvertToTexCoords(mOuterUV, texture.width, texture.height);
			}
			mChanged = true;
		}
	}

	public override void MakePixelPerfect()
	{
		if (sprite != null)
		{
			Texture texture = base.mainTexture;
			Vector3 localScale = base.cachedTransform.localScale;
			if (texture != null)
			{
				Rect rect = NGUIMath.ConvertToPixels(outerUV, texture.width, texture.height, true);
				float pixelSize = atlas.pixelSize;
				localScale.x = Mathf.RoundToInt(rect.width * pixelSize);
				localScale.y = Mathf.RoundToInt(rect.height * pixelSize);
				localScale.z = 1f;
				base.cachedTransform.localScale = localScale;
			}
			int num = Mathf.RoundToInt(localScale.x * (1f + mSprite.paddingLeft + mSprite.paddingRight));
			int num2 = Mathf.RoundToInt(localScale.y * (1f + mSprite.paddingTop + mSprite.paddingBottom));
			Vector3 localPosition = base.cachedTransform.localPosition;
			localPosition.z = Mathf.RoundToInt(localPosition.z);
			if (num % 2 == 1 && (base.pivot == Pivot.Top || base.pivot == Pivot.Center || base.pivot == Pivot.Bottom))
			{
				localPosition.x = Mathf.Floor(localPosition.x) + 0.5f;
			}
			else
			{
				localPosition.x = Mathf.Round(localPosition.x);
			}
			if (num2 % 2 == 1 && (base.pivot == Pivot.Left || base.pivot == Pivot.Center || base.pivot == Pivot.Right))
			{
				localPosition.y = Mathf.Ceil(localPosition.y) - 0.5f;
			}
			else
			{
				localPosition.y = Mathf.Round(localPosition.y);
			}
			base.cachedTransform.localPosition = localPosition;
		}
	}

	protected override void OnStart()
	{
		if (mAtlas != null)
		{
			UpdateUVs(true);
		}
	}

	public override bool OnUpdate()
	{
		if (mLastName != mSpriteName)
		{
			mSprite = null;
			mChanged = true;
			mLastName = mSpriteName;
			UpdateUVs(false);
			return true;
		}
		UpdateUVs(false);
		return false;
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color> cols)
	{
		Vector2 item = new Vector2(mOuterUV.xMin, mOuterUV.yMin);
		Vector2 item2 = new Vector2(mOuterUV.xMax, mOuterUV.yMax);
		verts.Add(new Vector3(1f, 0f, 0f));
		verts.Add(new Vector3(1f, -1f, 0f));
		verts.Add(new Vector3(0f, -1f, 0f));
		verts.Add(new Vector3(0f, 0f, 0f));
		uvs.Add(item2);
		uvs.Add(new Vector2(item2.x, item.y));
		uvs.Add(item);
		uvs.Add(new Vector2(item.x, item2.y));
		cols.Add(base.color);
		cols.Add(base.color);
		cols.Add(base.color);
		cols.Add(base.color);
	}
}
