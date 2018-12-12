import { PipeTransform, Pipe } from "@angular/core";

@Pipe({
        name:'shorten'
    })
export class ShortenPipe implements PipeTransform {
    transform(value: string, arg: number = null) {
        let maxLength = arg;
        if(!maxLength || maxLength < 0)
            maxLength = 100;

        if(value.length < maxLength)
            return value;

        return value.substr(0, maxLength) + '...';
    }
}