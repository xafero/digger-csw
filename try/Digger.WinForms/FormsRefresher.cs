using System.Drawing;
using System.Windows.Forms;
using DiggerAPI;

namespace Digger.WinForms
{
	internal class FormsRefresher : IRefresher
	{
		private readonly Panel _area;

		public FormsRefresher(Panel area, IColorModel model)
		{
			_area = area;
			Model = model;
		}

		public IColorModel Model { get; }

		public void NewPixels(int x, int y, int w, int h)
		{
			if (!_area.IsHandleCreated)
				return;
			var a = () => _area.Invalidate(new Rectangle(x, y, w, h));
			if (_area.InvokeRequired)
			{
				_area.Invoke(a);
				return;
			}
			a();
		}

		public void NewPixels()
		{
			if (!_area.IsHandleCreated)
				return;
			var a = () => _area.Invalidate();
			if (_area.InvokeRequired)
			{
				_area.Invoke(a);
				return;
			}
			a();
		}
	}
}