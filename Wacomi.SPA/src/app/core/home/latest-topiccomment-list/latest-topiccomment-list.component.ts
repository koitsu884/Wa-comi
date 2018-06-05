import { Component, OnInit, Input } from '@angular/core';
import { TopicComment } from '../../../_models/TopicComment';

@Component({
  selector: 'app-latest-topiccomment-list',
  templateUrl: './latest-topiccomment-list.component.html',
  styleUrls: ['./latest-topiccomment-list.component.css']
})
export class LatestTopiccommentListComponent implements OnInit {
  @Input() latestTopicComments : TopicComment[];
  constructor() { }

  ngOnInit() {
  }


}
