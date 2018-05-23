import { NgModule } from "@angular/core";
import { SharedModule } from "../shared/shared.module";
import { PhotoEditorComponent } from "./photo-editor/photo-editor.component";
import { PhotoViewerComponent } from "./photo-viewer/photo-viewer.component";
import { UserPhotoResolver } from "../_resolvers/userphoto.resolver";

@NgModule({
    declarations: [
        PhotoEditorComponent,
        PhotoViewerComponent,
    ],
    imports:[
        SharedModule,
    ],
    exports:[
        PhotoViewerComponent,
        PhotoEditorComponent,
    ]
})

export class PhotoModule {}