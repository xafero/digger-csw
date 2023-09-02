using System;
using System.Runtime.InteropServices;
using DiggerClassic.API;
using DiggerSkia.Cache;
using SkiaSharp;

namespace DiggerSkia.Render
{
	public sealed class InstScanLine : IScanLine
	{
		private readonly IDigger _digger;
		private readonly UintCache _cache;

		public InstScanLine(IDigger digger)
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

			var minFi = (int)minF;
			var pixels = new uint[width * height];

			for (var x = 0; x < w; x++)
			for (var y = 0; y < h; y++)
			{
				var arrayIndex = y * w + x;
				var paint = _cache.GetPaint(data[arrayIndex], model);
				var xP = shiftX + x * minF;
				var yP = shiftY + y * minF;
				var offset = (int)yP * width + (int)xP;

				for (var dx = 0; dx < minFi; dx++)
				for (var dy = 0; dy < minFi; dy++)
				{
					var dest = offset + dy * width + dx;
					pixels[dest] = paint;
				}
			}

			using var bitmap = new SKBitmap();
			var gcHandle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
			SKImageInfo sInfo = new(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);

			var ptr = gcHandle.AddrOfPinnedObject();
			var rowBytes = sInfo.RowBytes;
			bitmap.InstallPixels(sInfo, ptr, rowBytes, delegate { gcHandle.Free(); });

			g.DrawBitmap(bitmap, 0, 0);
		}
	}
}