import { Component, OnInit, Input } from '@angular/core';
import { AlertifyService } from '../../../_services/alertify.service';
import { NgForm } from '@angular/forms';
import * as fromApp from "../../../store/app.reducer";
import * as fromAccount from "../../../account/store/account.reducers";
import * as AccountActions from '../../../account/store/account.actions';
import * as BlogActions from '../../../blog/store/blogs.actions';
import { Store } from '@ngrx/store';
import { AppUser } from '../../../_models/AppUser';
import { City } from '../../../_models/City';
import { Blog } from '../../../_models/Blog';
import { Photo } from '../../../_models/Photo';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {
  @Input() appUser: AppUser;
  @Input() cities: City[];
  // @Input() blogs: Blog[];
  @Input() photos: Photo[];
  // blogs: Blog[];
  // canAddBlog: boolean = true;

  // private MAX_BLOGCOUNT = 1;
  // private MAX_BLOGCOUNT_PR = 5;

  constructor(private store: Store<fromApp.AppState>,
    private alertify: AlertifyService) { }

  ngOnInit() {
    // this.store.select('blogs').subscribe((state) => {
    //   this.blogs = state.blogs;

    //   if (this.blogs) {
    //     this.canAddBlog = true;
    //     if (this.appUser.isPremium) {
    //       if(this.blogs.length >= this.MAX_BLOGCOUNT_PR)
    //         this.canAddBlog = false;
    //     }
    //     else if (this.blogs.length >= this.MAX_BLOGCOUNT) {
    //       this.canAddBlog = false;
    //     }
    //   }
    // });
  }

  mainPhotoSelected(event, ngForm: NgForm) {
    this.appUser.mainPhotoUrl = event;
    ngForm.form.markAsDirty();
  }

  submit(userEditForm: NgForm) {
    this.store.dispatch(new AccountActions.UpdateAppUser(this.appUser));
    userEditForm.form.markAsPristine();
  }

  // onBlogDelete(id: number) {
  //   this.store.dispatch(new BlogActions.TryDeleteBlog(id));
  // }
}
