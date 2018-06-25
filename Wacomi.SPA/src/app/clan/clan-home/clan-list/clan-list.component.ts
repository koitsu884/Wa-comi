import { Component, OnInit, Input, trigger, state, style, transition, animate } from '@angular/core';
import { ClanSeek } from '../../../_models/ClanSeek';

@Component({
  selector: 'app-clan-list',
  templateUrl: './clan-list.component.html',
  animations: [
    trigger('clanCard', [
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
  styleUrls: ['./clan-list.component.css']
})
export class ClanListComponent implements OnInit {
  @Input() clanSeeks: ClanSeek[];
  @Input() appUserId: number;
  constructor() { }

  ngOnInit() {
  }

}
