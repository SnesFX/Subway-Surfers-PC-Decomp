using System;
using System.Collections;
using UnityEngine;

public class MysteryBoxHandler : MonoBehaviour
{
	private const string CONTINUELABEL_OPEN = "Tap to open";

	private const string CONTINUELABEL_CONTINUE = "Tap to continue";

	private const float BOX_IDLE_ANIM_SPEED = 2f;

	private const float BOX_IDLE_ANIM_AMOUNT = 10f;

	private const float BOX_IDLE_ANIM_END_SPEED = 20f;

	public GameObject boxParent;

	public GameObject rewardLabelTemplate;

	private Vector3 _outOfScreenPosition = new Vector3(0f, -500f, 0f);

	public GameObject openButton;

	public GameObject continueButton;

	public UILabel coinboxLabel;

	public GameObject mainSlot;

	public GameObject slotSecondBox;

	public GameObject boxPrefab;

	public GameObject testRewardPrefab;

	public GameObject rewardCoins;

	public GameObject rewardTokenTricky;

	public GameObject rewardTokenFresh;

	public GameObject rewardTokenSpike;

	public GameObject rewardTokenYutani;

	public GameObject rewardPowerupHoverboard;

	public GameObject rewardPowerupHeadstart500;

	public GameObject rewardPowerupHeadstart2000;

	public UILabel ContinueLabel;

	public AudioStateLoop audioStateLoop;

	private GameObject _boxMain;

	private GameObject _boxSecond;

	private bool anotherBox;

	private Vector3 _boxScale = new Vector3(1150f, 1150f, 1150f);

	private Vector3 _boxRotation = new Vector3(0f, 250f, 20f);

	private Vector3 _rewardStartRotation = new Vector3(15f, -10.5f, 0f);

	private Vector3 _rewardStartScale = new Vector3(5f, 5f, 5f);

	private Vector3 _rewardEndScale_coin = new Vector3(10f, 10f, 10f);

	private Vector3 _rewardEndScale = new Vector3(20f, 20f, 20f);

	private GameObject _rewardMain;

	private UILabel _labelSingle;

	private UILabel _labelDouble1;

	private Vector3 _labelPosition = new Vector3(0f, 134f, -5f);

	private bool _maySetTimeScale;

	public GameObject GlowEffect;

	private bool stopBoxIdleAnim;

	private bool openingBoxNow;

	private void Awake()
	{
		audioStateLoop = this.FindObject<AudioStateLoop>();
	}

	public void SetupMysteryBoxScreen()
	{
		boxParent.transform.position = UIModelController.Instance.MysteryBoxAnchor.transform.position;
		int mysteryBoxesToUnlock = PlayerInfo.Instance.mysteryBoxesToUnlock;
		PlayerInfo.Instance.mysteryBoxesToUnlock = 0;
		ContinueLabel.alpha = 1f;
		ContinueLabel.text = "Tap to open";
		foreach (Transform item in mainSlot.transform)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
		foreach (Transform item2 in slotSecondBox.transform)
		{
			UnityEngine.Object.Destroy(item2.gameObject);
		}
		if (mysteryBoxesToUnlock == 1)
		{
			SingleBox();
		}
		else if (mysteryBoxesToUnlock >= 2)
		{
			if (mysteryBoxesToUnlock > 2)
			{
				Debug.LogError("Mysterybox screen setup with " + mysteryBoxesToUnlock + " boxes. Should not happen ever.");
			}
			TwoBoxes();
		}
		else
		{
			Debug.LogError("Mysterybox screen setup with " + mysteryBoxesToUnlock + " boxes. Should not happen ever.");
		}
		if (_boxMain != null)
		{
			StartCoroutine(BoxIdleAnimCoroutine(_boxMain.transform));
		}
	}

	public void SkipNow()
	{
		if (_maySetTimeScale)
		{
			Time.timeScale = 3f;
		}
	}

	public void SingleBox()
	{
		GameObject gameObject = NGUITools.AddChild(mainSlot, boxPrefab);
		gameObject.transform.localScale = _boxScale;
		gameObject.transform.localRotation = Quaternion.Euler(_boxRotation);
		Utility.SetLayerRecursively(gameObject.transform, boxParent.layer);
		_boxMain = gameObject;
		GlowEffect.GetComponent<MeshRenderer>().material.SetColor("_MainColor", Color.black);
		openButton.GetComponent<Collider>().enabled = true;
	}

