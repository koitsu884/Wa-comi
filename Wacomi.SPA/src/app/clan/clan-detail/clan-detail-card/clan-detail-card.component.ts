import { Component, OnInit, Input } from '@angular/core';
import { ClanSeek } from '../../../_models/ClanSeek';

@Component({
  selector: 'app-clan-detail-card',
  templateUrl: './clan-detail-card.component.html',
  styleUrls: ['./clan-detail-card.component.css']
})
export class ClanDetailCardComponent implements OnInit {
  @Input() clanSeek : ClanSeek;
  constructor() { }

  ngOnInit() { 
  }

}
