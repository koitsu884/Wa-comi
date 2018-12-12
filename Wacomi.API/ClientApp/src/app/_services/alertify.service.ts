import { Injectable } from '@angular/core';
declare let alertify: any;

@Injectable()
export class AlertifyService {

constructor() { }
    confirm(message: string, okCallback: () => any) {
        alertify.confirm( 
            "確認",
            message,
            function() {
                okCallback();
            },
            null
        )
        .set(
            {transition:'fade'}
        )
    }

    prompt(title: string, okCallback: (evt, value) => any)
    {
        alertify.prompt(
            title,
            function(evt, value) {
                okCallback(evt, value);
            },
            null
        )
        .set(
            {transition:'fade'}
        )
    }

    success(message: string) {
        alertify.success(message);
    }

    error(message: string) {
        alertify.error(message);
    }

    warnig(message: string) {
        alertify.warnig(message);
    }

    message(message: string) {
        alertify.message(message);
    }
}
