using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button")]
public class UIButton : UIButtonColor
{
	public bool isEnabled
	{
		get
		{
			Collider collider = base.GetComponent<Collider>();
			return (bool)collider && collider.enabled;
		}
		set
		{
			Collider collider = base.GetComponent<Collider>();
			if ((bool)collider && collider.enabled != value)
			{
				collider.enabled = value;
				UpdateColor(value, false);
			}
		}
	}

	protected override void Start()
	{
		base.Start();
		if (!isEnabled)
		{
			UpdateColor(false, true);
		}
	}

	private void UpdateColor(bool shouldBeEnabled, bool immediate)
	{
		if ((bool)tweenTarget)
		{
			Color color = base.defaultColor;
			if (!shouldBeEnabled)
			{
				color.r *= 0.65f;
				color.g *= 0.65f;
				color.b *= 0.65f;
			}
			TweenColor tweenColor = TweenColor.Begin(tweenTarget, 0.15f, color);
			if (immediate)
			{
				tweenColor.color = color;
				tweenColor.enabled = false;
			}
		}
	}
}
