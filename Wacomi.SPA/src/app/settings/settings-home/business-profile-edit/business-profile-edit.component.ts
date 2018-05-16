import { Component, OnInit, Input } from '@angular/core';
import { BusinessUser } from '../../../_models/BusinessUser';
import { City } from '../../../_models/City';
import { Hometown } from '../../../_models/Hometown';
import { BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap';
import { AlertifyService } from '../../../_services/alertify.service';
import { Store } from '@ngrx/store';
import * as fromApp from "../../../store/app.reducer";
import * as fromAccount from "../../../account/store/account.reducers";
import * as AccountActions from '../../../account/store/account.actions';
import { Photo } from '../../../_models/Photo';
import { NgForm } from '@angular/forms';
import { Blog } from '../../../_models/Blog';

@Component({
  selector: 'app-business-profile-edit',
  templateUrl: './business-profile-edit.component.html',
  styleUrls: ['./business-profile-edit.component.css']
})
export class BusinessProfileEditComponent implements OnInit {
  @Input() bisUser: BusinessUser; 
  @Input() cities: City[];
  @Input() hometowns: Hometown[];
  @Input() photos: Photo[];
  @Input() blogs: Blog[];

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
      this.store.dispatch(new AccountActions.UpdateBisUser(this.bisUser));
      ngForm.form.markAsPristine();
    }

    mainPhotoSelected(event, ngForm: NgForm){
      this.bisUser.mainPhotoUrl = event;
      ngForm.form.markAsDirty();
    }

}
