using System;
using UnityEngine;

[Serializable]
public class AudioClipInfo
{
	public AudioClip Clip;

	public float minPitch = 0.8f;

	public float maxPitch = 1.1f;

	public float minVolume = 0.5f;

	public float maxVolume = 0.7f;

	public AudioRolloffMode Rollof = AudioRolloffMode.Linear;
}
