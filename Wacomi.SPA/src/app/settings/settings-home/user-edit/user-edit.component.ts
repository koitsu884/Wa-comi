import { Component, OnInit, Input } from '@angular/core';
import { AlertifyService } from '../../../_services/alertify.service';
import { NgForm } from '@angular/forms';
import * as fromApp from "../../../store/app.reducer";
import * as fromAccount from "../../../account/store/account.reducers";
import * as AccountActions from '../../../account/store/account.actions';
import { Store } from '@ngrx/store';
import { AppUser } from '../../../_models/AppUser';
import { City } from '../../../_models/City';
import { Blog } from '../../../_models/Blog';
import { Photo } from '../../../_models/Photo';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {
  @Input() appUser: AppUser;
  @Input() cities: City[];
  @Input() blogs: Blog[];
  @Input() photos: Photo[];
  
  constructor(private store: Store<fromApp.AppState>,
            private alertify: AlertifyService) { }

  ngOnInit() {
  }

  mainPhotoSelected(event, ngForm: NgForm){
    this.appUser.mainPhotoUrl = event;
    ngForm.form.markAsDirty();
  }

  submit(userEditForm: NgForm){
    this.store.dispatch(new AccountActions.UpdateAppUser(this.appUser));
    userEditForm.form.markAsPristine();
  }
}
