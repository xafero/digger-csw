namespace DiggerAPI
{
	public interface IRefresher
	{
		void NewPixels();

		void NewPixels(int x, int y, int width, int height);

		IColorModel Model { get; }
	}
}