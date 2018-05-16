import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BsDatepickerModule, TabsModule } from 'ngx-bootstrap';
import { TimeAgoPipe } from 'time-ago-pipe';
import { PhotoEditorComponent } from './photo-editor/photo-editor.component';
import { PhotoViewerComponent } from './photo-viewer/photo-viewer.component';
import { MainphotoSelectorComponent } from './mainphoto-selector/mainphoto-selector.component';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from '../app-routing.module';
import { StoreModule } from '@ngrx/store';
import { BlogListComponent } from './blog-list/blog-list.component';
import { BlogEditorComponent } from './blog-editor/blog-editor.component';
import { BlogEditFormComponent } from './blog-edit-form/blog-edit-form.component';

@NgModule({
  declarations: [
    TimeAgoPipe,
    PhotoEditorComponent,
    PhotoViewerComponent,
    MainphotoSelectorComponent,
    BlogListComponent,
    BlogEditorComponent,
    BlogEditFormComponent
],
  imports:[
    CommonModule,
    FormsModule,
   // AppRoutingModule,
  ],
  exports: [
    TimeAgoPipe,
    CommonModule,
    FormsModule,
    PhotoEditorComponent,
    PhotoViewerComponent,
    MainphotoSelectorComponent,
    BlogListComponent,
    BlogEditorComponent
  ]
})
export class SharedModule {}
