import { PipeTransform, Pipe } from "@angular/core";
import { TermEnum } from "../_models/PropertySearchOptions";

@Pipe({
    name: 'propertyTerms'
})
export class PropertyTermsPipe implements PipeTransform {
    transform(value: TermEnum) {
        switch (value) {
            case TermEnum.SHORT:
                return "短期"
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