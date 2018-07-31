import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { BlogFeed } from '../../../_models/BlogFeed';
import { AppUser } from '../../../_models/AppUser';

@Component({
  selector: 'app-blogfeed-row',
  templateUrl: './blogfeed-row.component.html',
  styleUrls: ['./blogfeed-row.component.css']
})
export class BlogfeedRowComponent implements OnInit {
  @Input() blogFeed: BlogFeed;
  @Input() appUser : AppUser;
  @Output() sendLike: EventEmitter<number> = new EventEmitter();
  @Output() toggleDisplayComments: EventEmitter<void> = new EventEmitter();
  
  constructor() { }

  ngOnInit() {
  }

  onSendLike(feedId: number){
    this.sendLike.emit(feedId);
  }

  onToggleDisplayComment(){
    this.toggleDisplayComments.emit();
  }

}
