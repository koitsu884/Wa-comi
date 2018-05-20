import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BsDatepickerModule, TabsModule } from 'ngx-bootstrap';
import { TimeAgoPipe } from 'time-ago-pipe';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from '../app-routing.module';
import { StoreModule } from '@ngrx/store';
import { MainphotoSelectorComponent } from './mainphoto-selector/mainphoto-selector.component';

@NgModule({
  declarations: [
    TimeAgoPipe,
    MainphotoSelectorComponent
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
    MainphotoSelectorComponent
  ]
})
export class SharedModule {}
