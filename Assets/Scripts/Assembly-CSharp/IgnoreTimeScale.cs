using UnityEngine;

[AddComponentMenu("NGUI/Internal/Ignore TimeScale Behaviour")]
public class IgnoreTimeScale : MonoBehaviour
{
	private float mTime;

	private float mActual;

	private float mDelta;

	public float realTimeDelta
	{
		get
		{
			return mDelta;
		}
	}

	private void OnEnable()
	{
		mTime = Time.realtimeSinceStartup;
	}

	private void Start()
	{
		mTime = Time.realtimeSinceStartup;
	}

	protected float UpdateRealTimeDelta()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		float b = realtimeSinceStartup - mTime;
		mActual += Mathf.Max(0f, b);
		mDelta = 0.001f * Mathf.Round(mActual * 1000f);
		mActual -= mDelta;
		mTime = realtimeSinceStartup;
		return mDelta;
	}
}
