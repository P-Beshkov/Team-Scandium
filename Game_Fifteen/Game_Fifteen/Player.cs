using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Fifteen
{
    /// <summary>
    /// name and score of each player
    /// </summary>
    class Player
    {
        private string name;
        private int score;

        public Player(string name, int score)
        {
            this.name = name;

            this.score = score;        
        }

        public string Name        
        {        
            get            
            {            
                return name;
            }            
            set            
            {            
                name = value;
            }
        }
        
        public int Score        
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
            }
        }
    }
}
