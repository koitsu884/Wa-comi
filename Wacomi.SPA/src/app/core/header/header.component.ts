import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { AlertifyService } from '../../_services/alertify.service';
import { Observable } from 'rxjs/Observable';

import * as fromApp from '../../store/app.reducer';
// import * as fromAccount from '../../account/store/account.reducers';
import * as AccountActions from '../../account/store/account.actions';
import { AppUser } from '../../_models/AppUser';
import { UserAccount } from '../../_models/UserAccount';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  // authState: Observable<fromAccount.State>;
  public isCollapsed = false;
  appUser: AppUser;
  account: UserAccount;
  isAdmin: boolean;
  authenticated: boolean;
  newMessagesCount: number;


  constructor(private store: Store<fromApp.AppState>,
              private alertify: AlertifyService) { }

  ngOnInit() {
    this.store.select('account')
      .subscribe((accountState) => {
          this.appUser = accountState.appUser;
          this.account = accountState.account;
        this.isAdmin = accountState.isAdmin;
        this.authenticated = accountState.authenticated;
        this.newMessagesCount = accountState.newMessagesCount;
      });
  }

  logout() {
    this.store.dispatch(new AccountActions.Logout());
  }
}
