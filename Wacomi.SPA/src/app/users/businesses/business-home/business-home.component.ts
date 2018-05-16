import { Component, OnInit } from '@angular/core';
import { BusinessUser } from '../../../_models/BusinessUser';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-business-home',
  templateUrl: './business-home.component.html',
  styleUrls: ['./business-home.component.css']
})
export class BusinessHomeComponent implements OnInit {
  bisUser: BusinessUser;
  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.bisUser = this.route.snapshot.data['bisUser'];
  }

}
