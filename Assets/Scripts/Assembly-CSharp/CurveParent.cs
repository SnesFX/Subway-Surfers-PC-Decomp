using UnityEngine;

public class CurveParent : MonoBehaviour
{
	public Color color = Color.red;

	public void OnDrawGizmos()
	{
		CurvePoint.DrawCurve(base.transform, color);
	}
}
