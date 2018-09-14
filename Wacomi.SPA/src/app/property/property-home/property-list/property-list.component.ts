import { Component, OnInit, Input } from '@angular/core';
import { state, style, transition, animate, trigger } from '@angular/animations';
import { Property } from '../../../_models/Property';
import { AppUser } from '../../../_models/AppUser';

@Component({
  selector: 'app-property-list',
  templateUrl: './property-list.component.html',
  styleUrls: ['./property-list.component.css'],
  animations: [
    trigger('propertyCard', [
      state('in', style({
        opacity: 1,
        transform: 'scale(1)'
      })),
      transition('void => *', [
        style({
          opacity: 0,
          transform: 'scale(0.1)'
        }),
        animate(200)
      ])
    ])
  ],
})
export class PropertyListComponent implements OnInit {
  @Input() propertyList: Property[];
  @Input() appUser: AppUser;

  constructor() { }

  ngOnInit() {
  }

}
