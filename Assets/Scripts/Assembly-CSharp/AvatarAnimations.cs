using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarAnimations : MonoBehaviour
{
	public Animation Target;

	public bool PlayIdleAnimations;

	public int MinIdleTimes;

	public int MaxIdleTimes;

	public AnimationClip Breath;

	public List<AnimationClip> Idles;

	public bool Paused;

	private IEnumerator animationRoutine;

	private float nextAnimationTime;

	private void Start()
	{
		Target = FindAnimationInParent(base.gameObject);
		if (Target == null)
		{
			Debug.Log(" No animation component for avatar animations");
		}
		else if (PlayIdleAnimations)
		{
			StartIdleAnimations();
		}
	}

	private Animation FindAnimationInParent(GameObject current)
	{
		Animation component = current.GetComponent<Animation>();
		if (component != null)
		{
			return component;
		}
		if (current.transform.parent != null)
		{
			return FindAnimationInParent(current.transform.parent.gameObject);
		}
		return null;
	}

	private void Update()
	{
		if (PlayIdleAnimations && animationRoutine != null && !Paused)
		{
			animationRoutine.MoveNext();
		}
	}

	public void StartIdleAnimations()
	{
		PlayIdleAnimations = true;
		Paused = false;
		Target.AddClip(Breath, Breath.name);
		foreach (AnimationClip idle in Idles)
		{
			Target.AddClip(idle, idle.name);
		}
		animationRoutine = Play();
		animationRoutine.MoveNext();
	}

	public void StopIdleAnimations()
	{
		PlayIdleAnimations = false;
		Target.AddClip(Breath, Breath.name);
		foreach (AnimationClip idle in Idles)
		{
			foreach (AnimationState item in Target)
			{
				if (item.clip == idle)
				{
					Target.RemoveClip(idle);
				}
			}
		}
		animationRoutine = null;
	}

	public void PauseIdleAnimations()
	{
		Paused = true;
		foreach (AnimationState item in Target.GetComponent<Animation>())
		{
			item.speed = 0f;
		}
	}

	public void ResumeIdleAnimations()
	{
		Paused = false;
		foreach (AnimationState item in Target.GetComponent<Animation>())
		{
			item.speed = 1f;
		}
	}

	private IEnumerator Play()
	{
		int index2 = 0;
		List<AnimationClip> tmpList2 = new List<AnimationClip>();
		while (PlayIdleAnimations)
		{
			int count = Random.Range(MinIdleTimes, MaxIdleTimes);
			for (int i = 0; i < count; i++)
			{
				Target.Play(Breath.name);
				nextAnimationTime = Breath.length;
				while (nextAnimationTime > 0f)
				{
					nextAnimationTime -= Time.deltaTime;
					yield return 0;
				}
			}
			tmpList2 = Idles.FindAll((AnimationClip a) => a != Idles[index2]);
			index2 = Random.Range(0, tmpList2.Count);
			Target.Play(tmpList2[index2].name);
			nextAnimationTime = tmpList2[index2].length;
			while (nextAnimationTime > 0f)
			{
				nextAnimationTime -= Time.deltaTime;
				yield return 0;
			}
		}
		animationRoutine = null;
	}
}
