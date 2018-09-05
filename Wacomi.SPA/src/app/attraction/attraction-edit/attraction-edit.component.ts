import * as fromAttraction from '../store/attraction.reducers';
import * as AttractionActions from '../store/attraction.actions';
import { Component, OnInit } from '@angular/core';
import { City } from '../../_models/City';
import { Category } from '../../_models/Category';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { AppUser } from '../../_models/AppUser';
import { Store } from '@ngrx/store';
import { NgForm } from '@angular/forms';
import { Location } from '@angular/common';
import { AttractionEdit } from '../../_models/AttractionEdit';

@Component({
  selector: 'app-attraction-edit',
  templateUrl: './attraction-edit.component.html',
  styleUrls: ['./attraction-edit.component.css']
})
export class AttractionEditComponent implements OnInit {
  readonly FILE_UPLOAD_LIMIT = 5;
  readonly IMAGE_SIZE = 600;
  
  id: number;
  appUser: AppUser;
  attraction: AttractionEdit;
  waitingResponse: boolean = false;
  // formData: FormData;
  cities: City[];
  categories: Category[];
  // selectedCategoryIds: number[] = [];
  selectedFiles: Array<File> = [];
  previewUrls: Array<string> = [];

  constructor(private route: ActivatedRoute, 
    private router:Router, 
    private store: Store<fromAttraction.FeatureState>,
  private location: Location) { }

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
      this.store.dispatch(new AttractionActions.GetAttraction(this.id));
      this.store.select("attraction").subscribe((attractionState) => {
        let tempAttraction = Object.assign({}, attractionState.selectedAttraction);
        this.attraction = <AttractionEdit>{
          id : tempAttraction.id,
          appUserId : this.appUser.id,
          name : tempAttraction.name,
          cityId : tempAttraction.cityId,
          photos: tempAttraction.photos,
          introduction : tempAttraction.introduction,
          accessInfo: tempAttraction.accessInfo,
          mainPhotoId : tempAttraction.mainPhotoId,
          websiteUrl : tempAttraction.websiteUrl,
          latitude : tempAttraction.latitude,
          longitude : tempAttraction.longitude,
          categorizations: []
        };
        if(tempAttraction.categories)
        {
          for( let category of tempAttraction.categories){
            this.attraction.categorizations.push({attractionCategoryId:category.id});
          }
        }
        //this.attraction = Object.assign({}, attractionState.selectedAttraction);
      })
    }
    else {
      this.attraction = <AttractionEdit>{};
      this.attraction.categorizations = [];
    }
    this.attraction.appUserId = this.appUser.id;
  }

  mainPhotoSelected(event, ngForm: NgForm) {
    this.attraction.mainPhotoId = event;
    ngForm.form.markAsDirty();
  }

  setSelectedFiles(event: {selectedFiles:Array<File>, previewUrls:Array<string>}){
    this.selectedFiles = event.selectedFiles;
    this.previewUrls = event.previewUrls;
  }

  submit(ngForm: NgForm) {
    this.waitingResponse = true;
    if (this.id) {
      ngForm.form.markAsDirty();
      this.store.dispatch(new AttractionActions.UpdateAttraction(this.attraction));
    }
    else {
      var formData = new FormData();

      for (let file of this.selectedFiles) {
        formData.append("files", file);
      }
      this.store.dispatch(new AttractionActions.ClearAttractionFilter());
      this.store.dispatch(new AttractionActions.TryAddAttraction({ attraction: this.attraction, formData:this.selectedFiles.length > 0 ? formData : null }));
    }
  }

  toggleCategory(ngForm: NgForm, id: number){
    ngForm.form.markAsDirty();
    let index = this.attraction.categorizations.findIndex(c => c.attractionCategoryId == id);
    if( index < 0)
      this.attraction.categorizations.push({attractionCategoryId: id});
    else
      this.attraction.categorizations.splice(index, 1);
  }

  isSelected(id : number){
    return this.attraction.categorizations.findIndex(c => c.attractionCategoryId == id) >= 0;
  }

  clearCategory(ngForm: NgForm){
    ngForm.form.markAsDirty();
    this.attraction.categorizations = [];
  }

  onCancel(){
    this.location.back();
  }

}
