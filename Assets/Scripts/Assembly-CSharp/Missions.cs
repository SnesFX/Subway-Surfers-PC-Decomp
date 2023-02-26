using System.Collections.Generic;
using UnityEngine;

public class Missions
{
	public enum MissionTarget
	{
		none = 0,
		EarnCoin = 1,
		SpendCoin = 2,
		Score = 3,
		JumpTrain = 4,
		Jump = 5,
		Roll = 6,
		RollLeft = 7,
		RollCenter = 8,
		RollRight = 9,
		RollUnderBarriers = 10,
		JumpBarriers = 11,
		DieToTrain = 12,
		Jetpack = 13,
		SuperSneakers = 14,
		Letters = 15,
		Magnets = 16,
		MysteryBoxes = 17,
		BeatFriends = 18,
		DailyQuests = 19,
		Tokens = 20,
		DodgeBarriers = 21,
		CrashBarriers = 22,
		CrashTrains = 23,
		Powerups = 24,
		Headstart = 25,
		CoinsWithMagnet = 26,
		BuyMysterybox = 27,
		CollectCoinPouch = 28,
		TimeDeath = 29,
		BumpTrain = 30,
		BumpBush = 31,
		BumpLightSignal = 32,
		BumpBarrier = 33,
		HoverBoard = 34,
		HoverBoardExpire = 35,
		NoCoinsBeforeScore = 36
	}

	public enum MissionType
	{
		none = 0,
		EarnCoin = 1,
		EarnCoinSingleRun = 2,
		SpendCoin = 3,
		ScoreCumulative = 4,
		JumpTrain = 5,
		JumpTrainSingleRun = 6,
		Jump = 7,
		JumpSingleRun = 8,
		Roll = 9,
		RollSingleRun = 10,
		RollLeft = 11,
		RollCenter = 12,
		RollRight = 13,
		RollUnderBarriers = 14,
		JumpBarriers = 15,
		DieToTrain = 16,
		Jetpack = 17,
		JetpackSingleRun = 18,
		SuperSneakers = 19,
		SuperSneakersSingleRun = 20,
		Letters = 21,
		Magnets = 22,
		MagnetsSingleRun = 23,
		MysteryBoxes = 24,
		BeatFriends = 25,
		BeatFriendsSingleRun = 26,
		DailyQuests = 27,
		Tokens = 28,
		DodgeBarriers = 29,
		CrashBarriers = 30,
		CrashBarriersSingleRun = 31,
		CrashTrains = 32,
		BumpTrainsSingleRun = 33,
		Powerups = 34,
		Headstart = 35,
		CoinsWithMagnet = 36,
		BuyMysterybox = 37,
		CollectCoinPouch = 38,
		TimeDeath = 39,
		BumpTrain = 40,
		BumpBush = 41,
		BumpLightSignal = 42,
		BumpBarrier = 43,
		ScoreSingleRun = 44,
		HoverBoard = 45,
		HoverBoardExpire = 46,
		NoCoinsBeforeScore = 47
	}

	public delegate void MissionSetCompleteHandler();

	public delegate void MissionCompleteHandler(string msg);

	private int _currentMissionSetLoaded = -1;

	private MissionTemplate[] _currentMissionTemplates;

	private int[] _currentRunProgress;

	public MissionSetCompleteHandler onMissionSetComplete;

	public MissionCompleteHandler onMissionComplete;

