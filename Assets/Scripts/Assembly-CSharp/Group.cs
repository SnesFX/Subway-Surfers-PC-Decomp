using UnityEngine;

public class Group : MonoBehaviour
{
	private bool groupActive;

	public bool GroupActive
	{
		get
		{
			return groupActive;
		}
		set
		{
			groupActive = value;
			UpdateChildren();
		}
	}

	public void Start()
	{
		UpdateChildren();
	}

	public void OnEnable()
	{
		UpdateChildren();
	}

	public void OnDisable()
	{
		UpdateChildren();
	}

	public void UpdateChildren()
	{
		SetChildrenActive(groupActive && base.gameObject.active);
	}

	private void SetChildrenActive(bool active)
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			base.transform.GetChild(i).gameObject.SetActiveRecursively(active);
		}
	}
}
