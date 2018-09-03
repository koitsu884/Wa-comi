import * as fromAttraction from '../store/attraction.reducers';
import * as AttractionActions from '../store/attraction.actions';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppUser } from '../../_models/AppUser';
import { Store } from '@ngrx/store';
import { Attraction } from '../../_models/Attraction';
import { AlertifyService } from '../../_services/alertify.service';

@Component({
  selector: 'app-attraction-details',
  templateUrl: './attraction-details.component.html',
  styleUrls: ['./attraction-details.component.css']
})
export class AttractionDetailsComponent implements OnInit {
  attractionId: number;
  appUser: AppUser;
  attraction: Attraction;

  constructor(private route: ActivatedRoute, 
    private router: Router, 
    private alertify: AlertifyService,
    private store: Store<fromAttraction.FeatureState>) { }

  ngOnInit() {
    this.attractionId = this.route.snapshot.params['id'];
    this.appUser= this.route.snapshot.data['appUser'];
    // this.memberId = appUser ? appUser.relatedUserClassId : null;
    if(!this.attractionId){
      this.router.navigate(['/clan']);
      return;
    }
    this.store.dispatch(new AttractionActions.GetAttraction(this.attractionId));
    this.store.select('attraction').subscribe((attractionState) => {this.attraction = attractionState.selectedAttraction});
  }

  onDelete(id: number){
    this.alertify.confirm('本当に削除しますか?', () => {
      this.store.dispatch(new AttractionActions.TryDeleteAttraction(id));
    })
  }
}
