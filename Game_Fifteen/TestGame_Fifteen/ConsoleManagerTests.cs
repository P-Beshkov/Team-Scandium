namespace TestGame_Fifteen
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Game_Fifteen;

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
