import { Component, OnInit } from '@angular/core';
import { Member } from './_models/Member';
import { Store } from '@ngrx/store';
import * as fromApp from './store/app.reducer';
import * as AccountActions from './account/store/account.actions';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{

  constructor(private store:Store<fromApp.AppState>) {}

  ngOnInit() {
    const token = localStorage.getItem('token');
    const appUser = JSON.parse(localStorage.getItem('appUser'));
    const profile = JSON.parse(localStorage.getItem('profile'));

    if (token){
      this.store.dispatch(new AccountActions.SetToken({token: token, appUser: appUser}));
      this.store.dispatch(new AccountActions.SetAppUser(appUser));
    }

    // if (token) {
    //   this.authService.userToken = token;
    //   let appUser = JSON.parse(localStorage.getItem('appUser'));
    //   this.authService.userType = appUser.userType;
    //   this.authService.changeCurrentUserDisplayname(appUser.displayName);
    //   // this.authService.decodedToken = this.jwtHelperService.decodeToken(token);
    //   // this.authService.appUser = JSON.parse(appUser);
    // }
  }
}
