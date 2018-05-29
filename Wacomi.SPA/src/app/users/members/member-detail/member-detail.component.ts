import { Component, OnInit, Input } from '@angular/core';
import { Member } from '../../../_models/Member';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  @Input() member: Member;
  
  constructor(private route: ActivatedRoute, private location: Location) { }

  ngOnInit() {
    this.member = this.route.snapshot.data['member'];
  }

  backClicked() {
    this.location.back();
  }
}
