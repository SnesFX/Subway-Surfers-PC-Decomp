using UnityEngine;

public class Curve
{
	public AnimationCurve curveX = new AnimationCurve();

	public AnimationCurve curveY = new AnimationCurve();

	public AnimationCurve curveZ = new AnimationCurve();

	private float min = float.PositiveInfinity;

	private float max = float.NegativeInfinity;

	public Curve()
	{
		curveX.postWrapMode = WrapMode.ClampForever;
		curveX.preWrapMode = WrapMode.ClampForever;
		curveY.postWrapMode = WrapMode.ClampForever;
		curveY.preWrapMode = WrapMode.ClampForever;
		curveZ.postWrapMode = WrapMode.ClampForever;
		curveZ.preWrapMode = WrapMode.ClampForever;
	}

	public void AddKey(float t, Vector3 value)
	{
		curveX.AddKey(t, value.x);
		curveY.AddKey(t, value.y);
		curveZ.AddKey(t, value.z);
		if (t < min)
		{
			min = t;
		}
		if (t > max)
		{
			max = t;
		}
	}

	public void AddKey(float t, Vector3 value, Vector3 inTangent, Vector3 outTangent)
	{
		curveX.AddKey(new Keyframe(t, value.x, inTangent.x, outTangent.x));
		curveY.AddKey(new Keyframe(t, value.y, inTangent.y, outTangent.y));
		curveZ.AddKey(new Keyframe(t, value.z, inTangent.z, outTangent.z));
		if (t < min)
		{
			min = t;
		}
		if (t > max)
		{
			max = t;
		}
	}

	public void MoveKey(int index, float t, Vector3 value, Vector3 inTangent, Vector3 outTangent)
	{
		curveX.MoveKey(index, new Keyframe(t, value.x, inTangent.x, outTangent.x));
		curveY.MoveKey(index, new Keyframe(t, value.y, inTangent.y, outTangent.y));
		curveZ.MoveKey(index, new Keyframe(t, value.z, inTangent.z, outTangent.z));
	}

	public void MoveKey(int index, float t, Vector3 value)
	{
		curveX.MoveKey(index, new Keyframe(t, value.x));
		curveY.MoveKey(index, new Keyframe(t, value.y));
		curveZ.MoveKey(index, new Keyframe(t, value.z));
	}

	public void SmoothTangents(int index, float weight)
	{
		curveX.SmoothTangents(index, weight);
		curveY.SmoothTangents(index, weight);
		curveZ.SmoothTangents(index, weight);
	}

	public Vector3 Evaluate(float t)
	{
		return new Vector3(curveX.Evaluate(t), curveY.Evaluate(t), curveZ.Evaluate(t));
	}

	public void DrawGizmos(Color color)
	{
		Gizmos.color = color;
		int num = 1000;
		Vector3 from = Evaluate(0f);
		for (int i = 0; i < num; i++)
		{
			float t = (max - min) * (float)i / (float)(num - 1);
			Vector3 vector = Evaluate(t);
			Gizmos.DrawLine(from, vector);
			from = vector;
		}
	}
}
