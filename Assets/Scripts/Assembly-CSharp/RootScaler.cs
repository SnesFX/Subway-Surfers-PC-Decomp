using UnityEngine;

[RequireComponent(typeof(UIRoot))]
public class RootScaler : MonoBehaviour
{
	private UIRoot myUIRoot;

	private void Awake()
	{
		myUIRoot = base.gameObject.GetComponent<UIRoot>();
		if (DeviceInfo.formFactor == DeviceInfo.FormFactor.iPad)
		{
			Debug.Log("iPad screen");
			int num = Mathf.RoundToInt(myUIRoot.manualHeight * 16 / 15);
			Debug.Log("New height: " + num);
			myUIRoot.manualHeight = num;
		}
	}
}
