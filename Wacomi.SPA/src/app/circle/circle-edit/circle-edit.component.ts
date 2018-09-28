import * as fromCircle from '../store/circle.reducers';
import * as CircleActions from '../store/circle.actions';
import * as GlobalActions from '../../store/global.actions';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { Location } from '@angular/common';
import { Store } from '@ngrx/store';
import { AppUser } from '../../_models/AppUser';
import { City } from '../../_models/City';
import { Category } from '../../_models/Category';
import { CircleEdit } from '../../_models/CircleEdit';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-circle-edit',
  templateUrl: './circle-edit.component.html',
  styleUrls: ['./circle-edit.component.css']
})
export class CircleEditComponent implements OnInit {
  id: number;
  appUser: AppUser;
  cities: City[];
  categories: Category[];
  editingCircle: CircleEdit;
  selectedFiles: Array<File> = [];
  previewUrls: Array<string> = [];

  constructor(private route: ActivatedRoute, 
    private router: Router, 
    private location: Location,
    private store: Store<fromCircle.FeatureState>) { }

  ngOnInit() {
    this.route.params
      .subscribe(
        (params: Params) => {
          this.id = +params['id'];
          this.appUser = this.route.snapshot.data['appUser'];
          if (this.appUser == null) {
            this.router.navigate(['/']);
            return;
          }
          this.cities = this.route.snapshot.data['cities'];
          this.categories = this.route.snapshot.data['categories'];
          this.initForm();
        }
      );
  }

  initForm() {
    if (this.id) {
      this.store.dispatch(new CircleActions.GetCircle(this.id));
      this.store.select("circleModule").subscribe((circleState) => {
        this.editingCircle = Object.assign(<CircleEdit>{}, circleState.circle.selectedCircle);
      })
    }
    else {
      this.editingCircle = <CircleEdit>{
        appUserId : this.appUser.id,
        cityId: null,
        categoryId:  this.categories.find(c => c.name == "その他").id,
        approvalRequired: false
      };
    }
  }

  mainPhotoSelected(event, ngForm: NgForm) {
    this.editingCircle.mainPhotoId = event;
    ngForm.form.markAsDirty();
  }

  setSelectedFiles(event: {selectedFiles:Array<File>, previewUrls:Array<string>}){
    this.selectedFiles = event.selectedFiles;
    this.previewUrls = event.previewUrls;
  }

  onCancel(){
    this.location.back();
  }

  submit(ngForm: NgForm) {
    if (this.id) {
      ngForm.form.markAsPristine();
      this.store.dispatch(new GlobalActions.UpdateRecord({
        recordType:'circle',
        record: this.editingCircle,
        callbackLocation: '/users/posts/' + this.appUser.id,
        recordSetActionType: CircleActions.SET_CIRCLE
      }));
    }
    else {
      var formData = new FormData();

      for (let file of this.selectedFiles) {
        formData.append("files", file);
      }
      this.store.dispatch(new GlobalActions.TryAddRecord(
        {
          recordType:'circle',
          record: this.editingCircle,
          formData:this.selectedFiles.length > 0 ? formData : null,
          callbackLocation: '/users/posts/' + this.appUser.id,
          callbackActions: [{ type: GlobalActions.SUCCESS, payload: "投稿しました" }]
        }));
    }
  }

}
