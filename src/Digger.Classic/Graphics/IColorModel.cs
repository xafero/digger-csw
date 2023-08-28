namespace DiggerClassic.Graphics
{
	public interface IColorModel
	{
		(int r, int g, int b) GetColor(int index);
	}
}