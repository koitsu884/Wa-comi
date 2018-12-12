import * as fromCircle from '../../../store/circle.reducers';
import * as CircleActions from '../../../store/circle.actions';
import * as CircleTopicActions from '../../../store/circletopic.actions';
import * as GlobalActions from '../../../../store/global.actions';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { CircleTopic } from '../../../../_models/CircleTopic';
import { CircleTopicComment } from '../../../../_models/CircleTopicComment';
import { ActivatedRoute, Params } from '@angular/router';
import { Store } from '@ngrx/store';
import { Subscription } from 'rxjs';
import { AppUser } from '../../../../_models/AppUser';
import { Pagination } from '../../../../_models/Pagination';
import { ShortComment } from '../../../../_models/ShortComment';
import { AlertifyService } from '../../../../_services/alertify.service';

@Component({
  selector: 'app-circle-topic-detail',
  templateUrl: './circle-topic-detail.component.html',
  styleUrls: ['./circle-topic-detail.component.css']
})
export class CircleTopicDetailComponent implements OnInit, OnDestroy {
  circleTopic: CircleTopic;
  circleTopicCommentList: CircleTopicComment[];
  commentPagination: Pagination;
  subscription: Subscription;
  openCommentForm: boolean = false;
  appUser: AppUser;
  loading:boolean = false;
  forcusCommentId: number;

  constructor(private route: ActivatedRoute, private store: Store<fromCircle.FeatureState>, private alertify: AlertifyService) { }

  ngOnInit() {
    let id = this.route.snapshot.params['id'];
    if (id == null) {
      console.log("No parameter was set");
      return;
    }
    this.appUser = this.route.parent.snapshot.data['appUser'];
    this.forcusCommentId = this.route.snapshot.params['forcusCommentId'];
    this.loading = true;
    this.store.dispatch(new CircleTopicActions.GetCircleTopic(id))
    this.subscription = this.store.select("circleModule").subscribe((circleModuleState) => {
      this.circleTopic = circleModuleState.circleTopic.selectedCircleTopic;
      this.loading = false;
    })
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  onDelete() {
    this.alertify.confirm("本当にこのトピックを削除しますか？", () => {
      this.store.dispatch(new GlobalActions.DeleteRecord({
        recordType:"CircleTopic", 
        recordId: this.circleTopic.id, 
        callbackLocation:'/circle/detail/' + this.circleTopic.circleId,
        callbackActions: [{type: CircleActions.GET_LATEST_CIRCLE_TOPIC_LIST, payload: this.circleTopic.circleId}]
      }));
    })
  }

}
