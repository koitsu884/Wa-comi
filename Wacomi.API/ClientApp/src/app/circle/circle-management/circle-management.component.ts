import * as fromCircle from '../store/circle.reducers';
import * as CircleManagementActions from '../store/circle-management.actions';
import { Component, OnInit } from '@angular/core';
import { Circle } from '../../_models/Circle';
import { AppUser } from '../../_models/AppUser';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Pagination } from '../../_models/Pagination';

@Component({
  selector: 'app-circle-management',
  templateUrl: './circle-management.component.html',
  styleUrls: ['./circle-management.component.css']
})
export class CircleManagementComponent implements OnInit {
  appUser: AppUser;
  myCircleList: Circle[];
  myCirclePagination: Pagination;
  ownCircleList: Circle[];
  ownCirclePagination: Pagination;

  constructor(private route: ActivatedRoute, private router: Router, private store: Store<fromCircle.FeatureState>) { }

  ngOnInit() {
    this.appUser = this.route.snapshot.data['appUser'];
    if (!this.appUser) {
      this.router.navigate(['/']);
      return;
    }
    if (this.appUser) {
      this.store.dispatch(new CircleManagementActions.GetMyCircleList({ userId: this.appUser.id, page: null }));
      this.store.dispatch(new CircleManagementActions.GetOwnCircleList({ userId: this.appUser.id, page: null }));
      this.store.select("circleModule").subscribe((circleModuleState) => {
        this.myCircleList = circleModuleState.circleManagement.myCircleList;
        this.myCirclePagination = Object.assign({}, circleModuleState.circleManagement.myCirclePagination);
        this.ownCircleList = circleModuleState.circleManagement.ownCircleList;
        this.ownCirclePagination = Object.assign({}, circleModuleState.circleManagement.ownCirclePagination);
      })
    };
  }

  myCirclePageChanged(event) {
    this.store.dispatch(new CircleManagementActions.SetMyCirclePage({ userId: this.appUser.id, page: event.page }));
  }

  ownCirclePageChanged(event) {
    this.store.dispatch(new CircleManagementActions.SetOwnCirclePage({ userId: this.appUser.id, page: event.page }));
  }

}
