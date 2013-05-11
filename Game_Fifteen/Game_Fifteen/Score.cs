using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Game_Fifteen
{
    class Score
    {
        private const int TopScoresAmount = 5;

        private const string TopScoresPersonPattern = @"^\d+\. (.+) --> (\d+) moves?$";

        public static void UpgradeTopScore(int turn)
        {
            string[] topScores = FileHandling.GetTopScoresFromFile();
            Console.Write("Please enter your name for the top scoreboard: ");
            string name = Console.ReadLine();
            if (name == string.Empty)
            {
                name = "Anonymous";
            }
            topScores[TopScoresAmount] = string.Format("0. {0} --> {1} move", name, turn);
            Array.Sort(topScores);

            Player[] topScoresPairs = UpgradeTopScorePairs(topScores);

            IOrderedEnumerable<Player> sortedScores =
            topScoresPairs.OrderBy(x => x.Score).ThenBy(x => x.Name);

            FileHandling.UpgradeTopScoreInFile(sortedScores);
        }

        public static Player[] UpgradeTopScorePairs(string[] topScores)
        {
            int startIndex = 0;
            while (topScores[startIndex] == null)
            {
                startIndex++;
            }

            int arraySize = Math.Min(TopScoresAmount - startIndex + 1, TopScoresAmount);
            Player[] topScoresPairs = new Player[arraySize];
            for (int topScoresPairsIndex = 0; topScoresPairsIndex < arraySize; topScoresPairsIndex++)
            {
                int topScoresIndex = topScoresPairsIndex + startIndex;
                string name = Regex.Replace(topScores[topScoresIndex], TopScoresPersonPattern, @"$1");
                string score = Regex.Replace(topScores[topScoresIndex], TopScoresPersonPattern, @"$2");
                int scoreInt = int.Parse(score);
                topScoresPairs[topScoresPairsIndex] = new Player(name, scoreInt);
            }

            return topScoresPairs;
        } 
    }
}
