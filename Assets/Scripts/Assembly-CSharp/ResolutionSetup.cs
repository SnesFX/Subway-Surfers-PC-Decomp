using System.Collections.Generic;
using UnityEngine;

public class ResolutionSetup : MonoBehaviour
{
	public UIAtlas[] usedAtlasses;

	public UIAtlas[] lowResAtlasses;

	public UIAtlas[] highResAtlasses;

	public UIFont[] usedFonts;

	public UIFont[] lowResFonts;

	public UIFont[] highResFonts;

	private List<UILabel> allLabels;

	private List<UIFont> allModifiedLabelsOldFonts;

	private void Awake()
	{
		if (lowResAtlasses.Length != highResAtlasses.Length)
		{
			Debug.LogError("Low res and high res atlasses do not fit!");
		}
		else if (lowResFonts.Length != highResFonts.Length)
		{
			Debug.LogError("Low res and high res fonts do not fit!");
		}
		else if (DeviceInfo.isHighres)
		{
			for (int i = 0; i < usedFonts.Length; i++)
			{
				usedFonts[i].replacement = highResFonts[i];
			}
			for (int j = 0; j < usedAtlasses.Length; j++)
			{
				usedAtlasses[j].replacement = highResAtlasses[j];
			}
		}
		else
		{
			for (int k = 0; k < usedFonts.Length; k++)
			{
				usedFonts[k].replacement = lowResFonts[k];
			}
			for (int l = 0; l < usedAtlasses.Length; l++)
			{
				usedAtlasses[l].replacement = lowResAtlasses[l];
			}
		}
	}

	private void OnDisable()
	{
		if (DeviceInfo.isHighres)
		{
			for (int i = 0; i < usedFonts.Length; i++)
			{
				usedFonts[i].replacement = lowResFonts[i];
			}
			for (int j = 0; j < highResAtlasses.Length; j++)
			{
				usedAtlasses[j].replacement = lowResAtlasses[j];
			}
		}
		else
		{
			Debug.Log("Is low res, no atlas change");
		}
	}

	public void SwitchFontResolution()
	{
		UILabel[] array = Resources.FindObjectsOfTypeAll(typeof(UILabel)) as UILabel[];
		allLabels = new List<UILabel>();
		allModifiedLabelsOldFonts = new List<UIFont>();
		UILabel[] array2 = array;
		foreach (UILabel uILabel in array2)
		{
			for (int j = 0; j < lowResFonts.Length; j++)
			{
				if (uILabel.font == lowResFonts[j])
				{
					Debug.Log("Switching to high res font now!");
					allLabels.Add(uILabel);
					allModifiedLabelsOldFonts.Add(lowResFonts[j]);
					uILabel.font = highResFonts[j];
					break;
				}
			}
		}
	}

	public void ResetFontResolution()
	{
		Debug.Log("Resetting fonts");
		for (int i = 0; i < allLabels.Count; i++)
		{
			allLabels[i].font = allModifiedLabelsOldFonts[i];
		}
	}
}
