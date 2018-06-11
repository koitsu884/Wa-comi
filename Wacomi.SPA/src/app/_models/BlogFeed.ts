export interface BlogFeed {
    id: number;
    blogId: number;
    publishingDate: Date;
    title: string;
    url: string;
    imageUrl: string;
    blogImageUrl: string;
    writerName: string;
    ownerId: number;
}
