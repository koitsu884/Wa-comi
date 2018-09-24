import * as CircleActions from '../store/circle.actions';
import * as fromCircle from '../store/circle.reducers';
import { Component, OnInit } from '@angular/core';
import { City } from '../../_models/City';
import { Category } from '../../_models/Category';
import { ActivatedRoute } from '@angular/router';
import { AppUser } from '../../_models/AppUser';
import { Store } from '@ngrx/store';
import { CircleSearchOptions } from '../../_models/CircleSearchOptions';
import { Circle } from '../../_models/Circle';
import { Pagination } from '../../_models/Pagination';

@Component({
  selector: 'app-circle-home',
  templateUrl: './circle-home.component.html',
  styleUrls: ['./circle-home.component.css']
})
export class CircleHomeComponent implements OnInit {
  cities: City[];
  categories: Category[];
  circles: Circle[];
  pagination: Pagination;
  appUser: AppUser;
  loading: boolean;
  searchParam: CircleSearchOptions;

  constructor(private route: ActivatedRoute, private store: Store<fromCircle.FeatureState>) { }

  ngOnInit() {
    const citiesFromStore: City[] = this.route.snapshot.data['cities'];
    this.cities = citiesFromStore.slice(0, citiesFromStore.length);
    this.cities.unshift({ id: 0, name: "全て", region: "" });
    const categoriesFromStore: Category[] = this.route.snapshot.data['categories'];
    this.categories = categoriesFromStore.slice(0, categoriesFromStore.length);
    this.categories.unshift({ id: 0, name: "全て" });
    this.appUser = this.route.snapshot.data['appUser'];

    this.loading = true;
    this.store.dispatch(new CircleActions.SearchCircle());
    
    this.store.select("circleModule").subscribe((circleState) => {
      this.searchParam = Object.assign({},circleState.circle.searchParam);
      this.circles = circleState.circle.circles;
      this.pagination = circleState.circle.pagination;
      this.loading = false;
    })
  }

  filterChanged(){
    this.store.dispatch(new CircleActions.SetCircleSearchOptions(this.searchParam));
  }

  pageChanged(event) {
     this.store.dispatch(new CircleActions.SetCirclePage(event.page));
  }

}
