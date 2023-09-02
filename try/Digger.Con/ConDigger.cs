using System;
using DiggerAPI;
using SkiaSharp;

namespace Digger.Con
{
	internal sealed class ConDigger : IFactory, IRefresher
	{
		public IDigger _digger;

		public ConDigger(IDigger parent)
		{
			_digger = parent;
		}

		public void SetFocusable(bool _)
		{
			// NO-OP!
		}

		public string GetSubmitParameter() => null;

		public int GetSpeedParameter() => 66;

		public void RequestFocus()
		{
			// NO-OP!
		}

		public IRefresher CreateRefresher(IDigger _, IColorModel model)
		{
			Model = model;
			return this;
		}

		public void NewPixels()
		{
			NewPixels(0, 0, -1, -1);
		}

		public void NewPixels(int x, int y, int width, int height)
		{
			// NO-OP!
		}

		public IColorModel Model { get; set; }

		public void OnPaintSurface(SKCanvas g, SKImageInfo info)
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

			var shiftX = (float)(rw - (w * minF)) / 2;
			var shiftY = (float)(rh - (h * minF)) / 2;
			var paint = new SKPaint { Style = SKPaintStyle.Fill };

			for (var x = 0; x < w; x++)
			{
				for (var y = 0; y < h; y++)
				{
					var arrayIndex = y * w + x;
					var (sr, sg, sb) = model.GetColor(data[arrayIndex]);
					paint.Color = new SKColor((byte)sr, (byte)sg, (byte)sb);
					g.DrawRect(shiftX + (x * minF), shiftY + (y * minF), minF, minF, paint);
				}
			}
		}
	}
}