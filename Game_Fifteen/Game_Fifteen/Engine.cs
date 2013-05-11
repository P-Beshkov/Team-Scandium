using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Game_Fifteen
{
    class Engine
    {
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

        // 
        private static int emptyCellRow;

        private static int emptyCellColumn;

        private static string[,] matrix;

        private static Random random = new Random();

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
                    string consoleInputLine = Console.ReadLine();
                    int cellNumber;
                    if (int.TryParse(consoleInputLine, out cellNumber))
                    {
                        //Input is a cell number.
                        NextMove(cellNumber);
                        if (CheckIfEmptyCellIsInPosition())
                        {
                            GameEnd();
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

        public static void GameEnd()
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
                if (CheckIfCellIsInMatrix(direction))
                {
                    MoveCell(direction);
                }
            }
            if (CheckIfEmptyCellIsInPosition())
            {
                ShuffleMatrix();
            }
        }

        private static bool CheckIfCellIsInMatrix(int direction)
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
                bool isDirValid = CheckIfCellIsInMatrix(dir);

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
