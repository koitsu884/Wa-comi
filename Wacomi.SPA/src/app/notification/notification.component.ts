import { Component, OnInit } from '@angular/core';
import * as fromApp from '../store/app.reducer';
import * as NotificationActions from './store/notification.action';
import { Store } from '@ngrx/store';
import { AppNotification } from '../_models/Notification';
import { AppUser } from '../_models/AppUser';
import { ActivatedRoute, Router } from '@angular/router';
import { GlobalService } from '../_services/global.service';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css']
})
export class NotificationComponent implements OnInit {
  notifications: AppNotification[] = null;
  notificationTypeArray = [];
  appUser: AppUser;

  constructor(private store: Store<fromApp.AppState>, 
    private route: ActivatedRoute, 
    private router: Router, 
    private globalService: GlobalService) { }

  ngOnInit() {
    this.appUser = this.route.snapshot.data["appUser"];
    if (!this.appUser) {
      console.log("AppUser 入ってないで");
      this.router.navigate(['/']);
    }

    this.store.select('notification')
      .subscribe((notificationState) => {
        this.notifications = notificationState.notifications;
      });
    this.store.dispatch(new NotificationActions.GetNotifications(this.appUser.id));
    this.notificationTypeArray = this.globalService.getNotificationTypes();
  }

  onDelete(id: number) {
    this.store.dispatch(new NotificationActions.TryDeleteNotification(id));
  }

  onClick(notification: AppNotification) {
    this.store.dispatch(new NotificationActions.TryDeleteNotification(notification.id));
    switch (notification.notificationType) {
      case this.notificationTypeArray["NewMessage"]:
        this.router.navigate(['/message']);
        break;
      case this.notificationTypeArray["NewPostOnFeedComment"]:
      case this.notificationTypeArray["RepliedOnFeedComment"]:
        this.router.navigate(['/blog/feed', notification.recordId]);
        break;
      case this.notificationTypeArray["NewPostOnTopicComment"]:
      case this.notificationTypeArray["RepliedOnTopicComment"]:
        this.router.navigate(['/dailytopic', notification.recordId]);
        break;
    }
  }

}
