import { Component, OnInit } from '@angular/core';
import { Member } from '../../../_models/Member';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-member-home',
  templateUrl: './member-home.component.html',
  styleUrls: ['./member-home.component.css']
})
export class MemberHomeComponent implements OnInit {
  member: Member;
  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.member = this.route.snapshot.data['member'];
  }
}
