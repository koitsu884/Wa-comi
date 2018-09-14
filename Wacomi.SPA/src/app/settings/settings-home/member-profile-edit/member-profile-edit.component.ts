import { Component, OnInit, Input } from '@angular/core';
import { BsDatepickerConfig, BsLocaleService, defineLocale} from 'ngx-bootstrap';
import { Hometown } from '../../../_models/Hometown';

import * as fromApp from "../../../store/app.reducer";
import * as AccountActions from '../../../account/store/account.actions';
import { Store } from '@ngrx/store';
import { NgForm } from '@angular/forms';
import { MemberProfile, GenderEnum } from '../../../_models/MemberProfile';
import {jaLocale} from 'ngx-bootstrap/locale';
defineLocale("ja", jaLocale);

@Component({
  selector: 'app-member-profile-edit',
  templateUrl: './member-profile-edit.component.html',
  styleUrls: ['./member-profile-edit.component.css']
})
export class MemberProfileEditComponent implements OnInit {
  @Input() member: MemberProfile; 
  @Input() hometowns: Hometown[];
  genderEnum = GenderEnum;
  
  blogAdding: boolean = false;

  bsConfig: Partial<BsDatepickerConfig>;
  
  constructor(private localeService: BsLocaleService,
             private store: Store<fromApp.AppState>) { }

  ngOnInit() {
    this.bsConfig = {
      dateInputFormat: "YYYY年MM月DD日"
    };
    this.localeService.use('ja');
  }

  submit(ngForm: NgForm){
    this.store.dispatch(new AccountActions.UpdateMember(this.member));
    ngForm.form.markAsPristine();
  }

  onClickAddBlog(){
    this.blogAdding = true;

    this.blogAdding
  }

  onClickDeleteBlog(id:number){

  }

}
