import { Component, OnInit, Input } from '@angular/core';
import { Property } from '../../../_models/Property';

@Component({
  selector: 'app-property-detail-info',
  templateUrl: './property-detail-info.component.html',
  styleUrls: ['./property-detail-info.component.css']
})
export class PropertyDetailInfoComponent implements OnInit {
  @Input() property: Property;
  
  constructor() { }

  ngOnInit() {

  }

}
