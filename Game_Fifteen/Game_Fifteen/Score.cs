//-----------------------------------------------------------------------
// <copyright file="Score.cs" company="TelerikAcademy">
//     All rights reserved © Telerik Academy 2012-2013
// </copyright>
//----------------------------------------------------------------------
namespace Game_Fifteen
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Score
    {
        private const int TopPlayersCount = 5;
        private const string ScorePattern = @"^\d+\. (.+) --> (\d+) moves?$";

        private IOrderedEnumerable<Player> sortedScores;
        private Player[] TopPlayers { get; set; }

        public void UpgradeTopScore(int turn)
        {
            string[] topScores = FileHandling.GetTopScoresFromFile();
            string name = ConsoleManager.ReadPlayerName();

            topScores[TopPlayersCount] = string.Format("0. {0} --> {1} move", name, turn);
            Array.Sort(topScores);

            this.UpdateTopPlayers(topScores);
            this.sortedScores = this.TopPlayers.OrderBy(x => x.Score).ThenBy(x => x.Name);

            FileHandling.UpgradeTopScoreInFile(this.sortedScores);
        }

        private void UpdateTopPlayers(string[] topScores)
        {
            int startIndex = 0;
            while (topScores[startIndex] == null)
            {
                startIndex++;
            }

            int arraySize = Math.Min(TopPlayersCount - startIndex + 1, TopPlayersCount);

            this.TopPlayers = new Player[arraySize];

            for (int topScoresPairsIndex = 0; topScoresPairsIndex < arraySize; topScoresPairsIndex++)
            {
                int topScoresIndex = topScoresPairsIndex + startIndex;
                string name = Regex.Replace(topScores[topScoresIndex], ScorePattern, @"$1");
                string score = Regex.Replace(topScores[topScoresIndex], ScorePattern, @"$2");
                int scoreInt = int.Parse(score);
                this.TopPlayers[topScoresPairsIndex] = new Player(name, scoreInt);
            }
        }
    }
}
