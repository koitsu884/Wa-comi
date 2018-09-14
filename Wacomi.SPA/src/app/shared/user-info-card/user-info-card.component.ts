import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { AppUser } from '../../_models/AppUser';
import * as fromApp from '../../store/app.reducer';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-user-info-card',
  templateUrl: './user-info-card.component.html',
  styleUrls: ['./user-info-card.component.css']
})
export class UserInfoCardComponent implements OnInit {
  @Input() appUser: AppUser;
  @Input() dateUpdate: Date;
  @Input() dateCreated: Date;
  @Output() messageSend = new EventEmitter();
  isMine: boolean = false;
  canSendMessage: boolean = false;

  constructor(private store: Store<fromApp.AppState>) { }

  ngOnInit() {
    this.store.select("account").take(1).subscribe((state) => {
      if(state.appUser){
        if(state.appUser.id == this.appUser.id)
          this.isMine = true;
        else
          this.canSendMessage = true;
      }
    })
  }

  onSend(){
    this.messageSend.emit();
  }

}
