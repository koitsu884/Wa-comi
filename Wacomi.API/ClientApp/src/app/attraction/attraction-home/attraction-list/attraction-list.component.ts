import { Component, OnInit, Input } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { Attraction } from '../../../_models/Attraction';

@Component({
  selector: 'app-attraction-list',
  templateUrl: './attraction-list.component.html',
  styleUrls: ['./attraction-list.component.css'],
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
export class AttractionListComponent implements OnInit {
  @Input() attractions: Attraction[];
  @Input() appUserId: number;
  constructor() { }

  ngOnInit() {

  }

}
