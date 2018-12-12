import { Component, OnInit, Input } from '@angular/core';
import { AttractionReview } from '../../../_models/AttractionReview';

@Component({
  selector: 'app-attraction-review-card',
  templateUrl: './attraction-review-card.component.html',
  styleUrls: ['./attraction-review-card.component.css']
})
export class AttractionReviewCardComponent implements OnInit {
  @Input() attractionReview : AttractionReview;
  starCheckedList: boolean[] = [false,false,false,false,false];

  constructor() { }

  ngOnInit() {
    for (var _i = 1; _i <= this.attractionReview.score; _i++) {
      this.starCheckedList[_i-1] = true;
    }
  }

  // starCheck(index: number){
  //   for (var _i = 0; _i < this.starCheckedList.length; _i++) {
  //     this.starCheckedList[_i] = (_i <= index);
  //   }
  // }
}
