import { PipeTransform, Pipe } from "@angular/core";
import { GenderEnum } from "../_models/MemberProfile";

@Pipe({
        name:'propertyGender'
    })
export class PropertyGenderPipe implements PipeTransform {
    transform(value: GenderEnum) {
        switch(value){
            case GenderEnum.MALE:
              return "男性のみ";
            case GenderEnum.MALE:
              return "女性のみ";
            default:
              return "どちらでも";
          }
    }
}