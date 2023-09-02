using System;
using System.Runtime.InteropServices;
using DiggerAPI;
using SkiaSharp;

namespace Digger.Con
{
	internal sealed class ConDigger : IFactory, IRefresher
	{
		public IDigger _digger;

		private readonly ColorCache _cache;

		public ConDigger(IDigger parent)
		{
			_digger = parent;
			_cache = new ColorCache();
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

		public void OnPaintSurface1(SKCanvas g, SKImageInfo info)
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

		public void OnPaintSurface2(SKCanvas g, SKImageInfo gInfo)
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

		private SKBitmap _bitmap;

		public void OnPaintSurface3(SKCanvas g, SKImageInfo info)
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