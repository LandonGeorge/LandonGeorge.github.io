using System;

namespace hangman
{
    public class HighScoreDto
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public DateTime DateTime { get; set; }
        public string Username { get; set; }
        public string Word { get; set; }
    }
}
