import { Component, OnInit } from '@angular/core';
import { GlobalService } from '../../_services/global.service';
import { AlertifyService } from '../../_services/alertify.service';

@Component({
  selector: 'app-password-forgot',
  templateUrl: './password-forgot.component.html',
  styleUrls: ['./password-forgot.component.css']
})
export class PasswordForgotComponent implements OnInit {
  userId: string;
  email: string;
  sent: boolean;

  constructor(private globalService: GlobalService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.sent=false;
  }

  submit(){
    this.globalService.sendPasswordResetCode(this.userId, this.email)
      .subscribe(() => {
        this.sent = true;
      }, (error) => {
        this.alertify.error(error);
      });
  }
}
