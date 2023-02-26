using System.Collections;
using UnityEngine;

public class TurnTableLoopPlayer : MonoBehaviour
{
	private AudioSource audioSource;

	private AudioSource rewardSource;

	public AudioLoopInfo audioLoopInfo;

	public AudioClipInfo audioRewardInfo;

	private Coroutine startRoutine;

	private Coroutine stopRoutine;

	private void Awake()
	{
		audioSource = base.gameObject.AddComponent<AudioSource>();
		rewardSource = base.gameObject.AddComponent<AudioSource>();
	}

	public void StartLoop()
	{
		startRoutine = StartCoroutine(StartLooper(audioLoopInfo));
		rewardSource.clip = audioRewardInfo.Clip;
		rewardSource.pitch = Random.Range(audioRewardInfo.minPitch, audioRewardInfo.maxPitch);
		rewardSource.volume = Random.Range(audioRewardInfo.minVolume, audioRewardInfo.maxVolume);
		rewardSource.Play();
	}

	public void StopLoop()
	{
		stopRoutine = StartCoroutine(StopLooper(audioLoopInfo));
	}

	private IEnumerator StartLooper(AudioLoopInfo audioLoopInfo)
	{
		audioSource.clip = audioLoopInfo.clips[Random.Range(0, audioLoopInfo.clips.Length)];
		audioSource.loop = true;
		audioSource.Play();
		float counter = 0f;
		float startFadeVol = Random.Range(audioLoopInfo.minStartVolume, audioLoopInfo.maxStartVolume);
		float startFadePitch = Random.Range(audioLoopInfo.minStartPitch, audioLoopInfo.maxStartPitch);
		float fadeSpeed = 1f / Random.Range(audioLoopInfo.minStartTime, audioLoopInfo.maxStartTime);
		float targetVol = Random.Range(audioLoopInfo.minTargetVolume, audioLoopInfo.maxTargatVolume);
		float targetPitch = Random.Range(audioLoopInfo.minTargetPitch, audioLoopInfo.maxTargetPitch);
		while (counter < 1f)
		{
			float currentVol = Mathf.Lerp(startFadeVol, targetVol, counter);
			audioSource.volume = currentVol;
			float currentPitch = Mathf.Lerp(startFadePitch, targetPitch, counter);
			audioSource.pitch = currentPitch;
			counter += Time.deltaTime * fadeSpeed;
			yield return 0;
		}
		audioSource.volume = targetVol;
		audioSource.pitch = targetPitch;
		startRoutine = null;
	}

	private IEnumerator StopLooper(AudioLoopInfo audioLoopInfo)
	{
		if (startRoutine != null)
		{
			StopCoroutine("StartLooper");
		}
		audioSource.Play();
		float counter = 0f;
		float startFadeVol = audioSource.volume;
		float startFadePitch = audioSource.pitch;
		float fadeSpeed = 1f / Random.Range(audioLoopInfo.minStopTime, audioLoopInfo.maxStopTime);
		float targetVol = 0f;
		float targetPitch = Random.Range(audioLoopInfo.minStopPitch, audioLoopInfo.maxStopPitch);
		while (counter < 1f)
		{
			float currentVol = Mathf.Lerp(startFadeVol, targetVol, counter);
			audioSource.volume = currentVol;
			float currentPitch = Mathf.Lerp(startFadePitch, targetPitch, counter);
			audioSource.pitch = currentPitch;
			counter += Time.deltaTime * fadeSpeed;
			yield return 0;
		}
		audioSource.volume = targetVol;
		audioSource.pitch = targetPitch;
		audioSource.Stop();
		stopRoutine = null;
	}
}
