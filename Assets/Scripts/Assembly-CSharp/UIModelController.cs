using System;
using UnityEngine;

public class UIModelController : MonoBehaviour
{
	public enum ModelScreen
	{
		Character = 0,
		GameOver = 1
	}

	public GameObject CharacterAnchor;

	public GameObject GameOverAnchor;

	public GameObject MysteryBoxAnchor;

	public GameObject ModelPrefab;

	public Action OnChangedCurrentlyShown;

	private int _currentlyShownModel;

	private int _numberOfModels = 8;

	private static UIModelController _instance;

	public int currentlyShownModel
	{
		get
		{
			return _currentlyShownModel;
		}
	}

	public static UIModelController Instance
	{
		get
		{
			return _instance ?? (_instance = UnityEngine.Object.FindObjectOfType(typeof(UIModelController)) as UIModelController);
		}
	}

	public void ActivateGameOverModel()
	{
		_ActivateModel((CharacterModels.ModelType)PlayerInfo.Instance.currentCharacter, ModelScreen.GameOver);
	}

	public void SelectCurrentModel()
	{
		PlayerInfo.Instance.currentCharacter = _currentlyShownModel;
		PlayerInfo.Instance.Save();
		if (Game.Instance != null)
		{
			Game.Instance.Character.characterModel.ChangeCharacterModel(((CharacterModels.ModelType)_currentlyShownModel).ToString());
		}
		Action onChangedCurrentlyShown = OnChangedCurrentlyShown;
		if (onChangedCurrentlyShown != null)
		{
			onChangedCurrentlyShown();
		}
	}

	public void ActivateCharacterModel()
	{
		_currentlyShownModel = PlayerInfo.Instance.currentCharacter;
		_ActivateModel((CharacterModels.ModelType)_currentlyShownModel, ModelScreen.Character);
		Action onChangedCurrentlyShown = OnChangedCurrentlyShown;
		if (onChangedCurrentlyShown != null)
		{
			onChangedCurrentlyShown();
		}
	}

	public void ChangeModelRight()
	{
		_currentlyShownModel++;
		_currentlyShownModel %= _numberOfModels;
		_ActivateModel((CharacterModels.ModelType)_currentlyShownModel, ModelScreen.Character);
		Action onChangedCurrentlyShown = OnChangedCurrentlyShown;
		if (onChangedCurrentlyShown != null)
		{
			onChangedCurrentlyShown();
		}
	}

	public void ChangeModelLeft()
	{
		_currentlyShownModel--;
		if (_currentlyShownModel < 0)
		{
			_currentlyShownModel = _numberOfModels - 1;
		}
		_ActivateModel((CharacterModels.ModelType)_currentlyShownModel, ModelScreen.Character);
		Action onChangedCurrentlyShown = OnChangedCurrentlyShown;
		if (onChangedCurrentlyShown != null)
		{
			onChangedCurrentlyShown();
		}
	}

	private void _ActivateModel(CharacterModels.ModelType name, ModelScreen screen)
	{
		ClearModels();
		switch (screen)
		{
		case ModelScreen.Character:
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate(ModelPrefab) as GameObject;
			gameObject2.transform.parent = CharacterAnchor.transform;
			gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
			Utility.SetLayerRecursively(gameObject2.transform, CharacterAnchor.layer);
			gameObject2.transform.localScale = new Vector3(21f, 21f, 21f);
			gameObject2.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
			CharacterModel component2 = gameObject2.GetComponent<CharacterModel>();
			component2.ChangeCharacterModel(name.ToString());
			component2.HideAllPowerups();
			component2.StartIdleAnimations();
			break;
		}
		case ModelScreen.GameOver:
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(ModelPrefab) as GameObject;
			gameObject.transform.parent = GameOverAnchor.transform;
			gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
			Utility.SetLayerRecursively(gameObject.transform, GameOverAnchor.layer);
			gameObject.transform.localScale = new Vector3(18f, 18f, 18f);
			gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
			CharacterModel component = gameObject.GetComponent<CharacterModel>();
			component.ChangeCharacterModel(name.ToString());
			component.HideAllPowerups();
			component.StartIdleAnimations();
			break;
		}
		}
	}

	public void ClearModels()
	{
		foreach (Transform item in CharacterAnchor.transform)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
		foreach (Transform item2 in GameOverAnchor.transform)
		{
			UnityEngine.Object.Destroy(item2.gameObject);
		}
	}
}
