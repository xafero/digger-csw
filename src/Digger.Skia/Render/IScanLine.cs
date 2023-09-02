using SkiaSharp;

namespace DiggerSkia.Render
{
	public interface IScanLine
	{
		void Paint(SKCanvas g, SKImageInfo info);
	}
}