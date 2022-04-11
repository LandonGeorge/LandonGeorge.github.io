using hangman.Blls;
using hangman.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace hangman.Controllers
{
	public class HangmanController : Controller
	{
		private readonly HangmanBll _bll;

		public HangmanController(HangmanBll bll)
		{
			_bll = bll;
		}

		public IActionResult Index()
		{
			return View();
		}

		// with a post request we're just going to send a generic result back to the front end, basically just says
		// that the post request was successful
		[HttpPost]
		public JsonResult PostDataToDBExample()
		{
			var result = new GenericResult();

			// call method here to post data, for example maybe call the BLL function to create an account

			return new JsonResult(result);
		}

		[HttpGet]
		public JsonResult GetDataFromDBExample(string username, string password)
		{
			// here's an example of user validation. REMEMBER: the controller is not used for logic, it's like a bridge
			//    between your View and your Model: VIEW -> CONTROLLER -> MODEL
			// this is why we call the bll, and then the function from the bll and pass in any necessary params
			var result = "call your function from the HangmanBllHere, for example:" +
						 "HangmanBll.ValidateUserLogin(username, password)";

			return new JsonResult(result);
		}

		[HttpPost]
		public JsonResult SignUpForAccount([FromBody] Login login)
		{
			bool userAdded = _bll.AddUser(login);

			return new JsonResult(userAdded);
		}

		[HttpPost]
		public JsonResult LoginUser([FromBody] Login login)
		{
			bool userValid = _bll.VerifyUser(login);

			return new JsonResult(userValid);
		}

		[HttpGet]
		public JsonResult Logout()
        {
			_bll.Logout();
			return new JsonResult(true);
        }

		[HttpGet]
		public JsonResult GetHighScores()
        {
			var highScores = _bll.GetHighScores();

			return new JsonResult(highScores);
        }

		[HttpGet]
		public JsonResult GetSessionData()
		{
			var sessionData = _bll.GetSessionData();

			return new JsonResult(sessionData);
		}

		[HttpGet]
		public JsonResult LoggedIn()
        {
			var loggedIn = _bll.LoggedIn();

			return new JsonResult(loggedIn);
        }

		[HttpGet]
		public JsonResult GetToken()
		{
			var token = _bll.GetToken();

			return new JsonResult(token);
		}

		[HttpGet]
		public JsonResult GetExpiration()
		{
			var expiration = _bll.GetExpiration();

			return new JsonResult(expiration);
		}

		[HttpGet]
		public JsonResult GenerateWord()
		{
			_bll.GenerateWord();

			return new JsonResult(true);
		}

		[HttpGet]
		public JsonResult GetWordLengthString()
		{
			var wordLengthString = _bll.GetWordLengthString();

			return new JsonResult(wordLengthString);
		}

		[HttpPost]
		public JsonResult ValidateUserGuess([FromBody] GuessedLetter guessedLetter)
		{
			var letter = guessedLetter.Letter;
			_bll.CheckUserGuess(letter[0]);

			return new JsonResult(true);
		}

		[HttpGet]
		public JsonResult InitializeWordLengthString()
		{
			var wordLengthString = _bll.InitializeWordLengthString();

			return new JsonResult(wordLengthString);
		}

		[HttpGet]
		public JsonResult GetCorrectlyGuessedLetters()
		{
			var correctlyGuessedLetters = _bll.GetCorrectlyGuessedLetters();

			return new JsonResult(correctlyGuessedLetters);
		}

		[HttpGet]
		public JsonResult GetIncorrectlyGuessedLetters()
		{
			var incorrectlyGuessedLetters = _bll.GetIncorrectlyGuessedLetters();

			return new JsonResult(incorrectlyGuessedLetters);
		}

		[HttpGet]
		public JsonResult GetWord()
		{
			var correctWord = _bll.GetWord();

			return new JsonResult(correctWord);
		}

		[HttpGet]
		public JsonResult GetGameState()
        {
			var gameState = _bll.GetGameState();
			return new JsonResult(gameState);
        }

		[HttpPost]
		public JsonResult SetGameState([FromBody] GameState gameState)
        {
			_bll.SetGameState(gameState.State);
			return new JsonResult(true);
        }
	}
}
