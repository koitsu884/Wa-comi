import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import * as jwt_decode from "jwt-decode";
import * as fromApp from './store/app.reducer';
import * as AccountActions from './account/store/account.actions';
import * as GlobalActions from './store/global.actions';
import { Location } from '@angular/common';
import { Meta, Title } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{

  constructor(private store:Store<fromApp.AppState>, private meta: Meta, private title: Title, public location: Location) {}

  ngOnInit() {
    this.title.setTitle("Wa-コミ | NZ情報検索・コミュニティーサイト");
    this.meta.addTag({name: 'description', content: 'ニュージーランド在住者、ワーホリメーカーの情報共有・検索・コミュニティーサイトです。'})

    const token = localStorage.getItem('token');
    if(token)
    {
      const decodedToken  = jwt_decode(token);
      // console.log(decodedToken);
      // console.log(new Date(decodedToken.exp * 1000) + "|" + new Date());
  
      if(new Date(decodedToken.exp * 1000) < new Date()){
        this.store.dispatch(new AccountActions.TokenExpired());
      }
      else{
        const appUser = JSON.parse(localStorage.getItem('appUser'));
        const account = JSON.parse(localStorage.getItem('account'));
        const memberProfile = localStorage.getItem('memberProfile');
        const businessProfile = localStorage.getItem('businessProfile');
        const isAdmin = JSON.parse(localStorage.getItem('isAdmin'));

        this.store.dispatch(new AccountActions.Login({
                tokenString: token,
                appUser: appUser,
                account: account,
                memberProfile: memberProfile == null ? null : JSON.parse(memberProfile),
                businessProfile: businessProfile == null ? null : JSON.parse(businessProfile),
                isAdmin : isAdmin
              }));
      }
    }

    this.store.dispatch(new GlobalActions.GetCityList());
    this.store.dispatch(new GlobalActions.GetHometownList());
    this.store.dispatch(new GlobalActions.GetClanCategoryList());
    this.store.dispatch(new GlobalActions.GetCircleCategoryList());
    this.store.dispatch(new GlobalActions.GetAttractionCategoryList());
    this.store.dispatch(new GlobalActions.GetPropertyCategoryList());
  }

  backClicked() {
    this.location.back();
  }
}
