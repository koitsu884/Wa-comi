import { BlogFeedComment } from "./BlogFeedComment";
import { ShortComment } from "./ShortComment";
import { Photo } from "./Photo";

export interface BlogFeed {
    id: number;
    blogId: number;
    blogTitle: string;
    publishingDate: Date;
    title: string;
    url: string;
    photo: Photo;
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
