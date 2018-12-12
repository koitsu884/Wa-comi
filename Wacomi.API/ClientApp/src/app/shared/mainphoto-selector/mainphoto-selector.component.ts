import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { Photo } from '../../_models/Photo';

@Component({
  selector: 'app-mainphoto-selector',
  templateUrl: './mainphoto-selector.component.html',
  styleUrls: ['./mainphoto-selector.component.css']
})
export class MainphotoSelectorComponent implements OnInit {
  @Input() photos: Photo[];
  @Input() mainPhotoId: number;
  @Output() photoSelected = new EventEmitter<number>();
  selectedPhoto: Photo;

  constructor() { }

  ngOnInit() {
    if(this.mainPhotoId){
      var index = this.photos.findIndex(x => x.id == this.mainPhotoId);
      this.selectedPhoto = this.photos[index];
    }
  }

  onSelect(photo){
    this.mainPhotoId=photo.id;
    var index = this.photos.findIndex(x => x.id == this.mainPhotoId);
    this.selectedPhoto = this.photos[index];
    this.photoSelected.emit(this.mainPhotoId);
  }
}
