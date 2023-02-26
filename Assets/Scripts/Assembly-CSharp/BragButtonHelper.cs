using UnityEngine;

public class BragButtonHelper : MonoBehaviour
{
	public UISlicedSprite fill;

	private string activeButtonFillName = "button_fill_brag";

	private string inactiveButtonFillName = "button_fill_shopItem_gray";

	[HideInInspector]
	public bool buttonEnabled;

	public void EnableButton()
	{
		NGUITools.AddWidgetCollider(base.gameObject);
		fill.spriteName = activeButtonFillName;
		buttonEnabled = true;
	}

	public void DisableButton()
	{
		if (base.gameObject.GetComponent<Collider>() != null)
		{
			Object.Destroy(base.gameObject.GetComponent<Collider>());
		}
		fill.spriteName = inactiveButtonFillName;
		buttonEnabled = false;
	}
}
