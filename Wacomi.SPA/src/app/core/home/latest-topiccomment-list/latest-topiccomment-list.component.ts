import { Component, OnInit, Input, trigger, state, style, transition, animate } from '@angular/core';
import { TopicComment } from '../../../_models/TopicComment';

@Component({
  selector: 'app-latest-topiccomment-list',
  templateUrl: './latest-topiccomment-list.component.html',
  animations: [
    trigger('topicComment', [
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
  styleUrls: ['./latest-topiccomment-list.component.css']
})
export class LatestTopiccommentListComponent implements OnInit {
  @Input() latestTopicComments : TopicComment[];
  constructor() { }

  ngOnInit() {
  }


}
