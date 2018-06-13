import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Router, ActivatedRoute, Params } from '@angular/router';

import * as fromMessage from '../../store/message.reducers';
import * as fromApp from '../../../store/app.reducer';
import * as MessageActions from '../../store/message.actions';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-message-sentlist',
  templateUrl: './message-sentlist.component.html',
  styleUrls: ['./message-sentlist.component.css']
})
export class MessageSentlistComponent implements OnInit {
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
          this.store.dispatch(new MessageActions.GetMessagesSent(this.appUserId));
        }
      );
  }

}
