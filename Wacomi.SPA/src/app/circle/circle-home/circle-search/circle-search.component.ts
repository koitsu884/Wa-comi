import { Component, OnInit, Input } from '@angular/core';
import { City } from '../../../_models/City';
import { Category } from '../../../_models/Category';
import { Circle } from '../../../_models/Circle';
import { Pagination } from '../../../_models/Pagination';
import { AppUser } from '../../../_models/AppUser';
import { CircleSearchOptions } from '../../../_models/CircleSearchOptions';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import * as fromCircle from '../../store/circle.reducers';
import * as CircleActions from '../../store/circle.actions';

@Component({
  selector: 'app-circle-search',
  templateUrl: './circle-search.component.html',
  styleUrls: ['./circle-search.component.css']
})
export class CircleSearchComponent implements OnInit {
  @Input() cities: City[];
  @Input() categories: Category[];
  @Input() appUser: AppUser;
  circles: Circle[];
  pagination: Pagination;
  loading: boolean;
  searchParam: CircleSearchOptions;

  constructor(private route: ActivatedRoute, private store: Store<fromCircle.FeatureState>) { }

  ngOnInit() {
    // const citiesFromStore: City[] = this.route.snapshot.data['cities'];
    // this.cities = citiesFromStore.slice(0, citiesFromStore.length);
    // this.cities.unshift({ id: 0, name: "全て", region: "" });
    // const categoriesFromStore: Category[] = this.route.snapshot.data['categories'];
    // this.categories = categoriesFromStore.slice(0, categoriesFromStore.length);
    // this.categories.unshift({ id: 0, name: "全て" });
    // this.appUser = this.route.snapshot.data['appUser'];

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
