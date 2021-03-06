﻿//-----------------------------------------------------------------------
// <copyright file="Engine.cs" company="TelerikAcademy">
//     All rights reserved © Telerik Academy 2012-2013
// </copyright>
//----------------------------------------------------------------------
namespace Game_Fifteen
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Contains main gameplay logic.
    /// </summary>
    public class Engine
    {
        private const string ScorePattern = @"^\d+\. (.+) --> (\d+) moves?$";
        private const int TopScoresAmount = 5;
        private const int GameBoardSize = 4;

        /// <summary>
        /// Starts game and executes all commands.
        /// </summary>
        public static void GameStart()
        {
            ConsoleManager.PrintWelcomeMessage();
            GameField field = new GameField();
            ConsoleManager.PrintMessage(field.GetMatrix, GameBoardSize);
            
            string userInput = ReadUserInput();

            while (userInput != "exit")
            {
                int cellNumber;

                if (int.TryParse(userInput, out cellNumber))
                {
                    field.MoveCellByPlayer(cellNumber);
                    if (field.IsMatrixOrdered())
                    {
                        GameEnd(field);
                        break;
                    }
                }
                else if (userInput == "top")
                {
                    ConsoleManager.PrintTopScores(TopScoresAmount);
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

        /// <summary>
        /// Performs ending operations.
        /// Asks for player name and starts new game.
        /// </summary>
        /// <param name="field"></param>
        public static void GameEnd(GameField field)
        {
            string moves = field.Moves == 1 ? "1 move" : string.Format("{0} moves", field.Moves);
            ConsoleManager.PrintCongratulation(moves);
            string[] topScores = FileHandling.GetTopScoresFromFile(TopScoresAmount);
            if (topScores[TopScoresAmount - 1] != null)
            {
                string lowestScore = Regex.Replace(topScores[TopScoresAmount - 1], ScorePattern, @"$2");
                if (int.Parse(lowestScore) < field.Moves)
                {
                    ConsoleManager.PrintScore(TopScoresAmount);
                    GameStart();
                }
            }

            Score score = new Score();
            score.UpgradeTopScore(field.Moves, ScorePattern);
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