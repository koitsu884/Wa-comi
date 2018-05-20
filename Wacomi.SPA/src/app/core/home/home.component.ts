import { Component, OnInit } from '@angular/core';
import { BlogFeed } from '../../_models/BlogFeed';
import { GlobalService } from '../../_services/global.service';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  blogFeedList : Observable<BlogFeed[]>;
  constructor(private globalService: GlobalService) { }

  ngOnInit() {
    this.blogFeedList = this.globalService.getBlogFeeds()
    .catch(error => {
        console.log('Error occured when getting blog feeds');
        return Observable.of(null);
    })
  }

}
