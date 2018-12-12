import { PipeTransform, Pipe } from "@angular/core";
import { Photo } from "../_models/Photo";
import { environment } from "../../environments/environment";

@Pipe({
        name:'thumburl'
    })
export class ThumbUrlPipe implements PipeTransform {
    isDevelopment = !environment.production;
    transform(value: Photo, person:boolean = false) {
        if(this.isDevelopment){
            if(value)
                return value.url;
            if(person)
                return "/assets/NoImage_Person.png";
            return "/assets/NoImage.png"; 
        }

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