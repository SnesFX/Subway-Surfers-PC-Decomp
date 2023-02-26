using System;
using UnityEngine;

public class CoinPlaceholder : MonoBehaviour
{
	private CoinPool coinPool;

	private Transform coin;

	private void Awake()
	{
		coinPool = CoinPool.Instance;
		TrackObject trackObject = GetComponent<TrackObject>() ?? base.gameObject.AddComponent<TrackObject>();
		trackObject.OnActivate = (TrackObject.OnActivateDelegate)Delegate.Combine(trackObject.OnActivate, new TrackObject.OnActivateDelegate(OnActivate));
		trackObject.OnDeactivate = (TrackObject.OnDeactivateDelegate)Delegate.Combine(trackObject.OnDeactivate, new TrackObject.OnDeactivateDelegate(OnDeactivate));
	}

	public void OnActivate()
	{
		coin = coinPool.GetCoin();
		coin.parent = base.transform;
		coin.position = base.transform.position;
		TrackObject component = coin.GetComponent<TrackObject>();
		component.Activate();
	}

	public void OnDeactivate()
	{
		if (coin != null)
		{
			TrackObject component = coin.GetComponent<TrackObject>();
			component.Deactivate();
			coinPool.Put(coin);
			coin = null;
		}
	}
}
