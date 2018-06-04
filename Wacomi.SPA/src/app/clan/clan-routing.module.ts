import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { ClanHomeComponent } from "./clan-home/clan-home.component";
import { CityListResolver } from "../_resolvers/citylist.resolver";
import { ClanSeekCategoryResolver } from "../_resolvers/clanseek-categories.resolver";
import { ClanDetailComponent } from "./clan-detail/clan-detail.component";
import { ClanSeekEditResolver } from "../_resolvers/clanseek-edit.resolver";
import { UserPhotoResolver } from "../_resolvers/userphoto.resolver";
import { AppUserResolver } from "../_resolvers/appuser.resolver";
import { ClanSeekResolver } from "./_resolver/clanseek.resolver";
import { ClanEditComponent } from "./clan-edit/clan-edit.component";
import { AuthGuard } from "../_guards/auth.guard";
import { MemberIdGuard } from "../_guards/memberid.guard";



const clanRoute: Routes = [
    {
        path: '',
        runGuardsAndResolvers: 'always',
        component:ClanHomeComponent,
        canActivate: [AuthGuard],
        resolve: {
            
            appUser:AppUserResolver,            
            // bisUser:BusinessEditResolver,
            // member:MemberEditResolver,
            cities:CityListResolver,
            categories:ClanSeekCategoryResolver,
            //hometowns:HomeTownListResolver
        },
       
    },
    {
        path: 'detail/:id', 
        component: ClanDetailComponent, 
        resolve: {
            appUser: AppUserResolver
        }
    },
    {
        path: 'edit/:memberId', 
        runGuardsAndResolvers: 'always',
        component: ClanEditComponent, 
        canActivate: [MemberIdGuard],
        resolve: {
            cities:CityListResolver,
            categories:ClanSeekCategoryResolver,
            photos:UserPhotoResolver,
        },
    },
    {
        path: 'edit/:memberId/:id', 
        component: ClanEditComponent, 
        canActivate: [MemberIdGuard],
        resolve: {
            cities:CityListResolver,
            categories:ClanSeekCategoryResolver,
            editingClan:ClanSeekEditResolver,
            photos:UserPhotoResolver,
        },
    },
];

@NgModule({
    imports: [
        RouterModule.forChild(clanRoute)
    ],
    exports: [RouterModule]
})
export class ClanRoutingModule {}