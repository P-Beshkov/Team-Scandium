using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Game_Fifteen
{
    class ConsoleManger
    {
        // Methods
        public static void PrintCellDoesNotExistMessage()
        {
            Console.WriteLine("That cell does not exist in the matrix.");
        }

        public static void PrintGoodbye()
        {
            Console.WriteLine("Good bye!");
        }

        public static void PrintIllegalCommandMessage()
        {
            Console.WriteLine("Illegal command!");
        }

        public static void PrintIllegalMoveMessage()
        {
            Console.WriteLine("Illegal move!");
        }
 
        public static void PrintNextMoveMessage()
        {
            Console.Write("Enter a number to move: ");
        }

        public static void PrintMatrix(string[,] matrix, int matrixSizeRows, int matrixSizeColumns)
        {
            StringBuilder horizontalBorder = new StringBuilder("  ");
            for (int i = 0; i < matrixSizeColumns; i++)
            {
                horizontalBorder.Append("---");
            }
            horizontalBorder.Append("- ");
            Console.WriteLine(horizontalBorder);
            for (int row = 0; row < matrixSizeRows; row++)
            {
                Console.Write(" |");
                for (int column = 0; column < matrixSizeColumns; column++)
                {
                    Console.Write("{0,3}", matrix[row, column]);
                }
                Console.WriteLine(" |");
            }
            Console.WriteLine(horizontalBorder);
        }

        public static void PrintTopScores()
        {
            Console.WriteLine("Scoreboard:");
            string[] topScores = FileHandling.GetTopScoresFromFile();
            if (topScores[0] == null)
            {
                Console.WriteLine("There are no scores to display yet.");
            }
            else
            {
                foreach (string score in topScores)
                {
                    if (score != null)
                    {
                        Console.WriteLine(score);
                    }
                }
            }
        }

        public static void PrintWelcomeMessage()
        {
            Console.Write("Welcome to the game \"15\". ");
            Console.WriteLine("Please try to arrange the numbers sequentially. ");
            Console.WriteLine("Use 'top' to view the top scoreboard, " +
                              "'restart' to start a new game and 'exit'  to quit the game.");
        }

        public static void PrintCongratulation(string moves)
        {
            Console.WriteLine("Congratulations! You won the game in {0}.", moves);
        }

        public static void PrintScore(int topScoresAmount)
        {
            Console.WriteLine("You couldn't get in the top {0} scoreboard.", topScoresAmount);
        }

        public static void PrintMessage(string message)
        {
            Console.WriteLine(message);
        }

        public static string ReadPlayerName()
        {
            Console.Write("Please enter your name for the top scoreboard: ");
            string name = Console.ReadLine();
            if (name == string.Empty)
            {
                name = "Anonymous";
            }

            return name;
        }
    }
}
