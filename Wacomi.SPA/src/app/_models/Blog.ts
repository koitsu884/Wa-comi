import { BlogFeed } from "./BlogFeed";
import { Photo } from "./Photo";

export interface Blog {
    id: number;
    ownerId: number;
    hideOwner: boolean;
    writerName: string;
    writerIntroduction: string;
    title: string;
    description: string;
    category: string;
    category2: string;
    category3: string;
    url: string;
    photo: Photo;
    rss: string;
    followerCount: number;
    hatedCount: number;
    blogFeeds?: BlogFeed[];
}
