import { Component, OnInit } from '@angular/core';
import { AppUser } from '../../../_models/AppUser';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import * as fromCircle from '../../store/circle.reducers';
import * as CircleActions from '../../store/circle.actions';
import { CircleEvent } from '../../../_models/CircleEvent';

@Component({
  selector: 'app-circle-event',
  templateUrl: './circle-event.component.html',
  styleUrls: ['./circle-event.component.css']
})
export class CircleEventComponent implements OnInit {
  selector: string = '.eventList';
  circleId: number;
  appUser: AppUser;
  isManageable: boolean = false;
  pastEvents: CircleEvent[] = [];
  
  constructor(private route: ActivatedRoute, private router:Router, private store: Store<fromCircle.FeatureState>) { }

  ngOnInit() {
    this.appUser= this.route.parent.snapshot.data['appUser'];
    this.circleId = this.route.parent.snapshot.params['id'];
    if(this.circleId == null)
    {
      this.router.navigate(['/circle']);
      return;
    }
    this.store.dispatch(new CircleActions.GetPastCircleEventList(this.circleId));
    this.store.select('circleModule').subscribe((circleState) => {
      if(!circleState.circle.selectedCircle)
        return;
      this.isManageable = circleState.circle.selectedCircle.isManageable;
      this.pastEvents = circleState.circle.pastEventList;
    })
  }

  

}
