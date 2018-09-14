import * as fromProperty from '../store/property.reducers';
import * as PropertyActions from '../store/property.actions';
import { Component, OnInit } from '@angular/core';
import { City } from '../../_models/City';
import { AppUser } from '../../_models/AppUser';
import { Category } from '../../_models/Category';
import { PropertySearchOptions, PropertyRequestEnum, TermEnum } from '../../_models/PropertySearchOptions';
import { Store } from '@ngrx/store';
import { ActivatedRoute } from '@angular/router';
import { GmapParameter } from '../../_models/GmapParameter';
import { Property } from '../../_models/Property';
import { Pagination } from '../../_models/Pagination';
import { BsLocaleService, BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import {jaLocale} from 'ngx-bootstrap/locale';
import { defineLocale } from 'ngx-bootstrap/chronos/locale/locales';
defineLocale("ja", jaLocale);

@Component({
  selector: 'app-property-home',
  templateUrl: './property-home.component.html',
  styleUrls: ['./property-home.component.css']
})
export class PropertyHomeComponent implements OnInit {
  properties: Property[];
  pagination: Pagination;
  cities: City[];
  categoryFilters: Category[];
  appUser: AppUser;
  termsEnum = TermEnum;
  propertyRequestEnum = PropertyRequestEnum;

  bsConfig: Partial<BsDatepickerConfig>;

  //From state
  searchParams: PropertySearchOptions;

  selectedCityId: number;
  loading: boolean;
  useGmap: boolean = false;
  advancedSearch: boolean = false;
  latitude: number;
  longitude: number;
  radius: number = 500;

  constructor(private route:ActivatedRoute,
    private localeService: BsLocaleService,
     private store: Store<fromProperty.FeatureState>) { }

  ngOnInit() {
    this.bsConfig = {
      dateInputFormat: "YYYY年MM月DD日"
    };
    this.localeService.use('ja');

    this.appUser = this.route.snapshot.data['appUser'];
    const citiesFromStore: City[] = this.route.snapshot.data['cities'];
    this.cities = citiesFromStore.slice(0, citiesFromStore.length);
    // let otherIndex = this.cities.findIndex(c => c.name == 'その他');
    // if(otherIndex)
    //   this.cities.splice(otherIndex, 1);
    this.cities.unshift({ id: 0, name: "全て", region: "" });
    this.categoryFilters = this.route.snapshot.data['categories'];
    
    // this.clearSearchFilter();
    this.loading = true;
    this.store.dispatch(new PropertyActions.SearchProperties());
    
    this.store.select("property").subscribe((propertyState) => {
      this.searchParams = Object.assign({},propertyState.searchParams);
      this.properties = propertyState.propertiesFound;
      this.pagination = propertyState.pagination;
      this.loading = false;
    })
  }

  clearSearchFilter() {
    this.searchParams = <PropertySearchOptions>{
      appUserId: this.appUser ? this.appUser.id : null,
      categoryIds: [],
      cityId: 0,
      pet: 0,
      child: 0,
    }
  }

  toggleCategory(id: number) {
    let index = this.searchParams.categoryIds.findIndex(c => c == id);
    if (index < 0)
      this.searchParams.categoryIds.push(id);
    else
      this.searchParams.categoryIds.splice(index, 1);
  }

  toggleAddvancedSearch(){
    this.advancedSearch != this.advancedSearch;
  }

  areaChanged(){
    let selectedCityIndex = this.cities.findIndex(c => c.id == this.searchParams.cityId);
    if(selectedCityIndex)
    {
      let selectedCity = this.cities[selectedCityIndex];
      this.latitude = selectedCity.latitude;
      this.longitude = selectedCity.longitude;
    }
  }

  toggleUseGmap(){
    if(!this.useGmap)
    {
      this.searchParams.area_bottom =null;
      this.searchParams.area_top =null;
      this.searchParams.area_right =null;
      this.searchParams.area_left =null;
    }
  }

  mapSelected(event:GmapParameter){
    this.longitude = event.lng;
    this.latitude = event.lat;
    this.radius = event.radius;
    this.searchParams.area_top = event.area_top;
    this.searchParams.area_left = event.area_left;
    this.searchParams.area_right = event.area_right;
    this.searchParams.area_bottom = event.area_bottom;
  }

  clearCategory() {
    this.searchParams.categoryIds = [];
  }

  isSelected(id: number) {
    return this.searchParams.categoryIds.findIndex(c => c == id) >= 0;
  }

  onSearch(){
    this.loading = true;
    this.store.dispatch(new PropertyActions.SetPropertySearchOptions(this.searchParams));
  }
}
