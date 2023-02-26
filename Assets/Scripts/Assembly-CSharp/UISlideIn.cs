using UnityEngine;

public class UISlideIn : IgnoreTimeScale
{
	protected Vector3 posOff = new Vector3(0f, 65f, 0f);

	protected Vector3 posOn = new Vector3(0f, -5f, 0f);

	private bool _triggerSlideOut;

	private float _slideOutTimer = 3f;

	private bool _triggerReadyForNext;

	private float _readyForNextTimer = 1f;

	protected virtual void Start()
	{
		base.transform.localPosition = posOff;
		base.gameObject.SetActiveRecursively(false);
	}

	public void SetupSlideIn()
	{
		base.gameObject.SetActiveRecursively(true);
		SlideIn();
	}

	protected virtual void SlideIn()
	{
		SpringPosition.Begin(base.gameObject, posOn, 10f).ignoreTimeScale = true;
		_slideOutTimer = 3f;
		_readyForNextTimer = 1f;
		_triggerSlideOut = true;
	}

	protected virtual void SlideOut()
	{
		SpringPosition.Begin(base.gameObject, posOff, 10f).ignoreTimeScale = true;
		_triggerReadyForNext = true;
	}

	protected virtual void ReadyForNewMessage()
	{
		base.gameObject.SetActiveRecursively(false);
		UIScreenController.Instance.ReadyForNextSlide();
	}

	private void Update()
	{
		float num = UpdateRealTimeDelta();
		if (_triggerSlideOut)
		{
			_slideOutTimer -= num;
			if (_slideOutTimer <= 0f)
			{
				SlideOut();
				_triggerSlideOut = false;
			}
		}
		if (_triggerReadyForNext)
		{
			_readyForNextTimer -= num;
			if (_readyForNextTimer <= 0f)
			{
				_triggerReadyForNext = false;
				ReadyForNewMessage();
			}
		}
	}
}
