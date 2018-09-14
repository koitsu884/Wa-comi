import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { City } from '../_models/City';
import { Hometown } from '../_models/Hometown';
import 'rxjs/add/operator/catch';
import { BlogFeed } from '../_models/BlogFeed';
import { ClanSeek } from '../_models/ClanSeek';
import { TopicComment } from '../_models/TopicComment';
import { Store } from '@ngrx/store';
import * as fromApp from '../store/app.reducer';
import { Blog } from '../_models/Blog';
import { Attraction } from '../_models/Attraction';
import { AttractionReview } from '../_models/AttractionReview';
import { Property } from '../_models/Property';

@Injectable()
export class GlobalService {
    baseUrl = environment.apiUrl;

    constructor(private httpClient: HttpClient, private store: Store<fromApp.AppState>) {
    }

    getBlogCategories() {
        return ["日常", "ワーホリ", "留学", "ニュース", "グルメ", "国際恋愛", "英語", "仕事", "オピニオン"];
    }

    getFeelings() {
        const array = [];
        array["Like"] = 1;
        array["Dislike"] = 2;
        array["Hate"] = 3;
        return array;
    }

    // public enum NotificationEnum {
//     NewMessage = 1,
//     NewPostOnFeedComment,
//     RepliedOnFeedComment,
//     NewPostOnTopicComment,
//     RepliedOnTopicComment,
// }

    getNotificationTypes() {
        const array = [];
        array["NewMessage"] = 1;
        array["NewPostOnFeedComment"] = 2;
        array["RepliedOnFeedComment"] = 3;
        array["NewPostOnTopicComment"] = 4;
        array["RepliedOnTopicComment"] = 5;
        return array;
    }

    getLatestBlogFeeds() {
        return this.httpClient.get<BlogFeed[]>(this.baseUrl + 'blogfeed/latest');
    }

    getLatestRecords(recordType: string) {
        return this.httpClient.get<Property[]>(this.baseUrl + recordType + '/latest');
    }


    getLatestAttractionList() {
        return this.httpClient.get<Attraction[]>(this.baseUrl + 'attraction/latest');
    }

    getLatestAttractionReviews() {
        return this.httpClient.get<AttractionReview[]>(this.baseUrl + 'attractionreview/latest');
    }

    getBlogFeedUri(url: string) {
        return this.httpClient.get(this.baseUrl + 'blog/rss?url=' + encodeURI(url), { responseType: 'text' });
    }

    getLatestClanSeekList() {
        return this.httpClient.get<ClanSeek[]>(this.baseUrl + 'clanseek?pageSize=10');
    }

    getClanSeekListByUser(appUserId: number){
        return this.httpClient.get<ClanSeek[]>(this.baseUrl + 'clanseek/user/' + appUserId);
    }

    getBlogListByUser(appUserId: number){
        return this.httpClient.get<Blog[]>(this.baseUrl + 'blog/user/' + appUserId + '?includeFeeds=true');
    }

    getMyClanSeeksCount(appUserId:number){
        return this.httpClient.get<number>(this.baseUrl + 'clanseek/' + appUserId + '/count');
    }

    getTodaysTopic() {
        return this.httpClient.get(this.baseUrl + 'dailytopic/today', { responseType: 'text' });
    }

    getLatestTopicComments() {
        return this.httpClient.get<TopicComment[]>(this.baseUrl + 'dailytopiccomment/list');
    }

    sendPasswordResetCode(userId:string, email: string) {
        return this.httpClient.post(
            this.baseUrl + 'auth/password/forgot',
            {userId:userId, email:email},
            {
                headers: new HttpHeaders().set('Content-Type', 'application/json')
            });
    }

    sendResetPasswordRequest(userId: string, code: string, password: string) {
        return this.httpClient.post(this.baseUrl + 'auth/password/reset', { UserId: userId, Code: code, Password: password });
    }

    sendChangePasswordRequest(userId: string, currentPassword: string, newPassword: string) {
        return this.httpClient.post(this.baseUrl + 'auth/password', { UserId: userId, CurrentPassword:currentPassword, NewPassword: newPassword });
    }
}
