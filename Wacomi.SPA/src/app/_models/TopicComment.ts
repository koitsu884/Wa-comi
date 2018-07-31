import { TopicReply } from "./TopicReply";
import { ShortComment } from "./ShortComment";

export interface TopicComment {
    id: number;
    mainPhotoUrl: string;
    memberId: number;
    appUserId: number;
    displayName: string;
    comment: string;
    likedCount: number;
    replyCount: number;
    created: Date;
    reactionByUser: number;

    //Client side only
    displayReplies: boolean;
    //topicReplies: TopicReply[];
    topicReplies: ShortComment[];
}