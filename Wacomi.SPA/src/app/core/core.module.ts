import { NgModule } from '@angular/core';
import { HomeComponent } from './home/home.component';
import { HeaderComponent } from './header/header.component';
import { AppRoutingModule } from '../app-routing.module';
import { CommonModule } from '@angular/common';
import { FooterComponent } from './footer/footer.component';
import { BsDropdownModule, CollapseModule } from 'ngx-bootstrap';
import { SharedModule } from '../shared/shared.module';
import { BlogModule } from '../blog/blog.module';
import { LatestClanListComponent } from './home/latest-clan-list/latest-clan-list.component';
import { LatestTopiccommentListComponent } from './home/latest-topiccomment-list/latest-topiccomment-list.component';

@NgModule({
  declarations: [
    HomeComponent,
    HeaderComponent,
    FooterComponent,
    LatestClanListComponent,
    LatestTopiccommentListComponent
],
  imports: [
    CommonModule,
    BlogModule,
    BsDropdownModule,
    AppRoutingModule,
    SharedModule,
    CollapseModule.forRoot()
  ],
  exports: [
    HeaderComponent,
    HomeComponent,
    FooterComponent,
    LatestClanListComponent
  ]
})
export class CoreModule {}
