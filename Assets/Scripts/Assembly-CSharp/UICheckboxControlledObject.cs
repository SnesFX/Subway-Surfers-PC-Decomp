using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Checkbox Controlled Object")]
public class UICheckboxControlledObject : MonoBehaviour
{
	public GameObject target;

	public bool inverse;

	private void OnActivate(bool isActive)
	{
		if (target != null)
		{
			NGUITools.SetActive(target, (!inverse) ? isActive : (!isActive));
		}
	}
}
