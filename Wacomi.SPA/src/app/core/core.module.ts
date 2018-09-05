import { NgModule } from '@angular/core';
import { HomeComponent } from './home/home.component';
import { HeaderComponent } from './header/header.component';
import { AppRoutingModule } from '../app-routing.module';
import { CommonModule } from '@angular/common';
import { FooterComponent } from './footer/footer.component';
import { BsDropdownModule, CollapseModule, RatingModule } from 'ngx-bootstrap';
import { SharedModule } from '../shared/shared.module';
import { BlogModule } from '../blog/blog.module';
import { LatestClanListComponent } from './home/latest-clan-list/latest-clan-list.component';
import { LatestTopiccommentListComponent } from './home/latest-topiccomment-list/latest-topiccomment-list.component';
import { StaticpageComponent } from './staticpage/staticpage.component';
import { ModalComponent } from './modal/modal.component';
import { UploadingComponent } from './modal/uploading/uploading.component';
import { NotificationComponent } from '../notification/notification.component';
import { LatestAttractionListComponent } from './home/latest-attraction-list/latest-attraction-list.component';
import { LatestAttractionReviewsComponent } from './home/latest-attraction-reviews/latest-attraction-reviews.component';

@NgModule({
  declarations: [
    HomeComponent,
    HeaderComponent,
    FooterComponent,
    ModalComponent,
    UploadingComponent,
    LatestClanListComponent,
    LatestTopiccommentListComponent,
    LatestAttractionListComponent,
    LatestAttractionReviewsComponent,
    NotificationComponent,
    StaticpageComponent
],
  imports: [
    CommonModule,
    BlogModule,
    BsDropdownModule,
    AppRoutingModule,
    SharedModule,
    CollapseModule.forRoot(),
    RatingModule.forRoot(),
  ],
  exports: [
    HeaderComponent,
    HomeComponent,
    FooterComponent,
    ModalComponent,
    LatestClanListComponent,
    UploadingComponent
  ],
  entryComponents: [UploadingComponent]
})
export class CoreModule {}
