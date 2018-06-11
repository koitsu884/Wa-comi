import { Component, OnInit, Input } from '@angular/core';
import { BsDatepickerConfig, BsLocaleService, defineLocale} from 'ngx-bootstrap';
import { City } from '../../../_models/City';
import { Hometown } from '../../../_models/Hometown';
import { AlertifyService } from '../../../_services/alertify.service';

import * as fromApp from "../../../store/app.reducer";
import * as fromAccount from "../../../account/store/account.reducers";
import * as AccountActions from '../../../account/store/account.actions';
import { Store } from '@ngrx/store';
import { Photo } from '../../../_models/Photo';
import { Observer } from 'rxjs/Observer';
import { Observable } from 'rxjs/Observable';
import { NgForm } from '@angular/forms';
import { Blog } from '../../../_models/Blog';
import { MemberProfile } from '../../../_models/MemberProfile';


@Component({
  selector: 'app-member-profile-edit',
  templateUrl: './member-profile-edit.component.html',
  styleUrls: ['./member-profile-edit.component.css']
})
export class MemberProfileEditComponent implements OnInit {
  @Input() member: MemberProfile; 
  @Input() hometowns: Hometown[];
  // @Input() photos: Photo[];
  // @Input() blogs: Blog[];
  
  blogAdding: boolean = false;

  bsConfig: Partial<BsDatepickerConfig>;
  
  constructor(private localeService: BsLocaleService,
             private alertify: AlertifyService,
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

  // mainPhotoSelected(event, ngForm: NgForm){
  //   this.member.mainPhotoUrl = event;
  //   ngForm.form.markAsDirty();
  // }

}
