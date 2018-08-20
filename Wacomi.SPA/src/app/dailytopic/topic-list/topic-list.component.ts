import { Component, OnInit, OnDestroy } from '@angular/core';
import { DailyTopic } from '../../_models/DailyTopic';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../../_services/alertify.service';
import { Store } from '@ngrx/store';

import * as fromDailyTopic from '../store/dailytopic.reducers';
import * as TopicActions from '../store/dailytopic.actions';
import { Observable } from 'rxjs/Observable';
import { AppUser } from '../../_models/AppUser';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-topic-list',
  templateUrl: './topic-list.component.html',
  styleUrls: ['./topic-list.component.css']
})
export class TopicListComponent implements OnInit, OnDestroy {
  dailyTopicState: Observable<fromDailyTopic.State>;
  userId: number;


  constructor(private route: ActivatedRoute,
             private alertify: AlertifyService,
              private store: Store<fromDailyTopic.FeatureState>) { }

  ngOnInit() {
    this.dailyTopicState = this.store.select('dailytopic');
    this.route.params.subscribe(params => {
      this.userId = params['userId'];
      this.store.dispatch(new TopicActions.GetTopicList(this.userId));
    })
    //this.dailyTopicList = this.route.snapshot.data['topicList'];
  }

  ngOnDestroy(){
    this.store.dispatch(new TopicActions.TopicClear());
  }

  sendLike(id: number){
    this.store.dispatch(new TopicActions.LikeTopic({supportAppUserId:this.userId, dailyTopicId:id}));
  }

  onCreate(form: NgForm){
    if(this.userId)
    {
      this.store.dispatch(new TopicActions.TryAddTopic({title:form.value.newTopic, userId:this.userId}));
      form.reset();
    }
  }
}
