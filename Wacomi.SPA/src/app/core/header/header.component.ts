import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { AlertifyService } from '../../_services/alertify.service';
import { Observable } from 'rxjs/Observable';

import * as fromApp from '../../store/app.reducer';
import * as fromAccount from '../../account/store/account.reducers';
import * as AccountActions from '../../account/store/account.actions';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  authState: Observable<fromAccount.State>;
  public isCollapsed = false;

  constructor(private store: Store<fromApp.AppState>,
              private alertify: AlertifyService) { }

  ngOnInit() {
    this.authState = this.store.select('account');
  }

  logout() {
    this.store.dispatch(new AccountActions.Logout());
  }
}
