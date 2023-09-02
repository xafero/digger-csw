using System.Collections.Generic;
using DiggerClassic.API;

namespace DiggerSkia.Cache
{
	public abstract class ColorCache<T> : IColorCache<T>
	{
		private readonly Dictionary<(int, int), T> _paints = new();

		public T GetPaint(int colorIdx, IColorModel model)
		{
			var key = (model.Id, colorIdx);
			if (_paints.TryGetValue(key, out var existing))
				return existing;
			var (sr, sg, sb) = model.GetColor(colorIdx);
			return _paints[key] = BuildColor(sr, sg, sb);
		}

		protected abstract T BuildColor(int r, int g, int b);
	}
}