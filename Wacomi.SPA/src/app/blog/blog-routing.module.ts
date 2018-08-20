import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { BlogfeedHomeComponent } from "./blogfeed/blogfeed-home/blogfeed-home.component";
import { BlogManagementComponent } from "./blog-management/blog-management.component";
import { AppUserResolver } from "../_resolvers/appuser.resolver";
import { BlogEditorComponent } from "./blog-editor/blog-editor.component";
import { AuthGuard } from "../_guards/auth.guard";
import { UserPhotoResolver } from "../_resolvers/userphoto.resolver";


const blogRoute: Routes = [
    {
        path: '',
        runGuardsAndResolvers: 'always',
        component:BlogManagementComponent,
        canActivate: [AuthGuard],
        resolve: {
            appUser: AppUserResolver
        }
    },
    {
        path:'feed',
        component: BlogfeedHomeComponent,
        resolve: {
            appUser: AppUserResolver
        }
    },
    { 
        path: 'edit/:id', 
        runGuardsAndResolvers: 'always',
        component: BlogEditorComponent, 
        canActivate: [AuthGuard],
    },
    { 
        path: 'add', 
        runGuardsAndResolvers: 'always',
        component: BlogEditorComponent, 
        canActivate: [AuthGuard],
    },
    //   { 
    //     path: 'editblog', 
    //     component: BlogEditorComponent, 
    //     canActivate: [AuthGuard],
    //     resolve: {
    //       photos: UserPhotoResolver
    //     }
    //   },
];

@NgModule({
    imports: [
        RouterModule.forChild(blogRoute)
    ],
    exports: [RouterModule]
})
export class BlogRoutingModule {}