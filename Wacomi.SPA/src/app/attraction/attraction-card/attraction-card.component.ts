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
  scoreAverage: number;
  // rateArray: string[] = [null, null, null, null, null]
  constructor() { }

  ngOnInit() {
    this.scoreAverage = Math.round( this.attraction.scoreAverage * 10) / 10;
    // var ratingValue = this.scoreAverage, rounded = (this.scoreAverage | 0);
    // var decimal = ratingValue - rounded;

    // for(var j = 0; j < rounded; j++){
    //   this.rateArray[j] = "full";
    // }
    // if(decimal >= 0.5 )
    //   this.rateArray[rounded] = "half";
  }

}
