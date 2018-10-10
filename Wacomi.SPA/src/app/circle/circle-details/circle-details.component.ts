import * as GlobalActions from '../../store/global.actions';
import * as CircleActions from '../store/circle.actions';
import * as CircleMemberActions from '../store/circlemember.actions';
import * as fromCircle from '../store/circle.reducers';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Circle } from '../../_models/Circle';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';
import { Photo } from '../../_models/Photo';

@Component({
  selector: 'app-circle-details',
  templateUrl: './circle-details.component.html',
  styleUrls: ['./circle-details.component.css']
})
export class CircleDetailsComponent implements OnInit {
  circleId: number;
  circle: Circle;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

  constructor(private route: ActivatedRoute, private router: Router, private store: Store<fromCircle.FeatureState>) { }

  ngOnInit() {
    this.circleId = this.route.snapshot.params['id'];
    if(!this.circleId)
      this.router.navigate(['/circle']);
    this.store.select('circleModule').take(1).subscribe((circleState) => {
      if(!circleState.circle.selectedCircle || circleState.circle.selectedCircle.id != this.circleId)
      {
        this.store.dispatch(new CircleActions.GetCircle(this.circleId));
        this.store.dispatch(new CircleActions.GetLatestCircleMemberList(this.circleId));
        this.store.dispatch(new CircleActions.GetLatestCircleTopicList(this.circleId));
        // this.store.dispatch(new CircleMemberActions.GetCircleMemberList({circleId: this.circleId, initPage: true}));
        
        //this.store.dispatch(new CircleActions.GetCircleRequestList(this.circleId));
      }
    });

    this.store.select('circleModule').subscribe((circleSate) => {
      this.circle = circleSate.circle.selectedCircle;
      if(this.circle)
        this.galleryImages = this.getImages(this.circle.photos);
    })



    this.galleryOptions = [
      {
        //breakpoint: 800,
        width: '100%',
        height: '100%',
        // imageSize:"contain",
       // imagePercent: 100,
      //  thumbnailsColumns:1,
      //  thumbnailMargin:0,
        imageArrowsAutoHide: true,
        imageAnimation: NgxGalleryAnimation.Fade,
        thumbnails: false,
        preview: true
      }
    ];
  }

  getImages(photos: Photo[]) {
    const imageUrls = [];
    for (let i = 0; i < photos.length; i++){
      imageUrls.push({
        small: photos[i].url,
        medium: photos[i].url,
        big: photos[i].url,
        description: photos[i].description
      });
    }
    return imageUrls;
  }
}
