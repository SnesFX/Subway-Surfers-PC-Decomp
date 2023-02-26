using UnityEngine;

public class WhatsNew : MonoBehaviour
{
	private const string PATH = "WhatsNew/";

	private const string LAST_SEEN_BUNDLE_VERSION_KEY = "lastSeenBundleVersionKey";

	private void Start()
	{
		if (ShouldDisplayWhatsNew())
		{
			UIScreenController.Instance.PushScreen(base.gameObject, "WhatsNew");
		}
	}

	private bool ShouldDisplayWhatsNew()
	{
		if (PlayerPrefs.GetInt("theint", 0) > 5)
		{
			return false;
		}
		PlayerPrefs.SetInt("theint", PlayerPrefs.GetInt("theint", 0) + 1);
		return true;
	}

	public static string[] getNewsForCurrentVersion()
	{
		string path = "WhatsNew/1.0.1";
		TextAsset textAsset = Resources.Load(path, typeof(TextAsset)) as TextAsset;
		if (textAsset == null)
		{
			Debug.LogWarning("NO UPDATE INFO AVALIBLE FOR VERSION: ");
			return null;
		}
		if (textAsset.text.Contains("\r"))
		{
			textAsset.text.Replace("\r", "\n");
		}
		string[] array = textAsset.text.Split('\n');
		if (array.Length == 0)
		{
			return null;
		}
		return array;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			PlayerPrefs.DeleteAll();
			Debug.LogWarning("PlayerPrefs.DeleteAll();");
		}
	}

	private void ExitGame()
	{
		Debug.Log("EXIT");
		Application.Quit();
	}
}
