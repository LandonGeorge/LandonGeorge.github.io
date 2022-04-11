using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hangman
{
    public class SessionData
    {
        public string Token { get; set; }
        public string Expiration { get; set; }

        public SessionData() {}

        public SessionData(string token, string expiration)
        {
            Token = token;
            Expiration = expiration;
        }
    }
}
