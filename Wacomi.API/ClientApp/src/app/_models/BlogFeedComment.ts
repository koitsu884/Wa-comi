export interface BlogFeedComment {
    id: number;
    blogFeedId: number;
    appUserId: number;
    displayName: string;
    iconUrl: string;
    comment:string;
    dateCreated: Date;
}