using System;
using System.Linq;

namespace Game_Fifteen
{
    /// <summary>
    /// name and score of each player
    /// </summary>
    public class Player
    {
        private string name;
        private int score;

        public Player(string name, int score)
        {
            if (score < 0)
            {
                throw new ArgumentException("Playes score can not be negative!");
            }
            this.name = name;
            this.score = score;        
        }

        public string Name        
        {        
            get            
            {            
                return this.name;
            }            
            private set            
            {
                this.name = value;
            }
        }
        
        public int Score        
        {
            get
            {
                return this.score;
            }
            set
            {
                this.score = value;
            }
        }
    }
}
