using UnityEngine;

public class StackTrace : MonoBehaviour
{
	private void OnDisable()
	{
		Debug.Log("I was disabled!!");
		Debug.Break();
	}
}
