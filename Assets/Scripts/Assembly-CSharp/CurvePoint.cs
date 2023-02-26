using UnityEngine;

public class CurvePoint : MonoBehaviour
{
	public float t;

	public bool smoothTangents = true;

	public CurvePointTangent customIn;

	public CurvePointTangent customOut;

	public float weight = 50f;

	public static Curve CreateCurve(Transform curvePointsParent, Vector3 offset)
	{
		Curve curve = new Curve();
		CurvePoint[] componentsInChildren = curvePointsParent.GetComponentsInChildren<CurvePoint>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			CurvePoint curvePoint = componentsInChildren[i];
			curve.AddKey(curvePoint.t, curvePoint.transform.localPosition + offset, (curvePoint.transform.TransformPoint(-curvePoint.customIn.transform.localPosition) - curvePoint.transform.position) * curvePoint.weight, (curvePoint.transform.TransformPoint(curvePoint.customOut.transform.localPosition) - curvePoint.transform.position) * curvePoint.weight);
			if (curvePoint.smoothTangents)
			{
				curve.SmoothTangents(i, curvePoint.weight);
			}
		}
		return curve;
	}

	public static void DrawCurve(Transform curvePointsParent, Color color)
	{
		DrawCurve(curvePointsParent, Vector3.zero, color);
	}

	public static void DrawCurve(Transform curvePointsParent, Vector3 offset, Color color)
	{
		CreateCurve(curvePointsParent, offset).DrawGizmos(color);
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(base.transform.position, 0.5f);
		if (!smoothTangents)
		{
			Vector3 position = customIn.transform.position;
			Vector3 position2 = customOut.transform.position;
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(base.transform.position, position);
			Gizmos.DrawLine(base.transform.position, position2);
			Gizmos.color = Color.yellow * 0.8f;
			Gizmos.DrawSphere(position, 0.3f);
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(position2, 0.3f);
		}
	}
}
