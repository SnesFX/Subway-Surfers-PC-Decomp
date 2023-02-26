using System.Collections;
using UnityEngine;

public class UIMessageHelper : MonoBehaviour
{
	public Color shownColor = Color.white;

	private UILabel _label;

	private void Awake()
	{
		_label = GetComponent<UILabel>();
		_label.alpha = 0f;
		base.gameObject.active = false;
	}

	public void ShowMessage(string message)
	{
		base.gameObject.active = true;
		_label.text = message;
		_label.color = shownColor;
		StartCoroutine("FadeOut");
	}

	private IEnumerator FadeOut()
	{
		yield return new WaitForSeconds(2f);
		float duration = 0.2f;
		float fadeTime = 0f;
		Vector3 scaleFrom = _label.transform.localScale;
		Vector3 scaleTo = new Vector3(scaleFrom.x, 0f, scaleFrom.z);
		while (fadeTime < 1f)
		{
			fadeTime += Time.deltaTime / duration;
			_label.transform.localScale = Vector3.Lerp(scaleFrom, scaleTo, fadeTime);
			yield return null;
		}
		yield return new WaitForSeconds(0.5f);
		_label.text = string.Empty;
		_label.transform.localScale = scaleFrom;
		base.gameObject.active = false;
		UIScreenController.Instance.ReadyForNextMessage();
	}

	public void SetTemporaryHidden(bool hidden)
	{
		if (_label != null)
		{
			_label.enabled = !hidden;
		}
	}
}
