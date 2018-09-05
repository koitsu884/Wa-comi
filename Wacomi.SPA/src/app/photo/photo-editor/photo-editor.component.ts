import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { Photo } from '../../_models/Photo';
import { FileUploader } from 'ng2-file-upload';
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
import * as loadImage from 'blueimp-load-image';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit, OnDestroy {
  readonly FILE_UPLOAD_LIMIT = 5;
  readonly IMAGE_SIZE = 500;

  title: string;
  photos: Photo[] = [];
  selectedPhoto: Photo;
  baseUrl = environment.apiUrl;
  selectedFile: File;
  previewUrl: string;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private alertify: AlertifyService,
    private location: Location,
    private store: Store<fromPhoto.FeatureState>) { }

  ngOnInit() {
    this.store.select('photo')
      .subscribe((state => {
        this.photos = state.photos
      }));

    this.route.params.subscribe(params => {
      let recordType = params["recordType"];
      let recordId = +params["recordId"];
      if (!recordType || !recordId) {
        this.alertify.error("パラメーターが未設定です");
        this.router.navigate(['/']);
        return;
      }
      switch (recordType) {
        case "AppUser":
          this.title = "プロフィール写真編集"
          break;
        case "ClanSeek":
          this.title = "仲間募集写真編集"
          break;
      }
      this.store.dispatch(new PhotoActions.GetPhotos({ recordType: recordType, recordId: recordId }));
    });
  }

  ngOnDestroy() {
    this.store.dispatch(new PhotoActions.ClearPhoto());
  }

  onFileChanged(event) {
   // this.selectedFile = event.target.files[0];
    loadImage(
      event.target.files[0],
      (canvas) => {
        if (canvas.type === "error") {
          console.log("Error loading image " + event.target.files[0].name);
        } else {
          let base64 = canvas.toDataURL('image/jpeg');
          this.previewUrl = base64;
          this.selectedFile = new File([this.dataURItoBlob(base64)], event.target.files[0].name, {type: "image/jpeg"});
        }
      },
      {
        maxWidth: this.IMAGE_SIZE,
        maxHeight: this.IMAGE_SIZE,
        canvas: true,
        orientation: true
      }
    );

    // if (this.selectedFile.size > 1024 * 1024 * 2) {
    //   this.selectedFile = null;
    //   this.alertify.error("2MB以下の画像を選択してください");
    //   return;
    // }
    // var reader = new FileReader();

    // reader.onload = (event: any) => {
    //   this.previewUrl = event.target.result;
    // }
    // reader.readAsDataURL(event.target.files[0]);
  }

  onUpload() {
    this.store.dispatch(new PhotoActions.TryAddPhoto(this.selectedFile));
  }

  deletePhoto(id: number) {
    this.alertify.confirm('この写真を本当に削除しますか?', () => {
      this.store.dispatch(new PhotoActions.TryDeletePhoto(id));
    })
  }

  onClickOk() {
    this.location.back();
  }

  private dataURItoBlob(dataURI) {
    var byteString = atob(dataURI.split(',')[1]);
    var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0]
    var ab = new ArrayBuffer(byteString.length);
    var ia = new Uint8Array(ab);

    for (var i = 0; i < byteString.length; i++) {
      ia[i] = byteString.charCodeAt(i);
    }

    var blob = new Blob([ab], { type: mimeString });
    return blob;
  }
}
