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

@NgModule({
  declarations: [
    TimeAgoPipe,
    MainphotoSelectorComponent,
    LoadingComponent
],
  imports:[
    CommonModule,
    FormsModule,
    Nl2BrPipeModule,
   // AppRoutingModule,
  ],
  exports: [
    TimeAgoPipe,
    CommonModule,
    FormsModule,
    Nl2BrPipeModule,
    LoadingComponent,
    MainphotoSelectorComponent
  ]
})
export class SharedModule {}
