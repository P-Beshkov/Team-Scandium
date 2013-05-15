using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Game_Fifteen
{
    public class Engine
    {
        public enum Command
        {
            MoveCel,
            Top,
            Exit,
            Illegal,
            Restart
        }

        // Const 
        private const string EmptyCellValue = " ";

        private const int MatrixSizeRows = 4;

        private const int MatrixSizeColumns = 4;        

        private const string TopScoresPersonPattern = @"^\d+\. (.+) --> (\d+) moves?$";

        private const int TopScoresAmount = 5;

        // ReadOnly
        private static readonly int[] DirectionRow = { -1, 0, 1, 0 };

        private static readonly int[] DirectionColumn = { 0, 1, 0, -1 };

        private static readonly Random random = new Random();

        // fields
        private static int emptyCellRow;

        private static int emptyCellColumn;

        private static string[,] matrix;

        private static int turn;

        // Methods
        private static void InitializeMatrix()
        {
            matrix = new string[MatrixSizeRows, MatrixSizeColumns];

            int cellValue = 1;

            for (int row = 0; row < MatrixSizeRows; row++)
            {
                for (int column = 0; column < MatrixSizeColumns; column++)
                {
                    matrix[row, column] = cellValue.ToString();
                    cellValue++;
                }
            }

            emptyCellRow = MatrixSizeRows - 1;
            emptyCellColumn = MatrixSizeColumns - 1;
            matrix[emptyCellRow, emptyCellColumn] = EmptyCellValue;
        }

        
        private static bool TryMakeMove(int cellNumber)
        {
            int direction = GetDirectionFromInputCell(cellNumber);
            if (direction == -1)
            {
                ConsoleManager.PrintIllegalMoveMessage();
            }
            else
            {
                MoveCell(direction);
            }
            
            return true;
        }
        
        private static int GetDirectionFromInputCell(int cellNumber)
        {
            int direction = -1;
            
            for (int dir = 0; dir < DirectionRow.Length; dir++)
            {
                bool isDirValid = CheckIfCellIsValid(dir);
                if (isDirValid)
                {
                    int nextCellRow = emptyCellRow + DirectionRow[dir];
                    int nextCellColumn = emptyCellColumn + DirectionColumn[dir];
                    
                    if (matrix[nextCellRow, nextCellColumn] == cellNumber.ToString())
                    {
                        direction = dir;
                        break;
                    }
                }
            }
            
            return direction;
        }
        
        private static bool IsCellValid(int cellNumber)
        {
            int matrixSize = MatrixSizeRows * MatrixSizeColumns;
            if (cellNumber <= 0 || cellNumber >= matrixSize)
            {
                ConsoleManager.PrintCellDoesNotExistMessage();
            }
            return true;
        }
        
        private static string ReadUserInput()
        {
            ConsoleManager.PrintMessage("Enter a number to move: ");
            string consoleInputLine = Console.ReadLine();
            return consoleInputLine;
        }
        
        
        public static void GameStart()
        {
            while (true)
            {
                InitializeMatrix();
                ShuffleMatrix();
                turn = 0;
                ConsoleManager.PrintWelcomeMessage();
                ConsoleManager.PrintMatrix(matrix, MatrixSizeRows, MatrixSizeColumns);
                while (true)
                {
                    ConsoleManager.PrintNextMoveMessage();
                    string userInput = ReadUserInput();
                    int cellNumber = 0;
                    bool needsRestart = false;
                    Command userAction;
                    if (int.TryParse(userInput, out cellNumber))
                    {
                        userAction = Command.MoveCel;
                    }
                    else if (userInput == "restart")
                    {
                        userAction = Command.Restart;
                    }
                    else if (userInput == "top")
                    {
                        userAction = Command.Top;
                    }
                    else if (userInput == "exit")
                    {
                        userAction = Command.Exit;
                    }
                    else
                    {
                        userAction = Command.Illegal;
                    }
                    switch (userAction)
                    {
                        case Command.MoveCel:
                            MoveCellByPlayer(cellNumber);
                            break;
                        case Command.Top:
                            ConsoleManager.PrintTopScores();
                            break;
                        case Command.Exit:
                            ConsoleManager.PrintMessage("Good bye!\n");
                            return;
                        case Command.Illegal:
                            ConsoleManager.PrintMessage("Illegal command!\n");
                            break;
                        case Command.Restart:
                            needsRestart = true;
                            break;
                    }
                    if (needsRestart)
                    {
                        break;
                    }
                }
            }
        }
  
        private static void MoveCellByPlayer(int cellNumber)
        {
            if (IsCellValid(cellNumber) == false)
            {
                ConsoleManager.PrintMessage("That cell does not exist in the matrix.");
            }
            if (TryMakeMove(cellNumber))
            {
                ConsoleManager.PrintMatrix(matrix, MatrixSizeRows, MatrixSizeColumns);
            }
            else
            {
                ConsoleManager.PrintMessage("Illegal move!");
            }
            if (CheckIfLevelFinished())
            {
                PerformEndingOperations();               
            }
        }
        
        public static void PerformEndingOperations()
        {
            string moves = turn == 1 ? "1 move" : string.Format("{0} moves", turn);
            ConsoleManager.PrintCongratulation(moves);
            string[] topScores = FileHandling.GetTopScoresFromFile();
            if (topScores[TopScoresAmount - 1] != null)
            {
                string lowestScore = Regex.Replace(topScores[TopScoresAmount - 1], TopScoresPersonPattern, @"$2");
                if (int.Parse(lowestScore) < turn)
                {
                    ConsoleManager.PrintScore(TopScoresAmount);
                    return;
                }
            }

            Score score = new Score();
            score.UpgradeTopScore(turn);
        }
        
        private static void ShuffleMatrix()
        {
            int matrixSize = MatrixSizeRows * MatrixSizeColumns;
            int shuffles = random.Next(matrixSize, matrixSize * 100);
            for (int i = 0; i < shuffles; i++)
            {
                int direction = random.Next(DirectionRow.Length);
                if (CheckIfCellIsValid(direction))
                {
                    MoveCell(direction);
                }
            }
            if (CheckIfLevelFinished())
            {
                ShuffleMatrix();
            }
        }
        
        private static bool CheckIfCellIsValid(int direction)
        {
            int nextCellRow = emptyCellRow + DirectionRow[direction];
            
            bool isRowValid = nextCellRow >= 0 && nextCellRow < MatrixSizeRows;
            
            int nextCellColumn = emptyCellColumn + DirectionColumn[direction];
            
            bool isColumnValid = nextCellColumn >= 0 && nextCellColumn < MatrixSizeColumns;
            
            bool isCellValid = isRowValid && isColumnValid;
            
            return isCellValid;
        }
        
        private static bool CheckIfLevelFinished()
        {
            bool isEmptyCellInPlace = emptyCellRow == MatrixSizeRows - 1 &&
                                      emptyCellColumn == MatrixSizeColumns - 1;
            if (!isEmptyCellInPlace)
            {
                return false;
            }
            
            int cellValue = 1;
            
            int matrixSize = MatrixSizeRows * MatrixSizeColumns;
            
            for (int row = 0; row < MatrixSizeRows; row++)
            {
                for (int column = 0; column < MatrixSizeColumns && cellValue < matrixSize; column++)
                {
                    if (matrix[row, column] != cellValue.ToString())
                    {
                        return false;
                    }
                    
                    cellValue++;
                }
            }
            return true;
        }
        
        private static void MoveCell(int direction)
        {
            int nextCellRow = emptyCellRow + DirectionRow[direction];
            int nextCellColumn = emptyCellColumn + DirectionColumn[direction];
            matrix[emptyCellRow, emptyCellColumn] = matrix[nextCellRow, nextCellColumn];
            matrix[nextCellRow, nextCellColumn] = EmptyCellValue;
            emptyCellRow = nextCellRow;
            emptyCellColumn = nextCellColumn;
            turn++;
        }
    }
}