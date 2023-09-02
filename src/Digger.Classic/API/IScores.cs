namespace DiggerClassic.API
{
	public interface IScores
	{
		string[] ScoreInit { get; }

		long[] ScoreHigh { get; }

		ScoreTuple[] ScoreTuples { set; }
	}
}