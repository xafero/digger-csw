using System;
using DiggerClassic.API;
using DiggerSkia.Cache;
using SkiaSharp;

namespace DiggerSkia.Render
{
	public sealed class SafeScanLine : IScanLine
	{
		private readonly IDigger _digger;
		private readonly UintCache _cache;
		private SKBitmap _bitmap;

		public SafeScanLine(IDigger digger)
		{
			_digger = digger;
			_cache = new UintCache();
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

			if (_bitmap != null && (_bitmap.Width != width || _bitmap.Height != height))
			{
				_bitmap.Dispose();
				_bitmap = null;
			}
			_bitmap ??= new SKBitmap(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);

			var minFi = (int)minF;
			var pixels = _bitmap.GetPixels();

			unsafe
			{
				var ptr = (uint*)pixels.ToPointer();

				for (var x = 0; x < w; x++)
				{
					var xP = shiftX + x * minF;
					for (var y = 0; y < h; y++)
					{
						var arrayIndex = y * w + x;
						var paint = _cache.GetPaint(data[arrayIndex], model);
						var yP = shiftY + y * minF;
						var offset = (int)yP * width + (int)xP;

						for (var dx = 0; dx < minFi; dx++)
						for (var dy = 0; dy < minFi; dy++)
						{
							var dest = offset + dy * width + dx;
							var copy = ptr + dest;
							*copy = paint;
						}
					}
				}
			}

			g.DrawBitmap(_bitmap, 0, 0);
		}
	}
}