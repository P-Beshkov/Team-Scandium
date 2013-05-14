using Game_Fifteen;
using System;
using System.Text;

internal class GameField
{
    private const string EmptyCellValue = " ";

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

    public int FieldRows
    {
        get
        {
            return matrix.GetLength(0);
        }
    }

    public int FieldColumns
    {
        get
        {
            return matrix.GetLength(1);
        }
    }

    public GameField(int size = 0)
    {
        this.InitializeMatrix(size);
        this.ShuffleMatrix();
    }

    private void InitializeMatrix(int size)
    {
        matrix = new string[size, size];

        int cellValue = 1;

        for (int row = 0; row < FieldRows; row++)
        {
            for (int column = 0; column < FieldColumns; column++)
            {
                matrix[row, column] = cellValue.ToString();
                cellValue++;
            }
        }

        emptyCellRow = FieldRows - 1;
        emptyCellColumn = FieldColumns - 1;
        matrix[emptyCellRow, emptyCellColumn] = EmptyCellValue;
    }

    private void ShuffleMatrix()
    {
        int matrixSize = FieldRows * FieldColumns;
        int shuffles = random.Next(matrixSize, matrixSize * 100);
        for (int i = 0; i < shuffles; i++)
        {
            int direction = random.Next(DirectionRow.Length);
            if (CheckIfCellIsValid(direction))
            {
                MoveCell(direction);
            }
        }
        if (IsMazeOrdered())
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

    private void MoveCell(int direction)
    {
        int nextCellRow = emptyCellRow + DirectionRow[direction];
        int nextCellColumn = emptyCellColumn + DirectionColumn[direction];
        matrix[emptyCellRow, emptyCellColumn] = matrix[nextCellRow, nextCellColumn];
        matrix[nextCellRow, nextCellColumn] = EmptyCellValue;
        emptyCellRow = nextCellRow;
        emptyCellColumn = nextCellColumn;
    }

    private bool CheckIfCellIsValid(int direction)
    {
        int nextCellRow = emptyCellRow + DirectionRow[direction];
        bool isRowValid = (nextCellRow >= 0 && nextCellRow < FieldRows);
        int nextCellColumn = emptyCellColumn + DirectionColumn[direction];
        bool isColumnValid = (nextCellColumn >= 0 && nextCellColumn < FieldColumns);
        bool isCellValid = isRowValid && isColumnValid;

        return isCellValid;
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
        int matrixSize = FieldRows * FieldColumns;
        if (cellNumber <= 0 || cellNumber >= matrixSize)
        {
            return false;
        }
        return true;
    }

    public void MoveCellByPlayer(int cellNumber)
    {
        if (IsCellValid(cellNumber) == false)
        {
            throw new ArgumentOutOfRangeException("That cell does not exist in the matrix.");
        }
        if (TryMakeMove(cellNumber) == false)
        {
            throw new ArgumentOutOfRangeException("Illegal move!");
        }
    }

    public override string ToString()
    {
        StringBuilder horizontalBorder = new StringBuilder("  ");
        for (int i = 0; i < matrix.GetLength(1); i++)
        {
            horizontalBorder.Append("---");
        }
        horizontalBorder.Append("- \n");

        StringBuilder buffer = new StringBuilder();
        buffer.Append(horizontalBorder);
        for (int row = 0; row < matrix.GetLength(0); row++)
        {
            buffer.Append(" |");
            for (int column = 0; column < matrix.GetLength(1); column++)
            {
                buffer.Append(String.Format("{0,3}", matrix[row, column]));
            }
            buffer.Append(" |\n");
        }
        buffer.Append(horizontalBorder);
        return buffer.ToString();
    }

    public bool IsMazeOrdered()
    {
        bool isEmptyCellInPlace = emptyCellRow == FieldRows - 1 &&
                                  emptyCellColumn == FieldColumns - 1;
        if (!isEmptyCellInPlace)
        {
            return false;
        }

        int cellValue = 1;

        int matrixSize = FieldRows * FieldColumns;

        for (int row = 0; row < FieldRows; row++)
        {
            for (int column = 0; column < FieldColumns && cellValue < matrixSize; column++)
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
}