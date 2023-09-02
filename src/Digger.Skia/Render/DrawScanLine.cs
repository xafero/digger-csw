using System;
using DiggerClassic.API;
using SkiaSharp;

namespace DiggerSkia.Render
{
	public sealed class DrawScanLine : IScanLine
	{
		private readonly IDigger _digger;

		public DrawScanLine(IDigger digger)
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

			var rw = info.Width * 1d;
			var rh = info.Height * 1d;

			var fw = rw / w;
			var fh = rh / h;
			var minF = (float)Math.Min(fw, fh);

			var data = pc.GetPixels();
			var model = pc.GetCurrentSource().Model;

			var shiftX = (float)(rw - w * minF) / 2;
			var shiftY = (float)(rh - h * minF) / 2;
			var paint = new SKPaint { Style = SKPaintStyle.Fill };

			for (var x = 0; x < w; x++)
			{
				for (var y = 0; y < h; y++)
				{
					var arrayIndex = y * w + x;
					var (sr, sg, sb) = model.GetColor(data[arrayIndex]);
					paint.Color = new SKColor((byte)sr, (byte)sg, (byte)sb);
					g.DrawRect(shiftX + x * minF, shiftY + y * minF, minF, minF, paint);
				}
			}
		}
	}
}