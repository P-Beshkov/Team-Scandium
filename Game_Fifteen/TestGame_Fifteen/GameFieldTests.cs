//-----------------------------------------------------------------------
// <copyright file="TestGameField.cs" company="TelerikAcademy">
//     All rights reserved © Telerik Academy 2012-2013
// </copyright>
//----------------------------------------------------------------------
namespace TestGame_Fifteen
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Game_Fifteen;

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
    }
}
