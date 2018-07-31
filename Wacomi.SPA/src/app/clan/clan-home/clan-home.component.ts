import { Component, OnInit } from '@angular/core';
import { ClanSeekCategory } from '../../_models/ClanSeekCategory';
import { ActivatedRoute } from '@angular/router';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ClanSeek } from '../../_models/ClanSeek';
import { environment } from '../../../environments/environment';
import { City } from '../../_models/City';
import { CityListResolver } from '../../_resolvers/citylist.resolver';
import { AlertifyService } from '../../_services/alertify.service';
import { Observable } from 'rxjs/Observable';

import * as fromClan from '../store/clan.reducers';
import * as ClanSeekActions from '../store/clan.actions';
import { Store } from '@ngrx/store';
import { AppUser } from '../../_models/AppUser';
import { PaginatedResult, Pagination } from '../../_models/Pagination';
import { ClanCardComponent } from './clan-list/clan-card/clan-card.component';

@Component({
  selector: 'app-clan-home',
  templateUrl: './clan-home.component.html',
  styleUrls: ['./clan-home.component.css']
})


export class ClanHomeComponent implements OnInit {
  readonly CLANSEEK_MAX = 5;
  categories: ClanSeekCategory[];
  cities: City[];
  clanSeeks: ClanSeek[];
  selectedCityId: number;
  selectedCategoryId: number;
  // baseUrl = environment.apiUrl;
  // authState: Observable<fromAccount.State>;
  appUser: AppUser;
  reachLimit: boolean;
  loading: boolean;
  pagingParams: any = {};
  pagination: Pagination;
  clanState: Observable<fromClan.State>;

  constructor(private route: ActivatedRoute,
    private httpClient: HttpClient,
    private store: Store<fromClan.FeatureState>,
    private alertify: AlertifyService) { }

  ngOnInit() {
    const citiesFromStore: City[] = this.route.snapshot.data['cities'];
    this.cities = citiesFromStore.slice(0, citiesFromStore.length);
    this.cities.unshift({ id: 0, name: "全て", region: "" });
    const categoriesFromStore: ClanSeekCategory[] = this.route.snapshot.data['categories'];
    this.categories = categoriesFromStore.slice(0, categoriesFromStore.length);
    this.categories.unshift({ id: 0, name: "全て" });
    this.appUser = this.route.snapshot.data['appUser'];
    this.clanState = this.store.select('clan');

    this.store.select('clan').take(1).subscribe((state) => {
      this.pagination = state.pagination;
      this.selectedCityId = state.selectedCityId;
      this.selectedCategoryId = state.selectedCategoryId;
      this.store.dispatch(new ClanSeekActions.SearchClanSeeks());
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
}
