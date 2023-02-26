using System.Collections;
using UnityEngine;

public class ResumeButtonHelper : MonoBehaviour
{
	public const float MIN_WAIT_TIME = 1f;

	public const float MAX_WAIT_TIME = 5f;

	public const string TEXT_WAIT = "WAIT";

	private const string TEXT_RESUME = "RESUME";

	public UILabel label;

	private Color initColor;

	public UISprite icon;

	private bool buttonEnabled = true;

	public void OnEnable()
	{
		icon.color = Color.white;
		StartCoroutine(EnableButtonWhenReady());
	}

	private IEnumerator EnableButtonWhenReady()
	{
		DisableButton();
		if ((bool)label)
		{
			label.text = "WAIT";
		}
		float startTime = Time.realtimeSinceStartup;
		float timeWaited = 0f;
		while (timeWaited < 1f)
		{
			timeWaited = Time.realtimeSinceStartup - startTime;
			yield return new WaitForEndOfFrame();
		}
		while (timeWaited < 5f && !SocialManager.instance.consolidatedFriendsCompleted)
		{
			timeWaited = Time.realtimeSinceStartup - startTime;
			yield return new WaitForEndOfFrame();
		}
		if ((bool)label)
		{
			label.text = "RESUME";
		}
		EnableButton();
	}

	public void EnableButton()
	{
		if (!buttonEnabled)
		{
			NGUITools.AddWidgetCollider(base.gameObject);
			icon.color = initColor;
			buttonEnabled = true;
		}
	}

	public void DisableButton()
	{
		if (buttonEnabled)
		{
			if (base.gameObject.GetComponent<Collider>() != null)
			{
				Object.Destroy(base.gameObject.GetComponent<Collider>());
			}
			initColor = icon.color;
			icon.color = Color.gray;
			buttonEnabled = false;
		}
	}
}
