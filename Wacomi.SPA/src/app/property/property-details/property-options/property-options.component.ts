import { Component, OnInit, Input } from '@angular/core';
import { Property } from '../../../_models/Property';
import { TermEnum } from '../../../_models/PropertySearchOptions';
import { GenderEnum } from '../../../_models/MemberProfile';

@Component({
  selector: 'app-property-options',
  templateUrl: './property-options.component.html',
  styleUrls: ['./property-options.component.css']
})
export class PropertyOptionsComponent implements OnInit {
  @Input() property: Property;
  constructor() { }

  ngOnInit() {
  }

  genderToString(gender: GenderEnum){
    switch(gender){
      case GenderEnum.MALE:
        return "男性のみ";
      case GenderEnum.FEMALE:
        return "女性のみ";
      default:
        return "どちらでも";
    }
  }
}
