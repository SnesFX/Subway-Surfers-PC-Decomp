using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Texture")]
public class UITexture : UIWidget
{
	public override bool keepMaterial
	{
		get
		{
			return true;
		}
	}

	public override void MakePixelPerfect()
	{
		Texture texture = base.mainTexture;
		if (texture != null)
		{
			Vector3 localScale = base.cachedTransform.localScale;
			localScale.x = texture.width;
			localScale.y = texture.height;
			localScale.z = 1f;
			base.cachedTransform.localScale = localScale;
		}
		base.MakePixelPerfect();
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color> cols)
	{
		verts.Add(new Vector3(1f, 0f, 0f));
		verts.Add(new Vector3(1f, -1f, 0f));
		verts.Add(new Vector3(0f, -1f, 0f));
		verts.Add(new Vector3(0f, 0f, 0f));
		uvs.Add(Vector2.one);
		uvs.Add(new Vector2(1f, 0f));
		uvs.Add(Vector2.zero);
		uvs.Add(new Vector2(0f, 1f));
		cols.Add(base.color);
		cols.Add(base.color);
		cols.Add(base.color);
		cols.Add(base.color);
	}
}
