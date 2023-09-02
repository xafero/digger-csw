using DiggerClassic.API;

namespace DiggerClassic.Graphics
{
	public class ColorModel : IColorModel
	{
		private readonly int _bits;
		private readonly int _size;
		private readonly byte[] _r;
		private readonly byte[] _g;
		private readonly byte[] _b;

		/// <summary>
		/// Constructs an indexed color model from the specified arrays of red, green, and blue components
		/// </summary>
		/// <param name="id">the id of the model</param>
		/// <param name="bits">the number of bits each pixel occupies</param>
		/// <param name="size">the size of the color component arrays</param>
		/// <param name="r">the array of red color components</param>
		/// <param name="g">the array of green color components</param>
		/// <param name="b">the array of blue color components</param>
		public ColorModel(int id, int bits, int size, byte[] r, byte[] g, byte[] b)
		{
			Id = id;
			_bits = bits;
			_size = size;
			_r = r;
			_g = g;
			_b = b;
		}

		public int Id { get; }

		public (int, int, int) GetColor(int index)
		{
			var r = _r[index];
			var g = _g[index];
			var b = _b[index];
			return (r, g, b);
		}
	}
}