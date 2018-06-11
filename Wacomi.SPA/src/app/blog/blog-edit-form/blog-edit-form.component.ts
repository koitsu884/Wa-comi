import { Component, OnInit, Input } from '@angular/core';
import { Blog } from '../../_models/Blog';
import { GlobalService } from '../../_services/global.service';
import { Store } from '@ngrx/store';
import * as fromBlog from '../store/blogs.reducers';
import * as BlogAction from '../store/blogs.actions';
import { Photo } from '../../_models/Photo';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-blog-edit-form',
  templateUrl: './blog-edit-form.component.html',
  styleUrls: ['./blog-edit-form.component.css']
})

// interface BlogCategoryCheckBox{
//   name: string;
//   selcted: boolean;
// }

export class BlogEditFormComponent implements OnInit {
  @Input() selectedBlog:Blog;
  @Input() photos: Photo[];
  blog : Blog;

  blogCategories : string[];
  constructor(private store : Store<fromBlog.State>, private global: GlobalService) { }

  ngOnInit() {
    this.blogCategories = this.global.getBlogCategories();
    if(this.selectedBlog)
      Object.assign(this.blog, this.selectedBlog);
    else
      this.blog = null;
  }

  submit(blogEditForm : NgForm){
    this.store.dispatch(new BlogAction.UpdateBlog(this.blog));
    blogEditForm.form.markAsPristine();
  }

  mainPhotoSelected(event, ngForm: NgForm){
    this.blog.blogImageUrl = event;
    ngForm.form.markAsDirty();
  }
}
