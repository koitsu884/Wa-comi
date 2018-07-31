import { Component, OnInit } from '@angular/core';
import { Pagination } from '../../../_models/Pagination';
import { Observable } from 'rxjs/Observable';
import * as fromBlog from '../../store/blogs.reducers';
import * as BlogActions from '../../store/blogs.actions';
import { Store } from '@ngrx/store';
import { BlogfeedCardComponent } from '../blogfeed-list/blogfeed-card/blogfeed-card.component';
import { GlobalService } from '../../../_services/global.service';
import { AppUser } from '../../../_models/AppUser';
import { ActivatedRoute } from '@angular/router';
import { ShortComment } from '../../../_models/ShortComment';
import { BlogFeed } from '../../../_models/BlogFeed';
import { AlertifyService } from '../../../_services/alertify.service';

@Component({
  selector: 'app-blogfeed-home',
  templateUrl: './blogfeed-home.component.html',
  styleUrls: ['./blogfeed-home.component.css']
})
export class BlogfeedHomeComponent implements OnInit {
  blogState: Observable<fromBlog.State>;
  blogCategories: string[];
  selectedCategory: string;
  appUser: AppUser; //resolve
  
  constructor(private store : Store<fromBlog.FeatureState>, 
    private alertify: AlertifyService,
    private globalService: GlobalService, 
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.appUser= this.route.snapshot.data['appUser'];
    this.blogCategories = this.globalService.getBlogCategories();
    this.blogCategories.unshift("全て");
    this.blogState = this.store.select("blogs");
    this.store.dispatch(new BlogActions.SearchFeeds());
  }

  filterChanged(){
    this.store.dispatch(new BlogActions.SetFeedSearchCategory(this.selectedCategory == "全て" ? "" : this.selectedCategory));
  }

  pageChanged(event) {
    this.store.dispatch(new BlogActions.SetFeedSearchPage(event.page));
  }

  sendLike(feedId: number){
    this.store.dispatch(new BlogActions.LikeFeed({blogFeedId: feedId, supportAppUserId: this.appUser.id}))
  }

  toggleDisplayComments(blogFeed: BlogFeed){
    if(!blogFeed.displayComments){
      this.store.dispatch(new BlogActions.GetFeedComments(blogFeed.id));
    }
    this.store.dispatch(new BlogActions.ToggleCommentForm(blogFeed.id));
  }

  addBlogFeedComment(blogFeedId:number, comment: string){
    this.store.dispatch(new BlogActions.TryAddFeedComment({blogFeedId:blogFeedId, appUserId: this.appUser.id, comment:comment}));
  }

  deleteFeedComment(shortComment: ShortComment){
    this.alertify.confirm("本当に削除しますか？", () => {
      this.store.dispatch(new BlogActions.TryDeleteFeedComment({blogFeedId: shortComment.ownerRecordId, feedCommentId: shortComment.id}));
    });
  }

}