	public void TwoBoxes()
	{
		GameObject gameObject = NGUITools.AddChild(mainSlot, boxPrefab);
		gameObject.transform.localScale = _boxScale;
		gameObject.transform.localRotation = Quaternion.Euler(_boxRotation);
		Utility.SetLayerRecursively(gameObject.transform, boxParent.layer);
		_boxMain = gameObject;
		gameObject = NGUITools.AddChild(slotSecondBox, boxPrefab);
		gameObject.transform.localScale = _boxScale;
		gameObject.transform.localRotation = Quaternion.Euler(_boxRotation);
		Utility.SetLayerRecursively(gameObject.transform, boxParent.layer);
		_boxSecond = gameObject;
		GlowEffect.GetComponent<MeshRenderer>().material.SetColor("_MainColor", Color.black);
		openButton.GetComponent<Collider>().enabled = true;
	}

	private IEnumerator BoxIdleAnimCoroutine(Transform boxTrans)
	{
		Vector3 baseLocalPos = boxTrans.localPosition;
		stopBoxIdleAnim = false;
		float t = 0f;
		Vector3 newLocalPos = baseLocalPos;
		while (!stopBoxIdleAnim)
		{
			t += Time.deltaTime;
			newLocalPos.y = baseLocalPos.y + Mathf.Sin(t * 2f) * 10f;
			boxTrans.localPosition = newLocalPos;
			yield return null;
		}
		bool doneResetting = false;
		while (!doneResetting)
		{
			newLocalPos.y = Mathf.MoveTowards(newLocalPos.y, baseLocalPos.y, Time.deltaTime * 20f);
			if (Mathf.Approximately(newLocalPos.y, baseLocalPos.y))
			{
				doneResetting = true;
			}
			boxTrans.localPosition = newLocalPos;
			yield return null;
		}
	}

	private IEnumerator _MoveSecondBoxToFront()
	{
		_boxMain = _boxSecond;
		_boxSecond = null;
		anotherBox = false;
		GlowEffect.GetComponent<MeshRenderer>().material.SetColor("_MainColor", Color.black);
		_boxMain.transform.parent = mainSlot.transform;
		yield return StartCoroutine(MoveGameObject(_boxMain.transform, 1f, Vector3.zero));
		openButton.GetComponent<Collider>().enabled = true;
		StartCoroutine(BoxIdleAnimCoroutine(_boxMain.transform));
	}

	public void TestDown()
	{
		stopBoxIdleAnim = true;
		Animation componentInChildren = _boxMain.GetComponentInChildren<Animation>();
		componentInChildren.Play("down");
	}

	public void TestUp()
	{
		openButton.GetComponent<Collider>().enabled = false;
		if (!openingBoxNow)
		{
			MysteryBoxReward reward = MysteryBox.Roll();
			StartCoroutine(AnimateAlpha(ContinueLabel, 0.5f, 0f));
			StartCoroutine(_ShowReward(reward, _boxMain));
			openingBoxNow = true;
		}
	}

