import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { HttpServiceService } from '../Services/http-service.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css'],
})
export class NavMenuComponent implements OnInit, OnDestroy {

  loggedIn: boolean;
  loginSub: Subscription;
  @Input() LoggedIn$: Observable<boolean>;

  constructor(private http: HttpServiceService, private router: Router, private cd: ChangeDetectorRef) {

  }

  ngOnInit(): void {
    this.LoggedIn$ = this.http.LoggedIn();
    this.loginSub = this.LoggedIn$.subscribe(state => {
      this.loggedIn = state;
      console.log("loggedIn: ", state);
      this.cd.markForCheck();
    });

    this.http.cdEmitter.subscribe(() => {
      this.http.LoggedIn().subscribe(state => {
        this.loggedIn = state;
        this.cd.detectChanges();
      });
    });
  }

  //private loggedIn$: Observable<boolean>;
  //private loggedIn: boolean;
  //private stateSub: Subscription;

  //constructor(private http: HttpServiceService, private cd: ChangeDetectorRef, private router: Router) {
    
  //}
  //  ngOnInit(): void {
  //    this.loggedIn$ = this.http.LoggedIn();
  //    this.loggedIn$.subscribe(state => {
  //      this.loggedIn = state;
  //      this.cd.markForCheck();
  //    })
  //    this.cd.markForCheck();
  //  }

  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logout() {
    this.http.Logout().subscribe(() => {
      this.router.navigate(["/login"]);
      this.http.cdEmitter.emit();
    });
  }

  ngOnDestroy(): void {
    this.loginSub.unsubscribe();
  }
}
