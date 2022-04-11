using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hangman
{
	public class GuessedLetter
	{
		public string Letter { get; set; }

		public GuessedLetter(string letter)
		{
			Letter = letter;
		}
	}
}
