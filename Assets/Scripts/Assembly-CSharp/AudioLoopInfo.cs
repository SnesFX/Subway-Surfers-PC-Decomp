using System;
using UnityEngine;

[Serializable]
public class AudioLoopInfo
{
	public AudioClip[] clips;

	public float minStartTime = 0.4f;

	public float maxStartTime = 3f;

	public float minStartPitch;

	public float maxStartPitch = 0.5f;

	public float minStartVolume;

	public float maxStartVolume;

	public float minTargetPitch = 0.8f;

	public float maxTargetPitch = 1.1f;

	public float minTargetVolume = 0.5f;

	public float maxTargatVolume = 0.7f;

	public float minStopTime = 0.4f;

	public float maxStopTime = 3f;

	public float minStopPitch;

	public float maxStopPitch = 0.5f;

	public AudioRolloffMode Rollof = AudioRolloffMode.Linear;
}