	private static readonly Mission[][] missions = new Mission[30][]
	{
		new Mission[3]
		{
			new Mission(MissionType.EarnCoin, 500),
			new Mission(MissionType.ScoreSingleRun, 1000),
			new Mission(MissionType.Powerups, 2)
		},
		new Mission[3]
		{
			new Mission(MissionType.EarnCoinSingleRun, 200),
			new Mission(MissionType.Jump, 20),
			new Mission(MissionType.SuperSneakers, 2)
		},
		new Mission[3]
		{
			new Mission(MissionType.Tokens, 2),
			new Mission(MissionType.Roll, 30),
			new Mission(MissionType.SpendCoin, 2000)
		},
		new Mission[3]
		{
			new Mission(MissionType.DailyQuests, 1),
			new Mission(MissionType.DodgeBarriers, 20),
			new Mission(MissionType.ScoreSingleRun, 6000)
		},
		new Mission[3]
		{
			new Mission(MissionType.EarnCoin, 2500),
			new Mission(MissionType.JumpSingleRun, 30),
			new Mission(MissionType.BuyMysterybox, 1)
		},
		new Mission[3]
		{
			new Mission(MissionType.HoverBoard, 1),
			new Mission(MissionType.Magnets, 5),
			new Mission(MissionType.BumpBarrier, 4)
		},
		new Mission[3]
		{
			new Mission(MissionType.Jetpack, 2),
			new Mission(MissionType.BeatFriends, 1),
			new Mission(MissionType.Headstart, 1)
		},
		new Mission[3]
		{
			new Mission(MissionType.BumpTrainsSingleRun, 8),
			new Mission(MissionType.CoinsWithMagnet, 40),
			new Mission(MissionType.TimeDeath, 10)
		},
		new Mission[3]
		{
			new Mission(MissionType.HoverBoardExpire, 1),
			new Mission(MissionType.MysteryBoxes, 2),
			new Mission(MissionType.RollSingleRun, 30)
		},
		new Mission[3]
		{
			new Mission(MissionType.ScoreSingleRun, 20000),
			new Mission(MissionType.Powerups, 12),
			new Mission(MissionType.JumpTrain, 2)
		},
		new Mission[3]
		{
			new Mission(MissionType.NoCoinsBeforeScore, 4000),
			new Mission(MissionType.RollCenter, 50),
			new Mission(MissionType.EarnCoin, 5000)
		},
		new Mission[3]
		{
			new Mission(MissionType.DailyQuests, 2),
			new Mission(MissionType.DodgeBarriers, 40),
			new Mission(MissionType.SuperSneakers, 5)
		},
		new Mission[3]
		{
			new Mission(MissionType.BumpBush, 4),
			new Mission(MissionType.CoinsWithMagnet, 160),
			new Mission(MissionType.MagnetsSingleRun, 2)
		},
		new Mission[3]
		{
			new Mission(MissionType.MysteryBoxes, 4),
			new Mission(MissionType.RollSingleRun, 40),
			new Mission(MissionType.EarnCoinSingleRun, 400)
		},
		new Mission[3]
		{
			new Mission(MissionType.ScoreCumulative, 100000),
			new Mission(MissionType.Jetpack, 5),
			new Mission(MissionType.BumpLightSignal, 12)
		},
		new Mission[3]
		{
			new Mission(MissionType.DailyQuests, 3),
			new Mission(MissionType.SuperSneakersSingleRun, 3),
			new Mission(MissionType.JumpTrain, 4)
		},
		new Mission[3]
		{
			new Mission(MissionType.ScoreSingleRun, 50000),
			new Mission(MissionType.SpendCoin, 4000),
			new Mission(MissionType.Magnets, 15)
		},
		new Mission[3]
		{
			new Mission(MissionType.JetpackSingleRun, 2),
			new Mission(MissionType.BumpTrainsSingleRun, 12),
			new Mission(MissionType.CrashTrains, 20)
		},
		new Mission[3]
		{
			new Mission(MissionType.HoverBoard, 5),
			new Mission(MissionType.MagnetsSingleRun, 3),
			new Mission(MissionType.Tokens, 5)
		},
		new Mission[3]
		{
			new Mission(MissionType.ScoreCumulative, 250000),
			new Mission(MissionType.JumpSingleRun, 40),
			new Mission(MissionType.Powerups, 25)
		},
		new Mission[3]
		{
			new Mission(MissionType.BumpBarrier, 15),
			new Mission(MissionType.BuyMysterybox, 3),
			new Mission(MissionType.CoinsWithMagnet, 240)
		},
		new Mission[3]
		{
			new Mission(MissionType.DodgeBarriers, 80),
			new Mission(MissionType.SpendCoin, 8000),
			new Mission(MissionType.SuperSneakersSingleRun, 4)
		},
		new Mission[3]
		{
			new Mission(MissionType.Headstart, 8),
			new Mission(MissionType.JumpTrain, 10),
			new Mission(MissionType.Jetpack, 15)
		},
		new Mission[3]
		{
			new Mission(MissionType.MysteryBoxes, 8),
			new Mission(MissionType.RollCenter, 200),
			new Mission(MissionType.DailyQuests, 4)
		},
		new Mission[3]
		{
			new Mission(MissionType.EarnCoin, 15000),
			new Mission(MissionType.ScoreSingleRun, 120000),
			new Mission(MissionType.JumpTrainSingleRun, 3)
		},
		new Mission[3]
		{
			new Mission(MissionType.RollSingleRun, 50),
			new Mission(MissionType.ScoreCumulative, 500000),
			new Mission(MissionType.NoCoinsBeforeScore, 12000)
		},
		new Mission[3]
		{
			new Mission(MissionType.Tokens, 10),
			new Mission(MissionType.BuyMysterybox, 6),
			new Mission(MissionType.BumpLightSignal, 20)
		},
		new Mission[3]
		{
			new Mission(MissionType.JumpSingleRun, 50),
			new Mission(MissionType.HoverBoard, 12),
			new Mission(MissionType.HoverBoardExpire, 4)
		},
		new Mission[3]
		{
			new Mission(MissionType.EarnCoinSingleRun, 750),
			new Mission(MissionType.BumpBarrier, 25),
			new Mission(MissionType.ScoreSingleRun, 250000)
		},
		new Mission[3]
		{
			new Mission(MissionType.none, 1),
			new Mission(MissionType.none, 1),
			new Mission(MissionType.none, 1)
		}
	};

