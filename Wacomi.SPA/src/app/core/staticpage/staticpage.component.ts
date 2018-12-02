
import {map} from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-staticpage',
  templateUrl: './staticpage.component.html',
  styleUrls: ['./staticpage.component.css']
})
export class StaticpageComponent implements OnInit {
  content: string;
  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.pipe(
    map(response => response['content']))
    .subscribe((html) => {
      this.content = html;
    });
  }

}
