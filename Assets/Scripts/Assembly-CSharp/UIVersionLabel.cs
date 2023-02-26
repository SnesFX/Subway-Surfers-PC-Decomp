using UnityEngine;

[RequireComponent(typeof(UILabel))]
public class UIVersionLabel : MonoBehaviour
{
	private void Start()
	{
		GetComponent<UILabel>().text = "v" + DeviceUtility.GetBundleVersion();
	}
}
