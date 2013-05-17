//-----------------------------------------------------------------------
// <copyright file="GameField.cs" company="TelerikAcademy">
//     All rights reserved © Telerik Academy 2012-2013
// </copyright>
//----------------------------------------------------------------------
namespace Game_Fifteen
{
    using System;

    /// <summary>
    /// Generates game field.
    /// </summary>
    public class GameField
    {
        private const string EmptyCellValue = " ";
        private const int GameBoardSize = 4;
        private const int MatrixSize = GameBoardSize * GameBoardSize;

        private static readonly int[] DirectionRow = { -1, 0, 1, 0 };
        private static readonly int[] DirectionColumn = { 0, 1, 0, -1 };
        private static readonly Random Random = new Random();

        private static int emptyCellRow;
        private static int emptyCellColumn;

        private int moves;
        private string[,] matrix;

        public GameField()
        {
            this.InitializeMatrix();
            this.ShuffleMatrix();
            this.Moves = 0;
        }

        public string[,] GetMatrix
        {
            get { return this.matrix; }
        }

        public int Moves
        {
            get { return this.moves; }

            set { this.moves = value; }
        }

        // Public methods
        /// <summary>
        /// Checks if the given cell can be moved and updates the matrix after the move.
        /// </summary>
        /// <param name="cellNumber">Cell to move.</param>
        public void MoveCellByPlayer(int cellNumber)
        {
            if (this.IsCellValid(cellNumber) == false)
            {
                ConsoleManager.PrintCellDoesNotExistMessage();
            }

            if (this.TryMakeMove(cellNumber) == false)
            {
                ConsoleManager.PrintIllegalMoveMessage();
            }

            ConsoleManager.PrintMessage(this.matrix, GameBoardSize);
        }

        /// <summary>
        /// Checks if matrix is ordered.
        /// </summary>
        /// <returns>True if successfully, else returns false</returns>
        public bool IsMatrixOrdered()
        {
            bool isEmptyCellInPlace = emptyCellRow == GameBoardSize - 1 &&
                                      emptyCellColumn == GameBoardSize - 1;
            if (!isEmptyCellInPlace)
            {
                return false;
            }

            int cellValue = 1;

            for (int row = 0; row < GameBoardSize; row++)
            {
                for (int column = 0; column < GameBoardSize && cellValue < MatrixSize; column++)
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

        // Private methods
        public void InitializeMatrix()
        {
            this.matrix = new string[GameBoardSize, GameBoardSize];

            int cellValue = 1;

            for (int row = 0; row < GameBoardSize; row++)
            {
                for (int column = 0; column < GameBoardSize; column++)
                {
                    this.matrix[row, column] = cellValue.ToString();
                    cellValue++;
                }
            }

            emptyCellRow = GameBoardSize - 1;
            emptyCellColumn = GameBoardSize - 1;
            this.matrix[emptyCellRow, emptyCellColumn] = EmptyCellValue;
        }

        // TODO change shuffle 
        private void ShuffleMatrix()
        {
            int shuffles = Random.Next(MatrixSize, MatrixSize * 100);
            for (int i = 0; i < 2; i++)
            {
                int direction = Random.Next(DirectionRow.Length);
                if (this.CheckIfCellIsValid(direction))
                {
                    this.MoveCell(direction);
                }
            }

            if (this.IsMatrixOrdered())
            {
                this.ShuffleMatrix();
            }
        }

        /// <summary>
        /// Tries to move given cell.
        /// </summary>
        /// <param name="cellNumber">Cell to move.</param>
        /// <returns>True if successfully, else returns false. </returns>
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

        /// <summary>
        /// Makes move in the given direction.
        /// </summary>
        /// <param name="direction">Direction to move.</param>
        private void MoveCell(int direction)
        {
            int nextCellRow = emptyCellRow + DirectionRow[direction];
            int nextCellColumn = emptyCellColumn + DirectionColumn[direction];
            this.matrix[emptyCellRow, emptyCellColumn] = this.matrix[nextCellRow, nextCellColumn];
            this.matrix[nextCellRow, nextCellColumn] = EmptyCellValue;
            emptyCellRow = nextCellRow;
            emptyCellColumn = nextCellColumn;
            this.Moves++;
        }

        /// <summary>
        /// Checks if cell is in the matrix.
        /// </summary>
        /// <returns>True if successfully, else returns false.</returns>
        private bool CheckIfCellIsValid(int direction)
        {
            int nextCellRow = emptyCellRow + DirectionRow[direction];
            bool isRowValid = nextCellRow >= 0 && nextCellRow < GameBoardSize;
            int nextCellColumn = emptyCellColumn + DirectionColumn[direction];
            bool isColumnValid = nextCellColumn >= 0 && (nextCellColumn < GameBoardSize);
            bool isCellValid = isRowValid && isColumnValid;

            return isCellValid;
        }

        /// <summary>
        /// Checks given cell for valid move direction.
        /// </summary>
        /// <param name="cellNumber">The cell to check.</param>
        /// <returns>Valid move direction.</returns>
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
        /// <summary>
        /// Checks if player's input cell is in the matrix bounds.
        /// </summary>
        /// <param name="cellNumber">Number of cell.</param>
        /// <returns></returns>
        private bool IsCellValid(int cellNumber)
        {
            if (cellNumber <= 0 || cellNumber >= MatrixSize)
            {
                return false;
            }

            return true;
        }
    }
}