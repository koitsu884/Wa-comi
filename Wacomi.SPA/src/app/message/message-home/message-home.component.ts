import { Component, OnInit } from '@angular/core';
import { AppUser } from '../../_models/AppUser';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';

@Component({
  selector: 'app-message-home',
  templateUrl: './message-home.component.html',
  styleUrls: ['./message-home.component.css']
})
export class MessageHomeComponent implements OnInit {
  appUser: AppUser;
  constructor(private route:ActivatedRoute) { }

  ngOnInit() {
    this.appUser = this.route.snapshot.data["appUser"];
    if(!this.appUser){
      console.log("AppUser 入ってないで");
    }
  }

}
