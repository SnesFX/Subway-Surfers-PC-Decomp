using System;
using UnityEngine;

public abstract class UIWidget : MonoBehaviour
{
	public enum Pivot
	{
		TopLeft = 0,
		Top = 1,
		TopRight = 2,
		Left = 3,
		Center = 4,
		Right = 5,
		BottomLeft = 6,
		Bottom = 7,
		BottomRight = 8
	}

	[SerializeField]
	[HideInInspector]
	private Material mMat;

	[SerializeField]
	[HideInInspector]
	private Color mColor = Color.white;

	[HideInInspector]
	[SerializeField]
	private Pivot mPivot = Pivot.Center;

	[HideInInspector]
	[SerializeField]
	private int mDepth;

	private Transform mTrans;

	private Texture mTex;

	private UIPanel mPanel;

	protected bool mChanged = true;

	protected bool mPlayMode = true;

	private Vector3 mDiffPos;

	private Quaternion mDiffRot;

	private Vector3 mDiffScale;

	private int mVisibleFlag = -1;

	private UIGeometry mGeom = new UIGeometry();

	public Color color
	{
		get
		{
			return mColor;
		}
		set
		{
			if (mColor != value)
			{
				mColor = value;
				mChanged = true;
			}
		}
	}

	public float alpha
	{
		get
		{
			return mColor.a;
		}
		set
		{
			Color color = mColor;
			color.a = value;
			this.color = color;
		}
	}

	public Pivot pivot
	{
		get
		{
			return mPivot;
		}
		set
		{
			if (mPivot != value)
			{
				mPivot = value;
				mChanged = true;
			}
		}
	}

