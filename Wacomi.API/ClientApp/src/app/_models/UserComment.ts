import { UserCommentReply } from "./UserCommentReply";
import { IAppUserLinkable } from "./IAppUserLinkable";
import { IDataItem } from "./IDataItem";
import { Photo } from "./Photo";

export interface UserComment extends IAppUserLinkable, IDataItem {
    ownerRecordType: string;
    ownerRecordId: number;
    comment:string;
    photo: Photo;
    photoId: number;
    replyCount:number;

    //Client side only
    displayReplies?: boolean;
    //topicReplies: TopicReply[];
    replies: UserCommentReply[];
}

