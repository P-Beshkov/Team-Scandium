using System;
using System.IO;
using System.Linq;

namespace Game_Fifteen
{
    class FileHandling
    {
        private const string TopScoresFileName = "Top.txt";

        private const int TopScoresAmount = 5;

        public static string[] GetTopScoresFromFile()
        {
            try
            {
                string[] topScores = new string[TopScoresAmount + 1];
                StreamReader topReader = new StreamReader(TopScoresFileName);
                using (topReader)
                {
                    int line = 0;
                    while (!topReader.EndOfStream && line < TopScoresAmount)
                    {
                        topScores[line] = topReader.ReadLine();
                        line++;
                    }
                }
                return topScores;
            }
            catch (FileNotFoundException)
            {
                StreamWriter topWriter =
                    new StreamWriter(TopScoresFileName);
                using (topWriter)
                {
                    topWriter.Write("");
                }
                return new string[TopScoresAmount];
            }
        }

        public static void UpgradeTopScoreInFile(IOrderedEnumerable<Player> sortedScores)
        {
            StreamWriter topWriter = new StreamWriter(TopScoresFileName);
            using (topWriter)
            {
                int position = 1;
                foreach (Player pair in sortedScores)
                {
                    string name = pair.Name;
                    int score = pair.Score;
                    string toWrite = string.Format(
                        "{0}. {1} --> {2} move", position, name, score);
                    if (score > 1)
                    {
                        toWrite += "s";
                    }
                    topWriter.WriteLine(toWrite);
                    position++;
                }
            }
        }
    }
}
