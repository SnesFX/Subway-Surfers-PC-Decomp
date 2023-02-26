using System;
using System.Collections;
using UnityEngine;

public static class AudioSourceExtensions
{
	public static IEnumerator fadeOut(this AudioSource audioSource, float duration, Action onComplete)
	{
		float startingVolume = audioSource.volume;
		while (audioSource.volume > 0f)
		{
			audioSource.volume -= Time.deltaTime * startingVolume / duration;
			yield return null;
		}
		if (onComplete != null)
		{
			onComplete();
		}
	}
}
