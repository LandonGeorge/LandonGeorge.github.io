using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hangman
{
	public class GameState
	{
		public bool State { get; set; }

		public GameState(bool state)
		{
			State = state;
		}
	}
}
