using UnityEngine;

public class UIFooterLoader : MonoBehaviour
{
	public GameObject FooterPrefab;

	private UIFootherHandler _footerHandler;

	public int selectedButton;

	private Color selectedColor = new Color(0.6627451f, 0.6627451f, 0.6627451f, 1f);

	private void Awake()
	{
		GameObject gameObject = NGUITools.AddChild(base.gameObject, FooterPrefab);
		_footerHandler = gameObject.GetComponent<UIFootherHandler>();
		switch (selectedButton)
		{
		case 1:
			Object.Destroy(_footerHandler.Button1.GetComponent<BoxCollider>());
			_footerHandler.Fill1.color = selectedColor;
			break;
		case 2:
			Object.Destroy(_footerHandler.Button2.GetComponent<BoxCollider>());
			_footerHandler.Fill2.color = selectedColor;
			break;
		case 3:
			Object.Destroy(_footerHandler.Button3.GetComponent<BoxCollider>());
			_footerHandler.Fill3.color = selectedColor;
			break;
		default:
			Debug.Log("No button was selected in the footer?", this);
			break;
		}
	}
}
