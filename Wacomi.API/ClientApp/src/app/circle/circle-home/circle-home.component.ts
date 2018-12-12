import { Component, OnInit } from '@angular/core';
import { City } from '../../_models/City';
import { Category } from '../../_models/Category';
import { ActivatedRoute } from '@angular/router';
import { AppUser } from '../../_models/AppUser';

@Component({
  selector: 'app-circle-home',
  templateUrl: './circle-home.component.html',
  styleUrls: ['./circle-home.component.css']
})
export class CircleHomeComponent implements OnInit {
  cities: City[];
  categories: Category[];
  appUser: AppUser;
  
  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    const citiesFromStore: City[] = this.route.snapshot.data['cities'];
    this.cities = citiesFromStore.slice(0, citiesFromStore.length);
    this.cities.unshift({ id: 0, name: "全て", region: "" });
    const categoriesFromStore: Category[] = this.route.snapshot.data['categories'];
    this.categories = categoriesFromStore.slice(0, categoriesFromStore.length);
    this.categories.unshift({ id: 0, name: "全て" });
    this.appUser = this.route.snapshot.data['appUser'];
  }
}
