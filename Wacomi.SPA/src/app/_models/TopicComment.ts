export interface TopicComment {
    id: number;
    mainPhotoUrl: string;
    memberId: number;
    displayName: string;
    comment: string;
    likedCount: number;
    replyCount: number;
    created: Date;
    reactionByUser: number;
}