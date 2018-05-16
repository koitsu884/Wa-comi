import { NgModule } from '@angular/core';
import { HomeComponent } from './home/home.component';
import { HeaderComponent } from './header/header.component';
import { AppRoutingModule } from '../app-routing.module';
import { CommonModule } from '@angular/common';
import { FooterComponent } from './footer/footer.component';
import { BsDropdownModule, BsDatepickerModule, TabsModule } from 'ngx-bootstrap';

@NgModule({
  declarations: [
    HomeComponent,
    HeaderComponent,
    FooterComponent
  ],
  imports: [
    CommonModule,
    BsDropdownModule,
    AppRoutingModule,
  ],
  exports: [
    HeaderComponent,
    HomeComponent,
    FooterComponent,
  ]
})
export class CoreModule {}
