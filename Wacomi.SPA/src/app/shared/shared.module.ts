import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BsDatepickerModule, TabsModule } from 'ngx-bootstrap';
import { TimeAgoPipe } from 'time-ago-pipe';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from '../app-routing.module';
import { StoreModule } from '@ngrx/store';
import { MainphotoSelectorComponent } from './mainphoto-selector/mainphoto-selector.component';
import {Nl2BrPipeModule} from 'nl2br-pipe';
import { LoadingComponent } from './loading/loading.component';
import { ClanCardComponent } from '../clan/clan-home/clan-list/clan-card/clan-card.component';
import { BlogfeedListComponent } from '../blog/blogfeed/blogfeed-list/blogfeed-list.component';
import { BlogfeedCardComponent } from '../blog/blogfeed/blogfeed-list/blogfeed-card/blogfeed-card.component';
import { ShortCommentFormComponent } from './short-comment-form/short-comment-form.component';
import { PhotoSelectorComponent } from './photo-selector/photo-selector.component';
import { NgxGalleryModule } from 'ngx-gallery';
import { AttractionCardComponent } from '../attraction/attraction-card/attraction-card.component';
import { StarRateComponent } from './star-rate/star-rate.component';
import { UserCommentComponent } from './user-comment/user-comment.component';
import { RateAverageComponent } from './rate-average/rate-average.component';
import { GmapSelectorComponent } from './gmap-selector/gmap-selector.component';
import { AgmCoreModule } from '@agm/core';

@NgModule({
  declarations: [
    TimeAgoPipe,
    MainphotoSelectorComponent,
    LoadingComponent,
    ClanCardComponent,
    AttractionCardComponent,
    BlogfeedListComponent,
    BlogfeedCardComponent,
    ShortCommentFormComponent,
    PhotoSelectorComponent,
    StarRateComponent,
    UserCommentComponent,
    RateAverageComponent,
    GmapSelectorComponent,
],
  imports:[
    CommonModule,
    FormsModule,
    Nl2BrPipeModule,
    NgxGalleryModule,
    RouterModule,
    AgmCoreModule
    // AppRoutingModule,
   // AppRoutingModule,
  ],
  exports: [
    TimeAgoPipe,
    CommonModule,
    ClanCardComponent,
    AttractionCardComponent,
    FormsModule,
    Nl2BrPipeModule,
    NgxGalleryModule,
    LoadingComponent,
    MainphotoSelectorComponent,
    BlogfeedListComponent,
    BlogfeedCardComponent,
    ShortCommentFormComponent,
    PhotoSelectorComponent,
    StarRateComponent,
    UserCommentComponent,
    RateAverageComponent,
    GmapSelectorComponent
  ]
})
export class SharedModule {}
