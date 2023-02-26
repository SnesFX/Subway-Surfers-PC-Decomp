using UnityEngine;

[AddComponentMenu("NGUI/Tween/Transform")]
public class TweenTransform : UITweener
{
	public Transform from;

	public Transform to;

	private Transform mTrans;

	protected override void OnUpdate(float factor)
	{
		if (from != null && to != null)
		{
			if (mTrans == null)
			{
				mTrans = base.transform;
			}
			mTrans.position = from.position * (1f - factor) + to.position * factor;
			mTrans.localScale = from.localScale * (1f - factor) + to.localScale * factor;
			mTrans.rotation = Quaternion.Slerp(from.rotation, to.rotation, factor);
		}
	}

	public static TweenTransform Begin(GameObject go, float duration, Transform from, Transform to)
	{
		TweenTransform tweenTransform = UITweener.Begin<TweenTransform>(go, duration);
		tweenTransform.from = from;
		tweenTransform.to = to;
		return tweenTransform;
	}
}
