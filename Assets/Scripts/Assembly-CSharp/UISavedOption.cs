using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Saved Option")]
public class UISavedOption : MonoBehaviour
{
	public string keyName;

	private string key
	{
		get
		{
			return (!string.IsNullOrEmpty(keyName)) ? keyName : ("NGUI State: " + base.name);
		}
	}

	private void OnEnable()
	{
		string @string = PlayerPrefs.GetString(key);
		if (string.IsNullOrEmpty(@string))
		{
			return;
		}
		UICheckbox component = GetComponent<UICheckbox>();
		if (component != null)
		{
			component.isChecked = @string == "true";
			return;
		}
		UICheckbox[] componentsInChildren = GetComponentsInChildren<UICheckbox>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			UICheckbox uICheckbox = componentsInChildren[i];
			UIEventListener uIEventListener = UIEventListener.Get(uICheckbox.gameObject);
			uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(Save));
			uICheckbox.isChecked = uICheckbox.name == @string;
			Debug.Log(@string);
			UIEventListener uIEventListener2 = UIEventListener.Get(uICheckbox.gameObject);
			uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(Save));
		}
	}

	private void OnDisable()
	{
		Save(null);
	}

	private void Save(GameObject go)
	{
		UICheckbox component = GetComponent<UICheckbox>();
		if (component != null)
		{
			PlayerPrefs.SetString(key, (!component.isChecked) ? "false" : "true");
			return;
		}
		UICheckbox[] componentsInChildren = GetComponentsInChildren<UICheckbox>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			UICheckbox uICheckbox = componentsInChildren[i];
			if (uICheckbox.isChecked)
			{
				PlayerPrefs.SetString(key, uICheckbox.name);
				break;
			}
		}
	}
}
