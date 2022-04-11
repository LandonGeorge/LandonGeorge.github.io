import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { HangmanGameComponent } from './hangman-game/hangman-game.component';
import { SignUpComponent } from './sign-up/sign-up.component';
import { HttpServiceService } from './Services/http-service.service';
import { LoginComponent } from './login/login.component';
import { AuthServiceService } from './Services/auth-service.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    HangmanGameComponent,
    SignUpComponent,
    LoginComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      {
        path: '', component: HomeComponent, pathMatch: 'full', canActivate: [AuthServiceService] },
      { path: 'hangman-game', component: HangmanGameComponent, canActivate: [AuthServiceService] },
      { path: 'sign-up', component: SignUpComponent },
      { path: 'login', component: LoginComponent },
      { path: '**', redirectTo: "/" }
    ])
  ],
  providers: [AuthServiceService ],
  bootstrap: [AppComponent]
})
export class AppModule { }
