import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import * as fromApp from '../../store/app.reducer';
import * as fromBlog from '../store/blogs.reducers';
// import * as fromPhoto from '../../photo/store/photos.reducers';
import * as BlogAction from '../store/blogs.actions';
// import * as PhotoActions from '../../photo/store/photos.action';
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
  photos: Photo[];
  editMode: boolean;
  loading: boolean;
  blog: any = {};
  blogCategories: string[];

  constructor(private store: Store<fromApp.AppState>,
    private global: GlobalService,
    private route: ActivatedRoute,
    private location: Location,
    private alertify: AlertifyService) { }

  ngOnInit() {
    this.blogCategories = this.global.getBlogCategories();
    this.route.data.subscribe(data => {
      this.photos = data['photos'];
    })
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

  mainPhotoSelected(event, ngForm: NgForm){
    this.blog.blogImageUrl = event;
    ngForm.form.markAsDirty();
  }

  onSubmit() {
    // console.log(this.blog);
    if (this.editMode) {
      this.store.dispatch(new BlogAction.UpdateBlog(this.blog));
    } else {
      this.store.dispatch(new BlogAction.TryAddBlog(this.blog));
    }
    this.location.back();
  }
}
