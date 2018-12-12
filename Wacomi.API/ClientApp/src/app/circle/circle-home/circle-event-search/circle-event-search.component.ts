import { Component, OnInit, Input } from '@angular/core';
import { AppUser } from '../../../_models/AppUser';
import { City } from '../../../_models/City';
import { Category } from '../../../_models/Category';
import { CircleEventSearchOptions } from '../../../_models/CircleEventSearchOptions';
import { Store } from '@ngrx/store';
import * as fromCircle from '../../store/circle.reducers';
import * as CircleActions from '../../store/circle.actions';

@Component({
  selector: 'app-circle-event-search',
  templateUrl: './circle-event-search.component.html',
  styleUrls: ['./circle-event-search.component.css']
})
export class CircleEventSearchComponent implements OnInit {
  @Input() cities: City[];
  @Input() categories: Category[];
  @Input() appUser: AppUser;
  fromDate: Date;
  cityId: number;
  categoryId: number;
  loading:true;

  constructor() { }

  ngOnInit() {
    this.loading = true;
    this.fromDate = new Date();
    this.cityId = null;
    this.categoryId = null;
  }
}
