import { Component, OnInit, Input } from '@angular/core';
import { TopicReply } from '../../../_models/TopicReply';
import { NgForm } from '@angular/forms';
import { Store } from '@ngrx/store';
import * as fromDailyTopic from '../../store/dailytopic.reducers';
import * as DailyTopicAction from '../../store/dailytopic.actions';
import { TopicComment } from '../../../_models/TopicComment';

@Component({
  selector: 'app-topic-reply',
  templateUrl: './topic-reply.component.html',
  styleUrls: ['./topic-reply.component.css']
})
export class TopicReplyComponent implements OnInit {
  @Input() topicReplies: TopicReply[];
  @Input() topicCommentId: number;
  @Input() currentMemberId: number;

  constructor(private store: Store<fromDailyTopic.FeatureState>) { }

  ngOnInit() {
  }

  onSubmit(form :NgForm){
      //console.log(form.value);
      this.store.dispatch(new DailyTopicAction.TryAddTopicReply({
                            topicCommentId: this.topicCommentId,
                            memberId: this.currentMemberId, 
                            reply:form.value.reply}));
  }

  onDelete(topicReply: TopicReply){
    this.store.dispatch(new DailyTopicAction.TryDeleteTopicReply(topicReply));
  }

}
