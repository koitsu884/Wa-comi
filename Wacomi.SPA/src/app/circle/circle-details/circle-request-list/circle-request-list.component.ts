import * as fromCircle from '../../store/circle.reducers';
import * as CircleActions from '../../store/circle.actions';
import { Component, OnInit, Input } from '@angular/core';
import { CircleRequest } from '../../../_models/CircleRequest';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-circle-request-list',
  templateUrl: './circle-request-list.component.html',
  styleUrls: ['./circle-request-list.component.css']
})
export class CircleRequestListComponent implements OnInit {
  // @Input() circleId: number;
  pendingRequests : CircleRequest[];
  deniedRequests : CircleRequest[];
  loading:boolean;
  constructor(private store : Store<fromCircle.FeatureState>) { }

  ngOnInit() {
    this.loading = true;
    this.store.select('circleModule').subscribe((circleState) => {
      let requests = circleState.circle.circleRequests;
      this.pendingRequests = [];
      this.deniedRequests = [];
      for(let request of requests){
        if(request.declined)
          this.deniedRequests.push(request);
        else
          this.pendingRequests.push(request);
      }
      this.loading = false;
    })
    // this.store.dispatch(new CircleActions.GetCircleRequestList(this.circleId));
  }

  onApprove(request: CircleRequest){
    this.store.dispatch(new CircleActions.ApproveCircleRequest(request));
  }

  onDecline(request: CircleRequest){
    this.store.dispatch(new CircleActions.DeclineCircleRequest(request));
  }

}
