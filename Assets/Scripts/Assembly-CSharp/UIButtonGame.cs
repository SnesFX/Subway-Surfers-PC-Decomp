public class UIButtonGame : UIBasicButton
{
	public enum GameMessage
	{
		_notSet = 0,
		StartNewRun = 1,
		RestartFromPause = 2
	}

	public GameMessage messageType;

	protected override void Send()
	{
		if (messageType == GameMessage.StartNewRun)
		{
			if (Game.Instance != null)
			{
				Game.Instance.StartNewRun();
			}
		}
		else if (messageType != GameMessage.RestartFromPause)
		{
		}
	}
}
