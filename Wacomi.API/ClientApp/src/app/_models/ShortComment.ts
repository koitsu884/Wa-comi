//TODO: Remove this interface and use UserCommentReply
export interface ShortComment {
    id: number;
    ownerRecordClass: string;
    ownerRecordId: number;
    appUserId:number;
    displayName:string;
    iconUrl:string;
    comment:string;
    dateCreated:Date;
}