	private static readonly Dictionary<MissionType, MissionTemplate> missionTemplates = new Dictionary<MissionType, MissionTemplate>
	{
		{
			MissionType.EarnCoin,
			new MissionTemplate
			{
				descriptionSingle = "Collect {0} coin. {1} left",
				description = "Collect {0} coins. {1} left",
				ultraShortDescriptionSingle = "Collect {0} coin",
				ultraShortDescription = "Collect {0} coins",
				missionTarget = MissionTarget.EarnCoin
			}
		},
		{
			MissionType.EarnCoinSingleRun,
			new MissionTemplate
			{
				descriptionSingle = "Collect {0} coin in a one run. {1} left",
				description = "Collect {0} coins in a one run. {1} left",
				ultraShortDescriptionSingle = "Collect {0} coin",
				ultraShortDescription = "Collect {0} coins",
				missionTarget = MissionTarget.EarnCoin,
				singleRun = true
			}
		},
		{
			MissionType.SpendCoin,
			new MissionTemplate
			{
				descriptionSingle = "Spend {0} coin. {1} left",
				description = "Spend {0} coins. {1} left",
				ultraShortDescriptionSingle = "Spend {0} coin",
				ultraShortDescription = "Spend {0} coins",
				missionTarget = MissionTarget.SpendCoin
			}
		},
		{
			MissionType.ScoreCumulative,
			new MissionTemplate
			{
				descriptionSingle = "Collect {0} point. {1} left",
				description = "Collect {0} points. {1} left",
				ultraShortDescriptionSingle = "Collect {0} point",
				ultraShortDescription = "Collect {0} points",
				missionTarget = MissionTarget.Score
			}
		},
		{
			MissionType.JumpTrain,
			new MissionTemplate
			{
				descriptionSingle = "Jump over {0} train. {1} left",
				description = "Jump over {0} trains. {1} left",
				ultraShortDescriptionSingle = "Jump {0} train",
				ultraShortDescription = "Jump {0} trains",
				missionTarget = MissionTarget.JumpTrain
			}
		},
		{
			MissionType.JumpTrainSingleRun,
			new MissionTemplate
			{
				descriptionSingle = "Jump over {0} train in one run. {1} left",
				description = "Jump over {0} trains in one run. {1} left",
				ultraShortDescriptionSingle = "Jump {0} train",
				ultraShortDescription = "Jump {0} trains",
				missionTarget = MissionTarget.JumpTrain,
				singleRun = true
			}
		},
		{
			MissionType.Jump,
			new MissionTemplate
			{
				descriptionSingle = "Jump {0} time. {1} left",
				description = "Jump {0} times. {1} left",
				ultraShortDescriptionSingle = "Jump {0} time",
				ultraShortDescription = "Jump {0} times",
				missionTarget = MissionTarget.Jump
			}
		},
		{
			MissionType.JumpSingleRun,
			new MissionTemplate
			{
				descriptionSingle = "Jump {0} time in one run. {1} left",
				description = "Jump {0} times in one run. {1} left",
				ultraShortDescriptionSingle = "Jump {0} time",
				ultraShortDescription = "Jump {0} times",
				missionTarget = MissionTarget.Jump,
				singleRun = true
			}
		},
		{
			MissionType.Roll,
			new MissionTemplate
			{
				descriptionSingle = "Roll {0} time. {1} left",
				description = "Roll {0} times in total. {1} left",
				ultraShortDescriptionSingle = "Roll {0} time",
				ultraShortDescription = "Roll {0} times",
				missionTarget = MissionTarget.Roll
			}
		},
		{
			MissionType.RollSingleRun,
			new MissionTemplate
			{
				descriptionSingle = "Roll {0} time in a single run. {1} left",
				description = "Roll {0} times in a single run. {1} left",
				ultraShortDescriptionSingle = "Roll {0} time",
				ultraShortDescription = "Roll {0} times",
				missionTarget = MissionTarget.Roll,
				singleRun = true
			}
		},
		{
			MissionType.RollLeft,
			new MissionTemplate
			{
				descriptionSingle = "Roll {0} time in left lane. {1} left",
				description = "Roll {0} times in left lane. {1} left",
				ultraShortDescriptionSingle = "Roll {0} time",
				ultraShortDescription = "Roll {0} times",
				missionTarget = MissionTarget.RollLeft
			}
		},
		{
			MissionType.RollCenter,
			new MissionTemplate
			{
				descriptionSingle = "Roll {0} time in center lane. {1} left",
				description = "Roll {0} times in center lane. {1} left",
				ultraShortDescriptionSingle = "Roll {0} time",
				ultraShortDescription = "Roll {0} times",
				missionTarget = MissionTarget.RollCenter
			}
		},
		{
			MissionType.RollRight,
			new MissionTemplate
			{
				descriptionSingle = "Roll {0} time in right lane. {1} left",
				description = "Roll {0} times in right lane. {1} left",
				ultraShortDescriptionSingle = "Roll {0} time",
				ultraShortDescription = "Roll {0} times",
				missionTarget = MissionTarget.RollRight
			}
		},
		{
			MissionType.RollUnderBarriers,
			new MissionTemplate
			{
				descriptionSingle = "Roll under {0} barrier. {1} left",
				description = "Roll under {0} barriers. {1} left",
				ultraShortDescriptionSingle = "Roll under {0} barrier",
				ultraShortDescription = "Roll under {0} barriers",
				missionTarget = MissionTarget.RollUnderBarriers
			}
		},
		{
			MissionType.JumpBarriers,
			new MissionTemplate
			{
				descriptionSingle = "Jump over {0} barrier. {1} left",
				description = "Jump over {0} barriers. {1} left",
				ultraShortDescriptionSingle = "Jump {0} barrier",
				ultraShortDescription = "Jump {0} barriers",
				missionTarget = MissionTarget.JumpBarriers
			}
		},
		{
			MissionType.DieToTrain,
			new MissionTemplate
			{
				descriptionSingle = "Get run over by {0} train. {1} left",
				description = "Get run over by {0} trains. {1} left",
				ultraShortDescriptionSingle = "Get run over {0} time",
				ultraShortDescription = "Get run over {0} times",
				missionTarget = MissionTarget.DieToTrain
			}
		},
		{
			MissionType.Jetpack,
			new MissionTemplate
			{
				descriptionSingle = "Pick up {0} Jetpack. {1} left",
				description = "Pick up {0} Jetpacks. {1} left",
				ultraShortDescriptionSingle = "Pick up {0} Jetpack",
				ultraShortDescription = "Pick up {0} Jetpacks",
				missionTarget = MissionTarget.Jetpack
			}
		},
		{
			MissionType.JetpackSingleRun,
			new MissionTemplate
			{
				descriptionSingle = "Pick up {0} Jetpack in one run. {1} left",
				description = "Pick up {0} Jetpacks in one run. {1} left",
				ultraShortDescriptionSingle = "Pick up {0} Jetpack",
				ultraShortDescription = "Pick up {0} Jetpacks",
				missionTarget = MissionTarget.Jetpack,
				singleRun = true
			}
		},
		{
			MissionType.SuperSneakers,
			new MissionTemplate
			{
				descriptionSingle = "Pick up {0} Super Sneaker. {1} left",
				description = "Pick up {0} Super Sneakers. {1} left",
				ultraShortDescriptionSingle = "Pick up {0} Sneaker",
				ultraShortDescription = "Pick up {0} Sneakers",
				missionTarget = MissionTarget.SuperSneakers
			}
		},
		{
			MissionType.SuperSneakersSingleRun,
			new MissionTemplate
			{
				descriptionSingle = "Pick up {0} Super Sneaker in one run. {1} left",
				description = "Pick up {0} Super Sneakers in one run. {1} left",
				ultraShortDescriptionSingle = "Pick up {0} Sneaker",
				ultraShortDescription = "Pick up {0} Sneakers",
				missionTarget = MissionTarget.SuperSneakers,
				singleRun = true
			}
		},
		{
			MissionType.Letters,
			new MissionTemplate
			{
				descriptionSingle = "Pick up {0} Daily Letter. {1} left",
				description = "Pick up {0} Daily Letters. {1} left",
				ultraShortDescriptionSingle = "Pick up {0} Letter",
				ultraShortDescription = "Pick up {0} Letters",
				missionTarget = MissionTarget.Letters
			}
		},
		{
			MissionType.Magnets,
			new MissionTemplate
			{
				descriptionSingle = "Pick up {0} Coin Magnet. {1} left",
				description = "Pick up {0} Coin Magnets. {1} left",
				ultraShortDescriptionSingle = "Pick up {0} Magnet",
				ultraShortDescription = "Pick up {0} Magnets",
				missionTarget = MissionTarget.Magnets
			}
		},
		{
			MissionType.MagnetsSingleRun,
			new MissionTemplate
			{
				descriptionSingle = "Pick up {0} Magnet in one run. {1} left",
				description = "Pick up {0} Magnets in one run. {1} left",
				ultraShortDescriptionSingle = "Pick up {0} Magnet",
				ultraShortDescription = "Pick up {0} Magnets",
				missionTarget = MissionTarget.Magnets,
				singleRun = true
			}
		},
		{
			MissionType.BeatFriends,
			new MissionTemplate
			{
				descriptionSingle = "Beat {0} friend. {1} left",
				description = "Beat {0} friends. {1} left",
				ultraShortDescriptionSingle = "Beat {0} friend",
				ultraShortDescription = "Beat {0} friends",
				missionTarget = MissionTarget.BeatFriends
			}
		},
		{
			MissionType.BeatFriendsSingleRun,
			new MissionTemplate
			{
				descriptionSingle = "Beat {0} friend in one run. {1} left",
				description = "Beat {0} friends in one run. {1} left",
				ultraShortDescriptionSingle = "Beat {0} friend",
				ultraShortDescription = "Beat {0} friends",
				missionTarget = MissionTarget.BeatFriends,
				singleRun = true
			}
		},
		{
			MissionType.DailyQuests,
			new MissionTemplate
			{
				descriptionSingle = "Complete {0} Daily Challenge. {1} left",
				description = "Complete {0} Daily Challenges. {1} left",
				ultraShortDescriptionSingle = "{0} Daily Challenge",
				ultraShortDescription = "{0} Daily Challenges",
				missionTarget = MissionTarget.DailyQuests
			}
		},
		{
			MissionType.Tokens,
			new MissionTemplate
			{
				descriptionSingle = "Get {0} character token. {1} left",
				description = "Get {0} character tokens. {1} left",
				ultraShortDescriptionSingle = "Get {0} token",
				ultraShortDescription = "Get {0} tokens",
				missionTarget = MissionTarget.Tokens
			}
		},
		{
			MissionType.DodgeBarriers,
			new MissionTemplate
			{
				descriptionSingle = "Dodge {0} barrier. {1} left",
				description = "Dodge {0} barriers. {1} left",
				ultraShortDescriptionSingle = "Dodge {0} barrier",
				ultraShortDescription = "Dodge {0} barriers",
				missionTarget = MissionTarget.DodgeBarriers
			}
		},
		{
			MissionType.CrashBarriers,
			new MissionTemplate
			{
				descriptionSingle = "Crash into {0} barrier. {1} left",
				description = "Crash into {0} barriers. {1} left",
				ultraShortDescriptionSingle = "Crash into {0} barrier",
				ultraShortDescription = "Crash into {0} barriers",
				missionTarget = MissionTarget.CrashBarriers
			}
		},
		{
			MissionType.CrashBarriersSingleRun,
			new MissionTemplate
			{
				descriptionSingle = "Crash into {0} barrier in one run. {1} left",
				description = "Crash into {0} barriers in one run. {1} left",
				ultraShortDescriptionSingle = "Crash into {0} barrier",
				ultraShortDescription = "Crash into {0} barriers",
				missionTarget = MissionTarget.CrashBarriers,
				singleRun = true
			}
		},
		{
			MissionType.CrashTrains,
			new MissionTemplate
			{
				descriptionSingle = "Crash into {0} train. {1} left",
				description = "Crash into {0} trains. {1} left",
				ultraShortDescriptionSingle = "Crash into {0} train",
				ultraShortDescription = "Crash into {0} trains",
				missionTarget = MissionTarget.CrashTrains
			}
		},
		{
			MissionType.BumpTrainsSingleRun,
			new MissionTemplate
			{
				descriptionSingle = "Bump into {0} train in one run. {1} left",
				description = "Bump into {0} trains in one run. {1} left",
				ultraShortDescriptionSingle = "Bump into {0} train",
				ultraShortDescription = "Bump into {0} trains",
				missionTarget = MissionTarget.BumpTrain,
				singleRun = true
			}
		},
		{
			MissionType.Powerups,
			new MissionTemplate
			{
				descriptionSingle = "Pickup {0} powerup. {1} left",
				description = "Pickup {0} powerups. {1} left",
				ultraShortDescriptionSingle = "Pickup {0} powerup",
				ultraShortDescription = "Pickup {0} powerups",
				missionTarget = MissionTarget.Powerups
			}
		},
		{
			MissionType.Headstart,
			new MissionTemplate
			{
				descriptionSingle = "Use {0} Headstart. {1} left",
				description = "Use {0} Headstarts. {1} left",
				ultraShortDescriptionSingle = "Use {0} Headstart",
				ultraShortDescription = "Use {0} Headstarts",
				missionTarget = MissionTarget.Headstart
			}
		},
		{
			MissionType.CoinsWithMagnet,
			new MissionTemplate
			{
				descriptionSingle = "Pickup {0} coin with a Magnet. {1} left",
				description = "Pickup {0} coins with a Magnet. {1} left",
				ultraShortDescriptionSingle = "{0} coin with Magnet",
				ultraShortDescription = "{0} coins with Magnet",
				missionTarget = MissionTarget.CoinsWithMagnet
			}
		},
		{
			MissionType.BuyMysterybox,
			new MissionTemplate
			{
				descriptionSingle = "Buy {0} Mystery box. {1} left",
				description = "Buy {0} Mystery boxes. {1} left",
				ultraShortDescriptionSingle = "Buy {0} Mystery box",
				ultraShortDescription = "Buy {0} Mystery boxes",
				missionTarget = MissionTarget.BuyMysterybox
			}
		},
		{
			MissionType.CollectCoinPouch,
			new MissionTemplate
			{
				descriptionSingle = "Pickup {0} coin pouch. {1} left",
				description = "Pickup {0} coin pouches in total. {1} left",
				ultraShortDescriptionSingle = "{0} coin pouche",
				ultraShortDescription = "{0} coin pouches",
				missionTarget = MissionTarget.CollectCoinPouch
			}
		},
		{
			MissionType.TimeDeath,
			new MissionTemplate
			{
				descriptionSingle = "Get caught in first {0} second of run. Ran {1} sec",
				description = "Get caught in first {0} seconds of run. Ran {1} sec",
				ultraShortDescriptionSingle = "Caught in {0} sec",
				ultraShortDescription = "Caught in {0} sec",
				missionTarget = MissionTarget.TimeDeath,
				singleRun = true,
				completeIfLess = true
			}
		},
		{
			MissionType.MysteryBoxes,
			new MissionTemplate
			{
				descriptionSingle = "Pickup {0} Mystery boxe. {1} left",
				description = "Pickup {0} Mystery boxes. {1} left",
				ultraShortDescriptionSingle = "{0} Mystery box",
				ultraShortDescription = "{0} Mystery boxes",
				missionTarget = MissionTarget.MysteryBoxes
			}
		},
		{
			MissionType.BumpBarrier,
			new MissionTemplate
			{
				descriptionSingle = "Stumble into {0} Barrier. {1} left",
				description = "Stumble into {0} Barriers. {1} left",
				ultraShortDescriptionSingle = "Stumble {0} Barrier",
				ultraShortDescription = "Stumble {0} Barriers",
				missionTarget = MissionTarget.BumpBarrier
			}
		},
		{
			MissionType.BumpLightSignal,
			new MissionTemplate
			{
				descriptionSingle = "Bump into {0} Light Signal. {1} left",
				description = "Bump into {0} Light Signals. {1} left",
				ultraShortDescriptionSingle = "{0} Light Signal",
				ultraShortDescription = "{0} Light Signal",
				missionTarget = MissionTarget.BumpLightSignal
			}
		},
		{
			MissionType.BumpBush,
			new MissionTemplate
			{
				descriptionSingle = "Bump {0} bush. {1} left",
				description = "Bump {0} bushes. {1} left",
				ultraShortDescriptionSingle = "Bump {0} Bush",
				ultraShortDescription = "Bump {0} Bushes",
				missionTarget = MissionTarget.BumpBush
			}
		},
		{
			MissionType.BumpTrain,
			new MissionTemplate
			{
				descriptionSingle = "Bump {0} train. {1} left",
				description = "Bump {0} trains. {1} left",
				ultraShortDescriptionSingle = "Bump {0} train",
				ultraShortDescription = "Bump {0} trains",
				missionTarget = MissionTarget.BumpTrain
			}
		},
		{
			MissionType.HoverBoard,
			new MissionTemplate
			{
				descriptionSingle = "Use {0} Hoverboard. {1} left",
				description = "Use {0} Hoverboards. {1} left",
				ultraShortDescriptionSingle = "Use {0} Hoverboard",
				ultraShortDescription = "Use {0} Hoverboards",
				missionTarget = MissionTarget.HoverBoard
			}
		},
		{
			MissionType.HoverBoardExpire,
			new MissionTemplate
			{
				descriptionSingle = "Use {0} Hoverboard without crashing. {1} left",
				description = "Use {0} Hoverboards without crashing. {1} left",
				ultraShortDescriptionSingle = "{0} Hoverboard no crash",
				ultraShortDescription = "{0} Hoverboards no crash",
				missionTarget = MissionTarget.HoverBoardExpire
			}
		},
		{
			MissionType.ScoreSingleRun,
			new MissionTemplate
			{
				descriptionSingle = "Score {0} point in single run. {1} left",
				description = "Score {0} points in single run. {1} left",
				ultraShortDescriptionSingle = "{0} point one run",
				ultraShortDescription = "{0} points one run",
				missionTarget = MissionTarget.Score,
				singleRun = true
			}
		},
		{
			MissionType.NoCoinsBeforeScore,
			new MissionTemplate
			{
				descriptionSingle = "Collect {0} point without collecting coins. {1} left",
				description = "Collect {0} points without collecting coins. {1} left",
				ultraShortDescriptionSingle = "{0} Point no coins",
				ultraShortDescription = "{0} Points no coins",
				missionTarget = MissionTarget.NoCoinsBeforeScore,
				singleRun = true
			}
		},
		{
			MissionType.none,
			new MissionTemplate
			{
				descriptionSingle = " ",
				description = " ",
				ultraShortDescriptionSingle = " ",
				ultraShortDescription = " "
			}
		}
	};

