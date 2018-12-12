export interface UserCommentReply {
    id: number;
    ownerRecordClass: string;
    ownerRecordId: number;
    appUserId:number;
    displayName:string;
    iconUrl:string;
    comment:string;
    dateCreated:Date;
}
