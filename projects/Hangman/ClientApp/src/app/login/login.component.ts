import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Login } from '../angular-models/login.model';
import { AuthServiceService } from '../Services/auth-service.service';
import { HttpServiceService } from '../Services/http-service.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
/** login component*/
export class LoginComponent implements OnInit {

  login: Login = new Login();
  invalidLogin: boolean;
  validLogin: boolean;
  hasClickedLogin: boolean;

  constructor(private http: HttpServiceService, private router: Router, private auth: AuthServiceService) {
    
  }

  loginUser() {
    this.hasClickedLogin = true;
    this.http.LoginUser(this.login).subscribe(validUser => {
      if (!validUser) {
        this.invalidLogin = true;
      } else {
        this.invalidLogin = false;
        this.http.LoggedIn().subscribe(state => {
          this.auth.loggedIn = state;
          this.router.navigateByUrl("hangman-game");
        })
      }
    });
  }


  ngOnInit(): void {
  }
}
