using System.Collections;
using UnityEngine;

public abstract class CharacterModifier : MonoBehaviour
{
	public enum StopSignal
	{
		DONT_STOP = 0,
		STOP = 1,
		STOP_NO_ENDING = 2
	}

	public bool Paused;

	protected StopSignal stop;

	private IEnumerator current;

	public StopSignal Stop
	{
		get
		{
			return stop;
		}
		set
		{
			stop = value;
		}
	}

	public virtual bool ShouldPauseInJetpack
	{
		get
		{
			return false;
		}
	}

	public IEnumerator Current
	{
		get
		{
			return current;
		}
		set
		{
			current = value;
		}
	}

	public virtual IEnumerator Begin()
	{
		yield break;
	}

	public virtual void Pause()
	{
		Paused = true;
	}

	public virtual void Resume()
	{
		Paused = false;
	}

	public virtual void Reset()
	{
	}
}
