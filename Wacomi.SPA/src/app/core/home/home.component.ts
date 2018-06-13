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
    //blogFeedList : Observable<BlogFeed[]>;
    blogFeedList: BlogFeed[];
    // latestClanSeekList : Observable<ClanSeek[]>;
    latestClanSeekList: ClanSeek[];
    //latestTopicComments : Observable<TopicComment[]>;
    latestTopicComments: TopicComment[];
    authState: Observable<fromAccount.State>;

    //todaysTopic: Observable<string>;
    todaysTopic: string;
    constructor(private store: Store<fromApp.AppState>, private globalService: GlobalService) { }

    ngOnInit() {
        this.authState = this.store.select('account');
        this.globalService.getBlogFeeds()
            .subscribe((result) => {
                this.blogFeedList = result;
            }, (error) => {
                console.log('Error occured when getting blog feeds');
                this.blogFeedList = [];
            });

        this.globalService.getLatestClanSeekList()
            .subscribe((result) => {
                this.latestClanSeekList = result;
            }, (error) => {
                this.latestClanSeekList = [];
                console.log('Error occured when getting latest clan list');
            })

        this.globalService.getTodaysTopic()
            .subscribe((result) => {
                this.todaysTopic = result;
            }, (error) => {
                console.log('Error occured when getting todays topic');
                this.todaysTopic = "トピックの取得に失敗しました";
            })

        this.globalService.getLatestTopicComments()
            .subscribe((result) => {
                this.latestTopicComments = result;
            }, (error) => {
                console.log('Error occured when getting topic comments');
                this.latestTopicComments = [];
            })

    }

}
