using UnityEngine;

public class FlurryInit : MonoBehaviour
{
	private const string API_KEY = "CIJUE322XIHTDWNV519J";

	private void Awake()
	{
		Flurry.StartSession("CIJUE322XIHTDWNV519J");
	}
}
