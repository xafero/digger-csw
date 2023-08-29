using System;
using System.IO;
using DiggerAPI;

namespace DiggerClassic.Score
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

		public static void createInStorage(Scores scores)
		{
			throw new NotImplementedException();
		}

		public static bool readFromStorage(Scores scores)
		{
			throw new NotImplementedException();
		}

		public static bool ReadFromStorage(IScores mem)
		{
			try
			{
				var scoFile = GetScoreFile();
				if (!File.Exists(scoFile))
					return false;
				using var br = new StreamReader(scoFile);
				TryReadFromStorage(br, out var sc);
				mem.ScoreTuples = sc!;
				return true;
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(e);
			}
			return false;
		}

		public static void WriteToStorage(IScores mem)
		{
			try
			{
				var scoFile = GetScoreFile();
				using var bw = new StreamWriter(scoFile);
				WriteToStorage(bw, mem);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(e);
			}
		}

		public static void CreateInStorage(IScores mem)
		{
			try
			{
				WriteToStorage(mem);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(e);
			}
		}

		private static string GetScoreFile()
		{
			var fileName = "digger.sco";
			var filePath = Path.GetFullPath(fileName);
			return filePath;
		}
	}
}