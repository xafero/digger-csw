using System.Threading;

namespace DiggerClassic
{
	internal static class Thready
	{
		public static void Sleep(int duration)
		{
			Thread.Sleep(duration);
		}

		public static Thread New(ThreadStart action)
		{
			return new Thread(action);
		}
	}
}