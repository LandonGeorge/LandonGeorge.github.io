import { Component } from '@angular/core';
import { Login } from '../angular-models/login.model';
import { } from '@angular/common/http'
import { HttpServiceService } from '../Services/http-service.service';
import { HighScore } from '../angular-models/highscore.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  word: string;
  wordLengthArray: string[];
  letters: string[] = ["a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"];
  correctlyGuessedLetters: string[] = [];
  incorrectlyGuessedLetters: string[] = [];
  pictureUrl: string = "/assets/images/hangman1.png";
  gameInSession: boolean;
  correctWord: string = "";
  numGuesses: number = 0;

  constructor(private httpService: HttpServiceService) {
    httpService.GetGameState().subscribe(gameState => {
      this.gameInSession = gameState;
      console.log("current game state", gameState);
      this.initializeGame(this.gameInSession);
    });
  }

  initializeGame(keepGameState: boolean) {
    if (keepGameState === false) {
      this.correctlyGuessedLetters = [];
      this.incorrectlyGuessedLetters = [];
      this.correctWord = "";
      this.incrementPicture(this.incorrectlyGuessedLetters.length);
      this.httpService.GenerateWord().subscribe(() => {
        this.httpService.InitializeWordLengthString().subscribe(wordString => {
          this.wordLengthArray = wordString.split("");
        });
      });
    }
    else {
      this.fetchData("1");
    }
  }

  validateLetter(letter: string) {
    this.httpService.ValidateUserGuess(letter).subscribe(() => {
      this.fetchData(letter);
      if (!this.correctlyGuessedLetters.includes(letter) && !this.incorrectlyGuessedLetters.includes(letter)) {
        this.numGuesses++;
      }

      // if game is over, fetch the correct word
      if (this.incorrectlyGuessedLetters.length === 5) {
        this.httpService.GetCorrectWord().subscribe(word => {
          this.correctWord = word;
        });
      }
    });
  }

  fetchData(letter: string) {
    this.httpService.GetCorrectlyGuessedLetters().subscribe(correctLetters => {
      this.correctlyGuessedLetters = correctLetters.split("");
    });
    this.httpService.GetIncorrectlyGuessedLetters().subscribe(incorrectLetters => {
      this.incorrectlyGuessedLetters = incorrectLetters.split("");
      if (this.incorrectlyGuessedLetters.includes(letter)) {
        this.incrementPicture(this.incorrectlyGuessedLetters.length);
      }
    });
    this.httpService.GetWordLengthString().subscribe(wordString => {
      this.wordLengthArray = wordString.split("");
    });
  }

  startGame() {
    this.httpService.SetGameState(true).subscribe();
    this.gameInSession = true;
  }

  resetGame() {
    this.httpService.SetGameState(false).subscribe();
    this.numGuesses = 0;
    this.gameInSession = false;
    this.initializeGame(false);
  }

  incrementPicture(lengthOfIncorrectLetters: number) {
    this.pictureUrl = "/assets/images/hangman" + (lengthOfIncorrectLetters + 1).toString() + ".png";
  }

}
