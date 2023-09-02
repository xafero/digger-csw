using System;
using DiggerClassic.API;
using DiggerSkia.Cache;
using SkiaSharp;

namespace DiggerSkia.Render
{
	public sealed class DrawScanLine : IScanLine
	{
		private readonly IDigger _digger;
		private readonly PaintCache _cache;

		public DrawScanLine(IDigger digger)
		{
			_digger = digger;
			_cache = new PaintCache();
		}

		public void Paint(SKCanvas g, SKImageInfo info)
		{
			if (_digger == null)
				return;

			var pc = _digger.GetPc();

			var w = pc.GetWidth();
			var h = pc.GetHeight();

			var width = info.Width;
			var height = info.Height;

			var rw = width * 1d;
			var rh = height * 1d;

			var fw = rw / w;
			var fh = rh / h;
			var minF = (float)Math.Min(fw, fh);

			var data = pc.GetPixels();
			var model = pc.GetCurrentSource().Model;

			var shiftX = (float)(rw - w * minF) / 2;
			var shiftY = (float)(rh - h * minF) / 2;

			for (var x = 0; x < w; x++)
			{
				var xP = shiftX + x * minF;
				for (var y = 0; y < h; y++)
				{
					var arrayIndex = y * w + x;
					var paint = _cache.GetPaint(data[arrayIndex], model);
					var yP = shiftY + y * minF;
					g.DrawRect(xP, yP, minF, minF, paint);
				}
			}
		}
	}
}