import { Component, OnInit } from '@angular/core';
import { ClanSeek } from '../../_models/ClanSeek';
import { ActivatedRoute, Router } from '@angular/router';
import * as fromClan from '../store/clan.reducers';
import * as fromAccount from '../../account/store/account.reducers';
import * as ClanSeekActions from '../store/clan.actions';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { AlertifyService } from '../../_services/alertify.service';
import { AppUser } from '../../_models/AppUser';

@Component({
  selector: 'app-clan-detail',
  templateUrl: './clan-detail.component.html',
  styleUrls: ['./clan-detail.component.css']
})
export class ClanDetailComponent implements OnInit {
  clanId: number;
  appUser: AppUser;
  memberId: number;
  // clanState: Observable<fromClan.State>;
  editingClan: ClanSeek;
  // accountState: Observable<fromAccount.State>;

  constructor(private route: ActivatedRoute, 
              private router:Router, 
              private store: Store<fromClan.FeatureState>,
              private alertify: AlertifyService) { }

  ngOnInit() {
    this.clanId = this.route.snapshot.params['id'];
    this.appUser= this.route.snapshot.data['appUser'];
    // this.memberId = appUser ? appUser.relatedUserClassId : null;
    if(!this.clanId){
      this.router.navigate(['/clan']);
      return;
    }
    this.store.dispatch(new ClanSeekActions.GetClanSeek(this.clanId));
    this.store.select('clan').subscribe((clanState) => {this.editingClan = clanState.editingClan});
    // this.accountState = this.store.select('account').take(1);
  }

  onDelete() {
    this.alertify.confirm("本当に削除しますか？", () => {
      this.store.dispatch(new ClanSeekActions.ClearClanseekFilters());
      this.store.dispatch(new ClanSeekActions.TryDeleteClanSeek(this.editingClan.id));
    });
  }
}
