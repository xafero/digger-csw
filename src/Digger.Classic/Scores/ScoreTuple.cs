using System;

namespace DiggerClassic.Scores
{
	public sealed class ScoreTuple : Tuple<string, int>
	{
		public ScoreTuple(string key, int value) : base(key, value)
		{
		}
	}
}