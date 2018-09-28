import { Component, OnInit, Input } from '@angular/core';
import { Circle } from '../../../../_models/Circle';
import { CircleTopic } from '../../../../_models/CircleTopic';

@Component({
  selector: 'app-circle-sideinfo',
  templateUrl: './circle-sideinfo.component.html',
  styleUrls: ['./circle-sideinfo.component.css']
})
export class CircleSideinfoComponent implements OnInit {
  @Input() circle: Circle;
  @Input() latestTopics: CircleTopic[];
  constructor() { }

  ngOnInit() {
  }

}
