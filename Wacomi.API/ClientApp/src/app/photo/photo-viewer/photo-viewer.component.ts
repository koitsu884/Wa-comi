import { Component, OnInit, Input } from '@angular/core';
import { Photo } from '../../_models/Photo';

@Component({
  selector: 'app-photo-viewer',
  templateUrl: './photo-viewer.component.html',
  styleUrls: ['./photo-viewer.component.css']
})
export class PhotoViewerComponent implements OnInit {
  @Input() photos: Photo[] = null;
  @Input() mainPhoto: Photo = null;

  // constructor(private store: Store<>) { }

  ngOnInit(){}

}
