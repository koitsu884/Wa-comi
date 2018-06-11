import { Component, OnInit } from '@angular/core';
import { UserDetails } from '../../_models/UserDetails';
import { MemberProfile } from '../../_models/MemberProfile';
import { BusinessProfile } from '../../_models/BusinessProfile';
import { ActivatedRoute } from '@angular/router';
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
  
  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.userDetail = this.route.snapshot.data['userDetail'];
    this.memberProfile = this.userDetail.memberProfile;
    this.businessProfile = this.userDetail.businessProfile;
  }
}
