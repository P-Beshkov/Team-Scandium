//-----------------------------------------------------------------------
// <copyright file="Player.cs" company="TelerikAcademy">
//     All rights reserved © Telerik Academy 2012-2013
// </copyright>
//----------------------------------------------------------------------
namespace Game_Fifteen
{
    using System;

    /// <summary>
    /// name and score of each player
    /// </summary>
    public class Player
    {
        public Player(string name, int score)
        {
            if (score < 0)
            {
                throw new ArgumentException("Playes score can not be negative!");
            }
            this.Name = name;
            this.Score = score;
        }

        public string Name { get; private set; }

        public int Score { get; private set; }
    }
}
