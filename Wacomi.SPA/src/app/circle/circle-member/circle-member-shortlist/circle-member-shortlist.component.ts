import { Component, OnInit, Input } from '@angular/core';
import { CircleMember } from '../../../_models/CircleMember';

@Component({
  selector: 'app-circle-member-shortlist',
  templateUrl: './circle-member-shortlist.component.html',
  styleUrls: ['./circle-member-shortlist.component.css']
})
export class CircleMemberShortlistComponent implements OnInit {
  @Input() circleId: number;
  @Input() latestMemberList : CircleMember[];
  // @Input() totalMemberCnt: number;
  constructor() { }

  ngOnInit() {
  }

}
