import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../../_services/alertify.service';
import * as fromApp from '../../store/app.reducer';
import * as fromAccount from '../../account/store/account.reducers';
import * as AccountActions from '../../account/store/account.actions';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private store:Store<fromApp.AppState>, private alertify: AlertifyService) { }
  userName: string;
  password: string;

  ngOnInit() {
  }

  login() {
    this.store.dispatch(new AccountActions.TryLogin({UserName:this.userName, Password:this.password}));
    }


}
