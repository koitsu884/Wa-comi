import { Component, OnInit, Input } from '@angular/core';
import { ClanSeek } from '../../../_models/ClanSeek';

@Component({
  selector: 'app-clan-detail-info',
  templateUrl: './clan-detail-info.component.html',
  styleUrls: ['./clan-detail-info.component.css']
})
export class ClanDetailInfoComponent implements OnInit {
  @Input() clanSeek: ClanSeek;
  description: string;
  constructor() { }

  ngOnInit() {
  }

}
