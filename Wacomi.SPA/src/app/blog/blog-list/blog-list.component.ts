import { Component, OnInit, Input } from '@angular/core';
import { Blog } from '../../_models/Blog';

@Component({
  selector: 'app-blog-list',
  templateUrl: './blog-list.component.html',
  styleUrls: ['./blog-list.component.css']
})
export class BlogListComponent implements OnInit {
  @Input() blogs: Blog[];
  constructor() { }

  ngOnInit() {
  }

}
