import * as fromCircle from '../../../store/circle.reducers';
import * as CircleActions from '../../../store/circle.actions';
import * as CircleTopicActions from '../../../store/circletopic.actions';
import * as GlobalActions from '../../../../store/global.actions';
import { Component, OnInit } from '@angular/core';
import { CircleTopic } from '../../../../_models/CircleTopic';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { AppUser } from '../../../../_models/AppUser';
import { Store } from '@ngrx/store';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-circle-topic-edit',
  templateUrl: './circle-topic-edit.component.html',
  styleUrls: ['./circle-topic-edit.component.css']
})
export class CircleTopicEditComponent implements OnInit {
  id: number;
  appUser: AppUser;
  circleTopic: CircleTopic;
  circleId: number;
  selectedFile: File = null;
  previewUrl: string;

  constructor(private route: ActivatedRoute, private router:Router, private store:Store<fromCircle.FeatureState>) { }

  ngOnInit() {
    this.circleId = this.route.parent.snapshot.params['id'];
    this.appUser= this.route.parent.snapshot.data['appUser'];
    if (this.appUser == null) {
      console.log("AppUser was not set");
      this.router.navigate(['/circle']);
      return;
    }

    this.route.params
      .subscribe(
        (params: Params) => {
          this.id = +params['id'];
          this.initForm();
        }
      );
  }

  initForm() {
    if (this.id) { //Edit mode
      this.store.dispatch(new CircleTopicActions.GetCircleTopic(this.id))
      this.store.select("circleModule").subscribe((circleModuleState) => {
        this.circleTopic = Object.assign(<CircleTopic>{}, circleModuleState.circleTopic.selectedCircleTopic);
      })
    }
    else {
      this.circleTopic = <CircleTopic>{};
      this.circleTopic.appUserId = this.appUser.id;
      this.circleTopic.circleId = this.circleId;
    }
    this.circleTopic.appUser = this.appUser;
  }

  setSelectedFiles(event: any, ngForm: NgForm){
    this.selectedFile = event.selectedFiles;
    this.previewUrl = event.previewUrls;
    ngForm.form.markAsDirty();
  }

  deletePhoto(){
    this.selectedFile = null;
    this.previewUrl = this.circleTopic.photo ? this.circleTopic.photo.url : null;
  }

  onSubmit() {
    if (this.id) {
      this.store.dispatch(new CircleTopicActions.UpdateCircleTopic({circleTopic:this.circleTopic, photo: this.selectedFile ? this.selectedFile[0] : null}));
     // this.location.back();
    } else {
      var formData = new FormData();
      if(this.selectedFile)
        formData.append("files", this.selectedFile[0]);

      this.store.dispatch(new GlobalActions.TryAddRecord({
        recordType:'circleTopic', 
        record: this.circleTopic, 
        formData: this.selectedFile ?  formData : null, 
        callbackLocation:'/circle/detail/' + this.circleId,
        callbackActions: [
          { type: GlobalActions.SUCCESS, payload: "投稿しました" },
          { type: CircleActions.ADD_NEW_CIRCLE_TOPIC, payload: this.circleTopic }
        ]
      }));
    }
  }

}
