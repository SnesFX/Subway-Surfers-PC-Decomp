using UnityEngine;

public class SpiralSpecialCase : MonoBehaviour
{
	public string highResName;

	private void Start()
	{
		if (DeviceInfo.isHighres)
		{
			UITiledSprite component = GetComponent<UITiledSprite>();
			component.spriteName = highResName;
		}
	}
}
