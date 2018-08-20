import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import * as fromBlog from '../store/blogs.reducers';
import * as BlogAction from '../store/blogs.actions';
import { Store } from '@ngrx/store';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../../_services/alertify.service';
import { Blog } from '../../_models/Blog';
import { Photo } from '../../_models/Photo';
import { FormGroup, FormControl, Validators, NgForm } from '@angular/forms';
import { GlobalService } from '../../_services/global.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-blog-editor',
  templateUrl: './blog-editor.component.html',
  styleUrls: ['./blog-editor.component.css']
})
export class BlogEditorComponent implements OnInit {
  editMode: boolean;
  loading: boolean;
  feedUriLoading: boolean;
  blog: any = {};
  blogCategories: string[];
  selectedFile: File = null;
  previewUrl: string;

  constructor(private store: Store<fromBlog.FeatureState>,
    private global: GlobalService,
    private route: ActivatedRoute,
    private location: Location,
    private alertify: AlertifyService) {
      this.feedUriLoading = false;
      this.loading = false;
     }

  ngOnInit() {
    this.blogCategories = this.global.getBlogCategories();
    this.route.params.subscribe(params => {
      let blogId = +params['id'];
      this.editMode = blogId ? true : false;
      if (this.editMode) {
        this.loading = true;
        this.store.select('blogs')
          .take(1)
          .subscribe((state: fromBlog.State) => {
            var index = state.blogs.findIndex(x => x.id == blogId);
            if (index >= 0) {
              Object.assign(this.blog, state.blogs[index]);
              this.loading = false;
              if(this.blog.photo)
                this.previewUrl = this.blog.photo.url;
            }
          });
      } else {
        this.store.select('account')
          .take(1)
          .subscribe((state) => {
            this.blog.ownerId = state.appUser.id;
          })
      }
    })
  }

  setSelectedFiles(event: any, ngForm: NgForm){
    this.selectedFile = event.selectedFiles;
    this.previewUrl = event.previewUrls;
    ngForm.form.markAsDirty();
  }

  deletePhoto(){
    this.selectedFile = null;
    this.previewUrl = this.blog.photo ? this.blog.photo.url : null;
  }

  mainPhotoSelected(event, ngForm: NgForm){
    this.blog.blogImageUrl = event;
    ngForm.form.markAsDirty();
  }

  onSubmit() {
    if (this.editMode) {
      this.store.dispatch(new BlogAction.UpdateBlog({blog:this.blog, photo: this.selectedFile ? this.selectedFile[0] : null}));
     // this.location.back();
    } else {
      this.store.dispatch(new BlogAction.TryAddBlog({blog:this.blog, photo: this.selectedFile ? this.selectedFile[0] : null}));
    }
  }

  onGetFeedUri(){
    this.feedUriLoading = true;
    this.global.getBlogFeedUri(this.blog.url)
      .subscribe((result) => {
        this.blog.rss = result;
        this.feedUriLoading = false;
      }, (error) => {
        this.feedUriLoading = false;
      });
  }
}
