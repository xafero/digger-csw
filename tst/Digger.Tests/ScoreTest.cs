using System.IO;
using DiggerClassic.API;
using DiggerClassic.Score;
using FluentAssertions;
using Xunit;

namespace Digger.Tests
{
	public class ScoreTest
	{
		[Fact]
		public void TestWrite()
		{
			var tupleIn = new ScoreTuple[]
			{
				new("aaa", 2), new("bbb", 3), new("ccc", 5), new("ddd", 12), new("eee", 13),
				new("fff", 15), new("ggg", 22), new("hhh", 23), new("iii", 25), new("jjj", 27)
			};
			using var sw = new StringWriter();
			ShouldWrite(sw, tupleIn);
			using var sr = new StringReader(sw.ToString());
			var tupleOut = ShouldRead(sr);
			tupleIn.Should().BeEquivalentTo(tupleOut, options => options.WithStrictOrdering());
		}

		[Fact]
		public void TestReadNull()
		{
			Assert.Null(ShouldRead(null));
		}

		private static ScoreTuple[] ShouldRead(TextReader reader)
		{
			return ScoreStorage.ReadFromStorage(reader);
		}

		private static void ShouldWrite(TextWriter writer, ScoreTuple[] tuples)
		{
			var scores = new Scores(null) { ScoreTuples = tuples };
			ScoreStorage.WriteToStorage(writer, scores);
		}
	}
}