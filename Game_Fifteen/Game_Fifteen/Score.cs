namespace Game_Fifteen
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Score
    {
        private const int TopPlayersCount = 5;
        private const string TopScoresPersonPattern = @"^\d+\. (.+) --> (\d+) moves?$";

        private IOrderedEnumerable<Player> sortedScores;

        public Score()
        {
        }

        public Player[] TopPlayers { get; private set; }

        public void UpgradeTopScore(int turn)
        {
            string[] topScores = FileHandling.GetTopScoresFromFile();

            string name = PrintingOnConsole.ReadPlayerName();

            topScores[TopPlayersCount] = string.Format("0. {0} --> {1} move", name, turn);
            Array.Sort(topScores);

            this.UpgradeTopPlayers(topScores);

            this.sortedScores = this.TopPlayers.OrderBy(x => x.Score).ThenBy(x => x.Name);

            FileHandling.UpgradeTopScoreInFile(this.sortedScores);
        }

        private void UpgradeTopPlayers(string[] topScores)
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
                string name = Regex.Replace(topScores[topScoresIndex], TopScoresPersonPattern, @"$1");
                string score = Regex.Replace(topScores[topScoresIndex], TopScoresPersonPattern, @"$2");
                int scoreInt = int.Parse(score);
                this.TopPlayers[topScoresPairsIndex] = new Player(name, scoreInt);
            }
        } 
    }
}
