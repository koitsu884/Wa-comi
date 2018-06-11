import { Component, OnInit, ComponentFactoryResolver, OnDestroy } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import * as fromDailyTopic from '../store/dailytopic.reducers';
import * as TopicActions from '../store/dailytopic.actions';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../../_services/alertify.service';
import { Store } from '@ngrx/store';
import { GlobalService } from '../../_services/global.service';
import { TopicCommentFeel } from '../../_models/TopicCommentFeel';
import { AppUser } from '../../_models/AppUser';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-topic-comment-list',
  templateUrl: './topic-comment-list.component.html',
  styleUrls: ['./topic-comment-list.component.css']
})
export class TopicCommentListComponent implements OnInit, OnDestroy {
  todaysTopic : string;
  dailyTopicState: Observable<fromDailyTopic.State>;
  test : TopicCommentFeel[];
  appUser: AppUser;
  commentFeelingEnum: any[];

  constructor(private route: ActivatedRoute,
    private alertify: AlertifyService,
    private globalService: GlobalService,
    private store: Store<fromDailyTopic.FeatureState>) { }

  ngOnInit() {
    this.appUser = this.route.snapshot.data['appUser'];
    console.log(this.appUser);
    this.commentFeelingEnum = this.globalService.getFeelings();
    // console.log(this.commentFeelingEnum);
    this.dailyTopicState = this.store.select('dailytopic');
    // console.log("memberId?" + this.appUser.relatedUserClassId);
    let memberId = this.appUser ? this.appUser.userProfileId : null;
    this.store.dispatch(new TopicActions.GetTopicComments(memberId));
    
    // this.store.dispatch(new TopicActions.GetCommentFeelings(this.appUser.relatedUserClassId));
    this.globalService.getTodaysTopic()
    .subscribe((topic) => {
      this.todaysTopic = topic;
    }, (error) => {
      console.log('Error occured when getting todays topic');
    })
  }

  ngOnDestroy(){
    this.store.dispatch(new TopicActions.TopicClear());
  }

  submit(form: NgForm){
      console.log(form.value);
      this.store.dispatch(new TopicActions.TryAddTopicComment({memberId: this.appUser.userProfileId, comment: form.value.comment, topicTitle:this.todaysTopic}));
  }
}
