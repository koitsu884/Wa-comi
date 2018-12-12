import { Component, OnInit, Input } from '@angular/core';
import { NgForm } from '@angular/forms';
import * as fromApp from "../../../store/app.reducer";
import * as AccountActions from '../../../account/store/account.actions';
import { Store } from '@ngrx/store';
import { AppUser } from '../../../_models/AppUser';
import { City } from '../../../_models/City';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {
  @Input() appUser: AppUser;
  @Input() cities: City[];

  constructor(private store: Store<fromApp.AppState>) { }

  ngOnInit() {
  }

  mainPhotoSelected(event, ngForm: NgForm) {
    this.appUser.mainPhotoId = event;
    ngForm.form.markAsDirty();
  }

  submit(userEditForm: NgForm) {
    this.store.dispatch(new AccountActions.UpdateAppUser(this.appUser));
    userEditForm.form.markAsPristine();
  }

}
