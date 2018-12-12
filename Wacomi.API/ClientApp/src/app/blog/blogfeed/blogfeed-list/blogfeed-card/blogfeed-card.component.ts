import { Component, OnInit, Input } from '@angular/core';
import { BlogFeed } from '../../../../_models/BlogFeed';

@Component({
  selector: 'app-blogfeed-card',
  templateUrl: './blogfeed-card.component.html',
  styleUrls: ['./blogfeed-card.component.css']
})
export class BlogfeedCardComponent implements OnInit {
  @Input() blogFeed: BlogFeed;
  blogImageUrl: string;
  articleImageUrl: string;
  defaultImageUrl = "assets/NoImage_Person.png";

  constructor() { }

  ngOnInit() {
    this.blogImageUrl = this.blogFeed.blogImageUrl  ? this.blogFeed.blogImageUrl : this.defaultImageUrl;
    this.articleImageUrl = this.blogFeed.photo  ? this.blogFeed.photo.url : this.blogImageUrl;
  }

}
