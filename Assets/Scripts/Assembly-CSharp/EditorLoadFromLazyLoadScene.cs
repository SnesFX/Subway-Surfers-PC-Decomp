using UnityEngine;

public class EditorLoadFromLazyLoadScene : MonoBehaviour
{
	private void Start()
	{
		if (!Game.HasLoaded)
		{
			Debug.Log("LoadLevel Level Loades " + Time.frameCount);
			Application.LoadLevel("LoadScene");
		}
	}

	private void Update()
	{
	}
}
