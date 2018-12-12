import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BsDatepickerModule, PaginationModule } from 'ngx-bootstrap';
import { TimeAgoPipe } from 'time-ago-pipe';
import { RouterModule } from '@angular/router';
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
import { UserCardSmallComponent } from './user-card-small/user-card-small.component';
import { ShortenPipe } from '../_pipes/shorten.pipe';
import { PhotoUrlPipe } from '../_pipes/photourl.pipe';
import { ThumbUrlPipe } from '../_pipes/thumburl.pipe';
import { IconUrlPipe } from '../_pipes/iconurl.pipe';
import { TimeAgoJpPipe } from '../_pipes/timeago_jp.pipe';
import { ClanListComponent } from '../clan/clan-home/clan-list/clan-list.component';
import { PropertyListComponent } from '../property/property-home/property-list/property-list.component';
import { UserInfoCardComponent } from './user-info-card/user-info-card.component';
import { PhotoGalleryComponent } from './photo-gallery/photo-gallery.component';
import { PropertyInternetPipe } from '../_pipes/propertyInternet.pipe';
import { PropertyGenderPipe } from '../_pipes/propertyGender.pipe';
import { PropertyTermsPipe } from '../_pipes/propertyTerms.pipe';
import { PropertyRentTypePipe } from '../_pipes/propertyRentType.pipe';
import { CircleListComponent } from '../circle/circle-list/circle-list.component';
import { CircleMemberShortlistComponent } from '../circle/circle-member/circle-member-shortlist/circle-member-shortlist.component';
import { CommentFormComponent } from './comment-form/comment-form.component';
import { TimeSpanJpPipe } from '../_pipes/timespan_jp.pipe';
import { CircleEventListComponent } from '../circle/circle-details/circle-event/circle-event-list/circle-event-list.component';
import { CommentListComponent } from './comment-list/comment-list.component';
import { CommentComponent } from './comment-list/comment/comment.component';

@NgModule({
  declarations: [
    TimeAgoPipe,
    ShortenPipe,
    PhotoUrlPipe,
    ThumbUrlPipe,
    IconUrlPipe,
    TimeAgoJpPipe,
    PropertyGenderPipe,
    PropertyTermsPipe,
    PropertyInternetPipe,
    PropertyRentTypePipe,
    TimeSpanJpPipe,

    MainphotoSelectorComponent,
    LoadingComponent,
    ClanCardComponent,
    AttractionCardComponent,
    BlogfeedListComponent,
    ClanListComponent,
    CircleEventListComponent,
    PropertyListComponent,
    CircleListComponent,
    CircleMemberShortlistComponent,
    BlogfeedCardComponent,
    CommentFormComponent,
    CommentListComponent,
    CommentComponent,
    ShortCommentFormComponent,
    PhotoSelectorComponent,
    PhotoGalleryComponent,
    StarRateComponent,
    UserCommentComponent,
    RateAverageComponent,
    GmapSelectorComponent,
    UserInfoCardComponent,
    UserCardSmallComponent
],
  imports:[
    CommonModule,
    FormsModule,
    Nl2BrPipeModule,
    NgxGalleryModule,
    RouterModule,
    AgmCoreModule,
    BsDatepickerModule.forRoot(),
    PaginationModule,
    // AppRoutingModule,
   // AppRoutingModule,
  ],
  exports: [
    TimeAgoPipe,
    ShortenPipe,
    PhotoUrlPipe,
    ThumbUrlPipe,
    IconUrlPipe,
    TimeAgoJpPipe,
    TimeSpanJpPipe,
    PropertyInternetPipe,
    PropertyGenderPipe,
    PropertyTermsPipe,
    PropertyRentTypePipe,

    CommonModule,
    ClanCardComponent,
    AttractionCardComponent,
    FormsModule,
    Nl2BrPipeModule,
    NgxGalleryModule,
    LoadingComponent,
    MainphotoSelectorComponent,
    BlogfeedListComponent,
    ClanListComponent,
    PropertyListComponent,
    CircleListComponent,
    CircleEventListComponent,
    CircleMemberShortlistComponent,
    BlogfeedCardComponent,
    CommentFormComponent,
    CommentListComponent,
    CommentComponent,
    ShortCommentFormComponent,
    PhotoSelectorComponent,
    PhotoGalleryComponent,
    StarRateComponent,
    UserCommentComponent,
    UserInfoCardComponent,
    UserCardSmallComponent,
    RateAverageComponent,
    GmapSelectorComponent,
    BsDatepickerModule,
  ]
})
export class SharedModule {}
