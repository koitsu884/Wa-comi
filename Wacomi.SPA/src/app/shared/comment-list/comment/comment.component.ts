import { Component, OnInit, Input, TemplateRef, Output, EventEmitter } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { AlertifyService } from '../../../_services/alertify.service';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.css']
})
export class CommentComponent implements OnInit {
  @Input() commentRecord: any;
  @Input() appUserId: number;
  @Input() forcused: boolean = false;
  @Input() displayReplies: boolean = false;
  @Output() toggleDisplayReplies: EventEmitter<any> = new EventEmitter();
  @Output() deleteComment: EventEmitter<any> = new EventEmitter();

  modalRef: BsModalRef;
  
  constructor(private alertify: AlertifyService, private modalService: BsModalService) { }

  ngOnInit() {
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  toggleReplyForm() {
    this.forcused = false;
    this.toggleDisplayReplies.emit(this.commentRecord);
  }

  onDelete() {
    this.deleteComment.emit(this.commentRecord);
  }

  commentWithName() {
    return "<b>" + this.commentRecord.appUser.displayName + "</b>: " + this.commentRecord.comment;
  }

}
