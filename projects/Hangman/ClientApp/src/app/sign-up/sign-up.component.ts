import { Component, OnInit } from '@angular/core';
import { Login } from '../angular-models/login.model';
import { HttpServiceService } from '../Services/http-service.service';
import { Router } from '@angular/router';
import { AuthServiceService } from '../Services/auth-service.service';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent implements OnInit {

  login: Login = new Login();
  httpService: HttpServiceService;
  passwordCheck: string;

  accountCreated: boolean;
  passwordsDontMatch: boolean;
  hasClickedLogin: boolean;

  constructor(httpService: HttpServiceService, private router: Router, private auth: AuthServiceService) {
    this.httpService = httpService;
  }

  addUser() {
    this.hasClickedLogin = true;
    if (this.passwordCheck !== this.login.Password) {
      // create banner that says the passwords don't match!
      this.passwordsDontMatch = true;
    }
    else {
      this.passwordsDontMatch = false;

      this.httpService.SignUpForAccount(this.login).subscribe(loginCreated => {
        this.accountCreated = loginCreated;

        if (!this.accountCreated) {
          this.login.Password = "";
          this.passwordCheck = "";
        }
        else {
          this.login.Username = "";
          this.login.Password = "";
          this.passwordCheck = "";
          this.httpService.LoggedIn().subscribe(state => {
            this.auth.loggedIn = state;
            this.router.navigateByUrl("hangman-game");
          })
        }
      });
    }
  }

  ngOnInit() {
  }

  showUsers() {

  }

}
