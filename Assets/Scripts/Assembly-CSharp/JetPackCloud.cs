using UnityEngine;

public class JetPackCloud : MonoBehaviour
{
	public float scrollSpeed = 0.5f;

	public Material material;

	public float startOffset;

	private void Awake()
	{
		material.mainTextureOffset = new Vector2(startOffset, 0f);
	}

	private void Update()
	{
		float x = material.mainTextureOffset.x;
		x = (x + Time.deltaTime * scrollSpeed) % 1f;
		material.mainTextureOffset = new Vector2(x, 0f);
	}
}
