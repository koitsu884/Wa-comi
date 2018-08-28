import { Component, OnInit, Input } from '@angular/core';
@Component({
  selector: 'app-user-card',
  templateUrl: './user-card.component.html',
  styleUrls: ['./user-card.component.css']
})
export class UserCardComponent implements OnInit {
  @Input() imageUrl : string;
  @Input() lastActive: Date;
  @Input() created: Date;
  @Input() city: string;
  @Input() totalLike: number;

  constructor() { }

  ngOnInit() {

  }
}
