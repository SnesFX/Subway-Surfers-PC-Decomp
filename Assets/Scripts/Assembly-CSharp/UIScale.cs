using UnityEngine;

public class UIScale : MonoBehaviour
{
	private Vector2 iPadSize = new Vector2(390f, 520f);

	private void Start()
	{
		if (DeviceInfo.formFactor == DeviceInfo.FormFactor.iPad)
		{
			base.transform.localScale = new Vector3(iPadSize.x, iPadSize.y, base.transform.localScale.z);
		}
	}
}
