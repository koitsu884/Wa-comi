import * as fromProperty from '../store/property.reducers';
import * as PropertyActions from '../store/property.actions';
import { Component, OnInit } from '@angular/core';
import { Property } from '../../_models/Property';
import { ActivatedRoute, Router } from '@angular/router';
import { AppUser } from '../../_models/AppUser';
import { Store } from '@ngrx/store';
import { AlertifyService } from '../../_services/alertify.service';
import { MessageService } from '../../_services/message.service';

@Component({
  selector: 'app-property-details',
  templateUrl: './property-details.component.html',
  styleUrls: ['./property-details.component.css']
})
export class PropertyDetailsComponent implements OnInit {
  property: Property;
  loading: boolean;
  propertyId: number;
  appUser: AppUser;
  isMine: boolean = false;
  
  constructor(private route:ActivatedRoute, 
    private alertify: AlertifyService,
    private router: Router, 
    private messageService: MessageService,
    private store: Store<fromProperty.FeatureState> ) { }

  ngOnInit() {
    this.property = null;
    this.loading = true;
    this.propertyId = this.route.snapshot.params['id'];
    this.appUser= this.route.snapshot.data['appUser'];
    // this.memberId = appUser ? appUser.relatedUserClassId : null;
    if(!this.propertyId){
      this.router.navigate(['/property']);
      return;
    }
    this.store.dispatch(new PropertyActions.GetProperty(this.propertyId));
    this.store.select('property').subscribe((propertyState) => {
      this.property = propertyState.selectedProperty;
      if(this.appUser && this.property)
        this.isMine = this.appUser.id == this.property.appUser.id;
      this.loading = false;
    });
  }

  onDelete(id: number){
    this.alertify.confirm('本当に削除しますか?', () => {
      this.loading = true;
      this.store.dispatch(new PropertyActions.TryDeleteProperty(id));
    })
  }

  onMessageSend() {
    this.messageService.preparSendingeMessage(
      {
        title: "RE:" + this.property.title,
        recipientDisplayName: this.property.appUser.displayName,
        recipientId: this.property.appUser.id,
        senderId: this.appUser.id
      },
      "<p class='text-info'>以下の住まい広告に対して返信します</p>"
       + "<h5>募集タイトル：" + this.property.title + "</h5>"
       + this.property.description
    );
    this.router.navigate(['/message/send']);
  }

}
