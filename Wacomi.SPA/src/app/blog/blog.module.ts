import { NgModule } from "@angular/core";
import { BlogListComponent } from "./blog-list/blog-list.component";
import { BlogEditorComponent } from "./blog-editor/blog-editor.component";
import { BlogfeedListComponent } from "./blogfeed/blogfeed-list/blogfeed-list.component";
import { BlogfeedCardComponent } from "./blogfeed/blogfeed-list/blogfeed-card/blogfeed-card.component";
import { SharedModule } from "../shared/shared.module";
import { RouterModule } from "@angular/router";

@NgModule({
    declarations: [
        BlogListComponent,
        BlogEditorComponent,
        BlogfeedListComponent,
        BlogfeedCardComponent,
    ],
    imports:[
        SharedModule,
        RouterModule
    ],
    exports:[
        BlogListComponent,
        BlogfeedListComponent
    ]
})

export class BlogModule {}