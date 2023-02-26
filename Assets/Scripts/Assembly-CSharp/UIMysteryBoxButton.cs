using UnityEngine;

public class UIMysteryBoxButton : UIBasicButton
{
	protected override void Send()
	{
		MysteryBoxReward mysteryBoxReward = MysteryBox.Roll();
		Debug.Log("Reward: " + mysteryBoxReward.amount + "x" + mysteryBoxReward.type.ToString());
	}
}
