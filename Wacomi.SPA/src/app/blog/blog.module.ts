import { NgModule } from "@angular/core";
import { BlogListComponent } from "./blog-list/blog-list.component";
import { BlogEditorComponent } from "./blog-editor/blog-editor.component";
import { BlogEditFormComponent } from "./blog-edit-form/blog-edit-form.component";
import { BlogfeedListComponent } from "./blogfeed/blogfeed-list/blogfeed-list.component";
import { BlogfeedCardComponent } from "./blogfeed/blogfeed-list/blogfeed-card/blogfeed-card.component";
import { SharedModule } from "../shared/shared.module";

@NgModule({
    declarations: [
        BlogListComponent,
        BlogEditorComponent,
        BlogEditFormComponent,
        BlogfeedListComponent,
        BlogfeedCardComponent,
    ],
    imports:[
        SharedModule,
    ],
    exports:[
        BlogListComponent,
        BlogfeedListComponent
    ]
})

export class BlogModule {}