import { Component, OnInit, Input } from '@angular/core';
import { Photo } from '../../_models/Photo';
import { FileUploader} from 'ng2-file-upload';
import { environment } from '../../../environments/environment';
import { AlertifyService } from '../../_services/alertify.service';
import * as fromApp from "../../store/app.reducer";
import * as fromPhoto from "../store/photos.reducers";
import * as PhotoActions from '../store/photos.action';
import { Store } from '@ngrx/store';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  // photos: Photo[];
  selectedPhoto: Photo;
  recordId: number;
  type: string;
  baseUrl = environment.apiUrl;
  photoState: Observable<fromPhoto.State>;
  selectedFile: File;
  previewUrl:string;

  constructor(private route: ActivatedRoute,
              private router: Router,
             private alertify: AlertifyService, 
             private store: Store<fromApp.AppState>) { }

  ngOnInit() {
    this.photoState = this.store.select('photos');
    this.route.params.subscribe(params => {
      this.recordId = +params['recordId'];
      this.type = params['type'];
      if(!this.recordId || !this.type){
        this.alertify.error("パラメーターが未設定です");
        console.log(params);
        this.router.navigate(['/home']);
        return;
      }
      this.store.dispatch(new PhotoActions.GetPhotos({type: this.type, recordId: this.recordId}));
    });
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
    this.store.dispatch(new PhotoActions.TryAddPhoto( {type: this.type, recordId: this.recordId, fileData: this.selectedFile}));
  }

  deletePhoto(id: number) {
    this.alertify.confirm('この写真を本当に削除しますか?', () => {
      this.store.dispatch(new PhotoActions.TryDeletePhoto({type: this.type, recordId: this.recordId, photoId: id}));
    })
  }

}
