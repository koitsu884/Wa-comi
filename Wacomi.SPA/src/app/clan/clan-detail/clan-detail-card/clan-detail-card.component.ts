import { Component, OnInit, Input } from '@angular/core';
import { ClanSeek } from '../../../_models/ClanSeek';
import { AppUser } from '../../../_models/AppUser';
import { Store } from '@ngrx/store';
import { AlertifyService } from '../../../_services/alertify.service';
import * as fromClan from '../../store/clan.reducers';
import * as ClanSeekActions from '../../store/clan.actions';
import { Router } from '@angular/router';
import { MessageService } from '../../../_services/message.service';

@Component({
  selector: 'app-clan-detail-card',
  templateUrl: './clan-detail-card.component.html',
  styleUrls: ['./clan-detail-card.component.css']
})
export class ClanDetailCardComponent implements OnInit {
  @Input() clanSeek: ClanSeek;
  @Input() appUser: AppUser;

  constructor(private store: Store<fromClan.FeatureState>,
    private alertify: AlertifyService,
    private messageService: MessageService,
    private router: Router) { }

  ngOnInit() {
  }

  onDelete() {
    this.alertify.confirm("本当に削除しますか？", () => {
      this.store.dispatch(new ClanSeekActions.TryDeleteClanSeek(this.clanSeek.id));
    });
  }

  onSend() {
    this.messageService.preparSendingeMessage(
      {
        title: "RE:" + this.clanSeek.title,
        recipientDisplayName: this.clanSeek.displayName,
        recipientId: this.clanSeek.appUserId,
        senderId: this.appUser.id
      },
      this.clanSeek.description
    );
    this.router.navigate(['/message/send']);
  }
}
