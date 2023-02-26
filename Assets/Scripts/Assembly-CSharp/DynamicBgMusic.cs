using System.Collections;
using UnityEngine;

public class DynamicBgMusic : MonoBehaviour
{
	public AudioClip[] audioClips;

	public AudioClip masterClip;

	private AudioSource masterSource;

	private AudioSource[] audioSources = new AudioSource[5];

	public float minFadeTime = 8f;

	public float maxFadeTime = 16f;

	public float minPlayingTime = 4f;

	public float maxPlayingTime = 10f;

	public float minPauseTime;

	public float maxPauseTime = 4f;

	public float minPlayingVolume = 0.3f;

	public float maxPlayingVolume = 0.6f;

	public float masterSourceVolume = 0.2f;

	private void Awake()
	{
		masterSource = base.gameObject.AddComponent<AudioSource>();
		masterSource.loop = true;
		masterSource.clip = masterClip;
		masterSource.volume = masterSourceVolume;
		masterSource.Play();
		for (int i = 0; i < audioSources.Length; i++)
		{
			audioSources[i] = base.gameObject.AddComponent<AudioSource>();
			audioSources[i].clip = audioClips[Random.Range(0, audioClips.Length)];
			audioSources[i].loop = true;
			if (Random.Range(0, 1) == 0)
			{
				audioSources[i].volume = 0f;
			}
			else
			{
				audioSources[i].volume = masterSourceVolume;
			}
		}
		for (int j = 0; j < audioSources.Length; j++)
		{
			StartCoroutine(LoopFader(j));
		}
	}

	private AudioClip FindNotYetPlayingLoop()
	{
		AudioClip audioClip;
		bool flag;
		do
		{
			audioClip = audioClips[Random.Range(0, audioClips.Length)];
			flag = true;
			for (int i = 0; i < audioSources.Length; i++)
			{
				if (audioClip == audioSources[i].clip)
				{
					flag = false;
					break;
				}
			}
		}
		while (!flag);
		return audioClip;
	}

	private IEnumerator LoopFader(int audioSourceID)
	{
		while (true)
		{
			audioSources[audioSourceID].clip = FindNotYetPlayingLoop();
			audioSources[audioSourceID].time = masterSource.time;
			audioSources[audioSourceID].Play();
			float counter2 = 0f;
			float startFade2 = audioSources[audioSourceID].volume;
			float fadeSpeed2 = 1f / Random.Range(minFadeTime, maxFadeTime);
			float targetVolume2 = Random.Range(minPlayingVolume, maxPlayingVolume);
			while (counter2 < 1f)
			{
				float nowValue = Mathf.Lerp(startFade2, targetVolume2, counter2);
				audioSources[audioSourceID].volume = nowValue;
				counter2 += Time.deltaTime * fadeSpeed2;
				yield return 0;
			}
			audioSources[audioSourceID].volume = targetVolume2;
			yield return new WaitForSeconds(Random.Range(minPlayingTime, maxPlayingTime));
			counter2 = 0f;
			startFade2 = audioSources[audioSourceID].volume;
			fadeSpeed2 = 1f / Random.Range(minFadeTime, maxFadeTime);
			targetVolume2 = 0f;
			while (counter2 < 1f)
			{
				float nowValue = Mathf.Lerp(startFade2, targetVolume2, counter2);
				audioSources[audioSourceID].volume = nowValue;
				counter2 += Time.deltaTime * fadeSpeed2;
				yield return 0;
			}
			audioSources[audioSourceID].volume = targetVolume2;
			yield return new WaitForSeconds(Random.Range(minPauseTime, maxPauseTime));
		}
	}
}
