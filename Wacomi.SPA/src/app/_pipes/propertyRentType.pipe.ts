import { PipeTransform, Pipe } from "@angular/core";
import { RentTypeEnum } from "../_models/PropertySearchOptions";

@Pipe({
    name: 'propertyRentType'
})
export class PropertyRentTypePipe implements PipeTransform {
    transform(value: RentTypeEnum) {
        switch (value) {
            case RentTypeEnum.OWN:
                return "オウンルーム"
            case RentTypeEnum.SHARE:
                return "ルームシェア"
            case RentTypeEnum.WHOLE:
                return "家丸ごと"
            case RentTypeEnum.HOMESTAY:
                return "ホームステイ"

        }
    }
}