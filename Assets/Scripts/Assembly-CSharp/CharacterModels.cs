using System.Collections.Generic;

public class CharacterModels
{
	public enum ModelType
	{
		slick = 0,
		tricky = 1,
		fresh = 2,
		spike = 3,
		yutani = 4,
		frank = 5,
		frizzy = 6,
		king = 7,
		lucy = 8,
		ninja = 9
	}

	public enum UnlockType
	{
		free = 0,
		tokens = 1,
		coins = 2
	}

	public class Model
	{
		public string ModelName = "not_set";

		public string TokenName = string.Empty;

		public int Price = -1;

		public UnlockType UnlockType;
	}

	public static readonly Dictionary<ModelType, Model> modelData = new Dictionary<ModelType, Model>
	{
		{
			ModelType.slick,
			new Model
			{
				ModelName = "Jake",
				TokenName = string.Empty
			}
		},
		{
			ModelType.tricky,
			new Model
			{
				ModelName = "Tricky",
				UnlockType = UnlockType.tokens,
				TokenName = "Tricky's Hat",
				Price = 3
			}
		},
		{
			ModelType.fresh,
			new Model
			{
				ModelName = "Fresh",
				TokenName = "Fresh's Stereo",
				UnlockType = UnlockType.tokens,
				Price = 50
			}
		},
		{
			ModelType.spike,
			new Model
			{
				ModelName = "Spike",
				TokenName = "Spike's Guitar",
				UnlockType = UnlockType.tokens,
				Price = 200
			}
		},
		{
			ModelType.yutani,
			new Model
			{
				ModelName = "Yutani",
				TokenName = "Yutani's UFO",
				UnlockType = UnlockType.tokens,
				Price = 700
			}
		},
		{
			ModelType.frank,
			new Model
			{
				ModelName = "Frank",
				UnlockType = UnlockType.coins,
				Price = 7000
			}
		},
		{
			ModelType.frizzy,
			new Model
			{
				ModelName = "Frizzy",
				UnlockType = UnlockType.coins,
				Price = 40000
			}
		},
		{
			ModelType.king,
			new Model
			{
				ModelName = "King",
				TokenName = string.Empty,
				UnlockType = UnlockType.coins,
				Price = 150000
			}
		},
		{
			ModelType.lucy,
			new Model
			{
				ModelName = "Lucy",
				TokenName = string.Empty,
				UnlockType = UnlockType.coins,
				Price = 20000
			}
		},
		{
			ModelType.ninja,
			new Model
			{
				ModelName = "Ninja",
				TokenName = string.Empty,
				UnlockType = UnlockType.coins,
				Price = 100000
			}
		}
	};
}
