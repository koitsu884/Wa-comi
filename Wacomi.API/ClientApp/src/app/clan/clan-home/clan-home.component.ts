
import {take} from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
import { Category } from '../../_models/Category';
import { ActivatedRoute } from '@angular/router';
import { ClanSeek } from '../../_models/ClanSeek';
import { City } from '../../_models/City';
import { Observable } from 'rxjs';

import * as fromClan from '../store/clan.reducers';
import * as ClanSeekActions from '../store/clan.actions';
import { Store } from '@ngrx/store';
import { AppUser } from '../../_models/AppUser';
import { Pagination } from '../../_models/Pagination';

@Component({
  selector: 'app-clan-home',
  templateUrl: './clan-home.component.html',
  styleUrls: ['./clan-home.component.css']
})


export class ClanHomeComponent implements OnInit {
  readonly CLANSEEK_MAX = 5;
  categories: Category[];
  cities: City[];
  clanSeeks: ClanSeek[];
  selectedCityId: number;
  selectedCategoryId: number;
  // baseUrl = environment.apiUrl;
  // authState: Observable<fromAccount.State>;
  appUser: AppUser;
  reachLimit: boolean;
  loading: boolean;
  // hideMine: boolean = false;
  pagingParams: any = {};
  pagination: Pagination;
  clanState: Observable<fromClan.State>;

  constructor(private route: ActivatedRoute,
    private store: Store<fromClan.FeatureState>) { }

  ngOnInit() {
    const citiesFromStore: City[] = this.route.snapshot.data['cities'];
    this.cities = citiesFromStore.slice(0, citiesFromStore.length);
    this.cities.unshift({ id: 0, name: "全て", region: "" });
    const categoriesFromStore: Category[] = this.route.snapshot.data['categories'];
    this.categories = categoriesFromStore.slice(0, categoriesFromStore.length);
    this.categories.unshift({ id: 0, name: "全て" });
    this.appUser = this.route.snapshot.data['appUser'];
    this.clanState = this.store.select('clan');

    this.store.select('clan').pipe(take(1)).subscribe((state) => {
      this.pagination = state.pagination;
      this.selectedCityId = state.selectedCityId;
      this.selectedCategoryId = state.selectedCategoryId;
      this.store.dispatch(new ClanSeekActions.SearchClanSeeks());
      if(this.appUser)
        this.store.dispatch(new ClanSeekActions.CheckClanseeksCountLimit(this.appUser.id));
    });

  }

  loadList() {
   this.store.dispatch(new ClanSeekActions.SearchClanSeeks());
  }

  filterChanged(){
    this.store.dispatch(new ClanSeekActions.SetClanSeekFilters({cityId: this.selectedCityId, categoryId: this.selectedCategoryId}));
  }

  pageChanged(event) {
    this.store.dispatch(new ClanSeekActions.SetClanSeekPage(event.page));
  }

  // toggleHideMine(){
  //   this.hideMine = !this.hideMine;
  // }
}
