namespace DiggerAPI
{
	public interface IPc
	{
		int GetWidth();

		int GetHeight();

		int[] GetPixels();

		IRefresher GetCurrentSource();
	}
}