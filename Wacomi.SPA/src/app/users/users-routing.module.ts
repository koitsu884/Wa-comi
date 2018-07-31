import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { UserDetailComponent } from "./user-detail/user-detail.component";
import { UserDetailResolver } from "../_resolvers/user-detail.resolver";
import { AuthGuard } from "../_guards/auth.guard";

const usersRoutes: Routes = [

    {
        path:'detail/:id',
        runGuardsAndResolvers: 'always',
        component: UserDetailComponent,
        canActivate: [AuthGuard],
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