import { PipeTransform, Pipe } from "@angular/core";

@Pipe({
        name:'propertyInternet'
    })
export class PropertyInternetPipe implements PipeTransform {
    transform(value: number) {
        switch(value){
            case 1:
              return "有（制限付き）";
            case 2:
              return "有（無制限）";
            default:
              return "無し";
          }
    }
}