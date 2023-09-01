using DiggerAPI;

namespace Digger.Wpf
{
	internal class WpfRefresher : IRefresher
	{
		private readonly WpfDigger _area;

		public WpfRefresher(WpfDigger area, IColorModel model)
		{
			_area = area;
			Model = model;
		}

		public IColorModel Model { get; }

		public void NewPixels(int x, int y, int width, int height)
		{
			if (!_area.IsInitialized)
				return;
			var a = () => _area.DoRender(x, y, width, height);
			if (!_area.Dispatcher.CheckAccess())
			{
				_area.Dispatcher.Invoke(a);
				return;
			}
			a();
		}

		public void NewPixels()
		{
			if (!_area.IsInitialized)
				return;
			var a = () => _area.DoRender();
			if (!_area.Dispatcher.CheckAccess())
			{
				_area.Dispatcher.Invoke(a);
				return;
			}
			a();
		}
	}
}