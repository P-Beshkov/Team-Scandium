//-----------------------------------------------------------------------
// <copyright file="FileHandling.cs" company="TelerikAcademy">
//     All rights reserved © Telerik Academy 2012-2013
// </copyright>
//----------------------------------------------------------------------
namespace Game_Fifteen
{
    using System;
    using System.IO;
    using System.Linq;

    public class FileHandling
    {
        private const string TopScoresFileName = "Top.txt";

        public static string[] GetTopScoresFromFile(int scoresAmount)
        {
            try
            {
                string[] topScores = new string[scoresAmount + 1];
                StreamReader topReader = new StreamReader(TopScoresFileName);
                using (topReader)
                {
                    int line = 0;
                    while (!topReader.EndOfStream && line < scoresAmount)
                    {
                        topScores[line] = topReader.ReadLine();
                        line++;
                    }
                }
                return topScores;
            }
            catch (FileNotFoundException)
            {
                StreamWriter topWriter = new StreamWriter(TopScoresFileName);
                using (topWriter)
                {
                    topWriter.Write("");
                }
                return new string[scoresAmount];
            }
        }

        public static void UpgradeTopScoreInFile(IOrderedEnumerable<Player> sortedScores)
        {
            StreamWriter topWriter = new StreamWriter(TopScoresFileName);
            using (topWriter)
            {
                int position = 1;
                foreach (Player player in sortedScores)
                {
                    string name = player.Name;
                    int score = player.Score;
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
