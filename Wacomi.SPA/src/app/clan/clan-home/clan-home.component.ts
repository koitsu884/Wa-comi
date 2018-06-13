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

import * as fromAccount from '../../account/store/account.reducers';
import * as fromApp from '../../store/app.reducer';
import { Store } from '@ngrx/store';
import { AppUser } from '../../_models/AppUser';

@Component({
  selector: 'app-clan-home',
  templateUrl: './clan-home.component.html',
  styleUrls: ['./clan-home.component.css']
})
export class ClanHomeComponent implements OnInit {
  clanSeeks: ClanSeek[];
  categories: ClanSeekCategory[];
  cities: City[];
  selectedCityId: number;
  selectedCategoryId : number;
  baseUrl = environment.apiUrl;
  // authState: Observable<fromAccount.State>;
  appUser: AppUser;
  loading: boolean;

  constructor( private route: ActivatedRoute, 
              private httpClient : HttpClient, 
              private store: Store<fromApp.AppState>,
              private alertify: AlertifyService) { }

  ngOnInit() {
    const citiesFromStore : City[] = this.route.snapshot.data['cities'];
    this.cities = citiesFromStore.slice(0, citiesFromStore.length);
    this.cities.unshift({id:0, name:"全て", region:""});
    const categoriesFromStore : ClanSeekCategory[] = this.route.snapshot.data['categories'];
    this.categories = categoriesFromStore.slice(0, categoriesFromStore.length);
    this.categories.unshift({id:0, name:"全て"});
    this.selectedCategoryId = 0;
    this.selectedCityId = 0;
    // this.authState = this.store.select('account');
    this.appUser = this.route.snapshot.data['appUser'];
    this.loadList();
  }

  loadList(){
    this.loading = true;
    let Params = new HttpParams();
    if(this.selectedCategoryId > 0)
      Params = Params.append('categoryId', this.selectedCategoryId.toString());
    if(this.selectedCityId > 0)
      Params = Params.append('cityId', this.selectedCityId.toString());
    
    this.httpClient.get<ClanSeek[]>(this.baseUrl + 'clanseek' , { params: Params })
                    .subscribe((result) => {
                      this.clanSeeks = result;
                      this.loading = false;
                    }, (error) => {
                      this.alertify.error(error);
                      this.loading = false;
                    });
  }
}
