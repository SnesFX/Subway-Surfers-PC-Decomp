using System.Collections;
using UnityEngine;

public class LoadLevelCtrl : MonoBehaviour
{
	private void Awake()
	{
		Object.DontDestroyOnLoad(base.transform.gameObject);
	}

	private IEnumerator Start()
	{
		yield return Application.LoadLevelAsync("Merge");
		Debug.Log("Merge Level Loaded " + Time.frameCount);
		yield return Application.LoadLevelAdditiveAsync("LazyLoad");
		Debug.Log("Chunks Level Loaded " + Time.frameCount);
	}
}
