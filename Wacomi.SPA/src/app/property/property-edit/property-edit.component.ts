import * as fromProperty from '../store/property.reducers';
import * as PropertyActions from '../store/property.actions';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { AppUser } from '../../_models/AppUser';
import { City } from '../../_models/City';
import { Category } from '../../_models/Category';
import { Store } from '@ngrx/store';
import { PropertyEdit } from '../../_models/PropertyEdit';
import { NgForm } from '@angular/forms';
import { Location } from '@angular/common';
import { TermEnum, RentTypeEnum } from '../../_models/PropertySearchOptions';
import { BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import {jaLocale} from 'ngx-bootstrap/locale';
import { defineLocale } from 'ngx-bootstrap/chronos/locale/locales';
defineLocale("ja", jaLocale);

@Component({
  selector: 'app-property-edit',
  templateUrl: './property-edit.component.html',
  styleUrls: ['./property-edit.component.css']
})
export class PropertyEditComponent implements OnInit {
  id: number;
  appUser: AppUser;
  cities: City[];
  categories: Category[];
  property: PropertyEdit;
  useGmap: boolean;
  selectedFiles: Array<File> = [];
  previewUrls: Array<string> = [];
  bsConfig: Partial<BsDatepickerConfig>;

  minTermArray: Array<number> = [];
  maxTermArray: Array<number> = [];

  rentTypeEnum = RentTypeEnum;
  
  constructor(private route: ActivatedRoute, 
    private router: Router, 
    private location: Location,
    private localeService: BsLocaleService,
    private store: Store<fromProperty.FeatureState>) { }

  ngOnInit() {
    this.bsConfig = {
      dateInputFormat: "YYYY年MM月DD日"
    };
    this.localeService.use('ja');

    for(let _i = 0; _i < TermEnum.LONG; _i++){
      this.minTermArray.push(_i);
      this.maxTermArray.push(_i + 1); 
    }
    
    this.route.params
      .subscribe(
        (params: Params) => {
          this.id = +params['id'];
          this.appUser = this.route.snapshot.data['appUser'];
          if (this.appUser == null) {
            this.router.navigate(['/']);
            return;
          }
          this.cities = this.route.snapshot.data['cities'].filter(c => c.region != "その他");
          this.categories = this.route.snapshot.data['categories'];
          this.initForm();
        }
      );
  }

  initForm() {
    if (this.id) {
      this.store.dispatch(new PropertyActions.GetProperty(this.id));
      this.store.select("property").subscribe((propertyState) => {
        let tempProperty = Object.assign({}, propertyState.selectedProperty);
        this.property = <PropertyEdit>{
          id : tempProperty.id,
          appUserId : this.appUser.id,
          isActive: tempProperty.isActive,
          title : tempProperty.title,
          cityId : tempProperty.cityId,
          rentType: tempProperty.rentType,
          photos: tempProperty.photos,
          description : tempProperty.description,
          mainPhotoId : tempProperty.mainPhoto ? tempProperty.mainPhoto.id : null,
          latitude : tempProperty.latitude,
          longitude : tempProperty.longitude,
          hasPet: tempProperty.hasPet,
          hasChild: tempProperty.hasChild,
          dateAvailable: tempProperty.dateAvailable,
          internet: tempProperty.internet,
          gender: tempProperty.gender,
          minTerm: tempProperty.minTerm,
          maxTerm: tempProperty.maxTerm,
          rent: tempProperty.rent,          
          categorizations: []
        };
        if(tempProperty.categories)
        {
          for( let category of tempProperty.categories){
            this.property.categorizations.push({propertySeekCategoryId:category.id});
          }
        }
        if(tempProperty.latitude)
          this.useGmap = true;
        //this.property = Object.assign({}, propertyState.selectedproperty);
      })
    }
    else {
      this.property = <PropertyEdit>{
        categorizations : [],
        isActive: true,
        hasPet: false,
        hasChild: false,
        rentType: RentTypeEnum.OWN,
        internet:0,
        gender:0,
        minTerm: TermEnum.SHORT,
        maxTerm: TermEnum.LONG
      };
      this.property.appUserId = this.appUser.id;
    }
  }

  termValues() {
    let termEnum = TermEnum;
    var values = Object.values(termEnum);
    return values.slice(values.length / 2)
  }

  minTermChanged(){
    if(this.property.maxTerm <= this.property.minTerm)
    {
      this.property.maxTerm = +this.property.minTerm + 1;
    }
  }

  maxTermChanged(){
    if(this.property.minTerm >= this.property.maxTerm)
    {
      this.property.minTerm = +this.property.maxTerm - 1;
    }
  }

  mainPhotoSelected(event, ngForm: NgForm) {
    this.property.mainPhotoId = event;
    ngForm.form.markAsDirty();
  }

  setSelectedFiles(event: {selectedFiles:Array<File>, previewUrls:Array<string>}){
    this.selectedFiles = event.selectedFiles;
    this.previewUrls = event.previewUrls;
  }

  toggleCategory(ngForm: NgForm, id: number){
    ngForm.form.markAsDirty();
    let index = this.property.categorizations.findIndex(c => c.propertySeekCategoryId == id);
    if( index < 0)
      this.property.categorizations.push({propertySeekCategoryId: id});
    else
      this.property.categorizations.splice(index, 1);
  }

  clearCategory(ngForm: NgForm){
    ngForm.form.markAsDirty();
    this.property.categorizations = [];
  }

  areaChanged(){
    let selectedCityIndex = this.cities.findIndex(c => c.id == this.property.cityId);
    if(selectedCityIndex)
    {
      let selectedCity = this.cities[selectedCityIndex];
      this.property.latitude = selectedCity.latitude;
      this.property.longitude = selectedCity.longitude;
    }
  }

  isSelected(id : number){
    return this.property.categorizations.findIndex(c => c.propertySeekCategoryId == id) >= 0;
  }

  mapSelected(ngForm: NgForm, event:{lat:number, lng:number, radius:number}){
    this.property.latitude = event.lat;
    this.property.longitude = event.lng;
    ngForm.form.markAsDirty();
  }

  onCancel(){
    this.location.back();
  }

  toggleUseGmap(ngForm: NgForm){
    this.useGmap != this.useGmap;
    ngForm.form.markAsDirty();
  }

  onActivate(ngForm: NgForm){
    this.property.isActive = true;
    ngForm.form.markAsPristine();
    this.store.dispatch(new PropertyActions.UpdateProperty(this.property));
  }

  onDeactivate(ngForm: NgForm) {
    this.property.isActive = false;
    ngForm.form.markAsPristine();
    this.store.dispatch(new PropertyActions.UpdateProperty(this.property));
  }

  submit(ngForm: NgForm) {
    if(!this.useGmap){
      this.property.latitude = null;
      this.property.longitude = null;
    }

    if (this.id) {
      ngForm.form.markAsPristine();
      this.store.dispatch(new PropertyActions.UpdateProperty(this.property));
    }
    else {
      var formData = new FormData();

      for (let file of this.selectedFiles) {
        formData.append("files", file);
      }
      this.store.dispatch(new PropertyActions.ClearPropertySearchOptions());
      this.store.dispatch(new PropertyActions.TryAddProperty({ property: this.property, formData:this.selectedFiles.length > 0 ? formData : null }));
    }
  }

}
