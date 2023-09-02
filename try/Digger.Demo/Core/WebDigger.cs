using System;
using DiggerClassic.API;
using DiggerSkia.Cache;
using SkiaSharp.Views.Blazor;

namespace DiggerDemo.Core
{
	internal sealed class WebDigger : AppletCompat, IFactory
	{
		public IDigger _digger;
		public SKGLView _canvas;

		private readonly PaintCache _cache;

		public WebDigger() : this(null)
		{
		}

		public WebDigger(IDigger digger)
		{
			_digger = digger;
			_cache = new PaintCache();
		}
		
		public void PaintSurface(SKPaintGLSurfaceEventArgs e)
		{
			if (_digger == null)
				return;

			var g = e.Surface.Canvas;
			var pc = _digger.GetPc();

			var w = pc.GetWidth();
			var h = pc.GetHeight();

			var rw = e.Info.Width * 1d;
			var rh = e.Info.Height * 1d;

			var fw = rw / w;
			var fh = rh / h;
			var minF = (float)Math.Min(fw, fh);

			var data = pc.GetPixels();
			var model = pc.GetCurrentSource().Model;

			int shiftX = 0, shiftY = 0;

			for (var x = 0; x < w; x++)
			{
				var xPos = shiftX + (x * minF);
				for (var y = 0; y < h; y++)
				{
					var arrayIndex = y * w + x;
					var paint = _cache.GetPaint(data[arrayIndex], model);
					g.DrawRect(xPos, shiftY + (y * minF), minF, minF, paint);
				}
			}
		}

		public override bool KeyUp(int key) => _digger.KeyUp(key);
		public override bool KeyDown(int key) => _digger.KeyDown(key);

		public IRefresher CreateRefresher(IDigger digger, IColorModel model)
			=> new WebRefresher(this, model);
	}
}