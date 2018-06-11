import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { UserDetailComponent } from "./user-detail/user-detail.component";
import { UserDetailResolver } from "../_resolvers/user-detail.resolver";

const usersRoutes: Routes = [

    {
        path:'detail/:id',
        runGuardsAndResolvers: 'always',
        component: UserDetailComponent,
        resolve: {
            userDetail:UserDetailResolver,
        }
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(usersRoutes)
    ],
    exports: [RouterModule]
})
export class UsersRoutingModule {}