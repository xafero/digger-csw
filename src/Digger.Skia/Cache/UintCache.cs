namespace DiggerSkia.Cache
{
	public sealed class UintCache : ColorCache<uint>
	{
		private readonly byte _alpha = byte.MaxValue;

		protected override uint BuildColor(int sr, int sg, int sb)
		{
			var color = (uint)sr + (uint)(sg << 8) + (uint)(sb << 16) + (uint)(_alpha << 24);
			return color;
		}
	}
}