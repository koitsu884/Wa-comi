import { NgModule } from "@angular/core";
import { BlogEditorComponent } from "./blog-editor/blog-editor.component";
import { BlogfeedListComponent } from "./blogfeed/blogfeed-list/blogfeed-list.component";
import { BlogfeedCardComponent } from "./blogfeed/blogfeed-list/blogfeed-card/blogfeed-card.component";
import { SharedModule } from "../shared/shared.module";
import { RouterModule } from "@angular/router";
import { BlogfeedHomeComponent } from "./blogfeed/blogfeed-home/blogfeed-home.component";
import { BlogManagementComponent } from "./blog-management/blog-management.component";
import { BlogRoutingModule } from "./blog-routing.module";
import { StoreModule } from "@ngrx/store";
import { EffectsModule } from "@ngrx/effects";
import { BlogEffects } from "./store/blogs.effect";
import { blogReducer } from "./store/blogs.reducers";
import { PaginationModule } from "ngx-bootstrap/pagination";
import { BlogfeedRowComponent } from "./blogfeed/blogfeed-row/blogfeed-row.component";

@NgModule({
    declarations: [
        BlogEditorComponent,
        BlogManagementComponent,
        BlogfeedHomeComponent,
        BlogfeedRowComponent
        // BlogfeedListComponent,
        // BlogfeedCardComponent,
    ],
    imports:[
        BlogRoutingModule,
        SharedModule,
        RouterModule,
        PaginationModule,
        StoreModule.forFeature('blogs', blogReducer),
        EffectsModule.forFeature([BlogEffects])
    ]
})

export class BlogModule {}