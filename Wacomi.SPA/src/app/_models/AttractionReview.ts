import { Photo } from "./Photo";

export interface AttractionReview {
    id: number;
    appUserId: number;
    appUserName: string;
    appUserIconUrl: string;
    attractionId: number;
    attractionName:string;
    attractionThumbnailUrl:string;
    cityName:string;
    photos: Photo[];
    mainPhoto: Photo;
    mainPhotoId: number;
    likedCount: number;
    isLiked: boolean;
    score: number;
    review: string;
    dateCreated: Date;
    dateUpdated: Date;
}

// public int Id { get; set;}
// public int AppUserId{ get; set;}
// public string AppUserName{ get; set;}
// public string AppUserMainPhotoUrl{ get; set;}
// public int AttractionId { get; set;}
// public string AttractionName { get; set;}
// public virtual ICollection<PhotoForReturnDto> Photos { get; set; }
// public int? MainPhotoId{ get; set;}
// public string MainPhotoUrl{ get; set;}
// // public virtual ICollection<AttractionReviewLike> AttractionReviewLikes { get; set;}
// public int Score{get; set;}
// public string Review{ get; set;}