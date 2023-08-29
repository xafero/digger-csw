using DiggerAPI;

namespace DiggerClassic.Graphics
{
	public sealed class ColorModel : IColorModel
	{
		private readonly byte[] _r;
		private readonly byte[] _g;
		private readonly byte[] _b;

		/// <summary>
		/// Constructs an indexed color model from the specified arrays of red, green, and blue components
		/// </summary>
		/// <param name="r">the array of red color components</param>
		/// <param name="g">the array of green color components</param>
		/// <param name="b">the array of blue color components</param>
		public ColorModel(byte[] r, byte[] g, byte[] b)
		{
			_r = r;
			_g = g;
			_b = b;
		}

		public (int r, int g, int b) GetColor(int index)
		{
			var r = _r[index];
			var g = _g[index];
			var b = _b[index];
			return (r, g, b);
		}
	}
}