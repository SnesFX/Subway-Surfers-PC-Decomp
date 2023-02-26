using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Checkbox Controlled Component")]
public class UICheckboxControlledComponent : MonoBehaviour
{
	public MonoBehaviour target;

	public bool inverse;

	private void OnActivate(bool isActive)
	{
		if (base.enabled && target != null)
		{
			target.enabled = ((!inverse) ? isActive : (!isActive));
		}
	}
}
