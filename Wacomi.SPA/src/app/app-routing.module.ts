import { NgModule } from '@angular/core';
import { Routes, RouterModule, PreloadAllModules } from '@angular/router';
import { HomeComponent } from './core/home/home.component';
import { MembersModule } from './users/members/members.module';
import { PhotoEditorComponent } from './photo/photo-editor/photo-editor.component';
import { AuthGuard } from './_guards/auth.guard';
import { BlogEditorComponent } from './blog/blog-editor/blog-editor.component';

const appRoutes: Routes = [
  { path: 'home', component: HomeComponent},
  { path: 'editphoto/:type/:recordId', component: PhotoEditorComponent, canActivate: [AuthGuard]},
  { path: 'editblog/:type/:recordId', component: BlogEditorComponent, canActivate: [AuthGuard]},
  { path: 'settings', loadChildren: './settings/settings.module#SettingsModule'},
  { path: 'clan', loadChildren: './clan/clan.module#ClanModule'},
  { path: 'admin', loadChildren: './admin/admin.module#AdminModule'},
  { path: 'member', loadChildren: './users/members/members.module#MembersModule'},
  { path: 'business', loadChildren: './users/businesses/businesses.module#BusinessesModule' },
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
