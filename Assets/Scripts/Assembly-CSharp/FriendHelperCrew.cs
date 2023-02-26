using System;
using UnityEngine;

public class FriendHelperCrew : MonoBehaviour
{
	public GameObject collectButtonPrefab;

	public GameObject progressPrefab;

	public UILabel friendName;

	public UITexture friendPicture;

	public UISlicedSprite friendBackground;

	public FriendCrewPokeHelper pokeHelper;

	private GameObject _collectionIndicator;

	public Texture2D dummyImage;

	private bool _imageSet;

	private Friend _friend;

	private bool _initialized;

	public void InitFriend(Friend friend, bool backgroundActive = false)
	{
		_friend = friend;
		if (!backgroundActive)
		{
			friendBackground.alpha = 0f;
		}
		else
		{
			friendBackground.alpha = 0.1f;
		}
		friendName.text = friend.name;
		friendPicture.material = new Material(Shader.Find("Unlit/Transparent Colored"));
		if (_friend.image != null)
		{
			friendPicture.material.mainTexture = friend.image;
			_imageSet = true;
		}
		else
		{
			friendPicture.material.mainTexture = dummyImage;
		}
		GameObject gameObject;
		if (friend.gamesToCashIn >= 50)
		{
			gameObject = NGUITools.AddChild(base.gameObject, collectButtonPrefab);
			gameObject.GetComponent<UIButtonMessage>().target = base.gameObject;
			_collectionIndicator = gameObject;
			pokeHelper.DeactivatePoke();
		}
		else
		{
			gameObject = NGUITools.AddChild(base.gameObject, progressPrefab);
			FriendProgressHelper component = gameObject.GetComponent<FriendProgressHelper>();
			component.label.text = friend.gamesToCashIn + "/ 50 runs";
			component.slider.sliderValue = (float)friend.gamesToCashIn / 50f;
			if ((DateTime.UtcNow - friend.status.lastPokeTime).Days > 0)
			{
				if (friend.status.lastPokeTime == DateTime.MinValue)
				{
					SocialManager.instance.SetPokeFirstTime(friend);
					pokeHelper.DeactivatePoke();
				}
				else
				{
					pokeHelper.ActivatePoke(friend);
				}
			}
			else
			{
				pokeHelper.DeactivatePoke();
			}
		}
		_collectionIndicator = gameObject;
		_initialized = true;
	}

	public void CollectReward()
	{
		Debug.Log("Collecting reward");
		SocialManager.instance.CollectFriendReward(_friend);
		int num = UnityEngine.Random.Range(50, 350);
		PlayerInfo.Instance.amountOfCoins += num;
		NGUITools.SetActive(_collectionIndicator, false);
		UnityEngine.Object.Destroy(_collectionIndicator);
		_collectionIndicator = NGUITools.AddChild(base.gameObject, progressPrefab);
		FriendProgressHelper component = _collectionIndicator.GetComponent<FriendProgressHelper>();
		component.label.text = _friend.gamesToCashIn + "/ 50 runs";
		component.slider.sliderValue = (float)_friend.gamesToCashIn / 50f;
		UIScreenController.Instance.SpawnCollectText(component.GetCoinPouchGlobalPosition(), num.ToString());
		PlayerInfo.Instance.Save();
		SocialManager.instance.Save();
	}

	private void Update()
	{
		if (_initialized && !_imageSet && _friend.image != null)
		{
			friendPicture.material.mainTexture = _friend.image;
			_imageSet = true;
		}
	}
}
