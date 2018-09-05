import { Component, OnInit, Input } from '@angular/core';
import { Attraction } from '../../../_models/Attraction';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';

@Component({
  selector: 'app-attraction-details-info',
  templateUrl: './attraction-details-info.component.html',
  styleUrls: ['./attraction-details-info.component.css']
})
export class AttractionDetailsInfoComponent implements OnInit {
  @Input() attraction: Attraction;
  description: string;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

  constructor() { }

  ngOnInit() {
    this.galleryOptions = [
      {
        //breakpoint: 800,
        width: '100%',
        height: '400px',
        // imageSize:"contain",
       // imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Fade,
        image: true,
        preview: false
      }
    ];
    this.galleryImages = this.getImages();
  }
  getImages() {
    if(this.attraction.photos == null)  return [];
    const imageUrls = [];
    for (let i = 0; i < this.attraction.photos.length; i++){
      imageUrls.push({
        small: this.attraction.photos[i].url,
        medium: this.attraction.photos[i].url,
        big: this.attraction.photos[i].url,
        description: this.attraction.photos[i].description
      });
    }
    return imageUrls;
  }

}
