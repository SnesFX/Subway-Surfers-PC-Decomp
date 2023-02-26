using System;
using System.Collections.Generic;
using UnityEngine;

public class CoinLine : MonoBehaviour
{
	public float length = 100f;

	public float coinSpacing = 15f;

	private CoinPool coinPool;

	private List<Transform> activeCoins = new List<Transform>();

	private void Awake()
	{
		TrackObject component = GetComponent<TrackObject>();
		component.OnActivate = (TrackObject.OnActivateDelegate)Delegate.Combine(component.OnActivate, new TrackObject.OnActivateDelegate(OnActivate));
		component.OnDeactivate = (TrackObject.OnDeactivateDelegate)Delegate.Combine(component.OnDeactivate, new TrackObject.OnDeactivateDelegate(OnDeactivate));
		coinPool = CoinPool.Instance;
	}

	private void OnActivate()
	{
		for (float num = 0f; num < length; num += coinSpacing)
		{
			Transform coin = coinPool.GetCoin();
			coin.parent = base.transform;
			coin.position = base.transform.position + base.transform.forward * num;
			TrackObject component = coin.GetComponent<TrackObject>();
			if (component != null)
			{
				component.OnActivate();
			}
			activeCoins.Add(coin);
		}
	}

	private void OnDeactivate()
	{
		foreach (Transform activeCoin in activeCoins)
		{
			activeCoin.GetComponent<TrackObject>().OnDeactivate();
		}
		coinPool.Put(activeCoins);
		activeCoins.Clear();
	}

	public void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(base.transform.position, base.transform.position + base.transform.forward * length);
		for (float num = 0f; num < length; num += coinSpacing)
		{
			Vector3 center = base.transform.position + base.transform.forward * num;
			Gizmos.DrawSphere(center, 1f);
		}
	}
}
