import { PipeTransform, Pipe } from "@angular/core";
import { Photo } from "../_models/Photo";

@Pipe({
        name:'thumburl'
    })
export class ThumbUrlPipe implements PipeTransform {
    transform(value: Photo, person:boolean = false) {
        if(value)
        {
            if(value.thumbnailUrl)
                return value.thumbnailUrl;
            if(value.url)
                return value.url;
        }
        if(person)
            return "/assets/NoImage_Person.png";
        return "/assets/NoImage.png";
    }
}