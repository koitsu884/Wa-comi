import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class UserService {
    baseUrl = environment.apiUrl;

    constructor(private httpClient: HttpClient) {
    }

    getMyRecords<T>(recordType:string, appUserId: number) {
        return this.httpClient.get<T>(this.baseUrl + recordType + '/user/' + appUserId);
    }
}
