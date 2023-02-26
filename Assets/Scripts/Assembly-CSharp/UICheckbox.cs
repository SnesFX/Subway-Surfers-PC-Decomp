using AnimationOrTween;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Checkbox")]
public class UICheckbox : MonoBehaviour
{
	public static UICheckbox current;

	public UISprite checkSprite;

	public Animation checkAnimation;

	public GameObject eventReceiver;

	public string functionName = "OnActivate";

	public bool startsChecked = true;

	public Transform radioButtonRoot;

	public bool optionCanBeNone;

	[SerializeField]
	[HideInInspector]
	private bool option;

	private bool mChecked = true;

	private bool mStarted;

	private Transform mTrans;

	public bool isChecked
	{
		get
		{
			return mChecked;
		}
		set
		{
			if (radioButtonRoot == null || value || optionCanBeNone || !mStarted)
			{
				Set(value);
			}
		}
	}

	private void Awake()
	{
		mTrans = base.transform;
		if (checkSprite != null)
		{
			checkSprite.alpha = ((!startsChecked) ? 0f : 1f);
		}
		if (option)
		{
			option = false;
			if (radioButtonRoot == null)
			{
				radioButtonRoot = mTrans.parent;
			}
		}
	}

	private void Start()
	{
		if (eventReceiver == null)
		{
			eventReceiver = base.gameObject;
		}
		mChecked = !startsChecked;
		mStarted = true;
		Set(startsChecked);
	}

	private void OnClick()
	{
		if (base.enabled)
		{
			isChecked = !isChecked;
		}
	}

	private void Set(bool state)
	{
		if (!mStarted)
		{
			mChecked = state;
			startsChecked = state;
			if (checkSprite != null)
			{
				checkSprite.alpha = ((!state) ? 0f : 1f);
			}
		}
		else
		{
			if (mChecked == state)
			{
				return;
			}
			if (radioButtonRoot != null && state)
			{
				UICheckbox[] componentsInChildren = radioButtonRoot.GetComponentsInChildren<UICheckbox>(true);
				int i = 0;
				for (int num = componentsInChildren.Length; i < num; i++)
				{
					UICheckbox uICheckbox = componentsInChildren[i];
					if (uICheckbox != this && uICheckbox.radioButtonRoot == radioButtonRoot)
					{
						uICheckbox.Set(false);
					}
				}
			}
			mChecked = state;
			if (checkSprite != null)
			{
				Color color = checkSprite.color;
				color.a = ((!mChecked) ? 0f : 1f);
				TweenColor.Begin(checkSprite.gameObject, 0.2f, color);
			}
			if (eventReceiver != null && !string.IsNullOrEmpty(functionName))
			{
				current = this;
				eventReceiver.SendMessage(functionName, mChecked, SendMessageOptions.DontRequireReceiver);
			}
			if (checkAnimation != null)
			{
				ActiveAnimation.Play(checkAnimation, state ? Direction.Forward : Direction.Reverse);
			}
		}
	}
}
