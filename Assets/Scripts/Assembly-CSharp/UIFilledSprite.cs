using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Sprite (Filled)")]
[ExecuteInEditMode]
public class UIFilledSprite : UISprite
{
	public enum FillDirection
	{
		Horizontal = 0,
		Vertical = 1,
		Radial90 = 2,
		Radial180 = 3,
		Radial360 = 4
	}

	[HideInInspector]
	[SerializeField]
	private FillDirection mFillDirection = FillDirection.Radial360;

	[SerializeField]
	[HideInInspector]
	private float mFillAmount = 1f;

	[SerializeField]
	[HideInInspector]
	private bool mInvert;

	public FillDirection fillDirection
	{
		get
		{
			return mFillDirection;
		}
		set
		{
			if (mFillDirection != value)
			{
				mFillDirection = value;
				mChanged = true;
			}
		}
	}

	public float fillAmount
	{
		get
		{
			return mFillAmount;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (mFillAmount != num)
			{
				mFillAmount = num;
				mChanged = true;
			}
		}
	}

	public bool invert
	{
		get
		{
			return mInvert;
		}
		set
		{
			if (mInvert != value)
			{
				mInvert = value;
				mChanged = true;
			}
		}
	}

	private bool AdjustRadial(Vector2[] xy, Vector2[] uv, float fill, bool invert)
	{
		if (fill < 0.001f)
		{
			return false;
		}
		if (!invert && fill > 0.999f)
		{
			return true;
		}
		float num = Mathf.Clamp01(fill);
		if (!invert)
		{
			num = 1f - num;
		}
		num *= (float)Math.PI / 2f;
		float num2 = Mathf.Sin(num);
		float num3 = Mathf.Cos(num);
		if (num2 > num3)
		{
			num3 *= 1f / num2;
			num2 = 1f;
			if (!invert)
			{
				xy[0].y = Mathf.Lerp(xy[2].y, xy[0].y, num3);
				xy[3].y = xy[0].y;
				uv[0].y = Mathf.Lerp(uv[2].y, uv[0].y, num3);
				uv[3].y = uv[0].y;
			}
		}
		else if (num3 > num2)
		{
			num2 *= 1f / num3;
			num3 = 1f;
			if (invert)
			{
				xy[0].x = Mathf.Lerp(xy[2].x, xy[0].x, num2);
				xy[1].x = xy[0].x;
				uv[0].x = Mathf.Lerp(uv[2].x, uv[0].x, num2);
				uv[1].x = uv[0].x;
			}
		}
		else
		{
			num2 = 1f;
			num3 = 1f;
		}
		if (invert)
		{
			xy[1].y = Mathf.Lerp(xy[2].y, xy[0].y, num3);
			uv[1].y = Mathf.Lerp(uv[2].y, uv[0].y, num3);
		}
		else
		{
			xy[3].x = Mathf.Lerp(xy[2].x, xy[0].x, num2);
			uv[3].x = Mathf.Lerp(uv[2].x, uv[0].x, num2);
		}
		return true;
	}

