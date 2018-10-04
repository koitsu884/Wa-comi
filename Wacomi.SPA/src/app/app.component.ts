import { Component, OnInit, HostListener, Inject } from '@angular/core';
import { Store } from '@ngrx/store';
import * as jwt_decode from "jwt-decode";
import * as fromApp from './store/app.reducer';
import * as AccountActions from './account/store/account.actions';
import * as GlobalActions from './store/global.actions';
import { Location } from '@angular/common';
import { Meta, Title, DOCUMENT } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  navIsFixed: boolean;

  constructor(private store:Store<fromApp.AppState>, private meta: Meta, private title: Title, public location: Location, @Inject(DOCUMENT) private document: Document) {}

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

  @HostListener("window:scroll", [])
    onWindowScroll() {
        if (window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop > 100) {
            this.navIsFixed = true;
        } else if (this.navIsFixed && window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop < 10) { this.navIsFixed = false; } } scrollToTop() { (function smoothscroll() { var currentScroll = document.documentElement.scrollTop || document.body.scrollTop; if (currentScroll > 0) {
                window.requestAnimationFrame(smoothscroll);
                window.scrollTo(0, currentScroll - (currentScroll / 5));
            }
        })();
    }
}
