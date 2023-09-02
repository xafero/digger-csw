using DiggerClassic.API;
using DiggerSkia.Render;
using SkiaSharp.Views.Blazor;

namespace DiggerDemo.Core
{
	internal sealed class WebDigger : AppletCompat, IFactory
	{
		public IDigger _digger;
		public SKGLView _canvas;

		public WebDigger() : this(null)
		{
		}

		private IScanLine _liner;

		public WebDigger(IDigger digger)
		{
			_digger = digger;
		}

		public void PaintSurface(SKPaintGLSurfaceEventArgs e)
		{
			if (_digger == null)
				return;
			_liner ??= new SafeScanLine(_digger);
			_liner.Paint(e.Surface.Canvas, e.Info);
		}

		public bool KeyUp(int key) => _digger.KeyUp(key);
		public bool KeyDown(int key) => _digger.KeyDown(key);

		public IRefresher CreateRefresher(IDigger digger, IColorModel model)
			=> new WebRefresher(this, model);
	}
}