import { Component, OnInit, Input } from '@angular/core';
import { Attraction } from '../../../_models/Attraction';
import { trigger, state, style, transition, animate } from '@angular/animations';

@Component({
  selector: 'app-latest-attraction-list',
  templateUrl: './latest-attraction-list.component.html',
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
  styleUrls: ['./latest-attraction-list.component.css']
})
export class LatestAttractionListComponent implements OnInit {
  @Input() latestAttractionList: Attraction[];
  mobile:boolean;
  constructor() { }

  ngOnInit() {
    this.mobile = window.screen.width < 500;
  }

}
