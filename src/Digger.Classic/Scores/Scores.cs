using System;

namespace DiggerClassic.Scores
{
	public class Scores : IScores
	{
		public string[] ScoreInit { get; } = new string[11];
		public long[] ScoreHigh { get; } = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

		public ScoreTuple[] ScoreTuples
		{
			set
			{
				var @in = new string[10];
				var sc = new int[10];
				for (var i = 0; i < Math.Min(value.Length, 10); i++)
				{
					@in[i] = value[i].Item1;
					sc[i] = value[i].Item2;
				}
				for (var i = 0; i < 10; i++)
				{
					ScoreInit[i + 1] = @in[i];
					ScoreHigh[i + 2] = sc[i];
				}
			}
		}
	}
}