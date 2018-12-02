
import {map, take} from 'rxjs/operators';
import { CanActivate, Router } from "@angular/router";
import { Observable } from "rxjs";
import { Injectable } from "@angular/core";
import { Store } from "@ngrx/store";
import { AlertifyService } from "../_services/alertify.service";
 
import * as fromApp from "../store/app.reducer";
import * as fromAccount from "../account/store/account.reducers";

@Injectable()
export class AuthGuard implements CanActivate {
    constructor(private store: Store<fromApp.AppState>, private router: Router, private alertify: AlertifyService){}

    canActivate(): Observable<boolean> | Promise<boolean> | boolean {
        return this.store.select('account').pipe(
        take(1),
        map((authState: fromAccount.State) => {
            if(!authState.authenticated){
                // this.alertify.error("このページにアクセスするにはログインが必要です");
                this.router.navigate(['/account/login']);
            }
             return authState.authenticated
        }),)
    }
}