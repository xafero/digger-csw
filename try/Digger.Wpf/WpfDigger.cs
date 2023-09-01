using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DiggerAPI;

namespace Digger.Wpf
{
	internal class WpfDigger : AppletCompat, IFactory
	{
		public IDigger _digger;
		private WriteableBitmap _buffer;
		private bool _setup;

		public WpfDigger(IDigger digger)
		{
			_digger = digger;
			RenderTransform = new ScaleTransform(4, 4);
			_setup = false;
			IsVisibleChanged += OnShow;
		}

		private void OnShow(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (_setup)
				return;
			_setup = true;
			var ctrl = (WpfDigger)sender;
			var form = (Window)ctrl.Parent;
			form.KeyDown += HandleKeyDown;
			form.KeyUp += HandleKeyUp;
		}

		protected override void OnRender(DrawingContext g)
		{
			if (_buffer == null)
			{
				var w = (Window)Parent;
				_buffer = new WriteableBitmap((int)w.Width, (int)w.Height, 96, 96, PixelFormats.Bgr24, null);
				Source = _buffer;
			}
			base.OnRender(g);
		}

		internal void DoRender(int x, int y, int width, int height) => DoRender();

		internal void DoRender()
		{
			if (_buffer == null)
				return;
			_buffer.Lock();
			DoRender(_buffer);
			_buffer.Unlock();
		}

		protected void DoRender(WriteableBitmap g)
		{
			var pc = _digger.GetPc();

			var w = pc.GetWidth();
			var h = pc.GetHeight();
			var data = pc.GetPixels();

			for (var x = 0; x < w; x++)
			{
				for (var y = 0; y < h; y++)
				{
					var arrayIndex = y * w + x;
					var color = pc.GetCurrentSource().Model.GetColor(data[arrayIndex]);
					DrawPixel(g, x, y, color);
				}
			}

			g.AddDirtyRect(new Int32Rect(0, 0, w, h));
		}

		private void DrawPixel(WriteableBitmap bitmap, int left, int top, (int R, int G, int B) color)
		{
			var colorData = color.R << 16;
			colorData |= color.G << 8;
			colorData |= color.B << 0;
			var bpp = bitmap.Format.BitsPerPixel / 8;
			unsafe
			{
				var backBuff = (long)bitmap.BackBuffer;
				backBuff += top * bitmap.BackBufferStride;
				backBuff += left * bpp;
				*(long*)backBuff = colorData;
				backBuff += bpp;
			}
		}

		protected override bool DoKeyUp(int key) => _digger.KeyUp(key);
		protected override bool DoKeyDown(int key) => _digger.KeyDown(key);

		public IRefresher CreateRefresher(IDigger digger, IColorModel model)
		   => new WpfRefresher(this, model);
	}
}