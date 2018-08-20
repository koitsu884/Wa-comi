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
import { ShortComment } from '../../_models/ShortComment';

@Component({
  selector: 'app-topic-comment-list',
  templateUrl: './topic-comment-list.component.html',
  styleUrls: ['./topic-comment-list.component.css']
})
export class TopicCommentListComponent implements OnInit, OnDestroy {
  todaysTopic : string;
  loading: boolean;
  dailyTopicState: Observable<fromDailyTopic.State>;
  test : TopicCommentFeel[];
  appUser: AppUser;
  commentFeelingEnum: any[];
  alreadyTweeted: boolean;

  constructor(private route: ActivatedRoute,
    private alertify: AlertifyService,
    private globalService: GlobalService,
    private store: Store<fromDailyTopic.FeatureState>) { }

  ngOnInit() {
    this.appUser = this.route.snapshot.data['appUser'];
    this.commentFeelingEnum = this.globalService.getFeelings();
    // console.log(this.commentFeelingEnum);
    this.loading = true;
    this.dailyTopicState = this.store.select('dailytopic');
    this.dailyTopicState.subscribe((result) => {
      this.loading = false;
      this.alreadyTweeted = result.todaysComment ? true : false;
    });
    this.store.dispatch(new TopicActions.GetTopicComments(this.appUser ? this.appUser.id : null));
    
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
      this.store.dispatch(new TopicActions.TryAddTopicComment({appUserId: this.appUser.id, comment: form.value.comment, topicTitle:this.todaysTopic}));
  }

  onAddTopicCommentReply(topicCommentId: number, comment:string){
    //console.log(form.value);
    if(this.appUser)
    {
      this.store.dispatch(new TopicActions.TryAddTopicReply({
                          topicCommentId: topicCommentId,
                          appUserId: this.appUser.id, 
                          reply:comment}));
    }
}

onDeleteTopicCommentReply(shortComment: ShortComment){
  this.alertify.confirm("本当にこのコメント(" + shortComment.comment + ")を削除しますか？", () => {
    this.store.dispatch(new TopicActions.TryDeleteTopicReply({
                          topicReplyId: shortComment.id,
                          topicCommentId: shortComment.ownerRecordId
                        }));
  })
}
}
