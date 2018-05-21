import { Component, OnInit } from '@angular/core';
import { ClanSeekCategory } from '../../_models/ClanSeekCategory';
import { ActivatedRoute } from '@angular/router';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ClanSeek } from '../../_models/ClanSeek';
import { environment } from '../../../environments/environment';
import { City } from '../../_models/City';
import { CityListResolver } from '../../_resolvers/citylist.resolver';
import { AlertifyService } from '../../_services/alertify.service';

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

  constructor( private route: ActivatedRoute, private httpClient : HttpClient, private alertify: AlertifyService) { }

  ngOnInit() {
    this.cities = this.route.snapshot.data['cities'];
    this.cities.unshift({id:null, name:"全て", region:""});
    this.categories = this.route.snapshot.data['categories'];
    this.categories.unshift({id:null, name:"全て"});
    this.loadList();
  }

  loadList(){
    let Params = new HttpParams();
    if(this.selectedCategoryId)
      Params = Params.append('categoryId', this.selectedCategoryId.toString());
    if(this.selectedCityId)
      Params = Params.append('cityId', this.selectedCityId.toString());
    
    this.httpClient.get<ClanSeek[]>(this.baseUrl + 'clanseek' , { params: Params })
                    .subscribe((result) => {
                      this.clanSeeks = result;
                    }, (error) => {
                      this.alertify.error(error);
                    });
  }
}