	public int depth
	{
		get
		{
			return mDepth;
		}
		set
		{
			if (mDepth != value)
			{
				mDepth = value;
				if (mPanel != null)
				{
					mPanel.MarkMaterialAsChanged(material, true);
				}
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

	public virtual Material material
	{
		get
		{
			return mMat;
		}
		set
		{
			if (mMat != value)
			{
				if (mMat != null && mPanel != null)
				{
					mPanel.RemoveWidget(this);
				}
				mPanel = null;
				mMat = value;
				mTex = null;
				if (mMat != null)
				{
					CreatePanel();
				}
			}
		}
	}

	public Texture mainTexture
	{
		get
		{
			if (mTex == null)
			{
				Material material = this.material;
				if (material != null)
				{
					mTex = material.mainTexture;
				}
			}
			return mTex;
		}
	}

	public UIPanel panel
	{
		get
		{
			if (mPanel == null)
			{
				CreatePanel();
			}
			return mPanel;
		}
		set
		{
			mPanel = value;
		}
	}

	public int visibleFlag
	{
		get
		{
			return mVisibleFlag;
		}
		set
		{
			mVisibleFlag = value;
		}
	}

	public virtual Vector2 pivotOffset
	{
		get
		{
			Vector2 zero = Vector2.zero;
			if (mPivot == Pivot.Top || mPivot == Pivot.Center || mPivot == Pivot.Bottom)
			{
				zero.x = -0.5f;
			}
			else if (mPivot == Pivot.TopRight || mPivot == Pivot.Right || mPivot == Pivot.BottomRight)
			{
				zero.x = -1f;
			}
			if (mPivot == Pivot.Left || mPivot == Pivot.Center || mPivot == Pivot.Right)
			{
				zero.y = 0.5f;
			}
			else if (mPivot == Pivot.BottomLeft || mPivot == Pivot.Bottom || mPivot == Pivot.BottomRight)
			{
				zero.y = 1f;
			}
			return zero;
		}
	}

	[Obsolete("Use 'relativeSize' instead")]
	public Vector2 visibleSize
	{
		get
		{
			return relativeSize;
		}
	}

	public virtual Vector2 relativeSize
	{
		get
		{
			return Vector2.one;
		}
	}

	public virtual bool keepMaterial
	{
		get
		{
			return false;
		}
	}

	public static int CompareFunc(UIWidget left, UIWidget right)
	{
		if (left.mDepth > right.mDepth)
		{
			return 1;
		}
		if (left.mDepth < right.mDepth)
		{
			return -1;
		}
		return 0;
	}

	public virtual void MarkAsChanged()
	{
		mChanged = true;
		if (mPanel != null && base.enabled && base.gameObject.active && !Application.isPlaying && material != null)
		{
			mPanel.AddWidget(this);
			CheckLayer();
		}
	}

	private void CreatePanel()
	{
		if (mPanel == null && base.enabled && base.gameObject.active && material != null)
		{
			mPanel = UIPanel.Find(cachedTransform);
			if (mPanel != null)
			{
				CheckLayer();
				mPanel.AddWidget(this);
				mChanged = true;
			}
		}
	}

	private void CheckLayer()
	{
		if (mPanel != null && mPanel.gameObject.layer != base.gameObject.layer)
		{
			Debug.LogWarning("You can't place widgets on a layer different than the UIPanel that manages them.\nIf you want to move widgets to a different layer, parent them to a new panel instead.", this);
			base.gameObject.layer = mPanel.gameObject.layer;
		}
	}

	private void CheckParent()
	{
		if (!(mPanel != null))
		{
			return;
		}
		bool flag = true;
		Transform parent = cachedTransform.parent;
		while (parent != null && !(parent == mPanel.cachedTransform))
		{
			if (!mPanel.WatchesTransform(parent))
			{
				flag = false;
				break;
			}
			parent = parent.parent;
		}
		if (!flag)
		{
			if (!keepMaterial)
			{
				material = null;
			}
			mPanel = null;
			CreatePanel();
		}
	}

	private void Awake()
	{
		if (GetComponents<UIWidget>().Length > 1)
		{
			Debug.LogError("Can't have more than one widget on the same game object.\nDestroying the second one.", this);
			NGUITools.Destroy(this);
		}
		else
		{
			mPlayMode = Application.isPlaying;
		}
	}

	private void OnEnable()
	{
		mChanged = true;
		if (!keepMaterial)
		{
			mMat = null;
			mTex = null;
		}
		if (mPanel != null && material != null)
		{
			mPanel.MarkMaterialAsChanged(mMat, false);
		}
	}

	private void Start()
	{
		OnStart();
		CreatePanel();
	}

	private void Update()
	{
		CheckLayer();
		if (mPanel == null)
		{
			CreatePanel();
		}
		Vector3 localScale = cachedTransform.localScale;
		if (localScale.z != 1f)
		{
			localScale.z = 1f;
			mTrans.localScale = localScale;
		}
	}

	private void OnDisable()
	{
		if (!keepMaterial)
		{
			material = null;
		}
		else if (mPanel != null)
		{
			mPanel.RemoveWidget(this);
		}
		mPanel = null;
	}

	private void OnDestroy()
	{
		if (mPanel != null)
		{
			mPanel.RemoveWidget(this);
			mPanel = null;
		}
	}

	public bool UpdateGeometry(ref Matrix4x4 worldToPanel, bool parentMoved, bool generateNormals)
	{
		if (material == null)
		{
			return false;
		}
		if (OnUpdate() || mChanged)
		{
			mChanged = false;
			mGeom.Clear();
			OnFill(mGeom.verts, mGeom.uvs, mGeom.cols);
			if (mGeom.hasVertices)
			{
				Vector3 vector = pivotOffset;
				Vector2 vector2 = relativeSize;
				vector.x *= vector2.x;
				vector.y *= vector2.y;
				mGeom.ApplyOffset(vector);
				mGeom.ApplyTransform(worldToPanel * cachedTransform.localToWorldMatrix, generateNormals);
			}
			return true;
		}
		if (mGeom.hasVertices && parentMoved)
		{
			mGeom.ApplyTransform(worldToPanel * cachedTransform.localToWorldMatrix, generateNormals);
		}
		return false;
	}

	public void WriteToBuffers(BetterList<Vector3> v, BetterList<Vector2> u, BetterList<Color> c, BetterList<Vector3> n, BetterList<Vector4> t)
	{
		mGeom.WriteToBuffers(v, u, c, n, t);
	}

	public virtual void MakePixelPerfect()
	{
		Vector3 localScale = cachedTransform.localScale;
		int num = Mathf.RoundToInt(localScale.x);
		int num2 = Mathf.RoundToInt(localScale.y);
		localScale.x = num;
		localScale.y = num2;
		localScale.z = 1f;
		Vector3 localPosition = cachedTransform.localPosition;
		localPosition.z = Mathf.RoundToInt(localPosition.z);
		if (num % 2 == 1 && (pivot == Pivot.Top || pivot == Pivot.Center || pivot == Pivot.Bottom))
		{
			localPosition.x = Mathf.Floor(localPosition.x) + 0.5f;
		}
		else
		{
			localPosition.x = Mathf.Round(localPosition.x);
		}
		if (num2 % 2 == 1 && (pivot == Pivot.Left || pivot == Pivot.Center || pivot == Pivot.Right))
		{
			localPosition.y = Mathf.Ceil(localPosition.y) - 0.5f;
		}
		else
		{
			localPosition.y = Mathf.Round(localPosition.y);
		}
		cachedTransform.localPosition = localPosition;
		cachedTransform.localScale = localScale;
	}

	protected virtual void OnStart()
	{
	}

	public virtual bool OnUpdate()
	{
		return false;
	}

	public virtual void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color> cols)
	{
	}
}
