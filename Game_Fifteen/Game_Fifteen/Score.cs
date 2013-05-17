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

    /// <summary>
    /// Updates top players score.
    /// </summary>
    public class Score
    {
        private const int TopPlayersCount = 5;

        private IOrderedEnumerable<Player> sortedScores;
        private Player[] TopPlayers { get; set; }

        /// <summary>
        /// Reads score list from file and
        /// updates top players
        /// </summary>
        /// <param name="move">Players moves.</param>
        /// <param name="pattern">Pattern for player data.</param>
        public void UpgradeTopScore(int move, string pattern)
        {
            string[] topScores = FileHandling.GetTopScoresFromFile(TopPlayersCount);
            string name = ConsoleManager.ReadPlayerName();

            topScores[TopPlayersCount] = string.Format("0. {0} --> {1} move", name, move);
            Array.Sort(topScores);

            this.UpdateTopPlayers(topScores, pattern);
            this.sortedScores = this.TopPlayers.OrderBy(x => x.Score).ThenBy(x => x.Name);

            FileHandling.UpgradeTopScoreInFile(this.sortedScores);
        }

        private void UpdateTopPlayers(string[] topScores, string pattern)
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
                string name = Regex.Replace(topScores[topScoresIndex], pattern, @"$1");
                string score = Regex.Replace(topScores[topScoresIndex], pattern, @"$2");
                int scoreInt = int.Parse(score);
                this.TopPlayers[topScoresPairsIndex] = new Player(name, scoreInt);
            }
        }
    }
}
