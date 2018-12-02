import { Component, OnInit, Input } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { ClanSeek } from '../../../_models/ClanSeek';

@Component({
  selector: 'app-latest-clan-list',
  templateUrl: './latest-clan-list.component.html',
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
  styleUrls: ['./latest-clan-list.component.css']
})
export class LatestClanListComponent implements OnInit {
  @Input() latestClanSeeklist: ClanSeek[];
  constructor() { }

  ngOnInit() {

  }

}
