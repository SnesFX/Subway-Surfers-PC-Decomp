using UnityEngine;

public class CurvePointTangent : MonoBehaviour
{
	public void OnDrawGizmosSelected()
	{
		if (base.transform.parent != null)
		{
			CurvePoint component = base.transform.parent.GetComponent<CurvePoint>();
			component.OnDrawGizmosSelected();
		}
	}
}
