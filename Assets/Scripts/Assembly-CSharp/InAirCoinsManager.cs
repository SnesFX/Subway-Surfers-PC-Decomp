using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAirCoinsManager : MonoBehaviour
{
	public GameObject coinPrefab;

	public int numberOfCoins = 200;

	public float stayInTrackDistance = 60f;

	public float coinDistance = 30f;

	private List<Transform> coins = new List<Transform>();

	private AnimationCurve curve;

	private Track track;

	private Jetpack jetpack;

	private CoinPool coinPool;

	public void Awake()
	{
		jetpack = Jetpack.Instance;
		track = Track.Instance;
		coinPool = CoinPool.Instance;
	}

	public void Spawn(float startZ, float length, float height)
	{
		curve = new AnimationCurve();
		int num = 1;
		for (float num2 = startZ; num2 < startZ + length; num2 += jetpack.characterChangeTrackLength + stayInTrackDistance)
		{
			curve.AddKey(new Keyframe(num2, track.GetTrackX(num)));
			curve.AddKey(new Keyframe(num2 + stayInTrackDistance, track.GetTrackX(num)));
			num = Mathf.Clamp(num + Random.Range(-1, 2), 0, track.numberOfTracks - 1);
			curve.AddKey(new Keyframe(num2 + stayInTrackDistance + jetpack.characterChangeTrackLength, track.GetTrackX(num)));
		}
		StartCoroutine(MoveCoins(startZ, length, height));
	}

	private IEnumerator MoveCoins(float StartZ, float length, float height)
	{
		float z = StartZ;
		while (z < StartZ + length)
		{
			Transform coin = coinPool.GetCoin();
			coin.position = Vector3.up * height + track.GetPosition(curve.Evaluate(z), z);
			coin.GetComponent<TrackObject>().Activate();
			z += coinDistance;
			coins.Add(coin);
			yield return null;
		}
	}

	public void ReleaseCoins()
	{
		coinPool.Put(coins);
		coins.Clear();
	}
}
