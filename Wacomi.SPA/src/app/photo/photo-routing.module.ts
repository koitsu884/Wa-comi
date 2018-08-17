import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { PhotoEditorComponent } from "./photo-editor/photo-editor.component";
import { AuthGuard } from "../_guards/auth.guard";


const photoRoute: Routes = [
    {
        path: 'edit/:recordType/:recordId',
        runGuardsAndResolvers: 'always',
        component:PhotoEditorComponent,
        canActivate: [AuthGuard],
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(photoRoute)
    ],
    exports: [RouterModule]
})
export class PhotoRoutingModule {}