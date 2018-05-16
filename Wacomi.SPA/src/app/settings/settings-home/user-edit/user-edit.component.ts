import { Component, OnInit, Input } from '@angular/core';
import { AppUser } from '../../../_models/AppUser';
import { AlertifyService } from '../../../_services/alertify.service';
import { NgForm } from '@angular/forms';
import * as fromApp from "../../../store/app.reducer";
import * as fromAccount from "../../../account/store/account.reducers";
import * as AccountActions from '../../../account/store/account.actions';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {
  @Input() appUser: AppUser;
  
  constructor(private store: Store<fromApp.AppState>,
            private alertify: AlertifyService) { }

  ngOnInit() {
  }

  submit(userEditForm: NgForm){
    this.store.dispatch(new AccountActions.UpdateAppUser(this.appUser));
    userEditForm.form.markAsPristine();
  }
}
