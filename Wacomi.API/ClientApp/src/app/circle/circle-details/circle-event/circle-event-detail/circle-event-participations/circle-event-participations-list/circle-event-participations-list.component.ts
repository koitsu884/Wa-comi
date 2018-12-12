import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { CircleEvent } from '../../../../../../_models/CircleEvent';
import { AppUser } from '../../../../../../_models/AppUser';
import { CircleEventParticipation, CircleEventParticipationStatus } from '../../../../../../_models/CircleEventParticipation';
import { Pagination } from '../../../../../../_models/Pagination';
import { Store } from '@ngrx/store';
import * as fromCircle from '../../../../../store/circle.reducers';
import * as CircleEventParticipationActions from '../../../../../store/circle-event-participations.actions';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-circle-event-participations-list',
  templateUrl: './circle-event-participations-list.component.html',
  styleUrls: ['./circle-event-participations-list.component.css']
})
export class CircleEventParticipationsListComponent implements OnInit, OnDestroy {
  @Input() circleEvent: CircleEvent;
  @Input() appUser: AppUser;
  @Input() status: CircleEventParticipationStatus;

  eventParticipants: CircleEventParticipation[];
  pagination: Pagination;
  subscription: Subscription;

  constructor(private store: Store<fromCircle.FeatureState>) { }

  ngOnInit() {
    switch (this.status) {
      case CircleEventParticipationStatus.Confirmed:
        this.store.dispatch(new CircleEventParticipationActions.GetCircleEventConfirmedList({ eventId: this.circleEvent.id }));
        this.subscription = this.store.select('circleModule').subscribe((circleModuleState) => {
          this.eventParticipants = circleModuleState.circleEventParticipation.eventParticipants;
          // this.eventParticipants.unshift(<CircleEventParticipation>{
          //   appUser: this.circleEvent.appUser,
          //   appUserId: this.circleEvent.appUserId,
          //   circleEventId: this.circleEvent.id,
          //   status: this.status,
          //   message: "",
          //   isOwner:true
          // });
          this.pagination = circleModuleState.circleEventParticipation.pagination;
        })
        break;
      case CircleEventParticipationStatus.Waiting:
        this.store.dispatch(new CircleEventParticipationActions.GetCircleEventWaitList({ eventId: this.circleEvent.id }));
        this.subscription = this.store.select('circleModule').subscribe((circleModuleState) => {
          this.eventParticipants = circleModuleState.circleEventParticipation.eventParticipantsWaiting;
          this.pagination = circleModuleState.circleEventParticipation.waitingPagination;
        })
        break;
      case CircleEventParticipationStatus.Canceled:
        this.store.dispatch(new CircleEventParticipationActions.GetCircleEventCanceledList({ eventId: this.circleEvent.id }));
        this.subscription = this.store.select('circleModule').subscribe((circleModuleState) => {
          this.eventParticipants = circleModuleState.circleEventParticipation.eventParticipantsCanceled;
          this.pagination = circleModuleState.circleEventParticipation.canceledPagination;
        })
        break;
    }
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  pageChanged(event) {
    switch (this.status) {
      case CircleEventParticipationStatus.Confirmed:
        this.store.dispatch(new CircleEventParticipationActions.GetCircleEventConfirmedList({ eventId: this.circleEvent.id, pageNumber: event.page }));
        break;
      case CircleEventParticipationStatus.Waiting:
        this.store.dispatch(new CircleEventParticipationActions.GetCircleEventWaitList({ eventId: this.circleEvent.id, pageNumber: event.page }));
        break;
      case CircleEventParticipationStatus.Canceled:
        this.store.dispatch(new CircleEventParticipationActions.GetCircleEventCanceledList({ eventId: this.circleEvent.id, pageNumber: event.page }));
        break;
    }
  }

  showAcceptButton() {
    if (this.status != CircleEventParticipationStatus.Waiting)
      return false;
    return this.circleEvent.appUserId == this.appUser.id;
  }

  onAccept(participant: CircleEventParticipation) {
    this.store.dispatch(new CircleEventParticipationActions.ApproveEventParticipationRequest({ appUserId: participant.appUserId, circleEventId: participant.circleEventId }));
  }

}
