import * as fromAttraction from '../store/attraction.reducers';
import * as AttractionActions from '../store/attraction.actions';
import { Component, OnInit } from '@angular/core';
import { City } from '../../_models/City';
import { Pagination } from '../../_models/Pagination';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { Category } from '../../_models/Category';
import { Attraction } from '../../_models/Attraction';
import { AppUser } from '../../_models/AppUser';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-attraction-home',
  templateUrl: './attraction-home.component.html',
  styleUrls: ['./attraction-home.component.css']
})
export class AttractionHomeComponent implements OnInit {
  cities: City[];
  selectedCityId: number;
  categoryFilters: Category[];
  appUser: AppUser;
  attractions: Attraction[];
  selectedCategoryIds: number[] = [];
  pagingParams: any = {};
  pagination: Pagination;
  loading: boolean;
  onlyMine: boolean = false;
  categoryTimer: any;
  subscription: Subscription;

  constructor(private route: ActivatedRoute, private store: Store<fromAttraction.FeatureState>) {
  }

  ngOnInit() {
    // this.loading = true;
    this.appUser = this.route.snapshot.data['appUser'];
    const citiesFromStore: City[] = this.route.snapshot.data['cities'];
    this.cities = citiesFromStore.slice(0, citiesFromStore.length);
    this.cities.unshift({ id: 0, name: "全て", region: "" });
    this.categoryFilters = this.route.snapshot.data['categories'];
    //  console.log(categories);
    // this.categoryFilters = [];
    // for(let category of categories){
    //   this.categoryFilters.push({category: category, selected:false});
    // }
    this.loading = true;
    this.store.select('attraction').subscribe((state) => {
      this.attractions = state.attractions;
      this.loading = false;
    }
    );
    this.store.dispatch(new AttractionActions.SearchAttraction({ categories: [], cityId: 0, appUserId: 0 }));
  }

  toggleOnlyMine() {
    this.onlyMine != this.onlyMine;
    this.startSearch();
  }

  cityChanged() {
    this.startSearch();
  }

  toggleCategory(id: number) {
    let index = this.selectedCategoryIds.findIndex(c => c == id);
    if (index < 0)
      this.selectedCategoryIds.push(id);
    else
      this.selectedCategoryIds.splice(index, 1);

    this.setCategoryTimer();
  }

  private setCategoryTimer() {
    if (this.categoryTimer) {
      clearTimeout(this.categoryTimer);
    }

    this.categoryTimer = setTimeout(this.startSearch.bind(this), 1000);
  }

  isSelected(id: number) {
    return this.selectedCategoryIds.findIndex(c => c == id) >= 0;
  }

  clearCategory() {
    this.selectedCategoryIds = [];
    this.setCategoryTimer();
  }

  onSearch() {
    this.loading = true;
    this.startSearch();
  }

  private startSearch() {
    this.loading = true;
    this.store.dispatch(new AttractionActions.SearchAttraction({
      categories: this.selectedCategoryIds,
      cityId: this.selectedCityId,
      appUserId: this.onlyMine ? this.appUser.id : 0
    }));
  }

}
