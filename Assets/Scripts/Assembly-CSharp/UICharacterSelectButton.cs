using System;
using UnityEngine;

public class UICharacterSelectButton : MonoBehaviour
{
	public UISlicedSprite fillSprite;

	public UILabel label;

	private string fillSelect = "button_fill_select";

	private string fillSelected = "button_fill_selected";

	private string fillNotAvailable = "button_fill_info";

	private string textSelect = "SELECT";

	private string textSelected = "SELECTED";

	private string textBuy = "BUY\n{0} COINS";

	private string textNotAvailable = "COLLECT ALL\n{0}/{1}";

	private bool isEnabled = true;

	private BoxCollider col;

	private void OnEnable()
	{
		OnChangedCurrentlyShownModel();
	}

	private void Awake()
	{
		UIModelController instance = UIModelController.Instance;
		instance.OnChangedCurrentlyShown = (Action)Delegate.Combine(instance.OnChangedCurrentlyShown, new Action(OnChangedCurrentlyShownModel));
		col = GetComponent<BoxCollider>();
		OnChangedCurrentlyShownModel();
	}

	private void OnDestroy()
	{
		UIModelController instance = UIModelController.Instance;
		instance.OnChangedCurrentlyShown = (Action)Delegate.Remove(instance.OnChangedCurrentlyShown, new Action(OnChangedCurrentlyShownModel));
	}

	private void OnChangedCurrentlyShownModel()
	{
		CharacterModels.ModelType currentlyShownModel = (CharacterModels.ModelType)UIModelController.Instance.currentlyShownModel;
		CharacterModels.Model model = CharacterModels.modelData[currentlyShownModel];
		if (PlayerInfo.Instance.currentCharacter == UIModelController.Instance.currentlyShownModel)
		{
			showAndEnable();
			fillSprite.spriteName = fillSelected;
			label.text = textSelected;
			col.enabled = false;
		}
		else if (PlayerInfo.Instance.IsCollectionComplete(currentlyShownModel) ? true : false)
		{
			showAndEnable();
			fillSprite.spriteName = fillSelect;
			label.text = textSelect;
			col.enabled = true;
		}
		else if (model.UnlockType == CharacterModels.UnlockType.tokens)
		{
			showAndEnable();
			fillSprite.spriteName = fillNotAvailable;
			label.text = string.Format(textNotAvailable, PlayerInfo.Instance.GetCollectedTokens(currentlyShownModel), model.Price);
			col.enabled = false;
		}
		else
		{
			hideAndDisable();
		}
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
}
