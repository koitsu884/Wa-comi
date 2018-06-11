import { Component, OnInit, Input } from '@angular/core';
import { ClanSeek } from '../../../_models/ClanSeek';

@Component({
  selector: 'app-clan-list',
  templateUrl: './clan-list.component.html',
  styleUrls: ['./clan-list.component.css']
})
export class ClanListComponent implements OnInit {
  @Input() clanSeeks: ClanSeek[];
  @Input() appUserId: number;
  constructor() { }

  ngOnInit() {
  }

}
