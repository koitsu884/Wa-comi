import { Component, OnInit, Input } from '@angular/core';
import { ClanSeek } from '../../../_models/ClanSeek';

@Component({
  selector: 'app-latest-clan-list',
  templateUrl: './latest-clan-list.component.html',
  styleUrls: ['./latest-clan-list.component.css']
})
export class LatestClanListComponent implements OnInit {
  @Input() latestClanSeeklist: ClanSeek[];
  constructor() { }

  ngOnInit() {

  }

}
