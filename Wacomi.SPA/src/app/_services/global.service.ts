import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { City } from '../_models/City';
import { Hometown } from '../_models/Hometown';
import 'rxjs/add/operator/catch';
import { BlogFeed } from '../_models/BlogFeed';
import { ClanSeek } from '../_models/ClanSeek';

@Injectable()
export class GlobalService {
    baseUrl = environment.apiUrl;

    constructor(private httpClient : HttpClient){
    }

    getClanSeekCategories(){
        return this.httpClient.get<{id: number, name: string}>(this.baseUrl + 'clanseek/categories');
    }

    getBlogCategories(){
        return ["日常", "ニュース", "グルメ", "国際恋愛", "仕事", "オピニオン"];
    }

    getBlogFeeds(){
        return this.httpClient.get<BlogFeed[]>(this.baseUrl + 'blogfeed');
    }

    getLatestClanSeekList(){
        return this.httpClient.get<ClanSeek[]>(this.baseUrl + 'clanseek?latest=true');
    }
}
