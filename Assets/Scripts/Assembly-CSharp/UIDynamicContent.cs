using UnityEngine;

public class UIDynamicContent : MonoBehaviour
{
	public GameObject[] PanelElements;

	public GameObject Header;

	public GameObject[] HeaderElements;

	private void Start()
	{
		InitElements();
	}

	public void InitElements()
	{
		for (int i = 0; i < PanelElements.Length; i++)
		{
			NGUITools.AddChild(base.gameObject, PanelElements[i]);
		}
		for (int j = 0; j < HeaderElements.Length; j++)
		{
			GameObject go = NGUITools.AddChild(Header, HeaderElements[j]);
			NGUITools.AddWidgetCollider(go);
		}
	}
}
