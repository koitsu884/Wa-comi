import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import * as jwt_decode from "jwt-decode";
import * as fromApp from './store/app.reducer';
import * as AccountActions from './account/store/account.actions';
import * as PhotoActions from './photo/store/photos.action';
import * as GlobalActions from './store/global.actions';
import { decode } from 'punycode';
import { Route } from '@angular/compiler/src/core';
import { Location } from '@angular/common';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{

  constructor(private store:Store<fromApp.AppState>, public location: Location) {}

  ngOnInit() {
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
        // const photos = JSON.parse(localStorage.getItem('photos'));
        // const blogs = JSON.parse(localStorage.getItem('blogs'));
        this.store.dispatch(new AccountActions.Login({
                tokenString: token,
                appUser: appUser,
                account: account,
                // photos: photos == null ? [] : photos,
                // blogs: blogs == null ? [] : blogs,
                memberProfile: memberProfile == null ? null : JSON.parse(memberProfile),
                businessProfile: businessProfile == null ? null : JSON.parse(businessProfile),
                isAdmin : isAdmin
              }));
        // this.store.dispatch(new AccountActions.SetToken({token: token, appUser: appUser}));
        // this.store.dispatch(new AccountActions.SetAppUser(appUser));
//        this.store.dispatch(new PhotoActions.GetPhotos({type: appUser.userType, recordId:appUser.relatedUserClassId}));   
      }
    }

    this.store.dispatch(new GlobalActions.GetCityList());
    this.store.dispatch(new GlobalActions.GetHometownList());
    this.store.dispatch(new GlobalActions.GetClanCategoryList());

    // if (token) {
    //   this.authService.userToken = token;
    //   let appUser = JSON.parse(localStorage.getItem('appUser'));
    //   this.authService.userType = appUser.userType;
    //   this.authService.changeCurrentUserDisplayname(appUser.displayName);
    //   // this.authService.decodedToken = this.jwtHelperService.decodeToken(token);
    //   // this.authService.appUser = JSON.parse(appUser);
    // }
  }

  backClicked() {
    this.location.back();
  }
}
