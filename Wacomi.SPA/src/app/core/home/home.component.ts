import { Component, OnInit } from '@angular/core';
import { BlogFeed } from '../../_models/BlogFeed';
import { GlobalService } from '../../_services/global.service';
import { Observable } from 'rxjs/Observable';
import { ClanSeek } from '../../_models/ClanSeek';
import { TopicComment } from '../../_models/TopicComment';

import * as fromApp from '../../store/app.reducer';
import * as fromAccount from '../../account/store/account.reducers';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  blogFeedList : Observable<BlogFeed[]>;
  latestClanSeekList : Observable<ClanSeek[]>;
  latestTopicComments : Observable<TopicComment[]>;
  authState: Observable<fromAccount.State>;

  todaysTopic: Observable<string>;
  constructor(private store: Store<fromApp.AppState>, private globalService: GlobalService) { }

  ngOnInit() {
    this.authState = this.store.select('account');
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

    this.latestTopicComments = this.globalService.getLatestTopicComments()
    .catch(error => {
      console.log('Error occured when getting topic comments');
      return Observable.of(null);
  })
  }

}
