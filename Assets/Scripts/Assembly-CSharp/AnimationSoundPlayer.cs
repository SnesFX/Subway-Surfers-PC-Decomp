using System.Collections.Generic;
using UnityEngine;

public class AnimationSoundPlayer : MonoBehaviour
{
	public Animation TargetAnimation;

	public List<KeyFrameAudio> AudioClips;

	private bool addedListeners;

	private static List<string> nodesInitialized = new List<string>();

	private AnimationEvent[] animationEvents;

	private void Start()
	{
		if (nodesInitialized.IndexOf(base.name) != -1)
		{
			return;
		}
		nodesInitialized.Add(base.name);
		animationEvents = new AnimationEvent[AudioClips.Count];
		int num = 0;
		foreach (KeyFrameAudio audioClip in AudioClips)
		{
			AnimationEvent animationEvent = new AnimationEvent();
			animationEvent.messageOptions = SendMessageOptions.RequireReceiver;
			animationEvent.time = (float)audioClip.KeyFrame / TargetAnimation[audioClip.clip].clip.frameRate;
			animationEvent.intParameter = num;
			animationEvent.functionName = "PlayKeyframeAnimation";
			TargetAnimation[audioClip.clip].clip.AddEvent(animationEvent);
			num++;
		}
	}

	public virtual void PlayKeyframeAnimation(int soundIndex)
	{
		KeyFrameAudio keyFrameAudio = AudioClips[soundIndex];
		if (keyFrameAudio.Callback != null)
		{
			keyFrameAudio.Callback(keyFrameAudio);
		}
		else
		{
			So.Instance.playSound(AudioClips[soundIndex].Audio);
		}
	}
}
