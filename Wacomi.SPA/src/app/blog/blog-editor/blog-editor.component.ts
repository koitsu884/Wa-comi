import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import * as fromApp from '../../store/app.reducer';
import * as fromBlog from '../store/blogs.reducers';
// import * as fromPhoto from '../../photo/store/photos.reducers';
import * as BlogAction from '../store/blogs.actions';
// import * as PhotoActions from '../../photo/store/photos.action';
import { Store } from '@ngrx/store';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from '../../_services/alertify.service';
import { Blog } from '../../_models/Blog';
import { Photo } from '../../_models/Photo';

@Component({
  selector: 'app-blog-editor',
  templateUrl: './blog-editor.component.html',
  styleUrls: ['./blog-editor.component.css']
})
export class BlogEditorComponent implements OnInit {
  blogState: Observable<fromBlog.State>;
  // photoState: Observable<fromPhoto.State>;
  //blogs: Blog[];
  photos: Photo[];
  selectedBlog: Blog;
  appUserId: number;

  constructor(private store : Store<fromApp.AppState>, 
              private route: ActivatedRoute, 
              private router: Router,
              private alertify: AlertifyService) { }

  ngOnInit() {
    this.blogState = this.store.select('blogs');
    // this.photoState = this.store.select('photos');

    this.selectedBlog = null;
    this.route.params.subscribe(params => {
      this.appUserId = +params['appUserId'];
      if(!this.appUserId ){
        this.alertify.error("パラメーターが未設定です");
        this.router.navigate(['/home']);
        return;
      }
      this.photos = this.route.snapshot.data["photos"];
      //this.blogs = this.route.snapshot.data["blogs"];
      // this.store.dispatch(new PhotoActions.GetPhotos({type: this.type, recordId: this.recordId}));
      this.store.dispatch(new BlogAction.GetBlog(this.appUserId));
    });
  }

  onSelect(blog : Blog){
    this.selectedBlog = blog;
  }

  onClickAdd(){
    this.store.dispatch(new BlogAction.TryAddBlog(this.appUserId));
  }

  onClickDelete(id: number){
    this.store.dispatch(new BlogAction.TryDeleteBlog(id));
    this.selectedBlog = null;
    console.log("Hwy" + this.selectedBlog);
  }
}
