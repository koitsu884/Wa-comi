import { Component, OnInit, Input } from '@angular/core';
import { Attraction } from '../../../_models/Attraction';
import { AppUser } from '../../../_models/AppUser';

@Component({
  selector: 'app-attraction-details-card',
  templateUrl: './attraction-details-card.component.html',
  styleUrls: ['./attraction-details-card.component.css']
})
export class AttractionDetailsCardComponent implements OnInit {
  @Input() attraction: Attraction;
  @Input() appUser: AppUser;
  constructor() { }

  ngOnInit() {
  }

}
