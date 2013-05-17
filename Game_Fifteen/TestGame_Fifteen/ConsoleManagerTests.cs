//-----------------------------------------------------------------------
// <copyright file="ConsoleManagerTests.cs" company="TelerikAcademy">
//     All rights reserved © Telerik Academy 2012-2013
// </copyright>
//----------------------------------------------------------------------
namespace TestGame_Fifteen
{
    using System;
    using Game_Fifteen;
    using Microsoft.VisualStudio.TestTools.UnitTesting;    

    [TestClass]
    public class ConsoleManagerTests
    {
        [TestMethod]
        public void TestReadMatrix()
        {
            GameField field = new GameField();
            field.InitializeMatrix();
            string[,] actualMatrix = field.GetMatrix;
            var actual = ConsoleManager.PrintMatrix(actualMatrix, 4);

            string expected = 
                "  ------------- " +
                Environment.NewLine +
                " |  1  2  3  4 |" +
                Environment.NewLine +
                " |  5  6  7  8 |" +
                Environment.NewLine +
                " |  9 10 11 12 |" +
                Environment.NewLine +
                " | 13 14 15    |" +
                Environment.NewLine +
                "  ------------- " +
                Environment.NewLine;

            Assert.AreEqual(expected, actual);           
        }
    }
}
