using DiggerAPI;

namespace Digger.Con
{
	internal sealed class ConRefresher : IRefresher
	{
		public void NewPixels()
		{
			NewPixels(0, 0, -1, -1);
		}

		public void NewPixels(int x, int y, int width, int height)
		{
			// NO-OP!
		}

		public IColorModel Model { get; set; }
		public IDigger Parent { get; set; }
	}
}