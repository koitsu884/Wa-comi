import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Property } from '../../_models/Property';
import { ClanSeek } from '../../_models/ClanSeek';
import { Attraction } from '../../_models/Attraction';
import { AttractionReview } from '../../_models/AttractionReview';
import { UserService } from '../../_services/user.service';

@Component({
  selector: 'app-user-posts',
  templateUrl: './user-posts.component.html',
  styleUrls: ['./user-posts.component.css']
})
export class UserPostsComponent implements OnInit {
  appUserId: number;
  propertyList: Property[];
  clanList: ClanSeek[];
  attractionList: Attraction[];
  attractionReviewList: AttractionReview[];

  constructor(private route:ActivatedRoute, private userService: UserService) { }

  ngOnInit() {
    this.route.params.subscribe((params) => {
      this.appUserId = params['appUserId'];
      //dispatch iroiro
      this.userService.getMyRecords<Property[]>('property', this.appUserId)
        .subscribe((properties) => {this.propertyList = properties});
      this.userService.getMyRecords<Attraction[]>('attraction', this.appUserId)
        .subscribe((attractions) => {this.attractionList = attractions});
        this.userService.getMyRecords<AttractionReview[]>('attractionreview', this.appUserId)
        .subscribe((reviews) => {this.attractionReviewList = reviews});
      this.userService.getMyRecords<ClanSeek[]>('clanseek', this.appUserId)
        .subscribe((clanSeeks) => {this.clanList = clanSeeks});
    })
  }

}
