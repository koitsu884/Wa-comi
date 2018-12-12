import { Component, OnInit, Input } from '@angular/core';
import { trigger, style, state, transition, animate } from '@angular/animations';

@Component({
  selector: 'app-user-comment',
  templateUrl: './user-comment.component.html',
  animations: [
    trigger('topicComment', [
      state('in', style({
        opacity: 1,
        transform: 'scale(1)'
      })),
      transition('void => *', [
        style({
          opacity: 0,
          transform: 'scale(0.1)'
        }),
        animate(200)
      ])
    ])
  ],
  styleUrls: ['./user-comment.component.css']
})
export class UserCommentComponent implements OnInit {
  @Input() userPhotoUrl : string;
  @Input() userName : string;
  @Input() comment: string;
  @Input() isMine: boolean;
  
  constructor() { }

  ngOnInit() {
  }

}
