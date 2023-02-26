using UnityEngine;

public class Glow : MonoBehaviour
{
	private MeshRenderer meshRenderer;

	public void Awake()
	{
		meshRenderer = GetComponentInChildren<MeshRenderer>();
	}

	public void SetVisible(bool visible)
	{
		if (meshRenderer != null)
		{
			meshRenderer.enabled = visible;
		}
		base.enabled = visible;
	}
}
