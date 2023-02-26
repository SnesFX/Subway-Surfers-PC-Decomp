using UnityEngine;

public class MessageCenter : MonoBehaviour
{
	private static MessageCenter _instance;

	public static MessageCenter Instance
	{
		get
		{
			if (_instance == null)
			{
				Debug.Log("Instance requested before being instantiated");
				_instance = Object.FindObjectOfType(typeof(MessageCenter)) as MessageCenter;
				if (_instance == null)
				{
					Debug.LogError("MessageCenter not found in the scene.");
				}
			}
			return _instance;
		}
	}

	public static bool IsInstanced
	{
		get
		{
			return _instance != null;
		}
	}

	private void Awake()
	{
		_instance = this;
	}
}