	private static Missions _instance;

	private MissionTemplate[] currentMissionTemplates
	{
		get
		{
			int num = PlayerInfo.Instance.currentMissionSet;
			if (_currentMissionSetLoaded != num)
			{
				Mission[] array = missions[num];
				_currentMissionTemplates = new MissionTemplate[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					_currentMissionTemplates[i] = missionTemplates[array[i].type];
				}
				_currentMissionSetLoaded = num;
			}
			return _currentMissionTemplates;
		}
	}

	public bool inRun
	{
		get
		{
			return _currentRunProgress != null;
		}
		set
		{
			if (value)
			{
				_currentRunProgress = new int[missions[PlayerInfo.Instance.currentMissionSet].Length];
			}
			else
			{
				if (_currentRunProgress == null)
				{
					return;
				}
				PlayerInfo instance = PlayerInfo.Instance;
				MissionTemplate[] array = currentMissionTemplates;
				Mission[] array2 = missions[instance.currentMissionSet];
				for (int i = 0; i < array2.Length; i++)
				{
					if (array[i].singleRun && array[i].completeIfLess && _currentRunProgress[i] < array2[i].goal)
					{
						instance.SetCurrentMissionProgress(i, array2[i].goal);
						MissionComplete(i);
					}
				}
				_currentRunProgress = null;
			}
		}
	}

