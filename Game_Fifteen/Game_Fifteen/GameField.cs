//-----------------------------------------------------------------------
// <copyright file="GameField.cs" company="TelerikAcademy">
//     All rights reserved © Telerik Academy 2012-2013
// </copyright>
//----------------------------------------------------------------------
namespace Game_Fifteen
{
    using System;
    using System.Text;

    public class GameField
    {
        private const string EmptyCellValue = " ";

        // private const int MatrixSizeRows = 4;
        // private const int MatrixSizeColumns = 4;  
        private const int GAME_BOARD_SIZE = 4;

        private static readonly int[] DirectionRow = { -1, 0, 1, 0 };
        private static readonly int[] DirectionColumn = { 0, 1, 0, -1 };
        private static readonly Random random = new Random();

        private static int emptyCellRow;
        private static int emptyCellColumn;

        private int moves;
        private string[,] matrix;

        // Singleton Design Pattern
        //private static readonly GameField instance = new GameField();

        //public static GameField GetGameFieldInstance()
        //{
        //    return instance;
        //}

        public GameField()
        {
            this.InitializeMatrix();
            this.ShuffleMatrix();
            this.Turns = 0;
        }

        // for deletion
        public int FieldRows
        {
            get { return this.matrix.GetLength(0); }
        }

        public int FieldColumns
        {
            get { return this.matrix.GetLength(1); }
        }

        public string[,] GetMatrix
        {
            get { return this.matrix; }
        }

        public int Turns
        {
            get { return this.moves; }

            set { this.moves = value; }
        }

        public void MoveCellByPlayer(int cellNumber)
        {
            if (this.IsCellValid(cellNumber) == false)
            {
                ConsoleManager.PrintCellDoesNotExistMessage();
            }

            if (this.TryMakeMove(cellNumber) == false)
            {
                ConsoleManager.PrintIllegalCommandMessage();
            }

            ConsoleManager.PrintMatrix(this.matrix, GAME_BOARD_SIZE);
        }

        public override string ToString()
        {
            StringBuilder horizontalBorder = new StringBuilder("  ");
            for (int i = 0; i < this.matrix.GetLength(1); i++)
            {
                horizontalBorder.Append("---");
            }

            horizontalBorder.Append("- \n");

            StringBuilder buffer = new StringBuilder();
            buffer.Append(horizontalBorder);
            for (int row = 0; row < this.matrix.GetLength(0); row++)
            {
                buffer.Append(" |");
                for (int column = 0; column < this.matrix.GetLength(1); column++)
                {
                    buffer.Append(string.Format("{0,3}", this.matrix[row, column]));
                }

                buffer.Append(" |\n");
            }

            buffer.Append(horizontalBorder);
            return buffer.ToString();
        }

        public bool IsMazeOrdered()
        {
            bool isEmptyCellInPlace = emptyCellRow == this.FieldRows - 1 &&
                                      emptyCellColumn == this.FieldColumns - 1;
            if (!isEmptyCellInPlace)
            {
                return false;
            }

            int cellValue = 1;

            int matrixSize = this.FieldRows * this.FieldColumns;

            for (int row = 0; row < this.FieldRows; row++)
            {
                for (int column = 0; column < this.FieldColumns && cellValue < matrixSize; column++)
                {
                    if (this.matrix[row, column] != cellValue.ToString())
                    {
                        return false;
                    }

                    cellValue++;
                }
            }

            return true;
        }

        private void InitializeMatrix()
        {
            this.matrix = new string[GAME_BOARD_SIZE, GAME_BOARD_SIZE];

            int cellValue = 1;

            for (int row = 0; row < this.FieldRows; row++)
            {
                for (int column = 0; column < this.FieldColumns; column++)
                {
                    this.matrix[row, column] = cellValue.ToString();
                    cellValue++;
                }
            }

            emptyCellRow = this.FieldRows - 1;
            emptyCellColumn = this.FieldColumns - 1;
            this.matrix[emptyCellRow, emptyCellColumn] = EmptyCellValue;
        }

        // TODO change shuffle 
        private void ShuffleMatrix() // testing
        {
            int matrixSize = this.FieldRows * this.FieldColumns;
            int shuffles = random.Next(matrixSize, matrixSize * 100);
            for (int i = 0; i < 2; i++)
            {
                int direction = random.Next(DirectionRow.Length);
                if (this.CheckIfCellIsValid(direction))
                {
                    this.MoveCell(direction);
                }
            }

            if (this.IsMazeOrdered())
            {
                this.ShuffleMatrix();
            }
        }

        private bool TryMakeMove(int cellNumber)
        {
            int direction = this.GetDirectionFromInputCell(cellNumber);
            if (direction == -1)
            {
                return false;
            }

            this.MoveCell(direction);
            return true;
        }

        private void MoveCell(int direction)
        {
            int nextCellRow = emptyCellRow + DirectionRow[direction];
            int nextCellColumn = emptyCellColumn + DirectionColumn[direction];
            this.matrix[emptyCellRow, emptyCellColumn] = this.matrix[nextCellRow, nextCellColumn];
            this.matrix[nextCellRow, nextCellColumn] = EmptyCellValue;
            emptyCellRow = nextCellRow;
            emptyCellColumn = nextCellColumn;
            this.Turns++;
        }

        private bool CheckIfCellIsValid(int direction)
        {
            int nextCellRow = emptyCellRow + DirectionRow[direction];
            bool isRowValid = nextCellRow >= 0 && nextCellRow < this.FieldRows;
            int nextCellColumn = emptyCellColumn + DirectionColumn[direction];
            bool isColumnValid = nextCellColumn >= 0 && (nextCellColumn < this.FieldColumns);
            bool isCellValid = isRowValid && isColumnValid;

            return isCellValid;
        }

        private int GetDirectionFromInputCell(int cellNumber)
        {
            int direction = -1;

            for (int dir = 0; dir < DirectionRow.Length; dir++)
            {
                bool isDirValid = this.CheckIfCellIsValid(dir);
                if (isDirValid)
                {
                    int nextCellRow = emptyCellRow + DirectionRow[dir];
                    int nextCellColumn = emptyCellColumn + DirectionColumn[dir];

                    if (this.matrix[nextCellRow, nextCellColumn] == cellNumber.ToString())
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
            int matrixSize = this.FieldRows * this.FieldColumns;
            if (cellNumber <= 0 || cellNumber >= matrixSize)
            {
                return false;
            }

            return true;
        }
    }
}