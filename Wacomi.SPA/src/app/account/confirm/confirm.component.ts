import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromApp from '../../store/app.reducer';
import * as fromAccount from '../store/account.reducers';
import * as AccountActions from '../store/account.actions';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-confirm',
  templateUrl: './confirm.component.html',
  styleUrls: ['./confirm.component.css']
})
export class ConfirmComponent implements OnInit {
  authState: Observable<fromAccount.State>;

  constructor(private store:Store<fromApp.AppState>, private route:ActivatedRoute) { }

  ngOnInit() {
    this.authState = this.store.select('account');
    this.route.params.subscribe((params) => {
      this.store.dispatch(new AccountActions.TryConfirmEmail({userId: params["id"], code:decodeURI(params["code"])}));
    })
  }

}