	private void Rotate(Vector2[] v, int offset)
	{
		for (int i = 0; i < offset; i++)
		{
			Vector2 vector = new Vector2(v[3].x, v[3].y);
			v[3].x = v[2].y;
			v[3].y = v[2].x;
			v[2].x = v[1].y;
			v[2].y = v[1].x;
			v[1].x = v[0].y;
			v[1].y = v[0].x;
			v[0].x = vector.y;
			v[0].y = vector.x;
		}
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color> cols)
	{
		float x = 0f;
		float y = 0f;
		float num = 1f;
		float num2 = -1f;
		float num3 = mOuterUV.xMin;
		float num4 = mOuterUV.yMin;
		float num5 = mOuterUV.xMax;
		float num6 = mOuterUV.yMax;
		if (mFillDirection == FillDirection.Horizontal || mFillDirection == FillDirection.Vertical)
		{
			float num7 = (num5 - num3) * mFillAmount;
			float num8 = (num6 - num4) * mFillAmount;
			if (fillDirection == FillDirection.Horizontal)
			{
				if (mInvert)
				{
					x = 1f - mFillAmount;
					num3 = num5 - num7;
				}
				else
				{
					num *= mFillAmount;
					num5 = num3 + num7;
				}
			}
			else if (fillDirection == FillDirection.Vertical)
			{
				if (mInvert)
				{
					num2 *= mFillAmount;
					num4 = num6 - num8;
				}
				else
				{
					y = 0f - (1f - mFillAmount);
					num6 = num4 + num8;
				}
			}
		}
		Vector2[] array = new Vector2[4];
		Vector2[] array2 = new Vector2[4];
		array[0] = new Vector2(num, y);
		array[1] = new Vector2(num, num2);
		array[2] = new Vector2(x, num2);
		array[3] = new Vector2(x, y);
		array2[0] = new Vector2(num5, num6);
		array2[1] = new Vector2(num5, num4);
		array2[2] = new Vector2(num3, num4);
		array2[3] = new Vector2(num3, num6);
		if (fillDirection == FillDirection.Radial90)
		{
			if (!AdjustRadial(array, array2, mFillAmount, mInvert))
			{
				return;
			}
		}
		else
		{
			if (fillDirection == FillDirection.Radial180)
			{
				Vector2[] array3 = new Vector2[4];
				Vector2[] array4 = new Vector2[4];
				for (int i = 0; i < 2; i++)
				{
					array3[0] = new Vector2(0f, 0f);
					array3[1] = new Vector2(0f, 1f);
					array3[2] = new Vector2(1f, 1f);
					array3[3] = new Vector2(1f, 0f);
					array4[0] = new Vector2(0f, 0f);
					array4[1] = new Vector2(0f, 1f);
					array4[2] = new Vector2(1f, 1f);
					array4[3] = new Vector2(1f, 0f);
					if (mInvert)
					{
						if (i > 0)
						{
							Rotate(array3, i);
							Rotate(array4, i);
						}
					}
					else if (i < 1)
					{
						Rotate(array3, 1 - i);
						Rotate(array4, 1 - i);
					}
					float from;
					float to;
					if (i == 1)
					{
						from = ((!mInvert) ? 1f : 0.5f);
						to = ((!mInvert) ? 0.5f : 1f);
					}
					else
					{
						from = ((!mInvert) ? 0.5f : 1f);
						to = ((!mInvert) ? 1f : 0.5f);
					}
					array3[1].y = Mathf.Lerp(from, to, array3[1].y);
					array3[2].y = Mathf.Lerp(from, to, array3[2].y);
					array4[1].y = Mathf.Lerp(from, to, array4[1].y);
					array4[2].y = Mathf.Lerp(from, to, array4[2].y);
					float fill = mFillAmount * 2f - (float)i;
					bool flag = i % 2 == 1;
					if (!AdjustRadial(array3, array4, fill, !flag))
					{
						continue;
					}
					if (mInvert)
					{
						flag = !flag;
					}
					if (flag)
					{
						for (int j = 0; j < 4; j++)
						{
							from = Mathf.Lerp(array[0].x, array[2].x, array3[j].x);
							to = Mathf.Lerp(array[0].y, array[2].y, array3[j].y);
							float x2 = Mathf.Lerp(array2[0].x, array2[2].x, array4[j].x);
							float y2 = Mathf.Lerp(array2[0].y, array2[2].y, array4[j].y);
							verts.Add(new Vector3(from, to, 0f));
							uvs.Add(new Vector2(x2, y2));
							cols.Add(base.color);
						}
						continue;
					}
					for (int num9 = 3; num9 > -1; num9--)
					{
						from = Mathf.Lerp(array[0].x, array[2].x, array3[num9].x);
						to = Mathf.Lerp(array[0].y, array[2].y, array3[num9].y);
						float x3 = Mathf.Lerp(array2[0].x, array2[2].x, array4[num9].x);
						float y3 = Mathf.Lerp(array2[0].y, array2[2].y, array4[num9].y);
						verts.Add(new Vector3(from, to, 0f));
						uvs.Add(new Vector2(x3, y3));
						cols.Add(base.color);
					}
				}
				return;
			}
			if (fillDirection == FillDirection.Radial360)
			{
				float[] array5 = new float[16]
				{
					0.5f, 1f, 0f, 0.5f, 0.5f, 1f, 0.5f, 1f, 0f, 0.5f,
					0.5f, 1f, 0f, 0.5f, 0f, 0.5f
				};
				Vector2[] array6 = new Vector2[4];
				Vector2[] array7 = new Vector2[4];
				for (int k = 0; k < 4; k++)
				{
					array6[0] = new Vector2(0f, 0f);
					array6[1] = new Vector2(0f, 1f);
					array6[2] = new Vector2(1f, 1f);
					array6[3] = new Vector2(1f, 0f);
					array7[0] = new Vector2(0f, 0f);
					array7[1] = new Vector2(0f, 1f);
					array7[2] = new Vector2(1f, 1f);
					array7[3] = new Vector2(1f, 0f);
					if (mInvert)
					{
						if (k > 0)
						{
							Rotate(array6, k);
							Rotate(array7, k);
						}
					}
					else if (k < 3)
					{
						Rotate(array6, 3 - k);
						Rotate(array7, 3 - k);
					}
					for (int l = 0; l < 4; l++)
					{
						int num10 = ((!mInvert) ? (k * 4) : ((3 - k) * 4));
						float from2 = array5[num10];
						float to2 = array5[num10 + 1];
						float from3 = array5[num10 + 2];
						float to3 = array5[num10 + 3];
						array6[l].x = Mathf.Lerp(from2, to2, array6[l].x);
						array6[l].y = Mathf.Lerp(from3, to3, array6[l].y);
						array7[l].x = Mathf.Lerp(from2, to2, array7[l].x);
						array7[l].y = Mathf.Lerp(from3, to3, array7[l].y);
					}
					float fill2 = mFillAmount * 4f - (float)k;
					bool flag2 = k % 2 == 1;
					if (!AdjustRadial(array6, array7, fill2, !flag2))
					{
						continue;
					}
					if (mInvert)
					{
						flag2 = !flag2;
					}
					if (flag2)
					{
						for (int m = 0; m < 4; m++)
						{
							float x4 = Mathf.Lerp(array[0].x, array[2].x, array6[m].x);
							float y4 = Mathf.Lerp(array[0].y, array[2].y, array6[m].y);
							float x5 = Mathf.Lerp(array2[0].x, array2[2].x, array7[m].x);
							float y5 = Mathf.Lerp(array2[0].y, array2[2].y, array7[m].y);
							verts.Add(new Vector3(x4, y4, 0f));
							uvs.Add(new Vector2(x5, y5));
							cols.Add(base.color);
						}
						continue;
					}
					for (int num11 = 3; num11 > -1; num11--)
					{
						float x6 = Mathf.Lerp(array[0].x, array[2].x, array6[num11].x);
						float y6 = Mathf.Lerp(array[0].y, array[2].y, array6[num11].y);
						float x7 = Mathf.Lerp(array2[0].x, array2[2].x, array7[num11].x);
						float y7 = Mathf.Lerp(array2[0].y, array2[2].y, array7[num11].y);
						verts.Add(new Vector3(x6, y6, 0f));
						uvs.Add(new Vector2(x7, y7));
						cols.Add(base.color);
					}
				}
				return;
			}
		}
		for (int n = 0; n < 4; n++)
		{
			verts.Add(array[n]);
			uvs.Add(array2[n]);
			cols.Add(base.color);
		}
	}
}
