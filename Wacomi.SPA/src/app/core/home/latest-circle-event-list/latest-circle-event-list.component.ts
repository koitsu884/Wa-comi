import { Component, OnInit, Input } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { CircleEvent } from '../../../_models/CircleEvent';

@Component({
  selector: 'app-latest-circle-event-list',
  templateUrl: './latest-circle-event-list.component.html',
  animations: [
    trigger('eventCard', [
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
  styleUrls: ['./latest-circle-event-list.component.css']
})
export class LatestCircleEventListComponent implements OnInit {
  @Input() latestCircleEventList: CircleEvent[];
  constructor() { }

  ngOnInit() {
  }

}
