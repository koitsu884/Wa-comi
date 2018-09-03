import { Component, OnInit, Input } from '@angular/core';
import { Attraction } from '../../_models/Attraction';

@Component({
  selector: 'app-attraction-card',
  templateUrl: './attraction-card.component.html',
  styleUrls: ['./attraction-card.component.css']
})
export class AttractionCardComponent implements OnInit {
  @Input() attraction: Attraction;
  @Input() isMine: boolean;
  constructor() { }

  ngOnInit() {

  }

}
