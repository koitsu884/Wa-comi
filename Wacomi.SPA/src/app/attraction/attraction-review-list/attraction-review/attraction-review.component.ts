import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AttractionReview } from '../../../_models/AttractionReview';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';

@Component({
  selector: 'app-attraction-review',
  templateUrl: './attraction-review.component.html',
  styleUrls: ['./attraction-review.component.css']
})
export class AttractionReviewComponent implements OnInit {
  @Input() attractionReview : AttractionReview;
  @Input() isMine: boolean;
  @Output() deleteReview = new EventEmitter<number>();

  starCheckedList: boolean[] = [false,false,false,false,false];
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

  constructor() { }

  ngOnInit() {
    for (var _i = 1; _i <= this.attractionReview.score; _i++) {
      this.starCheckedList[_i-1] = true;
    }
    this.galleryOptions = [
      {
        //breakpoint: 800,
        width: '100%',
        height: '100px',
        // imageSize:"contain",
       // imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Fade,
        image: false,
        preview: true
      }
    ];
    this.galleryImages = this.getImages();
  }

  getImages() {
    if(this.attractionReview.photos == null)  return [];
    const imageUrls = [];
    for (let i = 0; i < this.attractionReview.photos.length; i++){
      imageUrls.push({
        small: this.attractionReview.photos[i].url,
        medium: this.attractionReview.photos[i].url,
        big: this.attractionReview.photos[i].url,
        description: this.attractionReview.photos[i].description
      });
    }
    return imageUrls;
  }

  onDelete(id:number){
    this.deleteReview.emit(id);
  }
}
