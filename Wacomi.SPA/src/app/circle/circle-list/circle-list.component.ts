import { Component, OnInit, Input } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { Circle } from '../../_models/Circle';
import { AppUser } from '../../_models/AppUser';

@Component({
  selector: 'app-circle-list',
  templateUrl: './circle-list.component.html',
  styleUrls: ['./circle-list.component.css'],
  animations: [
    trigger('circleCard', [
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
export class CircleListComponent implements OnInit {
  @Input() circleList: Circle[];
  @Input() appUser: AppUser;
  constructor() { }

  ngOnInit() {
  }

}
