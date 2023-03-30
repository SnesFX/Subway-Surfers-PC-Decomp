using System;
using System.Runtime.InteropServices;

namespace Sensors
{
	public class Accelerometer
	{
		private static bool hasInitialized;

		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool init();

		public static extern void raw(ref IntPtr data);

		public static void raw(out AccelData accelData)
		{
			if (!hasInitialized)
			{
				hasInitialized = true;
				init();
			}
			IntPtr data = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(AccelData)));
			raw(ref data);
			accelData = (AccelData)Marshal.PtrToStructure(data, typeof(AccelData));
			Marshal.FreeHGlobal(data);
		}

		public static extern void terminate();
	}
}
