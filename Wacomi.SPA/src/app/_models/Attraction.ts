import { AppUser } from "./AppUser";
import { Photo } from "./Photo";
import { Category } from "./Category";
import { AttractionReview } from "./AttractionReview";

export interface Attraction {
    id: number;
    appUser: AppUser;
    appUserId?: number;
    cityName: string;
    cityId: number;
    name: string;
    introduction: string;
    accessInfo: string;
    latitude: number;
    longitude: number;
    photos: Photo[];
    reviewPhotos: Photo[];
    scoreAverage: number;
    likedCount: number;
    isLiked : boolean;
    reviewedCount: boolean;
    attractionReviews: AttractionReview[];
    categories: Category[];
    mainPhotoId?: number;
    mainPhotoUrl?: string;
    websiteUrl?: string;
    dateCreated: Date;
    dateUpdated: Date;
}
// public int Id {get; set;}
//         public AppUserForReturnDto AppUser{ get; set;}
//         public string CityName{ get; set;}
//         public string Name{ get; set;}
//         public string Introduction {get; set;}
//         public string AccessInfo{ get; set;}
//         public double Latitude{ get; set;}
//         public double Longitude{ get; set;}
//         public ICollection<PhotoForReturnDto> Photos { get; set; }
//         public int LikedCount{ get; set;}
//         public int ReviewedCount{ get; set;}
//         public ICollection<AttractionCategory> Categories { get; set; }
//         public string MainPhotoUrl { get; set;}
//         public string WebsiteUrl{ get; set;}
//         public DateTime DateCreated{ get; set;}
//         public DateTime DateUpdated{ get; set;}