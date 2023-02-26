using System.Collections;

public class DoubleScoreMultiplier : CharacterModifier
{
	public float ratio;

	public ActivePowerup Powerup;

	public override IEnumerator Begin()
	{
		GameStats.Instance.usePowerups++;
		Paused = false;
		stop = StopSignal.DONT_STOP;
		Powerup = GameStats.Instance.TriggerPowerup(PowerupType.doubleMultiplier);
		float duration = Powerup.timeLeft;
		PlayerInfo.Instance.doubleScore = true;
		while (Powerup.timeLeft > 0f && stop == StopSignal.DONT_STOP)
		{
			ratio = Powerup.timeLeft / duration;
			yield return 0;
		}
		PlayerInfo.Instance.doubleScore = false;
	}

	public override void Reset()
	{
		PlayerInfo.Instance.doubleScore = false;
	}
}
