using System;
using UnityEngine;

public class UITokenScreenHandler : MonoBehaviour
{
	public UISlider trickyProgress;

	public UILabel trickyLabel;

	public UISlider spikeProgress;

	public UILabel spikeLabel;

	public UISlider yutaniProgress;

	public UILabel yutaniLabel;

	public UISlider freshProgress;

	public UILabel freshLabel;

	public UILabel trickyNameLabel;

	public UILabel spikeNameLabel;

	public UILabel yutaniNameLabel;

	public UILabel freshNameLabel;

	private void Awake()
	{
		PlayerInfo instance = PlayerInfo.Instance;
		instance.OnTokenCollected = (Action<CharacterModels.ModelType>)Delegate.Combine(instance.OnTokenCollected, new Action<CharacterModels.ModelType>(UpdateTokens));
		UpdateTokens();
		trickyNameLabel.text = CharacterModels.modelData[CharacterModels.ModelType.tricky].TokenName;
		spikeNameLabel.text = CharacterModels.modelData[CharacterModels.ModelType.spike].TokenName;
		yutaniNameLabel.text = CharacterModels.modelData[CharacterModels.ModelType.yutani].TokenName;
		freshNameLabel.text = CharacterModels.modelData[CharacterModels.ModelType.fresh].TokenName;
	}

	private void UpdateTokens(CharacterModels.ModelType type = CharacterModels.ModelType.yutani)
	{
		int collectedTokens = PlayerInfo.Instance.GetCollectedTokens(CharacterModels.ModelType.tricky);
		int price = CharacterModels.modelData[CharacterModels.ModelType.tricky].Price;
		trickyProgress.sliderValue = (float)collectedTokens / (float)price;
		if (collectedTokens < price)
		{
			trickyLabel.text = collectedTokens + "/" + price;
		}
		else
		{
			trickyLabel.text = string.Empty + collectedTokens;
		}
		int collectedTokens2 = PlayerInfo.Instance.GetCollectedTokens(CharacterModels.ModelType.spike);
		int price2 = CharacterModels.modelData[CharacterModels.ModelType.spike].Price;
		spikeProgress.sliderValue = (float)collectedTokens2 / (float)price2;
		if (collectedTokens2 < price2)
		{
			spikeLabel.text = collectedTokens2 + "/" + price2;
		}
		else
		{
			spikeLabel.text = string.Empty + collectedTokens2;
		}
		int collectedTokens3 = PlayerInfo.Instance.GetCollectedTokens(CharacterModels.ModelType.yutani);
		int price3 = CharacterModels.modelData[CharacterModels.ModelType.yutani].Price;
		yutaniProgress.sliderValue = (float)collectedTokens3 / (float)price3;
		if (collectedTokens3 < price3)
		{
			yutaniLabel.text = collectedTokens3 + "/" + price3;
		}
		else
		{
			yutaniLabel.text = string.Empty + collectedTokens3;
		}
		int collectedTokens4 = PlayerInfo.Instance.GetCollectedTokens(CharacterModels.ModelType.fresh);
		int price4 = CharacterModels.modelData[CharacterModels.ModelType.fresh].Price;
		freshProgress.sliderValue = (float)collectedTokens4 / (float)price4;
		if (collectedTokens4 < price4)
		{
			freshLabel.text = collectedTokens4 + "/" + price4;
		}
		else
		{
			freshLabel.text = string.Empty + collectedTokens4;
		}
	}
}
