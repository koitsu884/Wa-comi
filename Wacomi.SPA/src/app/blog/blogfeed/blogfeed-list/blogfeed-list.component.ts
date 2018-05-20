import { Component, OnInit, Input } from '@angular/core';
import { BlogFeed } from '../../../_models/BlogFeed';

@Component({
  selector: 'app-blogfeed-list',
  templateUrl: './blogfeed-list.component.html',
  styleUrls: ['./blogfeed-list.component.css']
})
export class BlogfeedListComponent implements OnInit {
  @Input() blogFeedList: BlogFeed[];
  constructor() { }

  ngOnInit() {
  }

}
