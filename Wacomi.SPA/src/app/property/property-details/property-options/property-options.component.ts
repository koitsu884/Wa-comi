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

  interNetToString(internet:number){
    switch(internet){
      case 1:
        return "有（制限付き）";
      case 2:
        return "無制限";
      default:
        return "無";
    }
  }

  termToString(term: TermEnum) {
    switch (term) {
      case TermEnum.SHORT:
        return "短期OK"
      case TermEnum.WEEK:
        return "1週間"
      case TermEnum.WEEK_2:
        return "2週間"
      case TermEnum.WEEK_3:
        return "3週間"
      case TermEnum.MONTH:
        return "1ヶ月"
      case TermEnum.MONTH_2:
        return "2ヶ月"
      case TermEnum.MONTH_3:
        return "3ヶ月"
      case TermEnum.LONG:
        return "長期（3ヶ月以上）"
    }
  }

}
