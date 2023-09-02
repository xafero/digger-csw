using DiggerClassic.API;

namespace DiggerSkia.Cache
{
	public interface IColorCache<out T>
	{
		T GetPaint(int colorIdx, IColorModel model);
	}
}