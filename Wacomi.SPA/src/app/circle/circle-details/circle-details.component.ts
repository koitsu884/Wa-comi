import * as GlobalActions from '../../store/global.actions';
import * as CircleActions from '../store/circle.actions';
import * as CircleMemberActions from '../store/circlemember.actions';
import * as fromCircle from '../store/circle.reducers';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Circle } from '../../_models/Circle';

@Component({
  selector: 'app-circle-details',
  templateUrl: './circle-details.component.html',
  styleUrls: ['./circle-details.component.css']
})
export class CircleDetailsComponent implements OnInit {
  circleId: number;
  circle: Circle;

  constructor(private route: ActivatedRoute, private router: Router, private store: Store<fromCircle.FeatureState>) { }

  ngOnInit() {
    this.circleId = this.route.snapshot.params['id'];
    if(!this.circleId)
      this.router.navigate(['/circle']);
    this.store.select('circleModule').take(1).subscribe((circleState) => {
      if(!circleState.circle.selectedCircle || circleState.circle.selectedCircle.id != this.circleId)
      {
        this.store.dispatch(new CircleActions.GetCircle(this.circleId));
        // this.store.dispatch(new CircleMemberActions.GetCircleMemberList({circleId: this.circleId, initPage: true}));
        // this.store.dispatch(new CircleActions.GetCircleRequestList(this.circleId));
      }
    });

    this.store.select('circleModule').subscribe((circleSate) => {
      this.circle = circleSate.circle.selectedCircle;
    })
  }
}
