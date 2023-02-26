using UnityEngine;

[AddComponentMenu("NGUI/UI/Sprite (Tiled)")]
[ExecuteInEditMode]
public class UITiledSprite : UISlicedSprite
{
	public override Vector4 border
	{
		get
		{
			return Vector4.zero;
		}
	}

	public override void MakePixelPerfect()
	{
		Vector3 localPosition = base.cachedTransform.localPosition;
		localPosition.x = Mathf.RoundToInt(localPosition.x);
		localPosition.y = Mathf.RoundToInt(localPosition.y);
		localPosition.z = Mathf.RoundToInt(localPosition.z);
		base.cachedTransform.localPosition = localPosition;
		Vector3 localScale = base.cachedTransform.localScale;
		localScale.x = Mathf.RoundToInt(localScale.x);
		localScale.y = Mathf.RoundToInt(localScale.y);
		localScale.z = 1f;
		base.cachedTransform.localScale = localScale;
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color> cols)
	{
		Texture texture = material.mainTexture;
		if (texture == null)
		{
			return;
		}
		Rect rect = mInner;
		if (base.atlas.coordinates == UIAtlas.Coordinates.TexCoords)
		{
			rect = NGUIMath.ConvertToPixels(rect, texture.width, texture.height, true);
		}
		Vector2 vector = base.cachedTransform.localScale;
		float pixelSize = base.atlas.pixelSize;
		float num = Mathf.Abs(rect.width / vector.x) * pixelSize;
		float num2 = Mathf.Abs(rect.height / vector.y) * pixelSize;
		if (num < 0.01f || num2 < 0.01f)
		{
			Debug.LogWarning("The tiled sprite (" + NGUITools.GetHierarchy(base.gameObject) + ") is too small.\nConsider using a bigger one.");
			num = 0.01f;
			num2 = 0.01f;
		}
		Vector2 vector2 = new Vector2(rect.xMin / (float)texture.width, rect.yMin / (float)texture.height);
		Vector2 vector3 = new Vector2(rect.xMax / (float)texture.width, rect.yMax / (float)texture.height);
		Vector2 vector4 = vector3;
		for (float num3 = 0f; num3 < 1f; num3 += num2)
		{
			float num4 = 0f;
			vector4.x = vector3.x;
			float num5 = num3 + num2;
			if (num5 > 1f)
			{
				vector4.y = vector2.y + (vector3.y - vector2.y) * (1f - num3) / (num5 - num3);
				num5 = 1f;
			}
			for (; num4 < 1f; num4 += num)
			{
				float num6 = num4 + num;
				if (num6 > 1f)
				{
					vector4.x = vector2.x + (vector3.x - vector2.x) * (1f - num4) / (num6 - num4);
					num6 = 1f;
				}
				verts.Add(new Vector3(num6, 0f - num3, 0f));
				verts.Add(new Vector3(num6, 0f - num5, 0f));
				verts.Add(new Vector3(num4, 0f - num5, 0f));
				verts.Add(new Vector3(num4, 0f - num3, 0f));
				uvs.Add(new Vector2(vector4.x, 1f - vector2.y));
				uvs.Add(new Vector2(vector4.x, 1f - vector4.y));
				uvs.Add(new Vector2(vector2.x, 1f - vector4.y));
				uvs.Add(new Vector2(vector2.x, 1f - vector2.y));
				cols.Add(base.color);
				cols.Add(base.color);
				cols.Add(base.color);
				cols.Add(base.color);
			}
		}
	}
}
