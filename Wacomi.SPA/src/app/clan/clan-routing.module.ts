import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { ClanHomeComponent } from "./clan-home/clan-home.component";
import { CityListResolver } from "../_resolvers/citylist.resolver";
import { ClanSeekCategoryResolver } from "../_resolvers/clanseek-categories.resolver";



const clanRoute: Routes = [
    {
        path: '',
        runGuardsAndResolvers: 'always',
        component:ClanHomeComponent,
        resolve: {
            
            // appUser:AppUserEditResolver,
            // bisUser:BusinessEditResolver,
            // member:MemberEditResolver,
            cities:CityListResolver,
            categories:ClanSeekCategoryResolver,
            //hometowns:HomeTownListResolver
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