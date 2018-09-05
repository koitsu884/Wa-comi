import { Component, OnInit, Input } from '@angular/core';
import { ClanSeek } from '../../../_models/ClanSeek';
import { NgxGalleryOptions, NgxGalleryAnimation, NgxGalleryImage } from 'ngx-gallery';

@Component({
  selector: 'app-clan-detail-info',
  templateUrl: './clan-detail-info.component.html',
  styleUrls: ['./clan-detail-info.component.css']
})
export class ClanDetailInfoComponent implements OnInit {
  @Input() clanSeek: ClanSeek;
  description: string;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

  constructor() { }

  ngOnInit() {
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
    const imageUrls = [];
    for (let i = 0; i < this.clanSeek.photos.length; i++){
      imageUrls.push({
        small: this.clanSeek.photos[i].url,
        medium: this.clanSeek.photos[i].url,
        big: this.clanSeek.photos[i].url,
        description: this.clanSeek.photos[i].description
      });
    }
    return imageUrls;
  }

}
