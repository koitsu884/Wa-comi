import { Component, OnInit, Input } from '@angular/core';
import { BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap';
import { Store } from '@ngrx/store';
import * as fromApp from "../../../store/app.reducer";
import * as AccountActions from '../../../account/store/account.actions';
import { NgForm } from '@angular/forms';
import { BusinessProfile } from '../../../_models/BusinessProfile';

@Component({
  selector: 'app-business-profile-edit',
  templateUrl: './business-profile-edit.component.html',
  styleUrls: ['./business-profile-edit.component.css']
})
export class BusinessProfileEditComponent implements OnInit {
  @Input() bisUser: BusinessProfile; 

  bsConfig: Partial<BsDatepickerConfig>;

  constructor(private localeService: BsLocaleService,
    private store: Store<fromApp.AppState>) { }

    ngOnInit() {
      this.bsConfig = {
        dateInputFormat: "YYYY年MM月DD日"
      };
     // this.localeService.use('ja');
    }
  
    submit(ngForm: NgForm){
      this.store.dispatch(new AccountActions.UpdateBisUser(this.bisUser));
      ngForm.form.markAsPristine();
    }

}
