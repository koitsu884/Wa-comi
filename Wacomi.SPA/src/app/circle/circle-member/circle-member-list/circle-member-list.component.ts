
import * as fromCircle from '../../store/circle.reducers';
import * as CircleActions from '../../store/circle.actions';
import * as CircleMemberActions from '../../store/circlemember.actions';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { CircleMember, CircleRoleEnum } from '../../../_models/CircleMember';
import { Pagination } from '../../../_models/Pagination';
import { Circle } from '../../../_models/Circle';
import { AlertifyService } from '../../../_services/alertify.service';
import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: 'app-circle-member-list',
  templateUrl: './circle-member-list.component.html',
  styleUrls: ['./circle-member-list.component.css']
})
export class CircleMemberListComponent implements OnInit, OnDestroy {
  circleId: number;
  circle: Circle;
  members: CircleMember[];
  pagination: Pagination;
  roleEnum = CircleRoleEnum;
  subscription: Subscription;

  constructor(private route: ActivatedRoute, private alertify: AlertifyService, private router: Router, private store: Store<fromCircle.FeatureState>) { }

  ngOnInit() {
     this.circleId = this.route.parent.snapshot.params['id'];
    if(this.circleId == null)
    {
      this.router.navigate(['/circle']);
      return;
    }
    this.store.dispatch(new CircleMemberActions.GetCircleMemberList({circleId: this.circleId, initPage: true}));
    this.subscription = this.store.select('circleModule').subscribe((circleState) => {
      this.circle = circleState.circle.selectedCircle;
      this.members = circleState.circleMember.memberList;
      this.pagination = circleState.circleMember.pagination;
    })

  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  pageChanged(event) {
    this.store.dispatch(new CircleMemberActions.SetCircleMemberPage(event.page));
  }

  onRemoveMember(member: CircleMember) {
    this.alertify.confirm("本当に除名しますか？", ()=>{
      this.store.dispatch(new CircleMemberActions.DeleteCircleMember({circleId: member.circleId, appUserId: member.appUserId}));
      this.alertify.success("除名しました");
    });
  }
}
