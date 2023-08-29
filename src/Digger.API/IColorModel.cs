namespace DiggerAPI
{
	public interface IColorModel
	{
		(int r, int g, int b) GetColor(int index);
	}
}