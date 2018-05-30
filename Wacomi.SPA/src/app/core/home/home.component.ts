import { Component, OnInit } from '@angular/core';
import { BlogFeed } from '../../_models/BlogFeed';
import { GlobalService } from '../../_services/global.service';
import { Observable } from 'rxjs/Observable';
import { ClanSeek } from '../../_models/ClanSeek';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  blogFeedList : Observable<BlogFeed[]>;
  latestClanSeekList : Observable<ClanSeek[]>;
  todaysTopic: Observable<string>;
  constructor(private globalService: GlobalService) { }

  ngOnInit() {
    this.blogFeedList = this.globalService.getBlogFeeds()
    .catch(error => {
        console.log('Error occured when getting blog feeds');
        return Observable.of(null);
    });

    this.latestClanSeekList = this.globalService.getLatestClanSeekList()
    .catch(error => {
        console.log('Error occured when getting latest clan list');
        return Observable.of(null);
    })

    this.todaysTopic = this.globalService.getTodaysTopic()
    .catch(error => {
        console.log('Error occured when getting todays topic');
        return Observable.of(null);
    })
  }

}
