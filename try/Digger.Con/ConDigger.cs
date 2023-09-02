using DiggerClassic.API;
using DiggerSkia.Cache;

namespace Digger.Con
{
	internal sealed class ConDigger : IFactory, IRefresher
	{
		public IDigger _digger;

		private readonly PaintCache _cache;

		public ConDigger(IDigger parent)
		{
			_digger = parent;
			_cache = new PaintCache();
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
	}
}