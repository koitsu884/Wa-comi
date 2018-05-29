import { Component, OnInit } from '@angular/core';
import { DailyTopic } from '../../_models/DailyTopic';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../../_services/alertify.service';
import { Store } from '@ngrx/store';

import * as fromDailyTopic from '../store/dailytopic.reducers';
import * as TopicActions from '../store/dailytopic.actions';
import { Observable } from 'rxjs/Observable';
import { AppUser } from '../../_models/AppUser';

@Component({
  selector: 'app-topic-list',
  templateUrl: './topic-list.component.html',
  styleUrls: ['./topic-list.component.css']
})
export class TopicListComponent implements OnInit {
  //dailyTopicList: DailyTopic[];
  dailyTopicState: Observable<fromDailyTopic.State>;
  userId: string;


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

  sendLike(id: number){
    // this.alertify.message("Like!! " + id);
    this.store.dispatch(new TopicActions.LikeTopic({supportUserId:this.userId, dailyTopicId:id}));
  }
}
