import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { UserDetailComponent } from "./user-detail/user-detail.component";
import { UserDetailResolver } from "../_resolvers/user-detail.resolver";
import { AuthGuard } from "../_guards/auth.guard";
import { UserPostsComponent } from "./user-posts/user-posts.component";
import { MemberGuard } from "../_guards/member.guard";
import { AppUserGuard } from "../_guards/appuser.guard";

const usersRoutes: Routes = [

    {
        path:'detail/:id',
        // runGuardsAndResolvers: 'always',
        component: UserDetailComponent,
        canActivate: [AuthGuard],
        resolve: {
            userDetail:UserDetailResolver,
        }
    },
    {
        path:'posts/:appUserId',
        runGuardsAndResolvers: 'always',
        component: UserPostsComponent,
        canActivate: [AppUserGuard]
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(usersRoutes)
    ],
    exports: [RouterModule]
})
export class UsersRoutingModule {}