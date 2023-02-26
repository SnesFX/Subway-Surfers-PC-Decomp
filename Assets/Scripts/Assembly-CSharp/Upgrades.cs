using System.Collections.Generic;

public class Upgrades
{
	public static float UpgradeFirstSpawnMeters = 250f;

	public static float UpgradeSpawnSpacingMeters = 300f;

	public static readonly Dictionary<PowerupType, Upgrade> upgrades = new Dictionary<PowerupType, Upgrade>
	{
		{
			PowerupType.hoverboard,
			new Upgrade
			{
				name = "Hoverboard",
				description = "Protect yourself against dying for a period of time. Activate by double tapping.",
				durations = new float[1] { 30f },
				pricesRaw = new int[1] { 150 },
				iconName = "icon_upgrades_hoverboard"
			}
		},
		{
			PowerupType.headstart500,
			new Upgrade
			{
				name = "Headstart 250",
				durations = new float[1] { 250f },
				description = "Skip ahead 250 meters in a run.",
				pricesRaw = new int[1] { 200 },
				iconName = "icon_upgrades_headstart500"
			}
		},
		{
			PowerupType.headstart2000,
			new Upgrade
			{
				name = "Headstart 1000",
				durations = new float[1] { 1000f },
				description = "Skip ahead 1000 meters in a run.",
				pricesRaw = new int[1] { 1000 },
				iconName = "icon_upgrades_headstart2000"
			}
		},
		{
			PowerupType.mysterybox,
			new Upgrade
			{
				name = "Mystery Box",
				description = "Take a chance and go for the big win.",
				spawnProbability = 5f,
				minimumMeters = 99999,
				pricesRaw = new int[1] { 250 },
				iconName = "icon_upgrades_mysteryBox"
			}
		},
		{
			PowerupType.jetpack,
			new Upgrade
			{
				name = "Jetpack",
				description = "Increases the duration of the Spray Can Jetpack pickup.",
				numberOfTiers = 6,
				durations = new float[6] { 8f, 9f, 10.5f, 12.5f, 15f, 19f },
				spawnProbability = 10f,
				minimumMeters = 1000,
				pricesRaw = new int[6] { 0, 250, 750, 1500, 5000, 15000 },
				iconName = "icon_upgrades_jetpack"
			}
		},
		{
			PowerupType.supersneakers,
			new Upgrade
			{
				name = "Super Sneakers",
				description = "Increases the duration of the Super Sneakers.",
				numberOfTiers = 6,
				spawnProbability = 25f,
				durations = new float[6] { 10f, 11.5f, 13.4f, 15.8f, 19f, 24f },
				pricesRaw = new int[6] { 0, 250, 750, 1500, 5000, 15000 },
				iconName = "icon_upgrades_superSneakers"
			}
		},
		{
			PowerupType.coinmagnet,
			new Upgrade
			{
				name = "Coin Magnet",
				description = "Increases the duration of the Coin Magnet pickup.",
				numberOfTiers = 6,
				durations = new float[6] { 10f, 11.5f, 13.4f, 15.8f, 19f, 24f },
				spawnProbability = 25f,
				coinmagnetRange = 2,
				pricesRaw = new int[6] { 0, 250, 750, 1500, 5000, 15000 },
				iconName = "icon_upgrades_coinMagnet"
			}
		},
		{
			PowerupType.doubleMultiplier,
			new Upgrade
			{
				name = "2x Multiplier",
				description = "Increases the duration of the Double Multiplier pickup.",
				numberOfTiers = 6,
				durations = new float[6] { 10f, 11.5f, 13.4f, 15.8f, 19f, 24f },
				spawnProbability = 20f,
				pricesRaw = new int[6] { 0, 250, 750, 1500, 5000, 15000 },
				iconName = "icon_upgrades_doubleScore"
			}
		},
		{
			PowerupType.coinpouch,
			new Upgrade()
		},
		{
			PowerupType.skipmission1,
			new Upgrade
			{
				name = "Skip Mission 1",
				description = "Skip your current mission 1",
				pricesRaw = new int[1] { 1500 },
				iconName = "icon_upgrades_skipMission1",
				levelPriceMultiplyer = 50
			}
		},
		{
			PowerupType.skipmission2,
			new Upgrade
			{
				name = "Skip Mission 2",
				description = "Skip your current mission 2",
				pricesRaw = new int[1] { 1500 },
				iconName = "icon_upgrades_skipMission2",
				levelPriceMultiplyer = 50
			}
		},
		{
			PowerupType.skipmission3,
			new Upgrade
			{
				name = "Skip Mission 3",
				description = "Skip your current mission 3",
				pricesRaw = new int[1] { 1500 },
				iconName = "icon_upgrades_skipMission3",
				levelPriceMultiplyer = 50
			}
		},
		{
			PowerupType.letters,
			new Upgrade
			{
				spawnProbability = 15f
			}
		}
	};
}
