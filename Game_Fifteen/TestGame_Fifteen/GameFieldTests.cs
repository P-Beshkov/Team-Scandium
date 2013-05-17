//-----------------------------------------------------------------------
// <copyright file="GameFieldTests.cs" company="TelerikAcademy">
//     All rights reserved © Telerik Academy 2012-2013
// </copyright>
//----------------------------------------------------------------------
namespace TestGame_Fifteen
{
    using System;
    using Game_Fifteen;
    using Microsoft.VisualStudio.TestTools.UnitTesting;    

    [TestClass]
    public class GameFieldTests
    {
        [TestMethod]
        public void TestGameFieldMazeOrdered()
        {
            GameField field = new GameField();
            var isOrdered = field.IsMazeOrdered();
            Assert.IsFalse(isOrdered);
        }

        [TestMethod]
        public void TestGameFieldMatrixWidth()
        {
            GameField field = new GameField();
            string[,] actual = field.GetMatrix;
            var width = actual.GetLength(0);

            Assert.AreEqual(width, 4);
        }

        [TestMethod]
        public void TestGameFieldMatrixHeight()
        {
            GameField field = new GameField();
            string[,] actual = field.GetMatrix;
            var height = actual.GetLength(1);

            Assert.AreEqual(height, 4);
        }

        [TestMethod]
        public void TestGameFieldMoves()
        {
            GameField field = new GameField();

            var actual = field.Moves;
            var expected = 0;

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestGenerateOrderedMatrix()
        {
            var size = 4;
            string[,] expectedMatrix = new string[size, size];
            var counter = 1;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    expectedMatrix[i, j] = counter.ToString();
                    counter++;
                }
            }

            expectedMatrix[3, 3] = string.Empty;

            GameField field = new GameField();
            field.InitializeMatrix();
            var actualMatrix = field.GetMatrix;

            Assert.AreEqual(string.Join(",", expectedMatrix), string.Join(",", actualMatrix));
        }
    }
}
