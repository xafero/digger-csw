using SkiaSharp;

namespace DiggerSkia.Cache
{
	public sealed class PaintCache : ColorCache<SKPaint>
	{
		protected override SKPaint BuildColor(int sr, int sg, int sb)
			=> new()
			{
				Style = SKPaintStyle.Fill,
				Color = new SKColor((byte)sr, (byte)sg, (byte)sb)
			};
	}
}