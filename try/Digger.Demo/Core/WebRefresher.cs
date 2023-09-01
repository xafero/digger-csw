using DiggerAPI;

namespace DiggerDemo.Core
{
	internal class WebRefresher : IRefresher
	{
		private readonly WebDigger _area;

		public WebRefresher(WebDigger area, IColorModel model)
		{
			_area = area;
			Model = model;
		}

		public IColorModel Model { get; }

		public void NewPixels(int x, int y, int w, int h) => NewPixels();

		public void NewPixels()
		{
			var view = _area._canvas;
			view.Invalidate();
		}
	}
}