	public int currentMissionSet
	{
		get
		{
			return PlayerInfo.Instance.currentMissionSet;
		}
		set
		{
			if (value != PlayerInfo.Instance.currentMissionSet)
			{
				PlayerInfo.Instance.InitCurrentMissionSet(value, missions[value].Length);
			}
		}
	}

	public int missionSetCount
	{
		get
		{
			return missions.Length - 1;
		}
	}

	public static Missions Instance
	{
		get
		{
			return _instance ?? (_instance = new Missions());
		}
	}

	private Missions()
	{
		if (PlayerInfo.Instance.currentMissionSet == -1)
		{
			PlayerInfo.Instance.InitCurrentMissionSet(0, missions[0].Length);
		}
	}

	public void SkipMission(int missionNumber)
	{
		PlayerInfo.Instance.SetCurrentMissionProgress(missionNumber, missions[PlayerInfo.Instance.currentMissionSet][missionNumber].goal);
		MissionComplete(missionNumber);
	}

	private void MissionComplete(int mission)
	{
		PlayerInfo instance = PlayerInfo.Instance;
		int num = instance.currentMissionSet;
		MissionTemplate[] array = currentMissionTemplates;
		Mission[] array2 = missions[num];
		MissionCompleteHandler missionCompleteHandler = onMissionComplete;
		if (missionCompleteHandler != null)
		{
			string text = ((array2[mission].goal != 1) ? array[mission].ultraShortDescription : array[mission].ultraShortDescriptionSingle);
			missionCompleteHandler(string.Format(array[mission].ultraShortDescription, array2[mission].goal));
		}
		bool flag = true;
		for (int i = 0; i < currentMissionTemplates.Length; i++)
		{
			int currentMissionProgress = instance.GetCurrentMissionProgress(i);
			if (currentMissionProgress < array2[i].goal)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			Flurry.LogEventWithAParameter("Mission Set completed", "Mission Set", instance.currentMissionSet.ToString());
			int num2 = instance.currentMissionSet + 1;
			int num3 = 0;
			num3 = ((num2 >= missionSetCount) ? 3 : missions[num2].Length);
			instance.InitCurrentMissionSet(num2, num3);
			MissionSetCompleteHandler missionSetCompleteHandler = onMissionSetComplete;
			if (missionSetCompleteHandler != null)
			{
				missionSetCompleteHandler();
			}
		}
		instance.Save();
	}

