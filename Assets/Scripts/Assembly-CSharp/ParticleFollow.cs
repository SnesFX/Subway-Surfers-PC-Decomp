using UnityEngine;

public class ParticleFollow : MonoBehaviour
{
	public Transform Target;

	public float TweenTime;

	private float tweenVelocity;

	private Vector3 baseRotation;

	private Vector3 baseTargetRotation;

	private Vector3 baseScale;

	private float tweenRotVelocity;

	public float SineOffset;

	public float SineSpeed;

	public float RotationTweenTime;

	private void Awake()
	{
		baseRotation = base.transform.localEulerAngles;
		baseTargetRotation = Target.localEulerAngles;
		baseScale = base.transform.localScale;
		base.gameObject.SetActiveRecursively(false);
	}

	private void LateUpdate()
	{
		float num = base.transform.position.x - Target.position.x;
		Vector3 position = Target.position;
		position.x = base.transform.position.x;
		float num2 = Mathf.SmoothDamp(position.x, Target.position.x, ref tweenVelocity, TweenTime);
		if (!float.IsNaN(num2))
		{
			position.x = num2;
		}
		base.transform.position = position;
		float num3 = baseRotation.y - Target.localEulerAngles.y;
		Vector3 localEulerAngles = baseRotation;
		localEulerAngles.y = Mathf.SmoothDampAngle(localEulerAngles.y, Target.localEulerAngles.y, ref tweenRotVelocity, RotationTweenTime);
		float num4 = Mathf.Sin(SineOffset + Time.time * SineSpeed) * 5.5f;
		localEulerAngles.y += num4;
		base.transform.localEulerAngles = localEulerAngles;
	}
}
