import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { ShortComment } from '../../_models/ShortComment';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-short-comment-form',
  templateUrl: './short-comment-form.component.html',
  styleUrls: ['./short-comment-form.component.css']
})
export class ShortCommentFormComponent implements OnInit {
  @Input() shortComments: ShortComment[];
  @Input() currentAppUserId: number;
  @Output() addShortComment = new EventEmitter<string>()
  @Output() deleteShortComment = new EventEmitter<ShortComment>();

  constructor() { }

  ngOnInit() {
  }

  onSubmit(form: NgForm) {
    this.addShortComment.emit(form.value.comment);
    form.reset();
  }

  onDelete(shortComment: ShortComment) {
    this.deleteShortComment.emit(shortComment);
  }

}
