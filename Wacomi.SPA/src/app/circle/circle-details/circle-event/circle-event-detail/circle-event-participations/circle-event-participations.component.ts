import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromCircle from '../../../../store/circle.reducers';
import * as CircleEventParticipationActions from '../../../../store/circle-event-participations.actions';
import { CircleEventParticipation, CircleEventParticipationStatus } from '../../../../../_models/CircleEventParticipation';
import { Pagination } from '../../../../../_models/Pagination';
import { CircleEvent } from '../../../../../_models/CircleEvent';
import { ActivatedRoute, Router } from '@angular/router';
import { AppUser } from '../../../../../_models/AppUser';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-circle-event-participations',
  templateUrl: './circle-event-participations.component.html',
  styleUrls: ['./circle-event-participations.component.css']
})

export class CircleEventParticipationsComponent implements OnInit, OnDestroy {
  circleEvent: CircleEvent; //resolve
  appUser: AppUser; //resolve
  circleEventParticipationStatus = CircleEventParticipationStatus;
  numberOfParticipants:number;
  numberOfWaiting:number;
  numberOfCanceled:number;
  subscription: Subscription;
  // eventParticipants: CircleEventParticipation[];
  // pagination: Pagination;
  // eventParticipantsWaiting: CircleEventParticipation[];
  // waitingPagination: Pagination;
  // eventParticipantsCanceled: CircleEventParticipation[];
  // canceledPagination: Pagination;

  constructor(private store: Store<fromCircle.FeatureState>, private route:ActivatedRoute, private router:Router) {
   }

  ngOnInit() {
    this.route.data.subscribe((data) => {
      console.log(data);
      this.circleEvent = data['circleEvent'];
      this.appUser = data['appUser'];
      if(!this.circleEvent || !this.appUser){
        console.log("Parameter was not set");
        this.router.navigate(['/']);
      }

      this.subscription = this.store.select('circleModule').subscribe((circleModuleState) => {
        this.numberOfParticipants = circleModuleState.circleEventParticipation.numberOfParticipants;
        this.numberOfWaiting = circleModuleState.circleEventParticipation.numberOfWaiting;
        this.numberOfCanceled = circleModuleState.circleEventParticipation.numberOfCanceld;
      })
    })
  }

  ngOnDestroy(){
    this.subscription.unsubscribe();
  }

  pageChanged(event) {
    this.store.dispatch(new CircleEventParticipationActions.GetCircleEventConfirmedList({eventId: this.circleEvent.id, pageNumber: event.page}));
 }

  onAccept(participant: CircleEventParticipation){
    this.store.dispatch(new CircleEventParticipationActions.ApproveEventParticipationRequest({appUserId: participant.appUserId, circleEventId: participant.circleEventId}));
  }

}
