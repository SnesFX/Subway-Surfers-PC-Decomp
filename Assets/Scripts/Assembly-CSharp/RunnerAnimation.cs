using UnityEngine;

public class RunnerAnimation : MonoBehaviour
{
	private static bool addedListeners;

	public float AnimationSpeedUpFactor = 0.5f;

	public void SetAnimationSpeedEvent(AnimationEvent animEvent)
	{
		if (!addedListeners)
		{
			addedListeners = true;
			animEvent.animationState.speed = 1f + (Game.Instance.NormalizedGameSpeed - 1f) * AnimationSpeedUpFactor;
		}
	}
}
