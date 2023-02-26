using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class So : MonoBehaviour
{
	public static So Instance;

	public int initialCapacity = 5;

	public int maxCapacity = 50;

	private List<Sound> _soundList;

	private Sound _bgSound;

	private bool _audioWasPlaying;

	private float _audioTime;

	private void Awake()
	{
		if (Instance != null)
		{
			Object.Destroy(this);
			return;
		}
		Instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
		_soundList = new List<Sound>(initialCapacity);
		for (int i = 0; i < initialCapacity; i++)
		{
			_soundList.Add(new Sound(this));
		}
	}

	private void OnApplicationPause(bool didPause)
	{
		if (didPause)
		{
			if (_bgSound != null)
			{
				_audioWasPlaying = true;
				_audioTime = _bgSound.audioSource.time;
			}
		}
		else if (_audioWasPlaying)
		{
			_audioWasPlaying = false;
			_bgSound.audioSource.time = _audioTime;
			_bgSound.audioSource.Play();
		}
	}

	public void playBGMusic(AudioClip audioClip, float volume, bool loop)
	{
		if (_bgSound == null)
		{
			_bgSound = new Sound(this);
		}
		_bgSound.loop = loop;
	}

	public void beginPlaySound(AudioClipInfo audioClip)
	{
		StartCoroutine(playSoundAsync(audioClip));
	}

	private IEnumerator playSoundAsync(AudioClipInfo audioClip)
	{
		yield return 0;
		playSound(audioClip);
	}

	public Sound playSound(AudioClipInfo audioClip)
	{
		if (audioClip.Clip == null)
		{
			return null;
		}
		return playSound(audioClip.Clip, audioClip.Rollof, audioClip.minVolume, audioClip.maxVolume, audioClip.minPitch, audioClip.maxPitch, base.transform.position);
	}

	public Sound playSound(AudioClip audioClip, AudioRolloffMode rolloff, float volume, Vector3 position)
	{
		return playSound(audioClip, rolloff, volume, volume, 1f, 1f, position);
	}

	public Sound playSound(AudioClip audioClip, AudioRolloffMode rolloff, float minVolume, float maxVolume, float minPitch, float maxPitch, Vector3 position)
	{
		Sound sound = null;
		bool flag = false;
		bool flag2 = false;
		foreach (Sound sound2 in _soundList)
		{
			if (!sound2.available)
			{
				continue;
			}
			if (sound2.gameObject.name == audioClip.name)
			{
				sound = sound2;
				flag = true;
				break;
			}
			if (!flag2)
			{
				if (sound2.gameObject.name == "empty")
				{
					flag2 = true;
				}
				sound = sound2;
			}
		}
		if (sound == null)
		{
			sound = _soundList[0];
			_soundList.Add(sound);
		}
		if (flag)
		{
			StartCoroutine(sound.play(rolloff, minVolume, maxVolume, minPitch, maxPitch, position));
		}
		else
		{
			StartCoroutine(sound.playAudioClip(audioClip, rolloff, minVolume, maxVolume, minPitch, maxPitch, position));
		}
		return sound;
	}

	public void removeSound(Sound s)
	{
		_soundList.Remove(s);
	}
}
