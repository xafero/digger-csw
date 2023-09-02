using System;
using System.Runtime.InteropServices;
using DiggerClassic.API;
using SkiaSharp;

namespace DiggerSkia.Render
{
	public sealed class InstScanLine : IScanLine
	{
		private readonly IDigger _digger;

		public InstScanLine(IDigger digger)
		{
			_digger = digger;
		}

		public void Paint(SKCanvas g, SKImageInfo gInfo)
		{
			if (_digger == null)
				return;

			var pc = _digger.GetPc();

			var w = pc.GetWidth();
			var h = pc.GetHeight();

			var width = gInfo.Width;
			var height = gInfo.Height;

			var rw = width * 1f;
			var rh = height * 1f;

			var fw = rw / w;
			var fh = rh / h;

			var data = pc.GetPixels();
			var model = pc.GetCurrentSource().Model;

			var minF = Math.Min(fw, fh);
			var shiftX = (rw - w * minF) / 2;
			var shiftY = (rh - h * minF) / 2;

			var pixels = new uint[width * height];
			var alpha = byte.MaxValue;
			var minFi = (int)minF;

			for (var x = 0; x < w; x++)
			for (var y = 0; y < h; y++)
			{
				var arrayIndex = y * w + x;
				var (sr, sg, sb) = model.GetColor(data[arrayIndex]);
				var color = (uint)sr + (uint)(sg << 8) + (uint)(sb << 16) + (uint)(alpha << 24);

				var destStartX = x * minFi;
				var destStartY = y * minFi;
				var destIndex = destStartY * width + destStartX;

				for (var dx = 0; dx < minFi; dx++)
				for (var dy = 0; dy < minFi; dy++)
				{
					var destPixelIndex = destIndex + dy * width + dx;
					pixels[destPixelIndex] = color;
				}
			}

			using var bitmap = new SKBitmap();
			var gcHandle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
			SKImageInfo sInfo = new(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);

			var ptr = gcHandle.AddrOfPinnedObject();
			var rowBytes = sInfo.RowBytes;
			bitmap.InstallPixels(sInfo, ptr, rowBytes, delegate { gcHandle.Free(); });

			g.DrawBitmap(bitmap, shiftX, shiftY);
		}
	}
}