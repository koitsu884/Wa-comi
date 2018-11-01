import { Component, OnInit, Input } from '@angular/core';
import { Circle } from '../../../../_models/Circle';
import { CircleTopic } from '../../../../_models/CircleTopic';
import { CircleEvent } from '../../../../_models/CircleEvent';

@Component({
  selector: 'app-circle-sideinfo',
  templateUrl: './circle-sideinfo.component.html',
  styleUrls: ['./circle-sideinfo.component.css']
})
export class CircleSideinfoComponent implements OnInit {
  @Input() circle: Circle;
  @Input() latestTopics: CircleTopic[];
  @Input() latestEvents: CircleEvent[];
  constructor() { }

  ngOnInit() {
  }

}
