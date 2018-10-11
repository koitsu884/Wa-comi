import { Component, OnInit } from '@angular/core';
import * as fromApp from '../store/app.reducer';
import * as NotificationActions from './store/notification.action';
import { Store } from '@ngrx/store';
import { AppNotification, NotificationEnum } from '../_models/Notification';
import { AppUser } from '../_models/AppUser';
import { ActivatedRoute, Router } from '@angular/router';
import { GlobalService } from '../_services/global.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css']
})
export class NotificationComponent implements OnInit {
  notifications: AppNotification[] = null;
  notificationTypeArray = [];
  appUser: AppUser;
  loading: boolean = false;
  baseUrl = environment.apiUrl;

  constructor(private store: Store<fromApp.AppState>,
    private route: ActivatedRoute,
    private router: Router,
    private httpClient: HttpClient,
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

  getNotificationMessage(notification: AppNotification) {
    switch (notification.notificationType) {
      case NotificationEnum.NewMessage:
        return `${notification.fromUserName}さんから新しいメッセージが届いています`;
      case NotificationEnum.NewPostOnFeedComment:
        return `ブログフィード『${notification.targetRecordTitle}』に新しいコメントがあります`;
      case NotificationEnum.RepliedOnFeedComment:
        return `あなたがコメントしたブログフィード『${notification.targetRecordTitle}』に${notification.fromUserName}さんもコメントしました`;
      case NotificationEnum.NewPostOnTopicComment:
        return `あなたの一言『${notification.targetRecordTitle}』に新しいコメントがあります`;
      case NotificationEnum.RepliedOnTopicComment:
        return `あなたが一言トピック『${notification.targetRecordTitle}』にしたコメントに、${notification.fromUserName}さんもコメントしました`;
      case NotificationEnum.NewCircleMemberRequest:
        return `コミュニティ『${notification.targetRecordTitle}』に新しい参加希望者が居ます`;
      case NotificationEnum.CircleRequestAccepted:
        return `コミュニティ『${notification.targetRecordTitle}』への参加が承認されました`;
      case NotificationEnum.NewCircleTopicCreated:
        let titles = notification.targetRecordTitle.split("|", 2);
        return `コミュニティ『${titles[0]}』に新しいトピック『${titles[1]}』が作成されました`;
      case NotificationEnum.NewCircleCommentReplyByOwner:
        return `コミュニティトピック『${notification.targetRecordTitle}』にあなたがしたコメントに返信があります`;
      case NotificationEnum.NewCircleCommentReplyByMember:
        return `コミュニティトピック『${notification.targetRecordTitle}』であなたが返信したコメントに、${notification.fromUserName}さんも返信しました`;
    }
  }

  onClick(notification: AppNotification) {
    if (this.loading)
      return;
    this.store.dispatch(new NotificationActions.TryDeleteNotification(notification.id));
    switch (notification.notificationType) {
      case NotificationEnum.NewMessage:
        this.router.navigate(['/message']);
        break;
      case NotificationEnum.NewPostOnFeedComment:
      case NotificationEnum.RepliedOnFeedComment:
        this.router.navigate(['/blog/feed', notification.recordId]);
        break;
      case NotificationEnum.NewPostOnTopicComment:
      case NotificationEnum.RepliedOnTopicComment:
        this.router.navigate(['/dailytopic', notification.recordId]);
        break;
      case NotificationEnum.NewCircleMemberRequest:
      case NotificationEnum.CircleRequestAccepted:
        this.router.navigate(['/circle/detail', notification.recordId]);
        break;
      case NotificationEnum.NewCircleTopicCreated:
        this.router.navigate(['/circle/detail', notification.relatingRecordIds.Circle, 'topic', 'detail', notification.recordId]);
        break;
      case NotificationEnum.NewCircleCommentReplyByOwner:
      case NotificationEnum.NewCircleCommentReplyByMember:
        this.router.navigate(['/circle/detail', notification.relatingRecordIds.Circle, 'topic', 'detail', notification.relatingRecordIds.CircleTopic, notification.recordId]);
        break;
    }

    // switch (notification.notificationType) {
    //   case this.notificationTypeArray["NewMessage"]:
    //     this.router.navigate(['/message']);
    //     break;
    //   case this.notificationTypeArray["NewPostOnFeedComment"]:
    //   case this.notificationTypeArray["RepliedOnFeedComment"]:
    //     this.router.navigate(['/blog/feed', notification.recordId]);
    //     break;
    //   case this.notificationTypeArray["NewPostOnTopicComment"]:
    //   case this.notificationTypeArray["RepliedOnTopicComment"]:
    //     this.router.navigate(['/dailytopic', notification.recordId]);
    //     break;
    // }
  }

}
