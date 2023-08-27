using System;
using System.IO;

namespace DiggerClassic.Scores
{
	public static class ScoreStorage
	{
		public static void WriteToStorage(this TextWriter bw, IScores mem)
		{
			var scoreInit = mem.ScoreInit;
			var scoreHigh = mem.ScoreHigh;
			for (var i = 0; i < 10; i++)
			{
				bw.Write(scoreInit[i + 1]);
				bw.WriteLine();
				bw.Write(Convert.ToString(scoreHigh[i + 2]));
				bw.WriteLine();
			}
			bw.Flush();
			bw.Dispose();
		}

		public static bool TryReadFromStorage(TextReader br, out ScoreTuple[] sc)
		{
			try
			{
				sc = new ScoreTuple[10];
				for (var i = 0; i < 10; i++)
				{
					var name = br.ReadLine();
					var score = int.Parse(br.ReadLine()!);
					sc[i] = new ScoreTuple(name, score);
				}
				br.Dispose();
				return true;
			}
			catch
			{
				sc = null;
				return false;
			}
		}
	}
}