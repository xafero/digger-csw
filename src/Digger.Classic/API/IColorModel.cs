namespace DiggerClassic.API
{
	public interface IColorModel
	{
		(int r, int g, int b) GetColor(int index);

		int Id { get; }
	}
}