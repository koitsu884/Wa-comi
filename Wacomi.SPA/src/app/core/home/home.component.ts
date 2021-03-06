import { Component, OnInit } from '@angular/core';
import { BlogFeed } from '../../_models/BlogFeed';
import { GlobalService } from '../../_services/global.service';
import { Observable } from 'rxjs';
import { ClanSeek } from '../../_models/ClanSeek';
import { TopicComment } from '../../_models/TopicComment';

import * as fromApp from '../../store/app.reducer';
import * as fromAccount from '../../account/store/account.reducers';
import { Store } from '@ngrx/store';
import { Attraction } from '../../_models/Attraction';
import { AttractionReview } from '../../_models/AttractionReview';
import { Property } from '../../_models/Property';
import { Circle } from '../../_models/Circle';
import { CircleEvent } from '../../_models/CircleEvent';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
    blogFeedList: BlogFeed[];
    latestAttractionList: Attraction[];
    latestAttractionReviews: AttractionReview[];
    latestClanSeekList: ClanSeek[];
    latestCircleList: Circle[];
    latestCircleEventList: CircleEvent[];
    latestTopicComments: TopicComment[];
    latestPropertyList: Property[];
    authState: Observable<fromAccount.State>;
    todaysTopic: string;
    constructor(private store: Store<fromApp.AppState>, private globalService: GlobalService) { }

    ngOnInit() {
        this.authState = this.store.select('account');
        
        this.globalService.getLatestCircleList()
        .subscribe((result) => {
            this.latestCircleList = result;
        }, (error) => {
            this.latestCircleList = [];
            console.log('Error occured when getting latest circle list');
        });

        this.globalService.getLatestCircleEventList()
        .subscribe((result) => {
            this.latestCircleEventList = result;
        }, (error) => {
            this.latestCircleEventList = [];
            console.log('Error occured when getting latest circle event list');
        });

        this.globalService.getLatestClanSeekList()
        .subscribe((result) => {
            this.latestClanSeekList = result;
        }, (error) => {
            this.latestClanSeekList = [];
            console.log('Error occured when getting latest clan list');
        });

        this.globalService.getLatestAttractionList()
            .subscribe((result) => {
                this.latestAttractionList = result;
            }, (error) => {
                this.latestAttractionList = [];
                console.log('Error occured when getting latest attraction list');
            });

        this.globalService.getLatestAttractionReviews()
            .subscribe((result) => {
                this.latestAttractionReviews = result;
            }, (error) => {
                this.latestAttractionReviews = [];
                console.log('Error occured when getting latest attraction list');
            });

        this.globalService.getLatestRecords('property')
            .subscribe((result) => {
                this.latestPropertyList = result;
            }, (error) => {
                console.log('Error occured when getting blog feeds');
                this.latestPropertyList = [];
            });
        this.globalService.getLatestBlogFeeds()
            .subscribe((result) => {
                this.blogFeedList = result;
            }, (error) => {
                console.log('Error occured when getting blog feeds');
                this.blogFeedList = [];
            });

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
