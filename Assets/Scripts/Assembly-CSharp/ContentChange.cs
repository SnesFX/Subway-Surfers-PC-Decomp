using UnityEngine;

public class ContentChange : MonoBehaviour
{
	private bool foldedOut;

	public GameObject descriptionText;

	public GameObject button;

	public GameObject openButton;

	private UITable _table;

	private void Start()
	{
		foldedOut = false;
		ContentActivation(false);
		_table = NGUITools.FindInParents<UITable>(base.gameObject);
		_table.repositionNow = true;
	}

	private void OnEnable()
	{
		ContentActivation(foldedOut);
		if (_table != null)
		{
			_table.repositionNow = true;
		}
	}

	public void TriggerContent()
	{
		if (foldedOut)
		{
			ContentActivation(foldedOut);
		}
	}

	public void FoldClicked()
	{
		if (!foldedOut)
		{
			foldedOut = true;
			return;
		}
		foldedOut = false;
		ContentActivation(false);
	}

	private void ContentActivation(bool active)
	{
		descriptionText.active = active;
		if (button != null)
		{
			NGUITools.SetActive(button, active);
		}
		NGUITools.SetActive(openButton, !active);
		NGUITools.AddWidgetCollider(base.transform.parent.gameObject);
		if (button != null)
		{
			NGUITools.AddWidgetCollider(button);
			BoxCollider boxCollider = button.GetComponent<Collider>() as BoxCollider;
			boxCollider.size = new Vector3(70f, 50f, boxCollider.size.z);
		}
	}
}
