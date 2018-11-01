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
import { CircleTopicCommentComponent } from './circle-topic-comment/circle-topic-comment.component';

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
    if(this.forcusCommentId)
      this.store.dispatch(new CircleTopicActions.GetForcusedTopicComment(this.forcusCommentId));
    else
      this.store.dispatch(new CircleTopicActions.GetCircleTopicCommentList({topicId: id, initPage:true}));
    this.subscription = this.store.select("circleModule").subscribe((circleModuleState) => {
      this.circleTopic = circleModuleState.circleTopic.selectedCircleTopic;
      this.circleTopicCommentList = circleModuleState.circleTopic.circleTopicCommentList;
      this.commentPagination = Object.assign({}, circleModuleState.circleTopic.commentPagination);
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

  addComment(event: { comment: string, imageFile: File }) {
    var formData = new FormData();
      if(event.imageFile)
      {
        formData.append("files", event.imageFile);
      }

    var newComment = <CircleTopicComment>{
      circleTopicId: this.circleTopic.id,
      comment: event.comment,
      appUserId: this.appUser.id,
    }

    this.store.dispatch(new GlobalActions.TryAddRecord(
      {
        recordType: "CircleTopicComment", 
        record: newComment, 
        formData: event.imageFile ? formData : null,
        callbackActions: [{type: CircleTopicActions.GET_CIRCLE_TOPIC_COMMENT_LIST, payload: {topicId: this.circleTopic.id, initPage:true}}]
      }
    ));
  }

  commentPageChanged(event) {
    this.loading = true;
    this.store.dispatch(new CircleTopicActions.SetCircleTopicCommentPage(event.page));
  }

  onAddTopicCommentReply(topicCommentId: number, comment: string) {
    if (this.appUser) {
      this.store.dispatch(new GlobalActions.TryAddRecord({
        recordType:"CircleTopicCommentReply",
        record:{commentId:topicCommentId, reply:comment, appUserId:this.appUser.id},
        callbackActions:[{type:CircleTopicActions.GET_CIRCLE_TOPIC_REPLIES, payload:topicCommentId}]
      }))
    }
  }

  onDeleteTopicCommentReply(shortComment: ShortComment) {
    this.alertify.confirm("本当にこのコメント(" + shortComment.comment + ")を削除しますか？", () => {
      this.store.dispatch(new GlobalActions.DeleteRecord({
        recordType:"CircleTopicCommentReply", 
        recordId:shortComment.id,
        callbackActions:[{type:CircleTopicActions.GET_CIRCLE_TOPIC_REPLIES, payload:shortComment.ownerRecordId}]
      }))
    })
  }

}
