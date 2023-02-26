using UnityEngine;

public static class DeviceInfo
{
	public enum FormFactor
	{
		iPhone = 0,
		iPad = 1
	}

	public enum PerformanceLevel
	{
		Low = 0,
		Medium = 1,
		High = 2
	}

	public static string deviceModel;

	public static readonly FormFactor formFactor;

	public static readonly bool isHighres;

	static DeviceInfo()
	{
		deviceModel = SystemInfo.deviceModel;
	}
}
