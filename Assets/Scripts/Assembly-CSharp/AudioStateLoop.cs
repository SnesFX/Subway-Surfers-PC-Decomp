using System.Collections;
using UnityEngine;

public class AudioStateLoop : MonoBehaviour
{
	public AudioSource musicPlayer;

	public float menuMusicVolume = 0.3f;

	public float ingameMusicVolume = 0.4f;

	public AudioClip jetpackLoop;

	public float jetpackVolume = 0.4f;

	public float jetpackMinPitch = 0.8f;

	public float jetpackMaxPitch = 1.1f;

	public AudioClip magnetLoop;

	public float magnetVolume = 0.4f;

	public float magnetMinPitch = 0.8f;

	public float magnetMaxPitch = 1.1f;

	public AudioClip mysteryBoxOpenSound;

	public float mysteryVolume = 1f;

	private AudioSource jetpackSource;

	private AudioSource magnetSource;

	private AudioSource mysterySource;

	private bool hasPlayedIntro;

	public float fadeDownTime = 0.5f;

	public float pauseTime = 3f;

	public float fadeUpTime = 4f;

	private void Awake()
	{
		jetpackSource = base.gameObject.AddComponent<AudioSource>();
		jetpackSource.clip = jetpackLoop;
		jetpackSource.volume = jetpackVolume;
		jetpackSource.loop = true;
		jetpackSource.playOnAwake = false;
		magnetSource = base.gameObject.AddComponent<AudioSource>();
		magnetSource.clip = magnetLoop;
		magnetSource.volume = magnetVolume;
		magnetSource.loop = true;
		magnetSource.playOnAwake = false;
		mysterySource = base.gameObject.AddComponent<AudioSource>();
		mysterySource.clip = mysteryBoxOpenSound;
		mysterySource.volume = mysteryVolume;
		magnetSource.playOnAwake = false;
		musicPlayer.volume = ingameMusicVolume;
		musicPlayer.bypassEffects = true;
		musicPlayer.Play();
	}

	public void PlayMysteryBoxOpenSound()
	{
		mysterySource.Play();
		StartCoroutine(MusicFader());
	}

	private IEnumerator MusicFader()
	{
		float counter2 = 0f;
		float startFade = musicPlayer.volume;
		float fadeSpeed2 = 1f / fadeDownTime;
		while (counter2 < 1f)
		{
			musicPlayer.volume = Mathf.Lerp(startFade, 0f, counter2);
			counter2 += Time.deltaTime * fadeSpeed2;
			yield return 0;
		}
		musicPlayer.volume = 0f;
		yield return new WaitForSeconds(pauseTime);
		counter2 = 0f;
		fadeSpeed2 = 1f / fadeUpTime;
		while (counter2 < 1f)
		{
			musicPlayer.volume = Mathf.Lerp(0f, menuMusicVolume, counter2);
			counter2 += Time.deltaTime * fadeSpeed2;
			yield return 0;
		}
		musicPlayer.volume = menuMusicVolume;
	}

	public void ChangeLoop(AudioState audioState)
	{
		switch (audioState)
		{
		case AudioState.Menu:
			if (hasPlayedIntro)
			{
				musicPlayer.bypassEffects = false;
				musicPlayer.volume = menuMusicVolume;
			}
			break;
		case AudioState.Ingame:
			if (hasPlayedIntro)
			{
				musicPlayer.timeSamples = 0;
			}
			else
			{
				hasPlayedIntro = true;
			}
			musicPlayer.bypassEffects = true;
			musicPlayer.volume = ingameMusicVolume;
			break;
		case AudioState.Jetpack:
			PlayLoop(jetpackSource, jetpackMaxPitch, jetpackMaxPitch);
			break;
		case AudioState.JetpackStop:
			StopLoop(jetpackSource);
			break;
		case AudioState.Magnet:
			PlayLoop(magnetSource, magnetMinPitch, magnetMaxPitch);
			break;
		case AudioState.MagnetStop:
			StopLoop(magnetSource);
			break;
		}
	}

	public void PlayLoop(AudioSource audioSource)
	{
		audioSource.pitch = Random.Range(0.8f, 1.1f);
		audioSource.Play();
	}

	public void PlayLoop(AudioSource audioSource, float minPitch, float maxPitch)
	{
		audioSource.pitch = Random.Range(minPitch, maxPitch);
		audioSource.Play();
	}

	public void StopLoop(AudioSource audioSource)
	{
		audioSource.Stop();
	}
}