	private IEnumerator _ShowReward(MysteryBoxReward reward, GameObject box)
	{
		GameObject rewardGo = NGUITools.AddChild(prefab: ChooseRewardPrefab(reward), parent: mainSlot);
		rewardGo.transform.localScale = _rewardStartScale;
		rewardGo.transform.localRotation = Quaternion.Euler(_rewardStartRotation);
		Utility.SetLayerRecursively(rewardGo.transform, boxParent.layer);
		audioStateLoop.PlayMysteryBoxOpenSound();
		Animation animation = box.GetComponentInChildren<Animation>();
		animation.Play("up");
		while (animation["up"].normalizedTime < 0.5f)
		{
			yield return null;
		}
		_maySetTimeScale = true;
		if (reward.type == MysteryBoxRewardType.Coins)
		{
			StartCoroutine(ScaleGameObject(rewardGo.transform, 2f, _rewardEndScale_coin));
		}
		else
		{
			StartCoroutine(ScaleGameObject(rewardGo.transform, 2f, _rewardEndScale));
		}
		StartCoroutine(MoveGameObject(box.transform, 2f, _outOfScreenPosition));
		StartCoroutine(RotateGameObject(rewardGo.transform, 6f, new Vector3(0f, 1500f, 0f)));
		yield return new WaitForSeconds(0.5f);
		StartCoroutine(AnimateColor(GlowEffect.GetComponent<MeshRenderer>().material, 1.5f, Color.white));
		StartCoroutine(RotateGameObject(GlowEffect.transform, 3f, new Vector3(0f, 0f, -270f)));
		yield return new WaitForSeconds(1.3f);
		GameObject labelGo = NGUITools.AddChild(base.gameObject, rewardLabelTemplate);
		labelGo.transform.localPosition = _labelPosition;
		MysteryBoxRewardLabelTemplate template = labelGo.GetComponent<MysteryBoxRewardLabelTemplate>();
		if (reward.type == MysteryBoxRewardType.Coins)
		{
			template.SetupCoins(reward.amount);
		}
		else if (reward.type == MysteryBoxRewardType.powerup)
		{
			template.SetupPowerup(reward.powerupType, reward.amount);
		}
		else if (reward.type == MysteryBoxRewardType.token)
		{
			template.SetupToken(reward.tokenType);
		}
		StartCoroutine(AnimateAlpha(template, 0.2f, 1f));
		yield return new WaitForSeconds(0.5f);
		if (reward.type == MysteryBoxRewardType.Coins)
		{
			StartCoroutine(CountUpCoins(reward.amount, template));
		}
		yield return new WaitForSeconds(2.5f);
		StartCoroutine(AnimateColor(GlowEffect.GetComponent<MeshRenderer>().material, 0.5f, Color.black));
		if (reward.type == MysteryBoxRewardType.powerup)
		{
			PlayerInfo.Instance.IncreaseUpgradeAmount(reward.powerupType, reward.amount);
		}
		else if (reward.type == MysteryBoxRewardType.token)
		{
			PlayerInfo.Instance.CollectToken(reward.tokenType);
		}
		PlayerInfo.Instance.Save();
		Flurry.LogEvent("Mystery Box opened");
		if (_boxSecond != null)
		{
			anotherBox = true;
		}
		yield return new WaitForSeconds(2f);
		ContinueLabel.text = "Tap to continue";
		StartCoroutine(AnimateAlpha(ContinueLabel, 0.5f, 1f));
		while (!Input.GetMouseButtonUp(0))
		{
			yield return null;
		}
		Time.timeScale = 1f;
		_maySetTimeScale = false;
		UnityEngine.Object.Destroy(rewardGo);
		UnityEngine.Object.Destroy(labelGo);
		UnityEngine.Object.Destroy(box);
		_FinishOpening();
	}

	private void _FinishOpening()
	{
		openingBoxNow = false;
		if (anotherBox)
		{
			StartCoroutine(_MoveSecondBoxToFront());
			return;
		}
		UIScreenController.Instance.ClosePopup();
		openButton.GetComponent<Collider>().enabled = false;
	}

	private IEnumerator RotateGameObject(Transform trans, float duration, Vector3 angleToRotate)
	{
		Quaternion fromRotation = trans.localRotation;
		float factor2 = 0f;
		while (factor2 < 1f)
		{
			factor2 += Time.deltaTime / duration;
			factor2 = Mathf.Clamp01(factor2);
			float angle2 = 0f;
			angle2 = Mathf.Lerp(4.712389f, (float)Math.PI * 2f, factor2);
			float cosFactor = Mathf.Cos(angle2) * 0.5f + 0.5f;
			trans.localRotation = fromRotation;
			trans.Rotate(angleToRotate * cosFactor, Space.World);
			yield return null;
		}
	}

	private IEnumerator CountUpCoins(int amount, MysteryBoxRewardLabelTemplate rewardTemplate)
	{
		float countFactor = 0f;
		float countTime = Mathf.Lerp(0.3f, 2f, (float)amount / 100f);
		int coinboxFrom = PlayerInfo.Instance.amountOfCoins;
		int coinboxTo = coinboxFrom + amount;
		int rewardLabelTo = 0;
		yield return new WaitForSeconds(0.5f);
		while (countFactor < 1f)
		{
			countFactor += Time.deltaTime / countTime;
			rewardTemplate.UpdateCoins(Mathf.RoundToInt(Mathf.SmoothStep(amount, rewardLabelTo, countFactor)));
			coinboxLabel.text = string.Empty + Mathf.RoundToInt(Mathf.SmoothStep(coinboxFrom, coinboxTo, countFactor));
			yield return null;
		}
		Missions.Instance.PlayerDidThis(Missions.MissionTarget.EarnCoin, amount);
		PlayerInfo.Instance.amountOfCoins = coinboxTo;
		PlayerInfo.Instance.Save();
	}

