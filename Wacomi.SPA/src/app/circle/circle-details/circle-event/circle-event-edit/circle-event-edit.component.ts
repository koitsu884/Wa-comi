import * as fromCircle from '../../../store/circle.reducers';
import * as CircleEventActions from '../../../store/circleevent.actions';
import * as CircleActions from '../../../store/circle.actions';
import * as GlobalActions from '../../../../store/global.actions';
import { Component, OnInit } from '@angular/core';
import { AppUser } from '../../../../_models/AppUser';
import { CircleEvent } from '../../../../_models/CircleEvent';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { Store } from '@ngrx/store';
import { NgForm } from '@angular/forms';
import { City } from '../../../../_models/City';
import { Location } from '@angular/common';
import {jaLocale} from 'ngx-bootstrap/locale';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
defineLocale("ja", jaLocale);

@Component({
  selector: 'app-circle-event-edit',
  templateUrl: './circle-event-edit.component.html',
  styleUrls: ['./circle-event-edit.component.css']
})
export class CircleEventEditComponent implements OnInit {
  id: number;
  appUser: AppUser;
  cities: City[];
  circleEvent: CircleEvent;
  circleId: number;
  selectedFiles: Array<File> = [];
  previewUrls: Array<string> = [];
  hasLimit: boolean = false;
  bsConfig: Partial<BsDatepickerConfig>;

  loading:boolean = false;
  fromDate:Date;
  strFromTime: string;
  strToTime: string;

  constructor(private route: ActivatedRoute, 
    private router:Router, 
    private location:Location, 
    private localeService: BsLocaleService,
    private store:Store<fromCircle.FeatureState>) { }

  ngOnInit() {
    this.bsConfig = {
      dateInputFormat: "YYYY年MM月DD日"
    };
    this.localeService.use('ja');

    this.circleId = this.route.parent.snapshot.params['id'];
    this.appUser= this.route.parent.snapshot.data['appUser'];
    this.cities = this.route.snapshot.data['cities'];
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
      this.loading = true;
      this.store.dispatch(new CircleEventActions.GetCircleEvent(this.id))
      this.store.select("circleModule").subscribe((circleModuleState) => {
        if(!circleModuleState.circleEvent.selectedCircleEvent)
          return;
        this.circleEvent = Object.assign({}, circleModuleState.circleEvent.selectedCircleEvent);
        if(this.circleEvent.maxNumber)
          this.hasLimit = true;
        this.fromDate = new Date(this.circleEvent.fromDate);
        // console.log(this.circleEvent.fromDate);
        // console.log(this.fromDate);
        this.strFromTime = this.fromDate.toTimeString().substr(0, 5);

        if(this.circleEvent.toDate){
          let toDate = new Date(this.circleEvent.toDate);
          this.strToTime = toDate.toTimeString().substr(0, 5);
        }
        this.loading = false;
      })
    }
    else {
      this.circleEvent = <CircleEvent>{};
      this.circleEvent.appUserId = this.appUser.id;
      this.circleEvent.circleId = this.circleId;
      this.fromDate = new Date();
      this.strFromTime = "12:00";
      this.circleEvent.appUser = this.appUser;
      this.circleEvent.approvalRequired = false;
      this.circleEvent.isPublic = false;
    }
  }

  mainPhotoSelected(event, ngForm: NgForm) {
    this.circleEvent.mainPhotoId = event;
    ngForm.form.markAsDirty();
  }

  setSelectedFiles(event: {selectedFiles:Array<File>, previewUrls:Array<string>}){
    this.selectedFiles = event.selectedFiles;
    this.previewUrls = event.previewUrls;
  }

  submit(ngForm: NgForm) {
    if(!this.hasLimit){
      this.circleEvent.maxNumber = null;
    }
    let hourAndMin = this.strFromTime.split(':');
    this.fromDate.setHours(+hourAndMin[0]);
    this.fromDate.setMinutes(+hourAndMin[1]);
    this.circleEvent.fromDate = this.fromDate.toLocaleString("en-US");

    if(!this.strToTime || (new Date ('1/1/1999 ' + this.strToTime) < new Date ('1/1/1999 ' + this.strFromTime)))
    {
        this.circleEvent.toDate = null;
    }
    else{
      let toDate = new Date(this.fromDate);
      let hourAndMin = this.strToTime.split(':');
      toDate.setHours(+hourAndMin[0]);
      toDate.setMinutes(+hourAndMin[1]);
      this.circleEvent.toDate = toDate.toLocaleString("en-US");
    }

    if (this.id) {
      ngForm.form.markAsPristine();
      this.store.dispatch(new GlobalActions.UpdateRecord({
        recordType:'circleEvent',
        record: this.circleEvent,
        callbackLocation:  '/circle/detail/' + this.circleId + '/event/detail/' + this.id,
        recordSetActionType: CircleEventActions.SET_CIRCLE_EVENT
      }));
    }
    else {
      var formData = new FormData();

      for (let file of this.selectedFiles) {
        formData.append("files", file);
      }
      this.store.dispatch(new GlobalActions.TryAddRecord(
        {
          recordType:'circleEvent',
          record: this.circleEvent,
          formData:this.selectedFiles.length > 0 ? formData : null,
          callbackLocation: '/circle/detail/' + this.circleId,
          callbackActions: [
            { type: GlobalActions.SUCCESS, payload: "投稿しました" },
            { type: CircleActions.GET_LATEST_CIRCLE_EVENT_LIST, payload: this.circleEvent.circleId }
          ]
        }));
    }
    this.store.dispatch(new CircleEventActions.SetEventReloadFlag());
  }

  onCancel(){
    this.location.back();
  }

  onMaxNumberChanged(){
    if(this.circleEvent.maxNumber > 1000)
      this.circleEvent.maxNumber = 1000;
  }

  validateTime(){
    if(this.strToTime){
      return new Date ('1/1/1999 ' + this.strToTime) > new Date ('1/1/1999 ' + this.strFromTime);
    }
    return true;
  }
}
