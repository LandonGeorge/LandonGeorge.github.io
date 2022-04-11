import { HttpClient } from "@angular/common/http";
import { ApplicationRef, Injectable, Input, OnInit } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable, Subscription } from "rxjs";
import { catchError } from "rxjs/operators";
import { Login } from "../angular-models/login.model";
import { HttpServiceService } from "./http-service.service";

@Injectable()
export class AuthServiceService implements CanActivate{

  loggedIn: boolean;
  loginSub: Subscription;
  @Input() LoggedIn$: Observable<boolean>;

  constructor(private http: HttpServiceService, private router: Router, private ref: ApplicationRef) {
    this.LoggedIn$ = this.http.LoggedIn();
    this.loginSub = this.LoggedIn$.subscribe(state => {
      this.loggedIn = state;
      this.ref.tick();

      if (this.loggedIn) {
        this.router.navigate(['hangman-game'])
      }

      this.http.cdEmitter.subscribe(() => {
        this.http.LoggedIn().subscribe(state => {
          this.loggedIn = state;
          this.ref.tick();
        });
      });
    });
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    if (!this.loggedIn) {
      this.router.navigate(['login']);
      return false;
    }
    return true;
  }

}
