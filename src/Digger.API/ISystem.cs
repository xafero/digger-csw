namespace DiggerAPI
{
	public interface ISystem
	{
		string GetSubmitParameter();

		int GetSpeedParameter();

		void RequestFocus();
	}
}