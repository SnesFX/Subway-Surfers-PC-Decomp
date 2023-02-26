using UnityEngine;

[AddComponentMenu("GUI/Interaction/Button Message Center")]
public class UIButtonMessageCenter : UIBasicButton
{
	private GameObject target;

	public string functionName;

	protected override void Send()
	{
		if (!base.enabled || !base.gameObject.active || string.IsNullOrEmpty(functionName))
		{
			return;
		}
		if (target == null)
		{
			if (MessageCenter.IsInstanced)
			{
				target = MessageCenter.Instance.gameObject;
			}
			else
			{
				Debug.LogError("MessageCenter called but not instanced.");
			}
		}
		Transform[] componentsInChildren = target.GetComponentsInChildren<Transform>();
		Transform[] array = componentsInChildren;
		foreach (Transform transform in array)
		{
			transform.gameObject.SendMessage(functionName, base.gameObject, SendMessageOptions.DontRequireReceiver);
		}
	}
}
