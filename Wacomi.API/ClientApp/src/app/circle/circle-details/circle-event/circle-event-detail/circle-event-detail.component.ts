import * as fromCircle from '../../../store/circle.reducers';
import * as CircleActions from '../../../store/circle.actions';
import * as CircleEventActions from '../../../store/circleevent.actions';
import * as GlobalActions from '../../../../store/global.actions';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppUser } from '../../../../_models/AppUser';
import { CircleEvent } from '../../../../_models/CircleEvent';
import { Store } from '@ngrx/store';
import { AlertifyService } from '../../../../_services/alertify.service';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';
import { Photo } from '../../../../_models/Photo';
import { CircleEventParticipation, CircleEventParticipationStatus } from '../../../../_models/CircleEventParticipation';
import { MessageService } from '../../../../_services/message.service';

@Component({
  selector: 'app-circle-event-detail',
  templateUrl: './circle-event-detail.component.html',
  styleUrls: ['./circle-event-detail.component.css']
})
export class CircleEventDetailComponent implements OnInit {
  id:number;
  appUser:AppUser;
  event:CircleEvent = null;
  isMember:boolean = false;
  isPast:boolean = false;
  latestParticipations: CircleEventParticipation[] = null;
  loading:boolean = false;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[] = null;
  requestMessage: string = '';
  forcusCommentId: number = null;

  circleEventParticipationStatus = CircleEventParticipationStatus;
  
  constructor(private route:ActivatedRoute, 
    private store:Store<fromCircle.FeatureState>, 
    private alertify: AlertifyService,
    private messageService: MessageService,
    private router: Router
    ) { }

  ngOnInit() {
    let id = this.route.snapshot.params['id'];
    if (id == null) {
      console.log("No parameter was set");
      return;
    }
    this.appUser = this.route.parent.snapshot.data['appUser'];
    this.forcusCommentId = this.route.snapshot.params['forcusCommentId'];
    this.loading = true;
    this.store.dispatch(new CircleEventActions.GetCircleEvent(id));
    this.store.dispatch(new CircleEventActions.GetLatestEventParticipants(id));
    this.store.select('circleModule').subscribe((circleState) => {
      this.event = circleState.circleEvent.selectedCircleEvent;
      if(this.event){
        if(!this.event.isPublic && !this.event.isCircleMember){
          this.alertify.error("このイベントはサークルメンバー限定です");
          this.router.navigate(['/circle/detail', this.event.circleId]);
          return;
        }
        this.isPast = new Date(this.event.fromDate) < new Date();
        this.loading = false;
        if(!this.galleryImages){
          this.galleryImages = this.getImages(this.event.photos);
        }
      }
  
      this.latestParticipations = circleState.circleEvent.latestEventParticipants;
    })

    this.galleryOptions = [
      {
        //breakpoint: 800,
        width: '100%',
        height: '100%',
        // imageSize:"contain",
       // imagePercent: 100,
        thumbnailsColumns:4,
      //  thumbnailMargin:0,
        imageArrowsAutoHide: true,
        imageAnimation: NgxGalleryAnimation.Fade,
        thumbnails: true,
        preview: true,
        image: false
      }
    ];
  }

  getImages(photos: Photo[]) {
    const imageUrls = [];
    for (let i = 0; i < photos.length; i++){
      imageUrls.push({
        small: photos[i].thumbnailUrl,
        medium: photos[i].url,
        big: photos[i].url,
        description: photos[i].description
      });
    }
    return imageUrls
  }

  onDelete(){
    this.alertify.confirm("本当にこのイベントを削除しますか？", () => {
      this.store.dispatch(new GlobalActions.DeleteRecord({
        recordType:"CircleEvent", 
        recordId: this.event.id, 
        // callbackLocation: '/'
        callbackLocation:'/circle/detail/' + this.event.circleId,
        callbackActions: [{type: CircleActions.GET_LATEST_CIRCLE_EVENT_LIST, payload: this.event.circleId}]
      }));
    })
  }

  canSendRequest(){
    return (
      this.appUser 
      && this.appUser.id != this.event.appUserId 
      && (this.event.isPublic || this.event.isCircleMember) 
      && !this.event.myStatus
      )
  }

  onSendRequest() {
    this.alertify.confirm("このイベントに参加しますか？", () => {
      this.store.dispatch(new CircleEventActions.SendEventParticipationRequest({appUserId: this.appUser.id, circleEventId: this.event.id, message:''}))
    });
  }

  onCancelRequest() {
    this.store.dispatch(new CircleEventActions.DeleteEventParticipation({appUserId: this.appUser.id, circleEventId: this.event.id}));
  }

  onCancelParticipation() {
    this.alertify.confirm("本当に参加をキャンセルしますか？", () => {
      this.store.dispatch(new CircleEventActions.CancelEventParticipation({appUserId: this.appUser.id, circleEventId: this.event.id}));
    });
  }

  onSendMessage(){
    this.messageService.preparSendingeMessage(
      {
        title: "RE:" + this.event.title,
        recipientDisplayName: this.event.appUser.displayName,
        recipientId: this.event.appUserId,
        senderId: this.appUser.id
      },
      "<p class='text-info'>以下のイベントの開催者にメッセージを送ります</p>"
       + "<h5>イベントタイトル：" + this.event.title + "</h5>"
       + this.event.description
    );
    this.router.navigate(['/message/send']);
  }
}
