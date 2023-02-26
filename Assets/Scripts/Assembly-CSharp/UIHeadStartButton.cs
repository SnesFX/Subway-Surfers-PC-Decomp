using UnityEngine;

public class UIHeadStartButton : UIBasicButton
{
	public enum HeadStartType
	{
		_notSet = 0,
		headstart500 = 1,
		headstart2000 = 2
	}

	public UIHeadStartHelper helper;

	public HeadStartType type;

	protected override void Send()
	{
		if (type == HeadStartType.headstart500)
		{
			Debug.Log("Use a headstart500");
			Game.Instance.StartHeadStart500();
		}
		else if (type == HeadStartType.headstart2000)
		{
			Debug.Log("Use a headstart2000");
			Game.Instance.StartHeadStart2000();
		}
		helper.HideHeadStart();
	}
}
