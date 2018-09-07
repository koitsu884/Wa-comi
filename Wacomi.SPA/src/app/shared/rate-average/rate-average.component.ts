import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-rate-average',
  templateUrl: './rate-average.component.html',
  styleUrls: ['./rate-average.component.css']
})
export class RateAverageComponent implements OnInit {
  @Input() scoreAverage: number;
  rateArray: string[] = [null, null, null, null, null]
  constructor() { }

  ngOnInit() {
    this.scoreAverage = Math.round( this.scoreAverage * 10) / 10;
    var ratingValue = this.scoreAverage, rounded = (this.scoreAverage | 0);
    var decimal = ratingValue - rounded;

    for(var j = 0; j < rounded; j++){
      this.rateArray[j] = "full";
    }
    if(decimal >= 0.5 )
      this.rateArray[rounded] = "half";
  }

}
