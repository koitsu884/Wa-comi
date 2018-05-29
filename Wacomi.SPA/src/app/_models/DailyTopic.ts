export interface DailyTopic {
    id : number;
    isTemporary : boolean;
    isLiked : boolean;
    likedCount : number;
    title : string;
    created : Date;
    lastDiscussed : Date;
}