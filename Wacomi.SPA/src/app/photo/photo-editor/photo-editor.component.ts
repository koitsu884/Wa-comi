import { Component, OnInit, Input } from '@angular/core';
import { Photo } from '../../_models/Photo';
import { FileUploader} from 'ng2-file-upload';
import { environment } from '../../../environments/environment';
import { AlertifyService } from '../../_services/alertify.service';
import * as fromApp from "../../store/app.reducer";
import * as fromPhoto from "../store/photos.reducers";
import * as fromAccount from "../../account/store/account.reducers";
import * as PhotoActions from '../store/photos.action';
import { Store } from '@ngrx/store';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { Location } from '@angular/common';
import { AppUser } from '../../_models/AppUser';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  // photos: Photo[];
  selectedPhoto: Photo;
  appUserId: number;
  baseUrl = environment.apiUrl;
  photoState: Observable<fromPhoto.State>;
  // accountState: Observable<fromAccount.State>;
  selectedFile: File;
  previewUrl:string;

  constructor(private route: ActivatedRoute,
              private router: Router,
             private alertify: AlertifyService, 
             private location: Location,
             private store: Store<fromApp.AppState>) { }

  ngOnInit() {
    this.photoState = this.store.select('photos');
    this.store.select('account')
              .take(1)
              .subscribe((state: fromAccount.State) => {
                this.appUserId = state.appUser.id;
                this.store.dispatch(new PhotoActions.GetPhotos(state.appUser.id));
              }, (error) => {
                this.alertify.error('Problem retrieving data');
                this.router.navigate(['/home']);
                return Observable.of(null);
              });


    // this.route.params.subscribe(params => {
    //   this.appUserId = +params['appUserId'];
    //   if(!this.appUserId){
    //     this.alertify.error("パラメーターが未設定です");
    //     console.log(params);
    //     this.router.navigate(['/home']);
    //     return;
    //   }
    //   this.store.dispatch(new PhotoActions.GetPhotos(this.appUserId));
    // });
  }


  onFileChanged(event) {
    this.selectedFile = event.target.files[0];
    if(this.selectedFile.size > 1024 * 1024){
      this.selectedFile = null;
      this.alertify.error("１MB以下の画像を選択してください");
      return;
    }
    var reader = new FileReader();

    reader.onload = (event:any) => {
      this.previewUrl = event.target.result;
    }
    reader.readAsDataURL(event.target.files[0]);
  }

  onUpload() {
    console.log(this.selectedFile);
    this.store.dispatch(new PhotoActions.TryAddPhoto( {appUserId: this.appUserId, fileData: this.selectedFile}));
  }

  deletePhoto(id: number) {
    this.alertify.confirm('この写真を本当に削除しますか?', () => {
      this.store.dispatch(new PhotoActions.TryDeletePhoto({userId: this.appUserId, id: id}));
    })
  }
}
