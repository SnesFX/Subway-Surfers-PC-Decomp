using UnityEngine;

public class SmoothDampVector3
{
	private float smoothTime;

	private Vector3 value;

	private Vector3 target;

	private Vector3 velocity = Vector3.zero;

	public Vector3 Target
	{
		get
		{
			return target;
		}
		set
		{
			target = value;
		}
	}

	public Vector3 Value
	{
		get
		{
			return value;
		}
		set
		{
			this.value = value;
		}
	}

	public float SmoothTime
	{
		get
		{
			return smoothTime;
		}
		set
		{
			smoothTime = value;
		}
	}

	public Vector3 Velocity
	{
		get
		{
			return velocity;
		}
		set
		{
			velocity = value;
		}
	}

	public SmoothDampVector3(Vector3 value, float smoothTime)
	{
		this.smoothTime = smoothTime;
		this.value = value;
		target = value;
	}

	public void Update()
	{
		float num = Mathf.SmoothDamp(value.x, target.x, ref velocity.x, smoothTime, float.PositiveInfinity, Time.deltaTime);
		if (!float.IsNaN(num))
		{
			value.x = num;
		}
		float num2 = Mathf.SmoothDamp(value.y, target.y, ref velocity.y, smoothTime, float.PositiveInfinity, Time.deltaTime);
		if (!float.IsNaN(num2))
		{
			value.y = num2;
		}
		float num3 = Mathf.SmoothDamp(value.z, target.z, ref velocity.z, smoothTime, float.PositiveInfinity, Time.deltaTime);
		if (!float.IsNaN(num3))
		{
			value.z = num3;
		}
	}
}
