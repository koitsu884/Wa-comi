import { Component, OnInit } from '@angular/core';
import { Pagination } from '../../../_models/Pagination';
import { Observable } from 'rxjs';
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
import { trigger, state, style, transition, animate } from '@angular/animations';

@Component({
  selector: 'app-blogfeed-home',
  animations: [
    trigger('feedRow', [
      state('in', style({ opacity: 1, transform: 'translateX(0)' })),
      transition('void => *', [
        style({
          opacity: 0,
          transform: 'translateX(-100%)'
        }),
        animate('0.2s ease-in')
      ]),
      transition('* => void', [
        animate('0.2s 0.1s ease-out', style({
          opacity: 0,
          transform: 'translateX(100%)'
        }))
      ])
    ])
  ],
  templateUrl: './blogfeed-home.component.html',
  styleUrls: ['./blogfeed-home.component.css']
})
export class BlogfeedHomeComponent implements OnInit {
  blogState: Observable<fromBlog.State>;
  blogCategories: string[];
  selectedCategory: string;
  appUser: AppUser; //resolve
  onlyMine: boolean = false;
  forcusedRecordId: number = null;
  
  constructor(private store : Store<fromBlog.FeatureState>, 
    private alertify: AlertifyService,
    private globalService: GlobalService, 
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.appUser= this.route.snapshot.data['appUser'];
    this.forcusedRecordId = this.route.snapshot.params['id'];
    this.blogCategories = this.globalService.getBlogCategories();
    this.blogCategories.unshift("全て");
    this.blogState = this.store.select("blogs");
    if(this.forcusedRecordId)
      this.store.dispatch(new BlogActions.SearchFeedById(this.forcusedRecordId));
    else
      this.store.dispatch(new BlogActions.SearchFeeds());
  }

  toggleOnlyMine(){
    this.onlyMine != this.onlyMine;
    this.forcusedRecordId = null;
    this.store.dispatch(new BlogActions.SetSearchUserId(this.onlyMine ? this.appUser.id : null));
  }

  filterChanged(){
    this.forcusedRecordId = null;
    this.store.dispatch(new BlogActions.SetFeedSearchCategory(this.selectedCategory == "全て" ? "" : this.selectedCategory));
  }

  pageChanged(event) {
    this.store.dispatch(new BlogActions.SetFeedSearchPage(event.page));
  }

  sendLike(feedId: number){
    this.store.dispatch(new BlogActions.LikeFeed({blogFeedId: feedId, supportAppUserId: this.appUser.id}))
  }

  toggleDisplayComments(blogFeed: BlogFeed){
    this.forcusedRecordId = null;
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
