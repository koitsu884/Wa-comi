import { BlogFeedComment } from "./BlogFeedComment";
import { ShortComment } from "./ShortComment";

export interface BlogFeed {
    id: number;
    blogId: number;
    blogTitle: string;
    publishingDate: Date;
    title: string;
    url: string;
    imageUrl: string;
    blogImageUrl: string;
    writerName: string;
    ownerId: number;
    likedCount? : number;
    commentCount? : number;
    isLiked : boolean;
    
    //Client side only
    displayComments?: boolean;
    shortComments?: ShortComment[];
}
