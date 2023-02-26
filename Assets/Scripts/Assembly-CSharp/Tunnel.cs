using UnityEngine;

public class Tunnel : MonoBehaviour
{
	private Game game;

	private float tunnelLength;

	public AudioStateLoop audioStateLoop;

	private void Awake()
	{
		game = Game.Instance;
		tunnelLength = base.GetComponent<Collider>().bounds.size.z;
	}

	private void OnTriggerEnter(Collider collider)
	{
		if ("Player".Equals(collider.tag))
		{
			game.Running.StartTunnel(tunnelLength);
		}
	}

	private void OnTriggerExit(Collider collider)
	{
		if ("Player".Equals(collider.tag))
		{
			game.Running.EndTunnel();
		}
	}
}
