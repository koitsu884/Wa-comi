
import {switchMap, take} from 'rxjs/operators';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';

 

import * as fromApp from '../store/app.reducer';
import * as fromAccount from '../account/store/account.reducers';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private store: Store<fromApp.AppState>) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // const copiedReq = req.clone({headers: req.headers.set('', '')});
    return this.store.select('account').pipe(
      take(1),
      switchMap((authState: fromAccount.State) => {
        const copiedReq = req.clone({headers: req.headers.set('Authorization', "Bearer " + authState.token)});
        return next.handle(copiedReq);
      }),)

    // return null;
  }
}

export const AuthInterceptorProvider = {
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true
};
