import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { BsDropdownModule, defineLocale, BsLocaleService, jaLocale, TabsModule } from 'ngx-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
// import { JwtModule } from '@auth0/angular-jwt';
import { HttpClientModule } from '@angular/common/http';
//Core Modlue
import { AlertifyService } from './_services/alertify.service';
import { GlobalService } from './_services/global.service';
//import { TimeAgoPipe } from 'time-ago-pipe';
import { CityListResolver } from './_resolvers/citylist.resolver';
import { HomeTownListResolver } from './_resolvers/hometownlist.resolver';
import { ErrorInterceptorProvider } from './shared/error.interceptor';
// //AppRouteModule?
// import { appRoutes } from './routes';
import { RouterModule } from '@angular/router';
//Etc
import { SharedModule } from './shared/shared.module';
import { MembersModule } from './users/members/members.module';
import { BusinessesModule } from './users/businesses/businesses.module';
import { AccountModule } from './account/account.module';
import { AppRoutingModule } from './app-routing.module';
import { CoreModule } from './core/core.module';
import { SettingsModule } from './settings/settings.module';
import { StoreModule } from '@ngrx/store';
import { reducers } from './store/app.reducer';
import { EffectsModule } from '@ngrx/effects';
import { StoreRouterConnectingModule} from '@ngrx/router-store';
import { StoreDevtoolsModule} from '@ngrx/store-devtools';
import { AccountEffects } from './account/store/account.effects';
import { PhotoEffect } from './shared/store/photos.effect';
import { BlogEffects } from './shared/store/blogs.effect';
import { environment } from '../environments/environment';
import { AuthInterceptorProvider } from './shared/auth.interceptor';
import { AuthGuard } from './_guards/auth.guard';

defineLocale('ja', jaLocale); 

// export function getAccessToken(): string {
//   //return  localStorage.getItem('token');
//   return  localStorage.getItem('token');
// }

@NgModule({
  declarations: [
    AppComponent,
],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    CoreModule,
    SharedModule,
    SettingsModule,
    FormsModule,
    ReactiveFormsModule,
    StoreModule.forRoot(reducers),
    EffectsModule.forRoot([AccountEffects, PhotoEffect, BlogEffects]),
    StoreRouterConnectingModule,
    !environment.production ? StoreDevtoolsModule.instrument() : [],
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(),
    // JwtModule.forRoot({
    //   config: {
    //     tokenGetter: getAccessToken,
    //     whitelistedDomains: ['localhost:5000']
    //   }
    // })
  ],
  providers: [
    GlobalService,
    AlertifyService,
    CityListResolver,
    HomeTownListResolver,
    ErrorInterceptorProvider,
    AuthInterceptorProvider,
    AuthGuard,
    //Third Party
    BsLocaleService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
