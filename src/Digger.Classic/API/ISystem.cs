namespace DiggerClassic.API
{
	public interface ISystem
	{
		string GetSubmitParameter();

		int GetSpeedParameter();

		void RequestFocus();
	}
}