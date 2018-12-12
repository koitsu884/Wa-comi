import { PipeTransform, Pipe } from "@angular/core";
import { Photo } from "../_models/Photo";

@Pipe({
        name:'photourl'
    })
export class PhotoUrlPipe implements PipeTransform {
    transform(value: Photo, person:boolean = false) {
        if(value && value.url)
            return value.url;
        if(person)
            return "/assets/NoImage_Person.png";
        return "/assets/NoImage.png";
    }
}