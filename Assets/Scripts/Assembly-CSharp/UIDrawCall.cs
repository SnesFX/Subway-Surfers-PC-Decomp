using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/Internal/Draw Call")]
public class UIDrawCall : MonoBehaviour
{
	public enum Clipping
	{
		None = 0,
		HardClip = 1,
		AlphaClip = 2,
		SoftClip = 3
	}

	private Transform mTrans;

	private Material mSharedMat;

	private Mesh mMesh;

	private MeshFilter mFilter;

	private MeshRenderer mRen;

	private Clipping mClipping;

	private Vector4 mClipRange;

	private Vector2 mClipSoft;

	private Material mClippedMat;

	private Material mDepthMat;

	private int[] mIndices;

	private bool mDepthPass;

	private bool mReset = true;

	public bool depthPass
	{
		get
		{
			return mDepthPass;
		}
		set
		{
			if (mDepthPass != value)
			{
				mDepthPass = value;
				mReset = true;
			}
		}
	}

	public Transform cachedTransform
	{
		get
		{
			if (mTrans == null)
			{
				mTrans = base.transform;
			}
			return mTrans;
		}
	}

	public Material material
	{
		get
		{
			return mSharedMat;
		}
		set
		{
			mSharedMat = value;
		}
	}

	public int triangles
	{
		get
		{
			return mMesh.vertexCount >> 1;
		}
	}

	public Clipping clipping
	{
		get
		{
			return mClipping;
		}
		set
		{
			if (mClipping != value)
			{
				mClipping = value;
				mReset = true;
			}
		}
	}

	public Vector4 clipRange
	{
		get
		{
			return mClipRange;
		}
		set
		{
			mClipRange = value;
		}
	}

	public Vector2 clipSoftness
	{
		get
		{
			return mClipSoft;
		}
		set
		{
			mClipSoft = value;
		}
	}

	private void UpdateMaterials()
	{
		if (mClipping != 0)
		{
			Shader shader = null;
			if (mClipping != 0)
			{
				string text = mSharedMat.shader.name;
				text = text.Replace(" (HardClip)", string.Empty);
				text = text.Replace(" (AlphaClip)", string.Empty);
				text = text.Replace(" (SoftClip)", string.Empty);
				if (mClipping == Clipping.HardClip)
				{
					shader = Shader.Find(text + " (HardClip)");
				}
				else if (mClipping == Clipping.AlphaClip)
				{
					shader = Shader.Find(text + " (AlphaClip)");
				}
				else if (mClipping == Clipping.SoftClip)
				{
					shader = Shader.Find(text + " (SoftClip)");
				}
				if (shader == null)
				{
					mClipping = Clipping.None;
				}
			}
			if (shader != null)
			{
				mClippedMat = new Material(mSharedMat);
				mClippedMat.shader = shader;
			}
		}
		else if (mClippedMat != null)
		{
			NGUITools.Destroy(mClippedMat);
			mClippedMat = null;
		}
		if (mDepthPass)
		{
			if (mDepthMat == null)
			{
				Shader shader2 = Shader.Find("Depth");
				mDepthMat = new Material(shader2);
				mDepthMat.mainTexture = mSharedMat.mainTexture;
			}
		}
		else if (mDepthMat != null)
		{
			NGUITools.Destroy(mDepthMat);
			mDepthMat = null;
		}
		Material material = ((!(mClippedMat != null)) ? mSharedMat : mClippedMat);
		if (mDepthMat != null)
		{
			if (mRen.sharedMaterials == null || mRen.sharedMaterials.Length != 2 || !(mRen.sharedMaterials[1] == material))
			{
				mRen.sharedMaterials = new Material[2] { mDepthMat, material };
			}
		}
		else if (mRen.sharedMaterial != material)
		{
			mRen.sharedMaterials = new Material[1] { material };
		}
	}

	public void Set(BetterList<Vector3> verts, BetterList<Vector3> norms, BetterList<Vector4> tans, BetterList<Vector2> uvs, BetterList<Color> cols)
	{
		int size = verts.size;
		if (size > 0 && size == uvs.size && size == cols.size && size % 4 == 0)
		{
			int num = 0;
			int num2 = (size >> 1) * 3;
			if (mIndices == null || mIndices.Length != num2)
			{
				mIndices = new int[num2];
				for (int i = 0; i < size; i += 4)
				{
					mIndices[num++] = i;
					mIndices[num++] = i + 1;
					mIndices[num++] = i + 2;
					mIndices[num++] = i + 2;
					mIndices[num++] = i + 3;
					mIndices[num++] = i;
				}
			}
			if (mFilter == null)
			{
				mFilter = base.gameObject.GetComponent<MeshFilter>();
			}
			if (mFilter == null)
			{
				mFilter = base.gameObject.AddComponent<MeshFilter>();
			}
			if (mRen == null)
			{
				mRen = base.gameObject.GetComponent<MeshRenderer>();
			}
			if (mRen == null)
			{
				mRen = base.gameObject.AddComponent<MeshRenderer>();
				UpdateMaterials();
			}
			if (verts.size < 65000)
			{
				if (mMesh == null)
				{
					mMesh = new Mesh();
					mMesh.name = "UIDrawCall for " + mSharedMat.name;
				}
				else
				{
					mMesh.Clear();
				}
				mMesh.vertices = verts.ToArray();
				if (norms != null)
				{
					mMesh.normals = norms.ToArray();
				}
				if (tans != null)
				{
					mMesh.tangents = tans.ToArray();
				}
				mMesh.uv = uvs.ToArray();
				mMesh.colors = cols.ToArray();
				mMesh.triangles = mIndices;
				mMesh.RecalculateBounds();
				mFilter.mesh = mMesh;
			}
			else
			{
				if (mMesh != null)
				{
					mMesh.Clear();
				}
				Debug.LogError("Too many vertices on one panel: " + verts.size);
			}
		}
		else
		{
			if (mMesh != null)
			{
				mMesh.Clear();
			}
			Debug.LogError("UIWidgets must fill the buffer with 4 vertices per quad. Found " + size);
		}
	}

	private void OnWillRenderObject()
	{
		if (mReset)
		{
			mReset = false;
			UpdateMaterials();
		}
		if (mClippedMat != null)
		{
			mClippedMat.mainTextureOffset = new Vector2((0f - mClipRange.x) / mClipRange.z, (0f - mClipRange.y) / mClipRange.w);
			mClippedMat.mainTextureScale = new Vector2(1f / mClipRange.z, 1f / mClipRange.w);
			Vector2 vector = new Vector2(1000f, 1000f);
			if (mClipSoft.x > 0f)
			{
				vector.x = mClipRange.z / mClipSoft.x;
			}
			if (mClipSoft.y > 0f)
			{
				vector.y = mClipRange.w / mClipSoft.y;
			}
			mClippedMat.SetVector("_ClipSharpness", vector);
		}
	}

	private void OnDestroy()
	{
		NGUITools.DestroyImmediate(mMesh);
		NGUITools.DestroyImmediate(mClippedMat);
		NGUITools.DestroyImmediate(mDepthMat);
	}
}
