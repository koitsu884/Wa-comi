import * as fromCircle from '../../store/circle.reducers';
import * as fromCircleTopic from '../../store/circletopic.reducers';
import * as CircleTopicActions from '../../store/circletopic.actions';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { Circle } from '../../../_models/Circle';
import { CircleTopic } from '../../../_models/CircleTopic';
import { Pagination } from '../../../_models/Pagination';
import { AppUser } from '../../../_models/AppUser';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-circle-topic',
  templateUrl: './circle-topic.component.html',
  styleUrls: ['./circle-topic.component.css']
})
export class CircleTopicComponent implements OnInit, OnDestroy {
  circleId: number;
  circle:Circle;
  appUser: AppUser;
  topics: CircleTopic[];
  pagination: Pagination;
  circleTopicSubscription: Subscription;

  constructor(private store: Store<fromCircle.FeatureState>, private route:ActivatedRoute, private router:Router) { }

  ngOnInit() {
    this.appUser= this.route.parent.snapshot.data['appUser'];
    this.circleId = this.route.parent.snapshot.params['id'];
    if(this.circleId == null)
    {
      this.router.navigate(['/circle']);
      return;
    }
    this.store.dispatch(new CircleTopicActions.GetCircleTopicList({circleId: this.circleId, initPage:true}));
    this.circleTopicSubscription = this.store.select('circleModule').subscribe((circleState) => {
      this.circle = circleState.circle.selectedCircle;
      this.topics = circleState.circleTopic.topicList;
      this.pagination = circleState.circleTopic.pagination;
    })
  }

  pageChanged(event) {
    this.store.dispatch(new CircleTopicActions.SetCircleTopicPage(event.page));
  }

  ngOnDestroy(): void {
    console.log("Hello?");
    this.circleTopicSubscription.unsubscribe();
  }

}
