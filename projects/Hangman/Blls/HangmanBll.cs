using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using hangman.Data;
using hangman.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

// this is where all the actual logic for the hangman game should go that requires the backend's help
// typical examples would be any validation functions, or logic that you wouldn't want on the front end

namespace hangman.Blls
{
	public class HangmanBll
	{
		// private/static variables
		private readonly HangmanContext _ctx;
		private readonly HttpContext _http;
		private static Random random;


		// Bll Constructor
		public HangmanBll(HangmanContext ctx, IHttpContextAccessor http)
		//public HangmanBll(HangmanContext ctx)
		{
			_ctx = ctx;
			_http = http.HttpContext;
			random = new Random();
		}


		// GetHighScores returns list of all high scores, ordered by score
		public IEnumerable<HighScoreDto> GetHighScores()
		{
			var highScores = _ctx.HighScores
				.Select(h => h)
				.Include(h => h.User)
				.OrderBy(score => score.Score)
				.Take(10)
				.ToList();

			var highScoreDtos = new List<HighScoreDto>();

			highScores.ForEach(highScore =>
			{
				var hs = new HighScoreDto();
				hs.Id = highScore.Id;
				hs.Score = highScore.Score;
				hs.DateTime = highScore.DateTime;
				hs.Username = highScore.User.Username;
				hs.Word = highScore.Word;
				highScoreDtos.Add(hs);
			});

			return highScoreDtos;
		}

		public void AddHighScore()
        {
			var user = _ctx.Users
				.Where(user => user.Username == GetToken())
				.FirstOrDefault();
			var highScore = new HighScore();
			highScore.DateTime = DateTime.Now;
			highScore.Score = GetIncorrectlyGuessedLetters().Length;
			highScore.User = user;
			highScore.Word = GetWord();
			_ctx.HighScores.Add(highScore);
			_ctx.SaveChanges();

        }


		// Add new user to database
		public bool AddUser(Login login)
		{
			// salt and hash password, add to database
			try
			{
				if (_ctx.Users.Select(u => u.Username).Contains(login.Username))
				{
					// username already exists!
					return false;
				}

				var salt = GenerateSalt();
				var password = GenerateHash(login.Password + salt);
				var user = new User { Username = login.Username, Password = password, Salt = salt };
				_ctx.Users.Add(user);
				SetToken(login.Username);
				SetExpiration(DateTime.Today.AddDays(7).ToString());
				return _ctx.SaveChanges() > 0;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error Adding user: {ex}");
				return false;
			}

		}


