import { Component, OnInit, Input } from '@angular/core';
import { UserAccount } from '../../../_models/UserAccount';
import { NgForm } from '@angular/forms';
import { Store } from '@ngrx/store';

import * as fromAccount from '../../../account/store/account.reducers';
import * as AccountActions from '../../../account/store/account.actions';

@Component({
  selector: 'app-account-edit',
  templateUrl: './account-edit.component.html',
  styleUrls: ['./account-edit.component.css']
})
export class AccountEditComponent implements OnInit {
  @Input() account: UserAccount;
  constructor(private store: Store<fromAccount.State>,) { }

  ngOnInit() {
  }

  submit(userEditForm: NgForm){
    this.store.dispatch(new AccountActions.UpdateAccount(this.account));
    userEditForm.form.markAsPristine();
  }
}
