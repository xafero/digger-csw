using System;
using System.IO;
using DiggerAPI;

namespace DiggerClassic.Score
{
    public static class ScoreStorage
    {
        public static void createInStorage(Scores mem)
        {
            try
            {
                writeToStorage(mem);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }

        public static void writeToStorage(Scores mem)
        {
            try
            {
                var scoFile = getScoreFile();
                var bw = new StreamWriter(scoFile);
                var scoreinit = mem.scoreinit;
                var scorehigh = mem.scorehigh;
                for (var i = 0; i < 10; i++)
                {
                    bw.Write(scoreinit[i + 1]);
                    bw.WriteLine();
                    bw.Write(Convert.ToString(scorehigh[i + 2]));
                    bw.WriteLine();
                }
                bw.Flush();
                bw.Dispose();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }

        private static string getScoreFile()
        {
            var fileName = "digger.sco";
            var filePath = Path.GetFullPath(fileName);
            return filePath;
        }

        public static bool readFromStorage(Scores mem)
        {
            try
            {
                var scoFile = getScoreFile();
                if (!File.Exists(scoFile))
                    return false;
                var br = new StreamReader(scoFile);
                var sc = new ScoreTuple[10];
                for (var i = 0; i < 10; i++)
                {
                    var name = br.ReadLine();
                    var score = int.Parse(br.ReadLine());
                    sc[i] = new ScoreTuple(name, score);
                }
                br.Dispose();
                mem.scores = sc;
                return true;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
            return false;
        }
    }
}