//-----------------------------------------------------------------------
// <copyright file="PlayerTest.cs" company="TelerikAcademy">
//     All rights reserved © Telerik Academy 2012-2013
// </copyright>
//----------------------------------------------------------------------
namespace TestGame_Fifteen
{
    using System;
    using Game_Fifteen;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PlayerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestPlayerWithNegativScore()
        {
            Player player = new Player("Pesho", -12);
        }

        [TestMethod]
        public void TestCheckPlayerName()
        {
            Player player = new Player("Pesho", 10);
            Assert.AreEqual(player.Name, "Pesho");
        }

        [TestMethod]
        public void TestCheckPlayerMoves()
        {
            Player player = new Player("Ivan", 8);
            Assert.AreEqual(player.Score, 8);
        }
    }
}
