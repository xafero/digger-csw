using System.Diagnostics;
using DiggerAPI;

namespace Digger.Con
{
	internal class ConRefresher : IRefresher
	{
		public void NewPixels()
		{
			NewPixels(0, 0, -1, -1);
		}

		public void NewPixels(int x, int y, int width, int height)
		{
			// TODO Refresh frame?!
			Debug.WriteLine($"{nameof(NewPixels)}: {x} {y} {width} {height}");
		}

		public IColorModel Model { get; }
	}
}