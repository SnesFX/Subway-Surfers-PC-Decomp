using UnityEngine;

public class SocialTest : MonoBehaviour
{
	private bool started;

	private void OnGUI()
	{
		if (!started)
		{
			GUILayout.BeginArea(new Rect(0f, 0f, Screen.width, Screen.height));
			if (GUILayout.Button("Start"))
			{
				SocialManager.debugGUI = true;
				SocialManager instance = SocialManager.instance;
				started = true;
			}
			GUILayout.EndArea();
		}
	}
}
