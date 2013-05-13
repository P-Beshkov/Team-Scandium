using Game_Fifteen;
using System;
class GameField
{
    private const string EmptyCellValue = " ";
    private const int MatrixSizeRows = 4;
    private const int MatrixSizeColumns = 4;

    private static readonly int[] DirectionRow = { -1, 0, 1, 0 };
    private static readonly int[] DirectionColumn = { 0, 1, 0, -1 };
    private static readonly Random random = new Random();

    private static int emptyCellRow;
    private static int emptyCellColumn;

    private string[,] matrix;

    #region Singleton Design Pattern
    //private static readonly GameField instance = new GameField();

    //public static GameField GetGameFieldInstance()
    //{
    //    return instance;
    //}

    //private  GameField()
    //{
    //    this.InitializeMatrix();
    //    this.ShuffleMatrix();
    //}
    #endregion

    public GameField()
    {        
        this.InitializeMatrix();
        this.ShuffleMatrix();
    }
    private void InitializeMatrix()
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

    private void ShuffleMatrix()
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

    private bool TryMakeMove(int cellNumber)
    {
        int direction = GetDirectionFromInputCell(cellNumber);
        if (direction == -1)
        {
            return false;
        }
        MoveCell(direction);
        return true;
    }

    private int GetDirectionFromInputCell(int cellNumber)
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

    private bool IsCellValid(int cellNumber)
    {
        int matrixSize = MatrixSizeRows * MatrixSizeColumns;
        if (cellNumber <= 0 || cellNumber >= matrixSize)
        {
            return false;
        }
        return true;
    }

    private void MoveCellByPlayer(int cellNumber)
    {
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
        if (CheckIfLevelFinished())
        {
            PerformEndingOperations();
        }
    }

    

    private bool CheckIfCellIsValid(int direction)
    {
        int nextCellRow = emptyCellRow + DirectionRow[direction];

        bool isRowValid = (nextCellRow >= 0 && nextCellRow < MatrixSizeRows);

        int nextCellColumn = emptyCellColumn + DirectionColumn[direction];

        bool isColumnValid = (nextCellColumn >= 0 && nextCellColumn < MatrixSizeColumns);

        bool isCellValid = isRowValid && isColumnValid;

        return isCellValid;
    }

    private bool CheckIfLevelFinished()
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

    private void MoveCell(int direction)
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