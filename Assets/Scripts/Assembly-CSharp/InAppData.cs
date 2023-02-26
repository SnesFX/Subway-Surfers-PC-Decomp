using System.Collections.Generic;

public class InAppData
{
	public static readonly string inAppTier1 = "com.kiloo.subwaysurfers.coinstier1";

	public static readonly string inAppTier2 = "com.kiloo.subwaysurfers.coinstier2";

	public static readonly string inAppTier3 = "com.kiloo.subwaysurfers.coinstier3";

	public static readonly Dictionary<string, InAppProfile> inAppData = new Dictionary<string, InAppProfile>
	{
		{
			inAppTier1,
			new InAppProfile
			{
				amountOfCoins = 7500,
				title = "A pile of coins",
				iconName = "icon_coinPack_1"
			}
		},
		{
			inAppTier2,
			new InAppProfile
			{
				amountOfCoins = 45000,
				title = "A bunch of coins",
				iconName = "icon_coinPack_2"
			}
		},
		{
			inAppTier3,
			new InAppProfile
			{
				amountOfCoins = 180000,
				title = "A chest of coins",
				iconName = "icon_coinPack_3"
			}
		}
	};

	public int InAppPurchaseCount
	{
		get
		{
			return inAppData.Count;
		}
	}

	public string CommaSeparatedProductIds
	{
		get
		{
			string text = string.Empty;
			int num = 0;
			foreach (KeyValuePair<string, InAppProfile> inAppDatum in inAppData)
			{
				if (num > 0)
				{
					text += ",";
				}
				text += inAppDatum.Key;
				num++;
			}
			return text;
		}
	}
}
