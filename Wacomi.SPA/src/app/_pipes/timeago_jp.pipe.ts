import { PipeTransform, Pipe } from "@angular/core";

@Pipe({
        name:'timeago_jp'
    })

export class TimeAgoJpPipe implements PipeTransform {
    readonly SECOND_MILLISECOND = 1000;
    readonly MINUTE_MILLISECOND = 60 * this.SECOND_MILLISECOND;
    readonly HOUR_MILLISECOND = 60 * this.MINUTE_MILLISECOND;
    readonly DAY_MILLISECOND = 24 * this.HOUR_MILLISECOND;
    readonly WEEK_MILLISECOND = 7 * this.DAY_MILLISECOND;

    transform(value: Date) {
        let currentDate = new Date();
        let srcDate = new Date(value);
        let milSecondDistance = currentDate.getTime() - srcDate.getTime();

        if(milSecondDistance < this.MINUTE_MILLISECOND)
            return Math.floor(milSecondDistance / this.SECOND_MILLISECOND) + " 秒前";
        if(milSecondDistance < this.HOUR_MILLISECOND)
            return Math.floor(milSecondDistance / this.MINUTE_MILLISECOND) + " 分前";
        if(milSecondDistance < this.DAY_MILLISECOND)
            return Math.floor(milSecondDistance / this.HOUR_MILLISECOND) + " 時間前";
        if(milSecondDistance < this.WEEK_MILLISECOND)
            return Math.floor(milSecondDistance / this.DAY_MILLISECOND) + " 日前";
        
        let weekDistance = Math.floor(milSecondDistance / this.WEEK_MILLISECOND);
        if(weekDistance > 4)
            return "1カ月以上前";
        return weekDistance + " 週間前";
    }
}