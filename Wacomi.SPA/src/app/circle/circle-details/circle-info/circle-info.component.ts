import { Component, OnInit, Input } from '@angular/core';
import { Circle } from '../../../_models/Circle';

@Component({
  selector: 'app-circle-info',
  templateUrl: './circle-info.component.html',
  styleUrls: ['./circle-info.component.css']
})
export class CircleInfoComponent implements OnInit {
  @Input() circle: Circle;

  
  constructor() { }

  ngOnInit() {
  }

}
