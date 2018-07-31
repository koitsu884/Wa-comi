import { Component, OnInit } from '@angular/core';
import { AppUser } from '../../_models/AppUser';
import { Store } from '@ngrx/store';
import { Blog } from '../../_models/Blog';

import * as fromBlog from '../store/blogs.reducers';
import * as BlogActions from '../store/blogs.actions';
import { Observable } from 'rxjs/Observable';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from '../../_services/alertify.service';
import { BlogFeed } from '../../_models/BlogFeed';


@Component({
  selector: 'app-blog-management',
  templateUrl: './blog-management.component.html',
  styleUrls: ['./blog-management.component.css']
})
export class BlogManagementComponent implements OnInit {
  appUser: AppUser; //resolve
  blogs: Blog[];
  canAddBlog: boolean;
  private readonly MAX_BLOGCOUNT = 1;
  private readonly MAX_BLOGCOUNT_PR = 5;


  constructor(private route: ActivatedRoute, 
    private store: Store<fromBlog.FeatureState>, 
    private alertify: AlertifyService,
    private router: Router) { }

  ngOnInit() {
    this.appUser= this.route.snapshot.data['appUser'];
    if(this.appUser){
      this.store.dispatch(new BlogActions.GetBlog(this.appUser.id));
    };

    this.store.select('blogs').subscribe((blogState) => {
      this.blogs = blogState.blogs;
      if(this.blogs && this.appUser.isPremium){
        this.canAddBlog = this.blogs.length < this.MAX_BLOGCOUNT_PR; 
      }
      else{
        this.canAddBlog = this.blogs.length < this.MAX_BLOGCOUNT;
      }
    })
  }

  onBlogDelete(id: number) {
    this.alertify.confirm("本当に削除しますか？", () => {
      this.store.dispatch(new BlogActions.TryDeleteBlog(id));
    })
  }

  onFeedDelete(blogFeed: BlogFeed) {
    this.alertify.confirm("本当に削除しますか？", () => {
      this.store.dispatch(new BlogActions.TryDeleteFeed(blogFeed));
    })
  }

  onAddBlog(){
    this.router.navigate(['/blog/add']);
  }
}
