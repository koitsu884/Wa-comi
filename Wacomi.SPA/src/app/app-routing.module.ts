import { NgModule } from '@angular/core';
import { Routes, RouterModule, PreloadAllModules } from '@angular/router';
import { HomeComponent } from './core/home/home.component';
import { PhotoEditorComponent } from './photo/photo-editor/photo-editor.component';
import { AuthGuard } from './_guards/auth.guard';
import { BlogEditorComponent } from './blog/blog-editor/blog-editor.component';
import { UserPhotoResolver } from './_resolvers/userphoto.resolver';
import { UserBlogResolver } from './_resolvers/userblog.resolver';

const appRoutes: Routes = [
  { path: 'home', component: HomeComponent},
  { path: 'editphoto', component: PhotoEditorComponent, canActivate: [AuthGuard]},
  { 
    path: 'editblog/:id', 
    component: BlogEditorComponent, 
    canActivate: [AuthGuard],
    resolve: {
      photos: UserPhotoResolver
    }
  },
  { 
    path: 'editblog', 
    component: BlogEditorComponent, 
    canActivate: [AuthGuard],
    resolve: {
      photos: UserPhotoResolver
    }
  },
  { path: 'settings', loadChildren: './settings/settings.module#SettingsModule'},
  { path: 'dailytopic', loadChildren: './dailytopic/dailytopic.module#DailyTopicModule'},
  { path: 'clan', loadChildren: './clan/clan.module#ClanModule'},
  { path: 'message', loadChildren: './message/message.module#MessageModule'},
  { path: 'admin', loadChildren: './admin/admin.module#AdminModule'},
  // { path: 'member', loadChildren: './users/members/members.module#MembersModule'},
  { path: 'users', loadChildren: './users/users.module#UsersModule'},
  // { path: 'business', loadChildren: './users/businesses/businesses.module#BusinessesModule' },
  { path: 'account', loadChildren: './account/account.module#AccountModule' },
  { path: '**', redirectTo: 'home', pathMatch: 'full'},
];

@NgModule({
  imports: [
    RouterModule.forRoot(appRoutes, {preloadingStrategy: PreloadAllModules})
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {

}
