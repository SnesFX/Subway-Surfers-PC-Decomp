using UnityEngine;

[RequireComponent(typeof(UILabel))]
public class UIDailyTimerLabel : MonoBehaviour
{
	private UILabel label;

	private string baseText;

	private void OnEnable()
	{
		if (label == null)
		{
			label = GetComponent<UILabel>();
			baseText = label.text;
		}
		if (PlayerInfo.Instance.isDailyWordComplete())
		{
			label.text = "Next Challenge in";
		}
		else
		{
			label.text = baseText;
		}
	}
}