	public void PlayerDidThis(MissionTarget myTask, int magnitude = 1)
	{
		if (!HasMoreMissions())
		{
			return;
		}
		PlayerInfo instance = PlayerInfo.Instance;
		int num = instance.currentMissionSet;
		MissionTemplate[] array = currentMissionTemplates;
		if (array == null)
		{
			Debug.LogError("currentTemplates == null");
		}
		Mission[] array2 = missions[num];
		for (int i = 0; i < array.Length; i++)
		{
			if ((array[i].singleRun && !inRun) || array[i].missionTarget != myTask)
			{
				continue;
			}
			int num2 = instance.GetCurrentMissionProgress(i);
			if (array[i].singleRun && inRun && num2 < array2[i].goal && _currentRunProgress != null)
			{
				num2 = _currentRunProgress[i];
			}
			int num3 = num2 + magnitude;
			if (array[i].completeIfLess)
			{
				if (num3 > array2[i].goal)
				{
					if (array[i].singleRun && inRun)
					{
						if (_currentRunProgress != null)
						{
							_currentRunProgress[i] = num3;
						}
					}
					else
					{
						instance.SetCurrentMissionProgress(i, num3);
					}
					break;
				}
				if (array[i].singleRun && _currentRunProgress != null)
				{
					_currentRunProgress[i] = num3;
				}
				instance.SetCurrentMissionProgress(i, array2[i].goal);
				if (num2 > array2[i].goal)
				{
					MissionComplete(i);
				}
			}
			else if (num3 < array2[i].goal)
			{
				if (array[i].singleRun)
				{
					if (_currentRunProgress != null)
					{
						_currentRunProgress[i] = num3;
					}
				}
				else
				{
					instance.SetCurrentMissionProgress(i, num3);
				}
			}
			else
			{
				instance.SetCurrentMissionProgress(i, array2[i].goal);
				if (num2 < array2[i].goal)
				{
					MissionComplete(i);
				}
			}
			break;
		}
	}

