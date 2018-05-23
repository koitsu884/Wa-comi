import { Component, OnInit, Input } from '@angular/core';
import { ClanSeek } from '../../_models/ClanSeek';
import { City } from '../../_models/City';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ClanSeekCategoryResolver } from '../../_resolvers/clanseek-categories.resolver';
import { ClanSeekCategory } from '../../_models/ClanSeekCategory';
import { Photo } from '../../_models/Photo';
import { NgForm } from '@angular/forms';

import { Store } from '@ngrx/store';
import * as fromClan from '../store/clan.reducers';
import * as ClanActions from '../store/clan.actions';

@Component({
  selector: 'app-clan-edit',
  templateUrl: './clan-edit.component.html',
  styleUrls: ['./clan-edit.component.css']
})
export class ClanEditComponent implements OnInit {
  @Input() editMode : boolean;
  id: number;
  memberId: number;
  editingClan: ClanSeek;
  cities: City[];
  categories: ClanSeekCategory[];
  photos: Photo[];

  constructor(private route: ActivatedRoute, private router: Router, private store: Store<fromClan.FeatureState>) { }

  ngOnInit() {
    this.route.params
      .subscribe(
        (params: Params) => {
          this.id = +params['id'];
          this.memberId = +params['memberId'];
          console.log(params);
          if(this.memberId == null){
            this.router.navigate(['/home']);
            return;
          }
          this.editMode = params['id'] != null;
          this.cities = this.route.snapshot.data['cities'];
          this.photos = this.route.snapshot.data['photos'];
          this.categories = this.route.snapshot.data['categories'];
          this.initForm();
        }
      );
  }

  initForm(){
    if(this.editMode){
      this.editingClan = this.route.snapshot.data['editingClan'];
    }
    else{
      this.editingClan = <ClanSeek>{};
    }
    this.editingClan.memberId = this.memberId;
  }

  onClear(){
    this.initForm();
  }

  mainPhotoSelected(event, ngForm: NgForm){
    this.editingClan.mainPhotoUrl = event;
    ngForm.form.markAsDirty();
  }

  submit(){
    if(this.editMode)
      this.store.dispatch(new ClanActions.UpdateClanSeek(this.editingClan));
    else
      this.store.dispatch(new ClanActions.TryAddClanSeek(this.editingClan));
  }
}
