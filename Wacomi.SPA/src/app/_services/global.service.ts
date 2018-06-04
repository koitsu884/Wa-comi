import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { City } from '../_models/City';
import { Hometown } from '../_models/Hometown';
import 'rxjs/add/operator/catch';
import { BlogFeed } from '../_models/BlogFeed';
import { ClanSeek } from '../_models/ClanSeek';
import { TopicComment } from '../_models/TopicComment';
import { TopicCommentFeel } from '../_models/TopicCommentFeel';
import { TopicCommentComponent } from '../dailytopic/topic-comment-list/topic-comment/topic-comment.component';

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

    getFeelings(){
        const array = [];
        array["Like"]=1;
        array["Dislike"]=2;
        array["Hate"]=3;
        return array;
    }

    getBlogFeeds(){
        return this.httpClient.get<BlogFeed[]>(this.baseUrl + 'blogfeed');
    }

    getLatestClanSeekList(){
        return this.httpClient.get<ClanSeek[]>(this.baseUrl + 'clanseek?latest=true');
    }

    getTodaysTopic(){
        return this.httpClient.get(this.baseUrl + 'dailytopic/today', {responseType: 'text'});
    }

    getLatestTopicComments(){
        return this.httpClient.get<TopicComment[]>(this.baseUrl + 'dailytopiccomment/list');
    }

}
