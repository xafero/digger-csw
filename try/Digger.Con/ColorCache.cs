using System.Collections.Generic;
using DiggerAPI;
using SkiaSharp;

namespace Digger.Con
{
	public sealed class ColorCache
	{
		private readonly Dictionary<(int, int), SKPaint> _paints = new();

		private static SKPaint NewPaint()
		{
			var paint = new SKPaint { Style = SKPaintStyle.Fill };
			return paint;
		}

		public SKPaint GetPaint(int colorIdx, IColorModel model)
		{
			var key = (model.Id, colorIdx);
			if (_paints.TryGetValue(key, out var existing))
				return existing;
			var (sr, sg, sb) = model.GetColor(colorIdx);
			var paint = NewPaint();
			paint.Color = new SKColor((byte)sr, (byte)sg, (byte)sb);
			_paints[key] = paint;
			return paint;
		}
	}
}