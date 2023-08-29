namespace DiggerAPI
{
	public interface IFactory : ISystem
	{
		IRefresher CreateRefresher(IDigger digger, IColorModel model);
	}
}