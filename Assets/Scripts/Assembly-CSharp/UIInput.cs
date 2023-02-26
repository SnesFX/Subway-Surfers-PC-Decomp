using UnityEngine;

[AddComponentMenu("NGUI/UI/Input (Basic)")]
public class UIInput : MonoBehaviour
{
	public static UIInput current;

	public UILabel label;

	public int maxChars;

	public string caratChar = "|";

	public bool isPassword;

	public Color activeColor = Color.white;

	public GameObject eventReceiver;

	public string functionName = "OnSubmit";

	private string mText = string.Empty;

	private string mDefaultText = string.Empty;

	private Color mDefaultColor = Color.white;

	private string mLastIME = string.Empty;

	public string text
	{
		get
		{
			return mText;
		}
		set
		{
			mText = value;
			if (label != null)
			{
				if (string.IsNullOrEmpty(value))
				{
					value = mDefaultText;
				}
				label.supportEncoding = false;
				label.text = ((!selected) ? value : (value + caratChar));
				label.showLastPasswordChar = selected;
				label.color = ((!selected && !(value != mDefaultText)) ? mDefaultColor : activeColor);
			}
		}
	}

	public bool selected
	{
		get
		{
			return UICamera.selectedObject == base.gameObject;
		}
		set
		{
			if (!value && UICamera.selectedObject == base.gameObject)
			{
				UICamera.selectedObject = null;
			}
			else if (value)
			{
				UICamera.selectedObject = base.gameObject;
			}
		}
	}

	protected void Init()
	{
		if (label == null)
		{
			label = GetComponentInChildren<UILabel>();
		}
		if (label != null)
		{
			mDefaultText = label.text;
			mDefaultColor = label.color;
			label.supportEncoding = false;
		}
	}

	private void Awake()
	{
		Init();
	}

	private void OnEnable()
	{
		if (UICamera.IsHighlighted(base.gameObject))
		{
			OnSelect(true);
		}
	}

	private void OnDisable()
	{
		if (UICamera.IsHighlighted(base.gameObject))
		{
			OnSelect(false);
		}
	}

	private void OnSelect(bool isSelected)
	{
		if (!(label != null) || !base.enabled || !base.gameObject.active)
		{
			return;
		}
		if (isSelected)
		{
			mText = ((!(label.text == mDefaultText)) ? label.text : string.Empty);
			label.color = activeColor;
			if (isPassword)
			{
				label.password = true;
			}
			Input.imeCompositionMode = IMECompositionMode.On;
			Transform cachedTransform = label.cachedTransform;
			Vector3 position = label.pivotOffset;
			position.y += label.relativeSize.y;
			position = cachedTransform.TransformPoint(position);
			Input.compositionCursorPos = UICamera.currentCamera.WorldToScreenPoint(position);
			UpdateLabel();
			return;
		}
		if (string.IsNullOrEmpty(mText))
		{
			label.text = mDefaultText;
			label.color = mDefaultColor;
			if (isPassword)
			{
				label.password = false;
			}
		}
		else
		{
			label.text = mText;
		}
		label.showLastPasswordChar = false;
		Input.imeCompositionMode = IMECompositionMode.Off;
	}

	private void Update()
	{
		if (mLastIME != Input.compositionString)
		{
			mLastIME = Input.compositionString;
			UpdateLabel();
		}
	}

	private void OnInput(string input)
	{
		if (!selected || !base.enabled || !base.gameObject.active || Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return;
		}
		int i = 0;
		for (int length = input.Length; i < length; i++)
		{
			char c = input[i];
			if (c == '\b')
			{
				if (mText.Length > 0)
				{
					mText = mText.Substring(0, mText.Length - 1);
				}
				continue;
			}
			if (c == '\r' || c == '\n')
			{
				current = this;
				if (eventReceiver == null)
				{
					eventReceiver = base.gameObject;
				}
				eventReceiver.SendMessage(functionName, SendMessageOptions.DontRequireReceiver);
				current = null;
				selected = false;
				return;
			}
			if (c >= ' ')
			{
				mText += c;
			}
		}
		UpdateLabel();
	}

	private void UpdateLabel()
	{
		if (maxChars > 0 && mText.Length > maxChars)
		{
			mText = mText.Substring(0, maxChars);
		}
		if (label.font != null)
		{
			string text = ((!selected) ? mText : (mText + Input.compositionString + caratChar));
			text = label.font.WrapText(text, (float)label.lineWidth / label.cachedTransform.localScale.x, true, false);
			if (!label.multiLine)
			{
				string[] array = text.Split('\n');
				text = ((array.Length <= 0) ? string.Empty : array[array.Length - 1]);
			}
			label.text = text;
			label.showLastPasswordChar = selected;
		}
	}
}
