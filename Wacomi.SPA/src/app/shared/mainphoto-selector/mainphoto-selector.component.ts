import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { Photo } from '../../_models/Photo';

@Component({
  selector: 'app-mainphoto-selector',
  templateUrl: './mainphoto-selector.component.html',
  styleUrls: ['./mainphoto-selector.component.css']
})
export class MainphotoSelectorComponent implements OnInit {
  @Input() photos: Photo[];
  @Input() mainPhotoUrl: string;
  @Output() photoSelected = new EventEmitter<string>();

  constructor() { }

  ngOnInit() {
  }

  onSelect(photo){
    this.mainPhotoUrl=photo.url;
    this.photoSelected.emit(this.mainPhotoUrl);
  }
}
