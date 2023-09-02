using System;
using DiggerClassic.API;
using SkiaSharp;

namespace DiggerSkia.Render
{
	public sealed class SafeScanLine : IScanLine
	{
		private readonly IDigger _digger;
		private SKBitmap _bitmap;

		public SafeScanLine(IDigger digger)
		{
			_digger = digger;
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
			var minF = Math.Min(fw, fh);
			var minFi = (int)minF;

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
			var pixels = _bitmap.GetPixels();
			var alpha = byte.MaxValue;

			unsafe
			{
				var ptr = (uint*)pixels.ToPointer();

				for (var x = 0; x < w; x++)
				for (var y = 0; y < h; y++)
				{
					var (sr, sg, sb) = model.GetColor(data[y * w + x]);
					var color = (uint)sr + (uint)(sg << 8) + (uint)(sb << 16) + (uint)(alpha << 24);
					var offset = y * minFi * width + x * minFi;

					for (var dx = 0; dx < minFi; dx++)
					for (var dy = 0; dy < minFi; dy++)
					{
						var dest = offset + dy * width + dx;
						var copy = ptr + dest;
						*copy = color;
					}
				}
			}

			g.DrawBitmap(_bitmap, shiftX, shiftY);
		}
	}
}