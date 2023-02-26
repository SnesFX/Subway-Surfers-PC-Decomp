using System.Collections.Generic;
using UnityEngine;

public class KeyframeBehaviour : MonoBehaviour
{
	public Animation TargetAnimation;

	public List<ParticleSystem> TargetObjects;

	public List<KeyFrameAction> Actions;

	private AnimationEvent[] animationEvents;

	private void Start()
	{
		animationEvents = new AnimationEvent[Actions.Count];
		int num = 0;
		foreach (KeyFrameAction action in Actions)
		{
			AnimationEvent animationEvent = new AnimationEvent();
			animationEvent.messageOptions = SendMessageOptions.RequireReceiver;
			animationEvent.time = (float)action.KeyFrame / TargetAnimation[action.clip].clip.frameRate;
			animationEvent.intParameter = num;
			animationEvent.functionName = "DoKeyframeAnimation";
			TargetAnimation[action.clip].clip.AddEvent(animationEvent);
			num++;
		}
	}

	public void DoKeyframeAnimation(int soundIndex)
	{
		KeyFrameAction info = Actions[soundIndex];
		TargetObjects.ForEach(delegate(ParticleSystem t)
		{
			t.enableEmission = info.state;
		});
	}
}