	public MissionInfo[] GetMissionInfo()
	{
		PlayerInfo instance = PlayerInfo.Instance;
		int num = instance.currentMissionSet;
		if (num >= missionSetCount)
		{
			return new MissionInfo[0];
		}
		MissionTemplate[] array = currentMissionTemplates;
		Mission[] array2 = missions[num];
		MissionInfo[] array3 = new MissionInfo[array.Length];
		for (int i = 0; i < array3.Length; i++)
		{
			int num2 = instance.GetCurrentMissionProgress(i);
			bool flag = num2 >= array2[i].goal;
			if (!flag && array[i].singleRun && inRun)
			{
				num2 = _currentRunProgress[i];
			}
			else if (!(Game.Instance.GetDuration() > 0f))
			{
			}
			array3[i] = new MissionInfo(array2[i], array[i], num2, flag);
		}
		return array3;
	}

	public MissionInfo GetMissionInfo(int missonNumber)
	{
		PlayerInfo instance = PlayerInfo.Instance;
		int num = instance.currentMissionSet;
		MissionTemplate[] array = currentMissionTemplates;
		Mission[] array2 = missions[num];
		int num2 = instance.GetCurrentMissionProgress(missonNumber);
		bool flag = num2 >= array2[missonNumber].goal;
		if (!flag && array[missonNumber].singleRun && inRun)
		{
			num2 = _currentRunProgress[missonNumber];
		}
		return new MissionInfo(array2[missonNumber], array[missonNumber], num2, flag);
	}

	public bool HasMoreMissions()
	{
		return missionSetCount > currentMissionSet;
	}
}
