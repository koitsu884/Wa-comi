import { Component, OnInit, Input } from '@angular/core';
import { trigger, state, transition, style, animate } from '@angular/animations';
import { AttractionReview } from '../../../_models/AttractionReview';

@Component({
  selector: 'app-latest-attraction-reviews',
  templateUrl: './latest-attraction-reviews.component.html',
  styleUrls: ['./latest-attraction-reviews.component.css']
})
export class LatestAttractionReviewsComponent implements OnInit {
  @Input() latestAttractionReviews: AttractionReview[];
  maxLength: number;
  constructor() { }

  ngOnInit() {
    this.maxLength = window.screen.width < 500 ? 100 : 300;
  }
}
