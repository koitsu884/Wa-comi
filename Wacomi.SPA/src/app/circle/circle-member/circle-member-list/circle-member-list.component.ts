
import * as fromCircle from '../../store/circle.reducers';
import * as CircleMemberActions from '../../store/circlemember.actions';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { CircleMember, CircleRoleEnum } from '../../../_models/CircleMember';
import { Pagination } from '../../../_models/Pagination';
import { Circle } from '../../../_models/Circle';
import { AlertifyService } from '../../../_services/alertify.service';

@Component({
  selector: 'app-circle-member-list',
  templateUrl: './circle-member-list.component.html',
  styleUrls: ['./circle-member-list.component.css']
})
export class CircleMemberListComponent implements OnInit {
  // circleId: number;
  circle: Circle;
  members: CircleMember[];
  pagination: Pagination;
  roleEnum = CircleRoleEnum;

  constructor(private route: ActivatedRoute, private alertify: AlertifyService, private store: Store<fromCircle.FeatureState>) { }

  ngOnInit() {
    // this.circleId = this.route.parent.snapshot.params['id'];
    // if(this.circleId == null)
    // {
    //   this.router.navigate(['/circle']);
    //   return;
    // }
    // this.store.dispatch(new CircleMemberActions.GetCircleMemberList(this.circleId));
    this.store.select('circleModule').subscribe((circleState) => {
      this.circle = circleState.circle.selectedCircle;
      this.members = circleState.circleMember.memberList;
      this.pagination = circleState.circleMember.pagination;
    })

  }

  pageChanged(event) {
    console.log("page changed call");
    console.log(this.pagination);
    this.store.dispatch(new CircleMemberActions.SetCircleMemberPage(event.page));
  }

  onRemoveMember(member: CircleMember) {
    this.alertify.confirm("本当に除名しますか？", ()=>{
      this.store.dispatch(new CircleMemberActions.DeleteCircleMember({circleId: member.circleId, appUserId: member.appUserId}));
      this.alertify.success("除名しました");
    });
  }
}