	private IEnumerator AnimateAlpha(UILabel label, float duration, float toAlpha)
	{
		float fromAlpha = label.alpha;
		float factor2 = 0f;
		while (factor2 < 1f)
		{
			factor2 += Time.deltaTime / duration;
			factor2 = Mathf.Clamp01(factor2);
			label.alpha = Mathf.Lerp(fromAlpha, toAlpha, factor2);
			yield return null;
		}
	}

	private IEnumerator AnimateAlpha(MysteryBoxRewardLabelTemplate template, float duration, float toAlpha)
	{
		float fromAlpha = template.Alpha;
		float factor2 = 0f;
		while (factor2 < 1f)
		{
			factor2 += Time.deltaTime / duration;
			factor2 = Mathf.Clamp01(factor2);
			template.Alpha = Mathf.Lerp(fromAlpha, toAlpha, factor2);
			yield return null;
		}
	}

	private IEnumerator AnimateColor(UIWidget widget, float duration, Color toColor)
	{
		Color fromColor = widget.color;
		float factor2 = 0f;
		while (factor2 < 1f)
		{
			factor2 += Time.deltaTime / duration;
			factor2 = Mathf.Clamp01(factor2);
			widget.color = Color.Lerp(fromColor, toColor, factor2);
			yield return null;
		}
	}

	private IEnumerator AnimateColor(Material material, float duration, Color toColor)
	{
		Color fromColor = material.GetColor("_MainColor");
		float factor2 = 0f;
		while (factor2 < 1f)
		{
			factor2 += Time.deltaTime / duration;
			factor2 = Mathf.Clamp01(factor2);
			material.SetColor("_MainColor", Color.Lerp(fromColor, toColor, factor2));
			yield return null;
		}
	}

	private IEnumerator MoveGameObject(Transform trans, float duration, Vector3 toPos)
	{
		Vector3 fromPos = trans.localPosition;
		float factor2 = 0f;
		while (factor2 < 1f)
		{
			factor2 += Time.deltaTime / duration;
			factor2 = Mathf.Clamp01(factor2);
			trans.localPosition = Vector3.Lerp(fromPos, toPos, factor2 * factor2 * 6f);
			yield return null;
		}
	}

	private IEnumerator ScaleGameObject(Transform trans, float duration, Vector3 toScale)
	{
		float factor2 = 0f;
		Vector3 fromScale = trans.localScale;
		while (factor2 < 1f)
		{
			factor2 += Time.deltaTime / duration;
			factor2 = Mathf.Clamp01(factor2);
			float angle2 = 0f;
			angle2 = Mathf.Lerp((float)Math.PI, (float)Math.PI * 2f, factor2);
			float cosFactor = Mathf.Cos(angle2) * 0.5f + 0.5f;
			trans.localScale = Vector3.Lerp(fromScale, toScale, cosFactor);
			yield return null;
		}
	}

	private GameObject ChooseRewardPrefab(MysteryBoxReward reward)
	{
		GameObject gameObject = rewardCoins;
		switch (reward.type)
		{
		case MysteryBoxRewardType.Coins:
			gameObject = rewardCoins;
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.CollectCoinPouch);
			break;
		case MysteryBoxRewardType.powerup:
			switch (reward.powerupType)
			{
			case PowerupType.hoverboard:
				gameObject = rewardPowerupHoverboard;
				break;
			case PowerupType.headstart500:
				gameObject = rewardPowerupHeadstart500;
				break;
			case PowerupType.headstart2000:
				gameObject = rewardPowerupHeadstart2000;
				break;
			}
			break;
		case MysteryBoxRewardType.token:
			Missions.Instance.PlayerDidThis(Missions.MissionTarget.Tokens);
			switch (reward.tokenType)
			{
			case CharacterModels.ModelType.fresh:
				gameObject = rewardTokenFresh;
				break;
			case CharacterModels.ModelType.tricky:
				gameObject = rewardTokenTricky;
				break;
			case CharacterModels.ModelType.spike:
				gameObject = rewardTokenSpike;
				break;
			case CharacterModels.ModelType.yutani:
				gameObject = rewardTokenYutani;
				break;
			}
			break;
		}
		Debug.Log("Rewardprefab: " + gameObject.name);
		return gameObject;
	}
}
