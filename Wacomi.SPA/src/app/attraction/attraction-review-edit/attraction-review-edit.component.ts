import * as fromAttraction from '../store/attraction.reducers';
import * as AttractionActions from '../store/attraction.actions';
import { Component, OnInit } from '@angular/core';
import { AppUser } from '../../_models/AppUser';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { AttractionReview } from '../../_models/AttractionReview';
import { Store } from '@ngrx/store';
import { NgForm } from '@angular/forms';
import { Location } from '@angular/common';

@Component({
  selector: 'app-attraction-review-edit',
  templateUrl: './attraction-review-edit.component.html',
  styleUrls: ['./attraction-review-edit.component.css']
})
export class AttractionReviewEditComponent implements OnInit {
  appUser: AppUser;
  attractionReview: AttractionReview;
  id: number;
  waitingResponse: boolean = false;
  selectedFiles: Array<File> = [];
  previewUrls: Array<string> = [];
  starCheckedList: boolean[] = [false, false, false, false, false];


  constructor(private route: ActivatedRoute, private location: Location, private router: Router, private store: Store<fromAttraction.FeatureState>, ) { }

  ngOnInit() {
    this.route.params
      .subscribe(
        (params: Params) => {
          this.id = +params['reviewId'];
          let attractionId = +params['id'];
          this.appUser = this.route.snapshot.data['appUser'];
          if (this.appUser == null || attractionId == null) {
            console.log("No parameter");
            this.router.navigate(['/']);
            return;
          }
          if (this.id) {
            this.store.dispatch(new AttractionActions.GetAttractionReview(this.id));
            this.store.select("attraction").subscribe((attractionState) => {
              this.attractionReview = attractionState.selectedAttractionReview;
              this.waitingResponse = false;
              if(this.attractionReview)
                this.starCheck(this.attractionReview.score);
            });
          }
          else {
            this.attractionReview = <AttractionReview>{ appUserId:this.appUser.id,  attractionId: attractionId };
          }
        }
      );
  }

  submit(ngForm: NgForm) {
    this.waitingResponse = true;
    if (this.id) {
      ngForm.form.markAsDirty();
      this.store.dispatch(new AttractionActions.UpdateAttractionReview(this.attractionReview));
    }
    else {
      var formData = new FormData();

      for (let file of this.selectedFiles) {
        formData.append("files", file);
      }
      this.store.dispatch(new AttractionActions.TryAddAttractionReview({ attractionReview: this.attractionReview, formData: this.selectedFiles.length > 0 ? formData : null }));
    }
  }

  onCancel() {
    this.location.back();
  }

  mainPhotoSelected(event, ngForm: NgForm) {
    this.attractionReview.mainPhotoId = event;
    ngForm.form.markAsDirty();
  }

  setSelectedFiles(event: { selectedFiles: Array<File>, previewUrls: Array<string> }) {
    this.selectedFiles = event.selectedFiles;
    this.previewUrls = event.previewUrls;
  }

  starClicked(index: number, ngForm: NgForm) {
    ngForm.form.markAsDirty();
    var score = index + 1;
    this.starCheck(score);
    this.attractionReview.score = score;
  }

  private starCheck(score: number){
    for (var _i = 0; _i < this.starCheckedList.length; _i++) {
      this.starCheckedList[_i] = (_i <= score-1);
    }
  }
}
