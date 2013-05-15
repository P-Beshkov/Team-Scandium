using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Game_Fifteen;

namespace TestGame_Fifteen
{

    [TestClass]
    public class TestPlayer
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestPlayerWithNegativScore()
        {
            Player player = new Player("Pesho", -12);
        }

        [TestMethod]
        public void CheckPlayerName()
        {
            Player player = new Player("Pesho", 10);
            Assert.AreEqual(player.Name, "Pesho");
        }

        [TestMethod]
        public void CreatePlayerCheckMoves()
        {
            Player player = new Player("Ivan", 8);
            Assert.AreEqual(player.Score, 8);
        }
    }
}
