using System.Collections;
using UnityEngine;

public abstract class CharacterState : MonoBehaviour
{
	public virtual bool PauseActiveModifiers
	{
		get
		{
			return false;
		}
	}

	public virtual void HandleSwipe(SwipeDir swipeDir)
	{
	}

	public virtual IEnumerator Begin()
	{
		yield break;
	}

	public virtual void HandleCriticalHit()
	{
	}

	public virtual void HandleDoubleTap()
	{
	}
}
