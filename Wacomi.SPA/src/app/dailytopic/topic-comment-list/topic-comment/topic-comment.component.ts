import { Component, OnInit, Input } from '@angular/core';
import { TopicComment } from '../../../_models/TopicComment';
import { TopicCommentFeel } from '../../../_models/TopicCommentFeel';
import { Store } from '@ngrx/store';
import * as fromDailyTopic from '../../store/dailytopic.reducers';
import * as DailyTopicActions from '../../store/dailytopic.actions';
import { GlobalService } from '../../../_services/global.service';
import { AlertifyService } from '../../../_services/alertify.service';

@Component({
  selector: 'app-topic-comment',
  templateUrl: './topic-comment.component.html',
  styleUrls: ['./topic-comment.component.css']
})
export class TopicCommentComponent implements OnInit {
  @Input() topicComment : TopicComment;
  // @Input() commentFeelings: TopicCommentFeel[];
  @Input() appUserId: number;
  @Input() commentFeelEnum: any[];
  @Input() forcused: boolean = false;
  @Input() isMine: boolean;

  constructor(private store: Store<fromDailyTopic.FeatureState>, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  sendLike(topicCommentId: number){
    if(this.appUserId)
      this.store.dispatch(new DailyTopicActions.TryAddCommentFeeling({appUserId: this.appUserId, commentId: this.topicComment.id, feeling: this.commentFeelEnum["Like"]}));
  }

  toggleReplyForm(topicComment: TopicComment){
    this.forcused = false;
    if(!topicComment.displayReplies){
      this.store.dispatch(new DailyTopicActions.GetTopicReplies({commentId: topicComment.id}));
    }
    this.store.dispatch( new DailyTopicActions.ToggleReplyForm({commentId: topicComment.id}));
  }

  onDelete(topicComment: TopicComment){
    this.alertify.confirm("本当にこのコメント(" + topicComment.comment + ")を削除しますか？", () => {
      this.store.dispatch(new DailyTopicActions.TryDeleteTopicComment(topicComment.id));
    })
  }
}
