import { Component, OnInit } from '@angular/core';
import { AppUser } from '../../_models/AppUser';
import { ActivatedRoute, ActivatedRouteSnapshot, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Pagination } from '../../_models/Pagination';
import { Message } from '../../_models/Message';

import * as fromMessage from '../store/message.reducers';
import * as fromApp from '../../store/app.reducer';
import * as MessageActions from '../store/message.actions';

@Component({
  selector: 'app-message-home',
  templateUrl: './message-home.component.html',
  styleUrls: ['./message-home.component.css']
})
export class MessageHomeComponent implements OnInit {
  appUser: AppUser;
  isSent: boolean;
  appUserId: number;
  pagination: Pagination;
  messages: Message[];

  constructor(private store: Store<fromApp.AppState>, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    this.isSent = false;
    this.appUser = this.route.snapshot.data["appUser"];
    if (!this.appUser) {
      console.log("AppUser 入ってないで");
      this.router.navigate(['/home']);
    }

    this.store.select('messages')
      .subscribe((state: fromMessage.State) => {
        this.messages = state.messages;
        this.pagination = state.pagination;
      });
    this.loadList();
  }

  onSent(){
    this.isSent = true;
    this.pagination.currentPage = 1;
    this.loadList();
  }

  onReceived() {
    this.isSent = false;
    this.pagination.currentPage = 1;
    this.loadList();
  }

  pageChanged(event) {
    this.loadList(event.page);
  }

  loadList(pageNumber?: number){
    this.store.dispatch(new MessageActions.GetMessages({
      appUserId: this.appUser.id,
      type: this.isSent ? 'sent' : 'received',
      pageNumber: pageNumber ? pageNumber : 1
    }));
  }
}
