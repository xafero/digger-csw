﻿namespace DiggerClassic.Scores
{
	public interface IScores
	{
		string[] ScoreInit { get; }

		long[] ScoreHigh { get; }

		ScoreTuple[] ScoreTuples { set; }
	}
}