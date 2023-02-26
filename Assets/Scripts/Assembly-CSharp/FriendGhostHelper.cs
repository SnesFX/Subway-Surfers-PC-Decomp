using System.Collections;
using UnityEngine;

public class FriendGhostHelper : MonoBehaviour
{
	public UISlicedSprite background;

	public UISlicedSprite frame;

	public UILabel points;

	public UITexture picture;

	private Transform _cachedTransform;

	private Vector3 _resetPosition = new Vector3(80f, 0f, 0f);

	private Vector3 _activePosition = Vector3.zero;

	private Vector3 _moveOutPosition = new Vector3(0f, -140f, 0f);

	private float _backgroundAlphaDefault;

	private float _frameAlphaDefault;

	private float _pointsAlphaDefault;

	private float _pictureAlphaDefault;

	private bool inited;

	private bool _gameRunning;

	public bool noFriendsLeftToGhost;

	private FriendGhostHandler handler;

	public bool animatingNow;

	private void Awake()
	{
		if (!inited)
		{
			Init();
		}
	}

	private void Init()
	{
		_backgroundAlphaDefault = background.alpha;
		_frameAlphaDefault = frame.alpha;
		_pointsAlphaDefault = points.alpha;
		_pictureAlphaDefault = picture.alpha;
		_cachedTransform = base.transform;
		picture.material = new Material(Shader.Find("Unlit/Transparent Colored"));
		inited = true;
		handler = _cachedTransform.parent.GetComponent<FriendGhostHandler>();
	}

	public void NewGame()
	{
		if (!inited)
		{
			Init();
		}
		_gameRunning = true;
		_cachedTransform.localPosition = _resetPosition;
		background.alpha = _backgroundAlphaDefault;
		frame.alpha = _frameAlphaDefault;
		points.alpha = _pointsAlphaDefault;
		picture.alpha = _pictureAlphaDefault;
		noFriendsLeftToGhost = false;
	}

	public void AnimateIn()
	{
		if (!noFriendsLeftToGhost)
		{
			StartCoroutine(_AnimateIn());
		}
	}

	public void AnimateOut()
	{
		if (_gameRunning)
		{
			StartCoroutine(_AnimateOut());
		}
	}

	public void NoFriendsLeft()
	{
		_cachedTransform.localPosition = _resetPosition;
		background.alpha = _backgroundAlphaDefault;
		frame.alpha = _frameAlphaDefault;
		points.alpha = _pointsAlphaDefault;
		picture.alpha = _pictureAlphaDefault;
		noFriendsLeftToGhost = true;
	}

	public void GameOver()
	{
		_gameRunning = false;
		_cachedTransform.localPosition = _resetPosition;
		background.alpha = _backgroundAlphaDefault;
		frame.alpha = _frameAlphaDefault;
		points.alpha = _pointsAlphaDefault;
		picture.alpha = _pictureAlphaDefault;
	}

	private IEnumerator _AnimateIn()
	{
		animatingNow = true;
		float duration = 0.5f;
		float factor2 = 0f;
		while (factor2 < 1f && _gameRunning)
		{
			factor2 += Time.deltaTime / duration;
			factor2 = Mathf.Clamp01(factor2);
			_cachedTransform.localPosition = Vector3.Lerp(_resetPosition, _activePosition, factor2);
			yield return null;
		}
		if (!_gameRunning)
		{
			_cachedTransform.localScale = _resetPosition;
		}
		else
		{
			_cachedTransform.localPosition = _activePosition;
		}
		animatingNow = false;
	}

	private IEnumerator _AnimateOut()
	{
		animatingNow = true;
		float duration = 0.5f;
		float factor2 = 0f;
		while (factor2 < 1f && _gameRunning)
		{
			factor2 += Time.deltaTime / duration;
			factor2 = Mathf.Clamp01(factor2);
			_cachedTransform.localPosition = Vector3.Lerp(_activePosition, _moveOutPosition, factor2);
			background.alpha = Mathf.Lerp(_backgroundAlphaDefault, 0f, factor2);
			frame.alpha = Mathf.Lerp(_frameAlphaDefault, 0f, factor2);
			points.alpha = Mathf.Lerp(_pointsAlphaDefault, 0f, factor2);
			picture.alpha = Mathf.Lerp(_pictureAlphaDefault, 0f, factor2);
			yield return null;
		}
		_cachedTransform.localPosition = _resetPosition;
		background.alpha = _backgroundAlphaDefault;
		frame.alpha = _frameAlphaDefault;
		points.alpha = _pointsAlphaDefault;
		picture.alpha = _pictureAlphaDefault;
		animatingNow = false;
		handler.FinishedAnimatingOut();
	}
}
