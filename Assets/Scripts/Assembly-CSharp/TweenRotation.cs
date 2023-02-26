using UnityEngine;

[AddComponentMenu("NGUI/Tween/Rotation")]
public class TweenRotation : UITweener
{
	public Vector3 from;

	public Vector3 to;

	private Transform mTrans;

	public Transform cachedTransform
	{
		get
		{
			if (mTrans == null)
			{
				mTrans = base.transform;
			}
			return mTrans;
		}
	}

	public Quaternion rotation
	{
		get
		{
			return cachedTransform.localRotation;
		}
		set
		{
			cachedTransform.localRotation = value;
		}
	}

	protected override void OnUpdate(float factor)
	{
		cachedTransform.localRotation = Quaternion.Slerp(Quaternion.Euler(from), Quaternion.Euler(to), factor);
	}

	public static TweenRotation Begin(GameObject go, float duration, Quaternion rot)
	{
		TweenRotation tweenRotation = UITweener.Begin<TweenRotation>(go, duration);
		tweenRotation.from = tweenRotation.rotation.eulerAngles;
		tweenRotation.to = rot.eulerAngles;
		return tweenRotation;
	}
}
