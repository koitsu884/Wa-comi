import { Component, OnInit, Input } from '@angular/core';
import { Blog } from '../../_models/Blog';
import { GlobalService } from '../../_services/global.service';
import { Store } from '@ngrx/store';
import * as fromBlog from '../store/blogs.reducers';
import * as BlogAction from '../store/blogs.actions';

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
  @Input() blog:Blog;
  @Input() recordId: number;
  @Input() type: string;

  blogCategories : string[];
  constructor(private store : Store<fromBlog.State>, private global: GlobalService) { }

  ngOnInit() {
    this.blogCategories = this.global.getBlogCategories();
  }

  submit(){
    this.store.dispatch(new BlogAction.UpdateBlog({type: this.type, recordId: this.recordId, blog: this.blog}));
  }
}
