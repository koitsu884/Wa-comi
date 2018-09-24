import * as GlobalActions from '../../../store/global.actions';
import * as CircleActions from '../../store/circle.actions';
import * as CircleMemberActions from '../../store/circlemember.actions';
import * as fromCircle from '../../store/circle.reducers';
import { Component, OnInit } from '@angular/core';
import { CircleMember } from '../../../_models/CircleMember';
import { Circle } from '../../../_models/Circle';
import { AppUser } from '../../../_models/AppUser';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from '../../../_services/alertify.service';
import { Store } from '@ngrx/store';
import { MessageService } from '../../../_services/message.service';

@Component({
  selector: 'app-circle-overview',
  templateUrl: './circle-overview.component.html',
  styleUrls: ['./circle-overview.component.css']
})
export class CircleOverviewComponent implements OnInit {
  circle: Circle;
  latestMembers: CircleMember[];
  loading: boolean;
  appUser: AppUser;
  isMine: boolean;
  attachShortMessage: boolean = false;
  requestSent: boolean = false;
  shortMessage: string = "";
  
  constructor(private route: ActivatedRoute, 
    private router: Router, 
    private alertify: AlertifyService,
    private store: Store<fromCircle.FeatureState>,
    private messageService: MessageService) { }

  ngOnInit() {
    this.circle = null;
    this.loading = true;
    let id = this.route.parent.snapshot.params['id'];
    this.appUser= this.route.parent.snapshot.data['appUser'];
    // this.memberId = appUser ? appUser.relatedUserClassId : null;
    if(!id){
      this.router.navigate(['/circle']);
      return;
    }
    // this.store.dispatch(new CircleActions.GetCircle(id));
    this.store.select('circleModule').subscribe((circleState) => {
      this.circle = circleState.circle.selectedCircle;
      this.latestMembers = circleState.circleMember.latestMemberList;
      if(this.appUser && this.circle)
        this.isMine = this.appUser.id == this.circle.appUser.id;
      this.loading = false;
    });
  }

  onMessageSend() {
    this.messageService.preparSendingeMessage(
      {
        title: "RE:" + this.circle.name,
        recipientDisplayName: this.circle.appUser.displayName,
        recipientId: this.circle.appUser.id,
        senderId: this.appUser.id
      },
      "<p class='text-info'>以下のコミュニティに対して管理人にメッセージを送ります</p>"
       + "<h5>コミュニティ名：" + this.circle.name + "</h5>"
       + this.circle.description
    );
    this.router.navigate(['/message/send']);
  }

  onSendRequest(){
    this.store.dispatch(new CircleMemberActions.SendCircleRequest({
      appUserId: this.appUser.id, 
      circleId: this.circle.id, 
      requireApproval:this.circle.approvalRequired, 
      message: this.attachShortMessage ? this.shortMessage : null
    }));
    this.requestSent = true;
  }

  onDelete(id: number){
    this.alertify.confirm('本当に削除しますか?', () => {
      this.loading = true;
      this.store.dispatch(new CircleActions.InitCircleState());
      this.store.dispatch(new GlobalActions.DeleteRecord({recordType:'circle', recordId:id, callbackLocation:'/users/posts/' + this.appUser.id}));
    })
  }

  onLeaveCircle(){
    this.alertify.confirm('本当に脱退しますか？',()=>{
      this.store.dispatch(new CircleMemberActions.DeleteCircleMember({circleId: this.circle.id, appUserId: this.appUser.id}));
      this.alertify.success("脱退しました");
    })
  }

}
