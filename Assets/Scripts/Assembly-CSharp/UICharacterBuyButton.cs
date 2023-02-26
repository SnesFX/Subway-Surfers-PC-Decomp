using System;
using UnityEngine;

public class UICharacterBuyButton : MonoBehaviour
{
	private const string TEXT_BUY = "UNLOCK FOR\n{0} COINS";

	public UILabel label;

	private BoxCollider col;

	private bool isEnabled = true;

	private bool _purchaseInProgress;

	public Action OnChangedCurrentlyShown;

	private void OnEnable()
	{
		isEnabled = true;
	}

	private void Awake()
	{
		UIModelController instance = UIModelController.Instance;
		instance.OnChangedCurrentlyShown = (Action)Delegate.Combine(instance.OnChangedCurrentlyShown, new Action(OnChangedCurrentlyShownModel));
		col = GetComponent<BoxCollider>();
	}

	private void OnClick()
	{
		if (!_purchaseInProgress)
		{
			int currentlyShownModel = UIModelController.Instance.currentlyShownModel;
			CharacterModels.ModelType modelType = (CharacterModels.ModelType)currentlyShownModel;
			CharacterModels.Model model = CharacterModels.modelData[modelType];
			Debug.Log("Buy: " + modelType);
			PurchaseHandler.Instance.PurchaseCharacter(modelType, this);
		}
	}

	private void OnChangedCurrentlyShownModel()
	{
		CharacterModels.ModelType currentlyShownModel = (CharacterModels.ModelType)UIModelController.Instance.currentlyShownModel;
		CharacterModels.Model model = CharacterModels.modelData[currentlyShownModel];
		if (model.UnlockType != CharacterModels.UnlockType.coins)
		{
			hideAndDisable();
			return;
		}
		if (PlayerInfo.Instance.IsCollectionComplete(currentlyShownModel) ? true : false)
		{
			hideAndDisable();
			return;
		}
		showAndEnable();
		int price = model.Price;
		label.text = string.Format("UNLOCK FOR\n{0} COINS", price);
	}

	private void OnDestroy()
	{
		UIModelController instance = UIModelController.Instance;
		instance.OnChangedCurrentlyShown = (Action)Delegate.Remove(instance.OnChangedCurrentlyShown, new Action(OnChangedCurrentlyShownModel));
	}

	private void hideAndDisable()
	{
		if (isEnabled)
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				Transform child = base.transform.GetChild(i);
				child.gameObject.active = false;
			}
			col.enabled = false;
			isEnabled = false;
		}
	}

	private void showAndEnable()
	{
		if (!isEnabled)
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				Transform child = base.transform.GetChild(i);
				child.gameObject.active = true;
			}
			col.enabled = true;
			isEnabled = true;
		}
	}

	public void PurchaseSuccessful()
	{
		_purchaseInProgress = false;
		UIModelController.Instance.SelectCurrentModel();
	}

	public void PurchaseFailure()
	{
		_purchaseInProgress = false;
	}
}