		// verify given username and password with database
		public bool VerifyUser(Login login)
		{
			try
			{
				var user = _ctx.Users
					.Where(user => user.Username == login.Username)
					.FirstOrDefault();

				if (user == null)
				{
					// no username 
					return false;
				}
				var password = GenerateHash(login.Password + user.Salt);

                if (password == user.Password)
                {
                    SetToken(login.Username);
                    SetExpiration(DateTime.Today.AddDays(7).ToString());
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

		public void Logout()
        {
			_http.Session.Clear();
        }


		// Generate random salt
		public string GenerateSalt()
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			return new string(Enumerable.Repeat(chars, 5)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}


		// Generate sha256 hash for given string
		public string GenerateHash(string str)
		{
			var sb = new StringBuilder();

			using (SHA256 hash = SHA256Managed.Create())
			{
				Encoding enc = Encoding.UTF8;
				Byte[] result = hash.ComputeHash(enc.GetBytes(str));

				foreach (Byte b in result)
					sb.Append(b.ToString("x2"));
			}

			return sb.ToString();
		}


        public SessionData GetSessionData()
        {
            var token = GetToken();
            var expiration = GetExpiration();

            if (token == null)
            {
                token = "";
                expiration = new DateTime().ToString();
            }
            var sessionData = new SessionData(token, expiration);

            return sessionData;
        }

		public bool LoggedIn()
        {
			var token = GetToken();
			var expiration = GetExpiration();
			if(expiration == null)
            {
				return false;
            } else
            {
				return !(token == null || DateTime.Parse(expiration) < DateTime.Now);
            }
        }


        //get token
        public string GetToken()
        {
            return _http.Session.GetString("Token");
        }

		//set token
		public void SetToken(string username)
		{
			_http.Session.SetString("Token", username);
		}

		//get expiration
		public string GetExpiration()
		{
			return _http.Session.GetString("Expiration");
		}

		//set expiration
		public void SetExpiration(string expiration)
		{
			_http.Session.SetString("Expiration", expiration);
		}

		//get word
		public string GetWord()
		{
			return _http.Session.GetString("Word");
		}

		//set word
		public void SetWord(string word)
		{
			_http.Session.SetString("Word", word);
		}

		//get gameState
		public Boolean GetGameState()
        {
			var gameState = _http.Session.GetString("GameState");

			if(gameState == "true")
            {
				return true;
            }
            else
            {
				return false;
            }
        }

		//set gameState
		public void SetGameState(Boolean gameState)
        {

            if (gameState)
            {
				_http.Session.SetString("GameState", "true");
            }
            else
            {
				_http.Session.SetString("GameState", "false");
			}
			
        }


		//Generate new word
		public void GenerateWord()
		{
			Random rand = new Random();
			List<string> words = new List<string>();
			using (var sr = new StreamReader("./Data/words.txt"))
			{
				var endOfFile = false;
				while (!endOfFile)
				{
					var word = sr.ReadLine();
					
					if (word != null && !words.Contains(word))
					{
						words.Add(sr.ReadLine());
					}
					else if(word == null)
					{
						endOfFile = true;
					}
				}
			}

			var randomIndex = rand.Next(0, words.Count()-1);

			SetWord(words[randomIndex]);
		}

		public string InitializeWordLengthString()
		{
			var word = GetWord();

			string wordLengthString = "";

			foreach(var letter in word)
			{
				wordLengthString += "_";
			}

			_http.Session.SetString("WordLengthString", wordLengthString);
			_http.Session.SetString("CorrectlyGuessedLetters", "");
			_http.Session.SetString("IncorrectlyGuessedLetters", "");

			return wordLengthString;
		}

		public string GetWordLengthString()
		{
			return _http.Session.GetString("WordLengthString");
		}

		public void SetWordLengthString(char guessedLetter)
		{
			var word = GetWord();
			var wordLengthString = GetWordLengthString().ToCharArray();

			int pos = 0;
			foreach(var letter in word)
			{
				if(letter == guessedLetter)
				{
					wordLengthString[pos] = letter;
				}

				pos++;
			}

			if(!wordLengthString.Contains('_'))
            {
				AddHighScore();
            }

			_http.Session.SetString("WordLengthString", new string(wordLengthString));
		}

		public void CheckUserGuess(char userGuess)
		{
			var word = GetWord();
			var correctGuesses = GetCorrectlyGuessedLetters();
			var incorrectGuesses = GetIncorrectlyGuessedLetters();
			if (!correctGuesses.Contains(userGuess) && !incorrectGuesses.Contains(userGuess))
			{
				if (word.Contains(userGuess))
				{
					SetWordLengthString(userGuess);
					SetCorrectlyGuessedLetter(userGuess);
				}
				else
				{
					SetIncorrectlyGuessedLetter(userGuess);
				}
			}
		}

		public void SetCorrectlyGuessedLetter(char correctlyGuessedLetter)
		{
			var correctLetters = GetCorrectlyGuessedLetters();
			correctLetters += correctlyGuessedLetter;

			_http.Session.SetString("CorrectlyGuessedLetters", correctLetters);
		}

		public void SetIncorrectlyGuessedLetter(char incorrectlyGuessedLetter)
		{
			var incorrectLetters = GetIncorrectlyGuessedLetters();
			incorrectLetters += incorrectlyGuessedLetter;

			_http.Session.SetString("IncorrectlyGuessedLetters", incorrectLetters);
		}

		public string GetCorrectlyGuessedLetters()
		{
			var correctLetters = _http.Session.GetString("CorrectlyGuessedLetters");
			if (correctLetters == null)
			{
				_http.Session.SetString("CorrectlyGuessedLetters", "");
				return "";
			}

			return correctLetters;
		}

		public string GetIncorrectlyGuessedLetters()
		{
			var incorrectLetters = _http.Session.GetString("IncorrectlyGuessedLetters");
			if(incorrectLetters == null)
			{
				_http.Session.SetString("IncorrectlyGuessedLetters", "");
				return "";
			}

			return incorrectLetters;
		}
	}
}
