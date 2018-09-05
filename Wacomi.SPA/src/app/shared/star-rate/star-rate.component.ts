import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-star-rate',
  templateUrl: './star-rate.component.html',
  styleUrls: ['./star-rate.component.css']
})
export class StarRateComponent implements OnInit {
  @Input() starCheckedList: boolean[];
  @Output() rateSelected = new EventEmitter<number>();

  constructor() { }

  ngOnInit() {
  }

  rateClicked(score: number){
    this.rateSelected.emit(score);
  }

}
