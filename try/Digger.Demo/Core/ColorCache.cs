using System.Collections.Generic;
using DiggerAPI;
using SkiaSharp;

namespace DiggerDemo.Core
{
	public sealed class ColorCache
	{
		private readonly Dictionary<int, SKPaint> _paints = new();

		private static SKPaint NewPaint()
		{
			var paint = new SKPaint { Style = SKPaintStyle.Fill };
			return paint;
		}

		public SKPaint GetPaint(int colorIdx, IColorModel model)
		{
			if (_paints.TryGetValue(colorIdx, out var existing))
				return existing;
			var (sr, sg, sb) = model.GetColor(colorIdx);
			var paint = NewPaint();
			paint.Color = new SKColor((byte)sr, (byte)sg, (byte)sb);
			_paints[colorIdx] = paint;
			return paint;
		}
	}
}