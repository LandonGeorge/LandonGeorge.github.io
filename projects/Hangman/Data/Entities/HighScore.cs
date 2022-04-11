using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hangman.Data.Entities
{
    public class HighScore
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public DateTime DateTime { get; set; }
        public string Word { get; set; }
        public User User { get; set; }
    }
}
