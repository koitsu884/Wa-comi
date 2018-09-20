import * as CircleActions from '../store/circle.actions';
import * as fromCircle from '../store/circle.reducers';
import { Component, OnInit } from '@angular/core';
import { Circle } from '../../_models/Circle';
import { ActivatedRoute, Router } from '@angular/router';
import { AppUser } from '../../_models/AppUser';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-circle-details',
  templateUrl: './circle-details.component.html',
  styleUrls: ['./circle-details.component.css']
})
export class CircleDetailsComponent implements OnInit {
  circle: Circle;
  loading: boolean;
  appUser: AppUser;
  isMine: boolean;

  constructor(private route: ActivatedRoute, private router: Router, private store: Store<fromCircle.FeatureState>) { }

  ngOnInit() {
    this.circle = null;
    this.loading = true;
    let id = this.route.snapshot.params['id'];
    this.appUser= this.route.snapshot.data['appUser'];
    // this.memberId = appUser ? appUser.relatedUserClassId : null;
    if(!id){
      this.router.navigate(['/circle']);
      return;
    }
    this.store.dispatch(new CircleActions.GetCircle(id));
    this.store.select('circle').subscribe((circleState) => {
      this.circle = circleState.selectedCircle;
      if(this.appUser && this.circle)
        this.isMine = this.appUser.id == this.circle.appUser.id;
      this.loading = false;
    });
  }

}
