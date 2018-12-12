import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';

import * as fromApp from '../../store/app.reducer';
import * as NotificationActions from '../../notification/store/notification.action';
// import * as fromAccount from '../../account/store/account.reducers';
import * as AccountActions from '../../account/store/account.actions';
import { AppUser } from '../../_models/AppUser';
import { UserAccount } from '../../_models/UserAccount';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit, OnDestroy {
  // authState: Observable<fromAccount.State>;
  public isCollapsed = false;
  appUser: AppUser;
  account: UserAccount;
  isAdmin: boolean;
  authenticated: boolean;
  newMessagesCount: number;
  notificationCount: number;
  notificationTimer: any;


  constructor(private store: Store<fromApp.AppState>) { }

  ngOnInit() {
    this.store.select('account')
      .subscribe((accountState) => {
          this.appUser = accountState.appUser;
          this.account = accountState.account;
        this.isAdmin = accountState.isAdmin;
        this.authenticated = accountState.authenticated;
        // this.newMessagesCount = accountState.newMessagesCount;
      });

    this.store.select('notification')
      .subscribe((notificationState) => {
        this.notificationCount = notificationState.notifications.length;
      })

      this.notificationTimer = setInterval(() => {
        if(this.appUser)
          this.store.dispatch(new NotificationActions.GetNotifications(this.appUser.id))
      },
       1000 * 60 * 3
     // 10000
      );

  }

  ngOnDestroy(){
    if (this.notificationTimer) {
      clearInterval(this.notificationTimer);
    }
  }

  logout() {
    this.store.dispatch(new AccountActions.Logout());
  }
}
