
import {take} from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
import { UserDetails } from '../../_models/UserDetails';
import { MemberProfile } from '../../_models/MemberProfile';
import { BusinessProfile } from '../../_models/BusinessProfile';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import * as fromApp from '../../store/app.reducer';
import * as MessageActions from '../../message/store/message.actions';
import { MessageService } from '../../_services/message.service';
import { ClanSeek } from '../../_models/ClanSeek';
import { GlobalService } from '../../_services/global.service';
import { Blog } from '../../_models/Blog';

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
  clanSeeks: ClanSeek[];
  blogs: Blog[];
  
  constructor(private route: ActivatedRoute, 
    private router:Router, 
    private store: Store<fromApp.AppState>, 
    private messageService: MessageService,
    private globalService: GlobalService) { }

  ngOnInit() {
    this.userDetail = this.route.snapshot.data['userDetail'];
    this.memberProfile = this.userDetail.memberProfile;
    this.businessProfile = this.userDetail.businessProfile;
    this.globalService.getClanSeekListByUser(this.userDetail.appUser.id)
      .subscribe((clanSeeks) => {
        this.clanSeeks = clanSeeks
      });
    this.globalService.getBlogListByUser(this.userDetail.appUser.id)
      .subscribe((blogs) => {
        this.blogs = blogs;
      })
    this.store.select("account").pipe(take(1)).subscribe((account) => {
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
