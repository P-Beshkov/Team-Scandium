//-----------------------------------------------------------------------
// <copyright file="Engine.cs" company="TelerikAcademy">
//     All rights reserved © Telerik Academy 2012-2013
// </copyright>
//----------------------------------------------------------------------
namespace Game_Fifteen
{
    using System;
    using System.Text.RegularExpressions;

    public class Engine
    {
        private const string TopScoresPersonPattern = @"^\d+\. (.+) --> (\d+) moves?$";

        private const int TopScoresAmount = 5;

        public static void GameStart()
        {
            ConsoleManager.PrintWelcomeMessage();
            //GameField field = GameField.GetGameFieldInstance();
            GameField field = new GameField();
            ConsoleManager.PrintMatrix(field.GetMatrix, field.FieldRows);
            string userInput = ReadUserInput();

            while (userInput != "exit")
            {
                int cellNumber;
                if (int.TryParse(userInput, out cellNumber))
                {
                    field.MoveCellByPlayer(cellNumber);
                    if (field.IsMazeOrdered())
                    {
                        GameEnd(field);
                    }
                }
                else if (userInput == "top")
                {
                    ConsoleManager.PrintTopScores();
                }
                else if (userInput == "restart")
                {
                    GameStart();
                }
                else
                {
                    ConsoleManager.PrintIllegalCommandMessage();
                }

                userInput = ReadUserInput();
            }

            ConsoleManager.PrintGoodbye();
        }

        public static void GameEnd(GameField field)
        {
            string moves = field.Turns == 1 ? "1 move" : string.Format("{0} moves", field.Turns);
            ConsoleManager.PrintCongratulation(moves);
            string[] topScores = FileHandling.GetTopScoresFromFile();
            if (topScores[TopScoresAmount - 1] != null)
            {
                string lowestScore = Regex.Replace(topScores[TopScoresAmount - 1], TopScoresPersonPattern, @"$2");
                if (int.Parse(lowestScore) < field.Turns)
                {
                    ConsoleManager.PrintScore(TopScoresAmount);
                    GameStart();
                }
            }

            Score score = new Score();
            score.UpgradeTopScore(field.Turns);
            GameStart();
        }

        private static string ReadUserInput()
        {
            ConsoleManager.PrintNextMoveMessage();
            string consoleInputLine = Console.ReadLine();
            return consoleInputLine;
        }
    }
}