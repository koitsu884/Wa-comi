import { Component, OnInit, Input } from '@angular/core';
import { Photo } from '../../_models/Photo';
import { NgxGalleryImage, NgxGalleryOptions, NgxGalleryAnimation } from 'ngx-gallery';

@Component({
  selector: 'app-photo-gallery',
  templateUrl: './photo-gallery.component.html',
  styleUrls: ['./photo-gallery.component.css']
})
export class PhotoGalleryComponent implements OnInit {
  @Input() photos: Photo[];
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  
  constructor() { }

  ngOnInit() {
    let isMobile = window.screen.width < 500;
    let thumbnailsColumns = isMobile ? 3 : 4;
    this.galleryOptions = [
      {
        //breakpoint: 800,
        width: '100%',
        height: isMobile ? '100px' : '400px',
        // imageSize:"contain",
       // imagePercent: 100,
        thumbnailsColumns: thumbnailsColumns,
        imageAnimation: NgxGalleryAnimation.Fade,
        image: isMobile ? false : true,
        preview: true
      }
    ];

    this.galleryImages = this.getImages(this.photos);
  }

  getImages(photos: Photo[]) {
    const imageUrls = [];
    for (let i = 0; i < photos.length; i++){
      imageUrls.push({
        small: photos[i].iconUrl,
        medium: photos[i].url,
        big: photos[i].url,
        description: photos[i].description
      });
    }
    return imageUrls;
  }

}
