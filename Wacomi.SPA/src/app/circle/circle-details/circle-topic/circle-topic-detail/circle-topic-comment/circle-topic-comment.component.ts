import * as fromCircle from '../../../../store/circle.reducers';
import * as CircleTopicActions from '../../../../store/circletopic.actions';
import * as GlobalActions from '../../../../../store/global.actions';
import { Component, OnInit, Input } from '@angular/core';
import { CircleTopicComment } from '../../../../../_models/CircleTopicComment';
import { AlertifyService } from '../../../../../_services/alertify.service';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-circle-topic-comment',
  templateUrl: './circle-topic-comment.component.html',
  styleUrls: ['./circle-topic-comment.component.css']
})
export class CircleTopicCommentComponent implements OnInit {
  @Input() topicComment: CircleTopicComment;
  @Input() appUserId: number;
  @Input() forcused: boolean = false;

  constructor(private alertify: AlertifyService, private store: Store<fromCircle.FeatureState>) { }

  ngOnInit() {
  }

  sendLike(topicCommentId: number) {
    // if(this.appUserId)
    //   this.store.dispatch(new DailyTopicActions.TryAddCommentFeeling({appUserId: this.appUserId, commentId: this.topicComment.id, feeling: this.commentFeelEnum["Like"]}));
  }

  toggleReplyForm(topicComment: CircleTopicComment) {
    this.forcused = false;
    if (!topicComment.displayReplies) {
      this.store.dispatch(new CircleTopicActions.GetCircleTopicReplies(topicComment.id));
    }
    this.store.dispatch(new CircleTopicActions.ToggleCircleTopicReplyForm(topicComment.id));
  }

  onDelete(topicComment: CircleTopicComment) {
    this.alertify.confirm("本当にこのコメントを削除しますか？", () => {
      this.store.dispatch(new GlobalActions.DeleteRecord({
        recordType: "CircleTopicComment",
        recordId: topicComment.id,
        callbackActions: [{
           type: CircleTopicActions.GET_CIRCLE_TOPIC_COMMENT_LIST,
           payload: { topicId: topicComment.circleTopicId, initPage: true} 
          }]
      }));
    })
  }

  commentWithName() {
    return "<b>" + this.topicComment.appUser.displayName + "</b>: " + this.topicComment.comment;
  }

}
