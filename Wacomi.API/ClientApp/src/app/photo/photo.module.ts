import { NgModule } from "@angular/core";
import { SharedModule } from "../shared/shared.module";
import { PhotoEditorComponent } from "./photo-editor/photo-editor.component";
import { PhotoViewerComponent } from "./photo-viewer/photo-viewer.component";
import { UserPhotoResolver } from "../_resolvers/userphoto.resolver";
import { PhotoRoutingModule } from "./photo-routing.module";
import { StoreModule } from "@ngrx/store";
import { EffectsModule } from "@ngrx/effects";
import { photoReducer } from "./store/photos.reducers";
import { PhotoEffect } from "./store/photos.effect";

@NgModule({
    declarations: [
        PhotoEditorComponent,
        PhotoViewerComponent,
    ],
    imports:[
        SharedModule,
        PhotoRoutingModule,
        StoreModule.forFeature('photo', photoReducer),
        EffectsModule.forFeature([PhotoEffect])
    ],
    // exports:[
    //     PhotoViewerComponent,
    //     PhotoEditorComponent,
    // ]
})

export class PhotoModule {}