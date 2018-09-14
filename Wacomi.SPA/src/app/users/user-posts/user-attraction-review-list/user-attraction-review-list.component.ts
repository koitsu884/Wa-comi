import { Component, OnInit, Input } from '@angular/core';
import { AttractionReview } from '../../../_models/AttractionReview';
import { trigger, state, style, transition, animate } from '@angular/animations';

@Component({
  selector: 'app-user-attraction-review-list',
  templateUrl: './user-attraction-review-list.component.html',
  styleUrls: ['./user-attraction-review-list.component.css'],
  animations: [
    trigger('attractionReviewCard', [
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
export class UserAttractionReviewListComponent implements OnInit {
  @Input() attractionReviews: AttractionReview[];
  constructor() { }

  ngOnInit() {
  }

}
