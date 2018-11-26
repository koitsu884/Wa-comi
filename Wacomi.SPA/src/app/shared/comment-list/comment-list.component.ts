import { Component, OnInit, Input, TemplateRef, OnDestroy } from '@angular/core';
import { AppUser } from '../../_models/AppUser';
import { AlertifyService } from '../../_services/alertify.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Store } from '@ngrx/store';
import * as fromApp from '../../store/app.reducer';
import * as CommentActions from './store/comment.actions';
import * as GlobalActions from '../../store/global.actions';
import { ShortComment } from '../../_models/ShortComment';
import { Pagination } from '../../_models/Pagination';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-comment-list',
  templateUrl: './comment-list.component.html',
  styleUrls: ['./comment-list.component.css']
})
export class CommentListComponent implements OnInit, OnDestroy {
  @Input() ownerRecordType: string;
  @Input() ownerRecordId: number;
  @Input() commentList: any[];
  @Input() appUser: AppUser;
  @Input() forcusCommentId: number;
  modalRef: BsModalRef;
  subscription: Subscription;
  loading: boolean;
  commentPagination: Pagination;

  constructor(private alertify: AlertifyService, private modalService: BsModalService, private store: Store<fromApp.AppState>) { }

  ngOnInit() {
    this.loading = true;
    if(this.forcusCommentId)
      this.store.dispatch(new CommentActions.GetForcusedUserComment({ownerRecordType: this.ownerRecordType, ownerRecordId: this.ownerRecordId, id: this.forcusCommentId}));
    else
      this.store.dispatch(new CommentActions.GetUserCommentList({ownerRecordType: this.ownerRecordType, ownerRecordId: this.ownerRecordId}));

      this.subscription = this.store.select("comment").subscribe((commentState) => {
        this.commentList = commentState.userCommentList;
        this.commentPagination = Object.assign({}, commentState.pagination);
        this.loading = false;
      })
  }

  ngOnDestroy(){
    this.subscription.unsubscribe();
  }

  openModal(template: TemplateRef<any>) {
     this.modalRef = this.modalService.show(template);
  }

  addComment(event: { comment: string, imageFile?: File }) {
    var formData = new FormData();
      if(event.imageFile)
      {
        formData.append("files", event.imageFile);
      }

    var newComment = {
      ownerRecordId: this.ownerRecordId,
      comment: event.comment,
      appUserId: this.appUser.id,
    }

    this.loading = true;
    this.store.dispatch(new GlobalActions.TryAddRecord(
      {
        recordType: "CircleTopicComment", 
        record: newComment, 
        formData: event.imageFile ? formData : null,
        callbackActions: [{type: CommentActions.GET_USER_COMMENT_LIST, payload: {ownerRecordType: this.ownerRecordType, ownerRecordId: this.ownerRecordId, pageNumber:1}}]
      }
    ));
  }

  commentPageChanged(event) {
    this.loading = true;
    this.store.dispatch(new CommentActions.GetUserCommentList({ownerRecordType:this.ownerRecordType, ownerRecordId: this.ownerRecordId, pageNumber:event.page}));
  }

  toggleReplyForm(userComment:any) {
    if (!userComment.displayReplies) {
      this.store.dispatch(new CommentActions.GetUserReplies({ownerRecordType : this.ownerRecordType, commentId: userComment.id}));
    }
    this.store.dispatch(new CommentActions.ToggleUserReplyForm(userComment.id));
  }

  onDelete(userComment: any) {
    this.alertify.confirm("本当にこのコメントを削除しますか？", () => {
      this.loading = true;
      this.store.dispatch(new GlobalActions.DeleteRecord({
        recordType: this.ownerRecordType,
        recordId: userComment.id,
        callbackActions: [{
           type: CommentActions.GET_USER_COMMENT_LIST,
           payload: {  ownerRecordType:this.ownerRecordType, ownerRecordId: userComment.ownerRecordId, pageNumber: 1} 
          }]
      }));
    })
  }

  onAddCommentReply(commentId: number, comment: string) {
    if (this.appUser) {
      this.store.dispatch(new GlobalActions.TryAddRecord({
        recordType:this.ownerRecordType + 'commentreply',
        record:{commentId:commentId, reply:comment, appUserId:this.appUser.id},
        callbackActions:[{type:CommentActions.GET_USER_REPLIES, payload:{ownerRecordType : this.ownerRecordType, commentId: commentId}}]
      }))
    }
  }

  onDeleteCommentReply(shortComment: ShortComment) {
    this.alertify.confirm("本当にこのコメント(" + shortComment.comment + ")を削除しますか？", () => {
      this.store.dispatch(new GlobalActions.DeleteRecord({
        recordType:this.ownerRecordType + 'commentreply', 
        recordId:shortComment.id,
        callbackActions:[{type:CommentActions.GET_USER_REPLIES, payload:{ ownerRecordType:this.ownerRecordType, commentId: shortComment.ownerRecordId}}]
      }))
      })
  }

}
