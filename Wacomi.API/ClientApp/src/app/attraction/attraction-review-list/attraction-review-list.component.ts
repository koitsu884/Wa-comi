import * as fromAttraction from '../store/attraction.reducers';
import * as AttractionActions from '../store/attraction.actions';
import { Component, OnInit } from '@angular/core';
import { AttractionReview } from '../../_models/AttractionReview';
import { Store } from '@ngrx/store';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { AppUser } from '../../_models/AppUser';
import { Attraction } from '../../_models/Attraction';
import { AlertifyService } from '../../_services/alertify.service';

@Component({
  selector: 'app-attraction-review-list',
  templateUrl: './attraction-review-list.component.html',
  styleUrls: ['./attraction-review-list.component.css']
})
export class AttractionReviewListComponent implements OnInit {
  attractionReviews: AttractionReview[];
  attraction: Attraction;
  appUser: AppUser;
  constructor(private store: Store<fromAttraction.FeatureState>, 
    private route: ActivatedRoute, 
    private alertify: AlertifyService) { }

  ngOnInit() {
    this.appUser = this.route.snapshot.data['appUser'];
    this.route.data.subscribe((data) => {
      this.appUser = data['appUser'];
      this.attraction = data['attraction'];
      this.store.dispatch(new AttractionActions.GetAttractionReviewList(this.attraction.id));
    })
    this.store.select('attraction').subscribe((attractionState) => {
      this.attractionReviews = attractionState.attractionReviewList;
    });
  }

  deleteReview(id: number){
    this.alertify.confirm("このレビューを本当に削除しますか？", ()=>{
      this.store.dispatch(new AttractionActions.TryDeleteAttractionReview(id));
    })
  }

  sendLike(id: number){
    if(this.appUser)
      this.store.dispatch(new AttractionActions.LikeAttractionReview({appUserId: this.appUser.id, attractionReviewId: id}));
  }
}
