using System;
using UnityEngine;

public class TestStats : MonoBehaviour
{
	public int gamesPlayed;

	public float durationTotal;

	public float durationAvg = float.NaN;

	public int coinsTotal;

	public float coinsAvg = float.NaN;

	public float metersTotal;

	public float metersAvg = float.NaN;

	public int jumpsTotal;

	public float jumpsAvg = float.NaN;

	public int rollsTotal;

	public float rollsAvg = float.NaN;

	public int pickupsTotal;

	public float pickupsAvg = float.NaN;

	public int trackChangesTotal;

	public float trackChangesAvg = float.NaN;

	private void Start()
	{
		Game instance = Game.Instance;
		instance.OnGameOver = (Game.OnGameOverDelegate)Delegate.Combine(instance.OnGameOver, new Game.OnGameOverDelegate(OnGameOver));
	}

	private void OnGameOver(GameStats stats)
	{
		gamesPlayed++;
		durationTotal += stats.duration;
		durationAvg = durationTotal / (float)gamesPlayed;
		coinsTotal += stats.coins;
		coinsAvg = coinsTotal / gamesPlayed;
		metersTotal += stats.meters;
		metersAvg = metersTotal / (float)gamesPlayed;
		jumpsTotal += stats.jumps;
		jumpsAvg = jumpsTotal / gamesPlayed;
		rollsTotal += stats.rolls;
		rollsAvg = rollsTotal / gamesPlayed;
		pickupsTotal += stats.jetpackPickups + stats.superSneakerPickups + stats.letterPickups + stats.coinMagnetsPickups + stats.mysteryBoxPickups;
		pickupsAvg = pickupsTotal;
		trackChangesTotal = stats.trackChanges;
		trackChangesAvg = trackChangesTotal / gamesPlayed;
	}
}
