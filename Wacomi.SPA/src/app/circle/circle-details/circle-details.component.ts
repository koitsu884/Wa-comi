import * as GlobalActions from '../../store/global.actions';
import * as CircleActions from '../store/circle.actions';
import * as fromCircle from '../store/circle.reducers';
import { Component, OnInit } from '@angular/core';
import { Circle } from '../../_models/Circle';
import { ActivatedRoute, Router } from '@angular/router';
import { AppUser } from '../../_models/AppUser';
import { Store } from '@ngrx/store';
import { MessageService } from '../../_services/message.service';
import { AlertifyService } from '../../_services/alertify.service';

@Component({
  selector: 'app-circle-details',
  templateUrl: './circle-details.component.html',
  styleUrls: ['./circle-details.component.css']
})
export class CircleDetailsComponent implements OnInit {
  circle: Circle;
  loading: boolean;
  appUser: AppUser;
  isMine: boolean;

  constructor(private route: ActivatedRoute, 
        private router: Router, 
        private alertify: AlertifyService,
        private store: Store<fromCircle.FeatureState>,
        private messageService: MessageService) { }

  ngOnInit() {
    this.circle = null;
    this.loading = true;
    let id = this.route.snapshot.params['id'];
    this.appUser= this.route.snapshot.data['appUser'];
    // this.memberId = appUser ? appUser.relatedUserClassId : null;
    if(!id){
      this.router.navigate(['/circle']);
      return;
    }
    this.store.dispatch(new CircleActions.GetCircle(id));
    this.store.select('circle').subscribe((circleState) => {
      this.circle = circleState.selectedCircle;
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

  onDelete(id: number){
    this.alertify.confirm('本当に削除しますか?', () => {
      this.loading = true;
      this.store.dispatch(new CircleActions.InitCircleState());
      this.store.dispatch(new GlobalActions.DeleteRecord({recordType:'circle', recordId:id, callbackLocation:'/users/posts/' + this.appUser.id}));
    })
  }

}
