using System;

namespace DiggerAPI
{
	public sealed class ScoreTuple : Tuple<string, int>
	{
		public ScoreTuple(string key, int value) : base(key, value)
		{
		}
	}
}