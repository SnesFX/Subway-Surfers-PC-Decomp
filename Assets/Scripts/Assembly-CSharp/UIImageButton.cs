using UnityEngine;

[AddComponentMenu("NGUI/UI/Image Button")]
[ExecuteInEditMode]
public class UIImageButton : MonoBehaviour
{
	public UISprite target;

	public string normalSprite;

	public string hoverSprite;

	public string pressedSprite;

	private void Start()
	{
		if (target == null)
		{
			target = GetComponentInChildren<UISprite>();
		}
	}

	private void OnHover(bool isOver)
	{
		if (target != null)
		{
			target.spriteName = ((!isOver) ? normalSprite : hoverSprite);
			target.MakePixelPerfect();
		}
	}

	private void OnPress(bool pressed)
	{
		if (target != null)
		{
			target.spriteName = ((!pressed) ? normalSprite : pressedSprite);
			target.MakePixelPerfect();
		}
	}
}
