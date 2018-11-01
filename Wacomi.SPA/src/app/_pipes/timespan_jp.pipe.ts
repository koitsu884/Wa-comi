import { PipeTransform, Pipe } from "@angular/core";

@Pipe({
        name:'timespan_jp'
    })

export class TimeSpanJpPipe implements PipeTransform {
    transform(value: string) {
        let hms = value.split(':');
        return hms[0] + " 時 " + hms[1] + " 分";
    }
}