import { NgModule } from '@angular/core';
import { Routes, RouterModule, PreloadAllModules } from '@angular/router';
import { HomeComponent } from './core/home/home.component';
import { AuthGuard } from './_guards/auth.guard';
import { StaticpageComponent } from './core/staticpage/staticpage.component';
import { StaticPageResolver } from './_resolvers/staticpage.resolver';
import { ContactComponent } from './contact/contact.component';
import { NotificationComponent } from './notification/notification.component';
import { AppUserResolver } from './_resolvers/appuser.resolver';

const appRoutes: Routes = [
  { path: '', component: HomeComponent},
  { path: 'settings', loadChildren: './settings/settings.module#SettingsModule'},
  { path: 'dailytopic', loadChildren: './dailytopic/dailytopic.module#DailyTopicModule'},
  { path: 'clan', loadChildren: './clan/clan.module#ClanModule'},
  { path: 'blog', loadChildren: './blog/blog.module#BlogModule'},
  { path: 'photo', loadChildren: './photo/photo.module#PhotoModule'},
  { path: 'message', loadChildren: './message/message.module#MessageModule'},
  { path: 'attraction', loadChildren: './attraction/attraction.module#AttractionModule'},
  { path: 'admin', loadChildren: './admin/admin.module#AdminModule'},
  // { path: 'member', loadChildren: './users/members/members.module#MembersModule'},
  { path: 'users', loadChildren: './users/users.module#UsersModule'},
  // { path: 'business', loadChildren: './users/businesses/businesses.module#BusinessesModule' },
  { path: 'account', loadChildren: './account/account.module#AccountModule' },
  { path: 'contact', component: ContactComponent},
  { 
      path: 'notification', 
      component: NotificationComponent,
      canActivate: [AuthGuard],
      resolve: {
          appUser:AppUserResolver,            
      },
  },
  { path: 'home', redirectTo: '', pathMatch: 'full'},
  //Static Pages
  { path: 'static/:pageName', component: StaticpageComponent, resolve: {content: StaticPageResolver}
},
   { path: '**', redirectTo: '', pathMatch: 'full'},
];

@NgModule({
  imports: [
    RouterModule.forRoot(appRoutes, {preloadingStrategy: PreloadAllModules})
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {

}
