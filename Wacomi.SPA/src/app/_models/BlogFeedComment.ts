export interface BlogFeedComment {
    id: number;
    blogFeedId: number;
    appUserId: number;
    displayName: string;
    mainPhotoUrl: string;
    comment:string;
    dateCreated: Date;
}