public class MystBoxButtonTest : UIBasicButton
{
	public bool doubleBox;

	protected override void Send()
	{
		PlayerInfo.Instance.mysteryBoxesToUnlock = 1;
		if (doubleBox)
		{
			PlayerInfo.Instance.mysteryBoxesToUnlock = 2;
		}
	}
}
