import { Component, OnInit, Input } from '@angular/core';
import { ClanSeek } from '../../../../_models/ClanSeek';

@Component({
  selector: 'app-clan-card',
  templateUrl: './clan-card.component.html',
  styleUrls: ['./clan-card.component.css']
})
export class ClanCardComponent implements OnInit {
  @Input() isMine : boolean;
  @Input() clanSeek: ClanSeek;
  
  constructor() { }

  ngOnInit() {
  }

}
