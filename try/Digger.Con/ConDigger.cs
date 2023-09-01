using DiggerAPI;

namespace Digger.Con
{
	internal class ConDigger : IFactory
	{
		public IDigger _digger;

		public ConDigger(IDigger parent)
		{
			_digger = parent;
		}

		public void SetFocusable(bool _)
		{
			// NO-OP!
		}

		public string GetSubmitParameter()
		{
			return null;
		}

		public int GetSpeedParameter()
		{
			return 66 * 2;
		}

		public void RequestFocus()
		{
			// NO-OP!
		}

		public IRefresher CreateRefresher(IDigger digger, IColorModel model)
		{
			return new ConRefresher();
		}
	}
}