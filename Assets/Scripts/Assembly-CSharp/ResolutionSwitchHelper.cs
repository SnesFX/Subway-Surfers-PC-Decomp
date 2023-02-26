using UnityEngine;

[AddComponentMenu("GUI/Resolution/ResolutionHelper Component &r")]
public class ResolutionSwitchHelper : MonoBehaviour
{
	private void Awake()
	{
		if (!DeviceInfo.isHighres)
		{
			return;
		}
		base.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
		foreach (Transform item in base.transform)
		{
			item.localScale = new Vector3(item.localScale.x * 2f, item.localScale.y * 2f, item.localScale.z);
			item.localPosition = new Vector3(item.localPosition.x * 2f, item.localPosition.y * 2f, item.localPosition.z);
			TweenScale component = item.gameObject.GetComponent<TweenScale>();
			if (component != null)
			{
				component.from = new Vector3(component.from.x * 2f, component.from.y * 2f, component.from.z);
				component.to = new Vector3(component.to.x * 2f, component.to.y * 2f, component.to.z);
			}
			TweenPosition component2 = item.gameObject.GetComponent<TweenPosition>();
			if (component2 != null)
			{
				component2.from = new Vector3(component2.from.x * 2f, component2.from.y * 2f, component2.from.z);
				component2.to = new Vector3(component2.to.x * 2f, component2.to.y * 2f, component2.to.z);
			}
			TweenTransform component3 = item.gameObject.GetComponent<TweenTransform>();
			if (component3 != null)
			{
				Debug.LogError("TweenTransform used and not handled in resolution switcher.");
				Debug.Break();
			}
		}
	}
}
