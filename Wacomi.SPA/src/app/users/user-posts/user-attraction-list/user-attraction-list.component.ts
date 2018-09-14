import { Component, OnInit, Input } from '@angular/core';
import { Attraction } from '../../../_models/Attraction';
import { state, trigger, style, transition, animate } from '@angular/animations';

@Component({
  selector: 'app-user-attraction-list',
  templateUrl: './user-attraction-list.component.html',
  styleUrls: ['./user-attraction-list.component.css'],
  animations: [
    trigger('attractionCard', [
      state('in', style({
        opacity: 1,
        transform: 'scale(1)'
      })),
      transition('void => *', [
        style({
          opacity: 0,
          transform: 'scale(0.1)'
        }),
        animate(200)
      ])
    ])
  ],
})
export class UserAttractionListComponent implements OnInit {
  @Input() attractionList: Attraction[];
  constructor() { }

  ngOnInit() {
  }

}
