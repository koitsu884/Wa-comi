import { Component, OnInit, Input } from '@angular/core';
import { BusinessUser } from '../../../_models/BusinessUser';

@Component({
  selector: 'app-business-detail',
  templateUrl: './business-detail.component.html',
  styleUrls: ['./business-detail.component.css']
})
export class BusinessDetailComponent implements OnInit {
  @Input() bisUser: BusinessUser;
  
  constructor() { }

  ngOnInit() {
  }

}
