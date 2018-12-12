import * as fromCircle from '../../../store/circle.reducers';
import * as CircleEventActions from '../../../store/circleevent.actions';

import { Component, OnInit, Input, SimpleChanges, OnChanges, SimpleChange } from '@angular/core';
import { CircleEvent } from '../../../../_models/CircleEvent';
import { Store } from '@ngrx/store';
import { CircleEventParticipationStatus } from '../../../../_models/CircleEventParticipation';

@Component({
  selector: 'app-circle-event-calendar',
  templateUrl: './circle-event-calendar.component.html',
  styleUrls: ['./circle-event-calendar.component.css']
})
export class CircleEventCalendarComponent implements OnChanges, OnInit {
  @Input() circleId: number;
  @Input() categoryId: number;
  @Input() fromDate: Date;
  @Input() cityId: number;
  displayAttendingOnly: boolean = false;
  events: CircleEvent[];
  finish: boolean;
  selector:string = '.eventList';
  circleEventParticipationStatus = CircleEventParticipationStatus;

  constructor(private store : Store<fromCircle.FeatureState>) { }

  ngOnInit() {
    this.store.select('circleModule').subscribe((circleModuleState) => {
      this.events = circleModuleState.circleEvent.eventList;
      this.finish = circleModuleState.circleEvent.finish;
    })
  }

  ngOnChanges(changes: SimpleChanges) {
    if(changes.circleId && changes.circleId.currentValue > 0)
    {
      this.store.dispatch(new CircleEventActions.SearchCircleEvent({
        circleId: changes.circleId.currentValue
      }))
    }
    else{
      this.store.dispatch(new CircleEventActions.SearchCircleEvent({
        categoryId: changes.categoryId ? changes.categoryId.currentValue : null,
        fromDate: changes.fromDate ? changes.fromDate.currentValue : null,
        cityId: changes.cityId ? changes.cityId.currentValue: null
      }))
    }
  }

  onScroll(){
    if(!this.finish)
      this.store.dispatch(new CircleEventActions.LoadNextCircleEventList());
  }

  dateChanged(prevEventDateTime:Date, eventDateTime:Date){
    return new Date(prevEventDateTime).setHours(0, 0, 0, 0) != new Date(eventDateTime).setHours(0, 0, 0, 0); 
  }

  timeSpan(timeSpanStr: string){
    let temp = timeSpanStr.split(':');
    return temp[0] + ":" + temp[1];
  }

}
