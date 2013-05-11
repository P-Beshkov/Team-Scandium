using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Game_Fifteen
{
    class Engine
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

        private const string TopScoresFileName = "Top.txt";

        private const string TopScoresPersonPattern = @"^\d+\. (.+) --> (\d+) moves?$";

        private const int TopScoresAmount = 5;

        // ReadOnly
        private static readonly int[] DirectionRow = { -1, 0, 1, 0 };

        private static readonly int[] DirectionColumn = { 0, 1, 0, -1 };

        private static readonly Random random = new Random();

        // 
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
        #region Pavel Methods
        private static bool TryMakeMove(int cellNumber)
        {
            int direction = GetDirectionFromInputCell(cellNumber);
            if (direction == -1)
            {
                return false;
            }
            MoveCell(direction);
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
                return false;
            }
            return true;
        }
        private static string ReadUserInput()
        {
            PrintingOnConsole.PrintMessage("Enter a number to move: ");
            string consoleInputLine = Console.ReadLine();
            return consoleInputLine;
        }
        #endregion
        
        public static void GameStart()
        {
            while (true)
            {
                InitializeMatrix();
                ShuffleMatrix();
                turn = 0;
                PrintingOnConsole.PrintWelcomeMessage();
                PrintingOnConsole.PrintMatrix(matrix, MatrixSizeRows, MatrixSizeColumns);
                while (true)
                {
                    PrintingOnConsole.PrintNextMoveMessage();
                    string userInput = ReadUserInput();
                    int cellNumber = 0;
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
                            if (IsCellValid(cellNumber) == false)
                            {
                                PrintingOnConsole.PrintMessage("That cell does not exist in the matrix.");
                            }
                            if (TryMakeMove(cellNumber))
                            {
                                PrintingOnConsole.PrintMatrix(matrix, MatrixSizeRows, MatrixSizeColumns);
                            }
                            else
                            {
                                PrintingOnConsole.PrintMessage("Illegal move!");
                            }
                            if (CheckIfEmptyCellIsInPosition())
                            {
                                PerformEndingOperations();
                                break;
                            }
                            break;
                        case Command.Top:
                            PrintingOnConsole.PrintTopScores();
                            break;
                        case Command.Exit:
                            PrintingOnConsole.PrintMessage("Good bye!\n");
                            break;
                        case Command.Illegal:
                            PrintingOnConsole.PrintMessage("Illegal command!\n");
                            break;
                        case Command.Restart:
                        //break;
                        default:
                            break;
                    }


                    string consoleInputLine = Console.ReadLine();                    
                    if (int.TryParse(consoleInputLine, out cellNumber))
                    {
                        //Input is a cell number.
                        NextMove(cellNumber);
                        if (CheckIfEmptyCellIsInPosition())
                        {
                            PerformEndingOperations();
                            break;
                        }
                    }
                    else
                    {
                        //Input is a command.
                        if (consoleInputLine == "restart")
                        {
                            break;
                        }
                        switch (consoleInputLine)
                        {
                            case "top":
                                PrintingOnConsole.PrintTopScores();
                                break;
                            case "exit":
                                PrintingOnConsole.PrintGoodbye();
                                return;
                            default:
                                PrintingOnConsole.PrintIllegalCommandMessage();
                                break;
                        }
                    }
                }
            }
        }

        public static void PerformEndingOperations()
        {
            string moves = turn == 1 ? "1 move" : string.Format("{0} moves", turn);
            PrintingOnConsole.PrintCongratulation(moves);
            string[] topScores = FileHandling.GetTopScoresFromFile();
            if (topScores[TopScoresAmount - 1] != null)
            {
                string lowestScore = Regex.Replace(topScores[TopScoresAmount - 1], TopScoresPersonPattern, @"$2");
                if (int.Parse(lowestScore) < turn)
                {
                    PrintingOnConsole.PrintScore(TopScoresAmount);
                    return;
                }
            }
            Score.UpgradeTopScore(turn);
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
            if (CheckIfEmptyCellIsInPosition())
            {
                ShuffleMatrix();
            }
        }

        private static bool CheckIfCellIsValid(int direction)
        {

            int nextCellRow = emptyCellRow + DirectionRow[direction];

            bool isRowValid = (nextCellRow >= 0 && nextCellRow < MatrixSizeRows);

            int nextCellColumn = emptyCellColumn + DirectionColumn[direction];

            bool isColumnValid = (nextCellColumn >= 0 && nextCellColumn < MatrixSizeColumns);

            bool isCellValid = isRowValid && isColumnValid;

            return isCellValid;
        }

        private static bool CheckIfEmptyCellIsInPosition()
        {
            bool isEmptyCellInPlace =
                emptyCellRow == MatrixSizeRows - 1 &&
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

        private static void NextMove(int cellNumber)
        {
            int matrixSize = MatrixSizeRows * MatrixSizeColumns;
            if (cellNumber <= 0 || cellNumber >= matrixSize)
            {
                PrintingOnConsole.PrintCellDoesNotExistMessage();
                return;
            }

            int direction = CellNumberToDirection(cellNumber);
            if (direction == -1)
            {
                PrintingOnConsole.PrintIllegalMoveMessage();
                return;
            }

            MoveCell(direction);
            PrintingOnConsole.PrintMatrix(matrix, MatrixSizeColumns, MatrixSizeColumns);
        }

        private static int CellNumberToDirection(int cellNumber)
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
    }
}
