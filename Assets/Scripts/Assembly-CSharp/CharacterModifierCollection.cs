using System.Collections.Generic;
using UnityEngine;

public class CharacterModifierCollection
{
	private CoinMagnet coinMagnet;

	private SuperSneakers superSneakers;

	private Hoverboard hoverboard;

	private DoubleScoreMultiplier doubleScoreMultiplier;

	private List<CharacterModifier> modifiers = new List<CharacterModifier>();

	private List<CharacterModifier> deadModifiers = new List<CharacterModifier>();

	public CoinMagnet CoinMagnet
	{
		get
		{
			return coinMagnet;
		}
	}

	public SuperSneakers SuperSneakes
	{
		get
		{
			return superSneakers;
		}
	}

	public Hoverboard Hoverboard
	{
		get
		{
			return hoverboard;
		}
	}

	public DoubleScoreMultiplier DoubleScoreMultiplier
	{
		get
		{
			return doubleScoreMultiplier;
		}
	}

	public CharacterModifierCollection()
	{
		coinMagnet = Object.FindObjectOfType(typeof(CoinMagnet)) as CoinMagnet;
		superSneakers = Object.FindObjectOfType(typeof(SuperSneakers)) as SuperSneakers;
		hoverboard = Hoverboard.Instance;
		doubleScoreMultiplier = Object.FindObjectOfType(typeof(DoubleScoreMultiplier)) as DoubleScoreMultiplier;
	}

	public void Add(CharacterModifier modifier)
	{
		if (!modifiers.Contains(modifier))
		{
			modifiers.Add(modifier);
			modifier.Current = modifier.Begin();
		}
		else
		{
			modifier.Reset();
			modifier.Current = modifier.Begin();
		}
	}

	public void Update()
	{
		if (modifiers.Count <= 0)
		{
			return;
		}
		deadModifiers.Clear();
		foreach (CharacterModifier modifier in modifiers)
		{
			if (!modifier.Paused && !modifier.Current.MoveNext())
			{
				deadModifiers.Add(modifier);
			}
		}
		if (deadModifiers.Count <= 0)
		{
			return;
		}
		foreach (CharacterModifier deadModifier in deadModifiers)
		{
			modifiers.Remove(deadModifier);
		}
	}

	public bool IsActive(CharacterModifier modifier)
	{
		return modifiers.Contains(modifier);
	}

	public void StopWithNoEnding()
	{
		foreach (CharacterModifier modifier in modifiers)
		{
			modifier.Stop = CharacterModifier.StopSignal.STOP_NO_ENDING;
		}
	}

	public void Stop()
	{
		foreach (CharacterModifier modifier in modifiers)
		{
			modifier.Stop = CharacterModifier.StopSignal.STOP;
		}
	}

	public void Pause()
	{
		foreach (CharacterModifier modifier in modifiers)
		{
			modifier.Pause();
		}
	}

	public void PauseInJetpackMode()
	{
		foreach (CharacterModifier modifier in modifiers)
		{
			if (modifier.ShouldPauseInJetpack)
			{
				modifier.Pause();
			}
		}
	}

	public void Resume()
	{
		foreach (CharacterModifier modifier in modifiers)
		{
			modifier.Resume();
		}
	}

	public void Reset()
	{
		foreach (CharacterModifier modifier in modifiers)
		{
			modifier.Reset();
		}
		modifiers.Clear();
	}
}
