import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Photo } from '../../_models/Photo';
import { NgForm } from '@angular/forms';
import { OuterSubscriber } from 'rxjs/OuterSubscriber';

@Component({
  selector: 'app-comment-form',
  templateUrl: './comment-form.component.html',
  styleUrls: ['./comment-form.component.css']
})
export class CommentFormComponent implements OnInit {
  @Input() maxLength: number;
  @Input() originalPhoto: Photo;
  @Output() addComment = new EventEmitter<{comment:string, imageFile:File}>(); 

  selectedFile: File = null;
  previewUrl: string;
  comment:string = '';
  rowNum: number;

  constructor() { }

  ngOnInit() {
    this.rowNum = this.maxLength / 100;
    if(this.rowNum > 10)
      this.rowNum = 10;
  }

  onSubmit(){
    this.addComment.emit({comment:this.comment, imageFile:this.selectedFile ? this.selectedFile[0] : null});
    this.selectedFile = null;
    this.previewUrl = null;
    this.comment = '';
  }

  setSelectedFiles(event: any){
    this.selectedFile = event.selectedFiles;
    this.previewUrl = event.previewUrls;
    console.log(this.selectedFile);
  }

  deletePhoto(){
    this.selectedFile = null;
    this.previewUrl = this.originalPhoto ? this.originalPhoto.url : null;
  }
}
