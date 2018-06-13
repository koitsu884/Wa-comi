import { Component, OnInit } from '@angular/core';
import { UserDetails } from '../../_models/UserDetails';
import { MemberProfile } from '../../_models/MemberProfile';
import { BusinessProfile } from '../../_models/BusinessProfile';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import * as fromApp from '../../store/app.reducer';
import * as MessageActions from '../../message/store/message.actions';
import { MessageService } from '../../_services/message.service';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent implements OnInit {

  userDetail: UserDetails;
  displayName: string;
  memberProfile: MemberProfile;
  businessProfile: BusinessProfile;
  appUserId: number;
  
  constructor(private route: ActivatedRoute, private router:Router, private store: Store<fromApp.AppState>, private messageService: MessageService) { }

  ngOnInit() {
    this.userDetail = this.route.snapshot.data['userDetail'];
    this.memberProfile = this.userDetail.memberProfile;
    this.businessProfile = this.userDetail.businessProfile;
    this.store.select("account").take(1).subscribe((account) => {
      this.appUserId = account.appUser ? account.appUser.id : null;
    });
  }

  
  onSend(){
    this.messageService.preparSendingeMessage(
      {
        title: "",
        recipientDisplayName: this.displayName,
        recipientId: this.userDetail.appUser.id,
        senderId: this.appUserId
      },
      null
    );

    this.router.navigate(['/message/send']);
  }
}
