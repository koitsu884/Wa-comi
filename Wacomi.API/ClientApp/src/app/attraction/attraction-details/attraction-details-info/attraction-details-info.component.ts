import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Attraction } from '../../../_models/Attraction';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';
import { Photo } from '../../../_models/Photo';


@Component({
  selector: 'app-attraction-details-info',
  templateUrl: './attraction-details-info.component.html',
  styleUrls: ['./attraction-details-info.component.css']
})
export class AttractionDetailsInfoComponent implements OnInit {
  @Input() attraction: Attraction;
  @Output() likeClicked =  new EventEmitter();

  description: string;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  reviewerGalleryOptions: NgxGalleryOptions[];
  reviewerGalleryImages: NgxGalleryImage[];
  thumbnailsColumns: number;
  scoreAverage:number;

  constructor() { }

  ngOnInit() {
    this.scoreAverage = Math.round( this.attraction.scoreAverage * 10) / 10;
    this.thumbnailsColumns = typeof window !== 'undefined' && window.screen.width < 500 ? 3 : 4;
    this.galleryOptions = [
      {
        //breakpoint: 800,
        width: '100%',
        height: '100px',
        imageSize:"cover",
       // imagePercent: 100,
        thumbnailsColumns: this.thumbnailsColumns,
        imageAnimation: NgxGalleryAnimation.Fade,
        image: false,
        preview: true
      }
    ];

    let thumbnailRows = 1;
    let reviewPhotoNum = this.attraction.reviewPhotos.length;
    if(reviewPhotoNum > this.thumbnailsColumns * 2)
      thumbnailRows = 3;
    else if( reviewPhotoNum > this.thumbnailsColumns)
      thumbnailRows = 2;

    this.reviewerGalleryOptions = [
      {
        //breakpoint: 800,
        width: '100%',
        height: 120 * thumbnailRows + 'px',
        imageSize:"contain",
       // imagePercent: 100,
        thumbnailsColumns: this.thumbnailsColumns,
        thumbnailsRows: thumbnailRows,
        thumbnailsOrder: 2,
        imageAnimation: NgxGalleryAnimation.Fade,
        image: false,
        preview: true
      }
    ];
    this.galleryImages = this.getImages(this.attraction.photos);
    this.reviewerGalleryImages = this.getImages(this.attraction.reviewPhotos);
  }

  getImages(photos: Photo[]){
    if(photos == null)  return [];
    const imageUrls = [];
    for (let i = 0; i < photos.length; i++){
      imageUrls.push({
        small: photos[i].iconUrl,
        medium: photos[i].thumbnailUrl,
        big: photos[i].url,
        description: photos[i].description
      });
    }
    return imageUrls;
  }

  onLike(){
    if(!this.attraction.isLiked)
      this.likeClicked.emit();
  }

}
