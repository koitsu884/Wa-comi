import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-user-card-small',
  templateUrl: './user-card-small.component.html',
  styleUrls: ['./user-card-small.component.css']
})
export class UserCardSmallComponent implements OnInit {
  @Input() appUserId: number;
  @Input() appUserIconUrl: string;
  @Input() appUserDisplayName: string;
  
  constructor() { }

  ngOnInit() {
  }

}
