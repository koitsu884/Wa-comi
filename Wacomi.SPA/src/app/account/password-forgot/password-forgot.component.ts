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
  status:string;
  //sent: boolean;

  constructor(private globalService: GlobalService, private alertify: AlertifyService) { }

  ngOnInit() {
    // this.sent=false;
    this.status="";
  }

  submit(){
    this.status="sending";
    this.globalService.sendPasswordResetCode(this.userId, this.email)
      .subscribe(() => {
        this.status = "sent";
      }, (error) => {
        this.alertify.error(error);
      });
  }
}
