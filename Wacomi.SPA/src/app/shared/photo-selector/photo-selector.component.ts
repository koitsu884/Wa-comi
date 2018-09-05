import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { AlertifyService } from '../../_services/alertify.service';
import * as loadImage from 'blueimp-load-image';

@Component({
  selector: 'app-photo-selector',
  templateUrl: './photo-selector.component.html',
  styleUrls: ['./photo-selector.component.css']
})
export class PhotoSelectorComponent implements OnInit {
  @Output() photoSelected = new EventEmitter<any>();
  @Input() multipleSelect: boolean = true;

  readonly FILE_UPLOAD_LIMIT = 5;
  readonly IMAGE_SIZE = 600;
  selectedFiles: Array<File> = [];
  previewUrls: Array<string> = [];

  constructor(private alertify: AlertifyService) { }

  ngOnInit() {
  }

  setUploadingFiles(files: File[]) {
    if (files.length == 0)
      return;
    if (files.length > this.FILE_UPLOAD_LIMIT) {
      this.alertify.error(`アップロードできるファイルは${this.FILE_UPLOAD_LIMIT}つまでです`);
      this.previewUrls = [];
      return;
    }

    this.selectedFiles = [];
    this.previewUrls = [];

    for (let file of files) {
      loadImage(
        file,
        (canvas) => {
          // console.log(canvas);
          if (canvas.type === "error") {
            console.log("Error loading image " + file.name);
          } else {
            let base64 = canvas.toDataURL('image/jpeg');
            this.previewUrls.push(base64);
            var selectedFile = new File([this.dataURItoBlob(base64)], file.name, {type: "image/jpeg"});
            this.selectedFiles.push(selectedFile);
          }
        },
        {
          maxWidth: this.IMAGE_SIZE,
          maxHeight: this.IMAGE_SIZE,
          canvas: true,
          orientation: true
        }
      );
    }

    this.photoSelected.emit({selectedFiles: this.selectedFiles, previewUrls: this.previewUrls});
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
