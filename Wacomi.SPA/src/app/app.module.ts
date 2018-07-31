import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { BsDropdownModule, defineLocale, BsLocaleService, jaLocale, TabsModule, PaginationModule } from 'ngx-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AlertifyService } from './_services/alertify.service';
import { GlobalService } from './_services/global.service';
import { CityListResolver } from './_resolvers/citylist.resolver';
import { HomeTownListResolver } from './_resolvers/hometownlist.resolver';
import { ErrorInterceptorProvider } from './shared/error.interceptor';
//Etc
import { SharedModule } from './shared/shared.module';
import { AppRoutingModule } from './app-routing.module';
import { CoreModule } from './core/core.module';
import { BlogModule } from './blog/blog.module';
import { PhotoModule } from './photo/photo.module';

import { StoreModule } from '@ngrx/store';
import { reducers } from './store/app.reducer';
import { EffectsModule } from '@ngrx/effects';
import { StoreRouterConnectingModule} from '@ngrx/router-store';
import { StoreDevtoolsModule} from '@ngrx/store-devtools';
import { AccountEffects } from './account/store/account.effects';
import { PhotoEffect } from './photo/store/photos.effect';
import { BlogEffects } from './blog/store/blogs.effect';

import { environment } from '../environments/environment';

import { AuthInterceptorProvider } from './shared/auth.interceptor';
import { AuthGuard } from './_guards/auth.guard';
import { GlobalEffect } from './store/global.effects';
import { UserPhotoResolver } from './_resolvers/userphoto.resolver';
import { AppUserResolver } from './_resolvers/appuser.resolver';
import { StaticPageResolver } from './_resolvers/staticpage.resolver';
import { MemberIdGuard } from './_guards/memberid.guard';
import { MemberGuard } from './_guards/member.guard';
import { MessageEffects } from './message/store/message.effects';
import { MessageService } from './_services/message.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoadingComponent } from './shared/loading/loading.component';


defineLocale('ja', jaLocale); 

@NgModule({
  declarations: [
    AppComponent
],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    AppRoutingModule,
    CoreModule,
    ReactiveFormsModule,
    SharedModule,
    BlogModule,
    PhotoModule,
    StoreModule.forRoot(reducers),
    EffectsModule.forRoot([AccountEffects, PhotoEffect, GlobalEffect, MessageEffects]),
    StoreRouterConnectingModule,
    !environment.production ? StoreDevtoolsModule.instrument() : [],
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(),
    PaginationModule.forRoot(),
  ],
  providers: [
    GlobalService,
    MessageService,
    AlertifyService,
    CityListResolver,
    HomeTownListResolver,
    UserPhotoResolver,
    AppUserResolver,
    StaticPageResolver,
    ErrorInterceptorProvider,
    AuthInterceptorProvider,
    AuthGuard,
    MemberGuard,
    MemberIdGuard,
    //Third Party
    BsLocaleService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
