import { Component, OnInit } from '@angular/core';
import { Feedback } from '../_models/Feedback';
import { Store } from '@ngrx/store';
import { AlertifyService } from '../_services/alertify.service';
import * as fromApp from '../store/app.reducer';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Location } from '@angular/common';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})
export class ContactComponent implements OnInit {
  baseUrl = environment.apiUrl;
  feedback: Feedback = <Feedback>{};
  appUserId: number;
  email: string = "info@wa-comi.com";
  sending:boolean = false;
  constructor(private store: Store<fromApp.AppState>,
    private alertify: AlertifyService,
    private location: Location,
  private httpClient: HttpClient) { }

  ngOnInit() {
    this.store.select('account').take(1)
      .subscribe((accountState) => {
          this.feedback.senderId = accountState.appUser ? accountState.appUser.id : null;
          this.feedback.senderName = accountState.appUser ? accountState.appUser.displayName : "";
      });
  }

  submit(){
    this.sending = true;
    this.httpClient.post<Feedback>(this.baseUrl + 'feedback',
                this.feedback,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                })
                .subscribe(() => {
                  this.alertify.success("お問い合わせ内容を送信しました");
                  this.location.back();
                }, (error) => {
                  this.alertify.error(error);
                  this.location.back();
                })
  }

  
}
