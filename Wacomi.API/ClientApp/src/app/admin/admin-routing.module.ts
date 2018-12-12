import { NgModule } from "@angular/core";
import { AdminGuard } from "../_guards/admin.guard";
import { Routes, RouterModule } from "@angular/router";
import { AdminHomeComponent } from "./admin-home/admin-home.component";


const adminRoot: Routes = [
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AdminGuard],
        component:AdminHomeComponent,
    },
];

@NgModule({
    imports: [
        RouterModule.forChild(adminRoot)
    ],
    exports: [RouterModule]
})
export class AdminRoutingModule {}