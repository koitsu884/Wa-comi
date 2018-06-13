import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';

import * as fromMessage from '../../store/message.reducers';
import * as fromApp from '../../../store/app.reducer';
import * as MessageActions from '../../store/message.actions';
import { Observable } from 'rxjs/Observable';
import { ActivatedRoute, Params, Router } from '@angular/router';

@Component({
  selector: 'app-message-receivedlist',
  templateUrl: './message-receivedlist.component.html',
  styleUrls: ['./message-receivedlist.component.css']
})
export class MessageReceivedlistComponent implements OnInit {
  messageState: Observable<fromMessage.State>;
  appUserId: number;


  constructor(private store: Store<fromApp.AppState>, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    this.messageState = this.store.select('messages');
    this.route.params
      .subscribe(
        (params: Params) => {
          this.appUserId = +params['userId'];
          if (this.appUserId == null) {
            this.router.navigate(['/home']);
            return;
          }
          this.store.dispatch(new MessageActions.GetMessagesReceived(this.appUserId));
        }
      );
  }

}
