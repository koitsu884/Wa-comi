import { Component, OnInit, Input, ElementRef, ViewChild, TemplateRef, OnDestroy } from '@angular/core';
import { ClanSeek } from '../../_models/ClanSeek';
import { City } from '../../_models/City';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ClanSeekCategoryResolver } from '../../_resolvers/clanseek-categories.resolver';
import { ClanSeekCategory } from '../../_models/ClanSeekCategory';
import { Photo } from '../../_models/Photo';
import { NgForm } from '@angular/forms';

import { Store } from '@ngrx/store';
import * as fromApp from '../../store/app.reducer';
// import * as fromPhoto from '../../photo/store/photos.reducers';
import * as fromClan from '../store/clan.reducers';
import * as ClanActions from '../store/clan.actions';
// import * as PhotoActions from '../../photo/store/photos.action';
import { Observable } from 'rxjs/Observable';
import { HttpRequest } from '@angular/common/http';
import { AlertifyService } from '../../_services/alertify.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-clan-edit',
  templateUrl: './clan-edit.component.html',
  styleUrls: ['./clan-edit.component.css']
})

export class ClanEditComponent implements OnInit {
  @Input() editMode: boolean;
  readonly FILE_UPLOAD_LIMIT = 5;
  readonly IMAGE_SIZE = 600;

  id: number;
  appUserId: number;
  editingClan: ClanSeek;
  waitingResponse: boolean = false;
  // formData: FormData;
  cities: City[];
  categories: ClanSeekCategory[];
  selectedFiles: Array<File> = [];
  previewUrls: Array<string> = [];
  //photos: Photo[];
  // photoState: Observable<fromPhoto.State>;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private location: Location,
    private appStore: Store<fromApp.AppState>,
    private store: Store<fromClan.FeatureState>,
    private alertify: AlertifyService) { }

  ngOnInit() {
    this.route.params
      .subscribe(
        (params: Params) => {
          this.id = +params['id'];
          this.appUserId = +params['appUserId'];
          // console.log(params);
          if (this.appUserId == null) {
            this.router.navigate(['/home']);
            return;
          }
          this.editMode = params['id'] != null;
          this.cities = this.route.snapshot.data['cities'];
          //this.photos = this.route.snapshot.data['photos'];
          // this.photoState = this.appStore.select('photos');
          this.categories = this.route.snapshot.data['categories'];
          this.initForm();
        }
      );
  }

  initForm() {
    if (this.editMode) {
      this.store.dispatch(new ClanActions.GetClanSeek(this.id));
      this.store.select("clan").subscribe((clanState) => {
        this.editingClan = Object.assign({}, clanState.editingClan);
      })
      //this.editingClan = this.route.snapshot.data['editingClan'];
      // this.appStore.dispatch(new PhotoActions.GetPhotos({recordType: "ClanSeek", recordId: this.id}));
    }
    else {
      this.editingClan = <ClanSeek>{};
    }
    this.editingClan.appUserId = this.appUserId;
  }

  onClear() {
    this.initForm();
  }

  mainPhotoSelected(event, ngForm: NgForm) {
    this.editingClan.mainPhotoId = event;
    ngForm.form.markAsDirty();
  }

  setSelectedFiles(event: {selectedFiles:Array<File>, previewUrls:Array<string>}){
    this.selectedFiles = event.selectedFiles;
    this.previewUrls = event.previewUrls;
  }

  submit(ngForm: NgForm) {
    this.waitingResponse = true;
    if (this.editMode) {
      // ngForm.form.markAsDirty();
      this.store.dispatch(new ClanActions.UpdateClanSeek(this.editingClan));
    }
    else {
      // ngForm.form.markAsPristine();
      var formData = new FormData();

      for (let file of this.selectedFiles) {
        formData.append("files", file);
      }
      this.store.dispatch(new ClanActions.ClearClanseekFilters());
      this.store.dispatch(new ClanActions.TryAddClanSeek({ clanSeek: this.editingClan, formData: this.selectedFiles.length > 0 ? formData : null }));
    }
  }

  onCancel(){
    this.location.back();
  }

}
