import { Photo } from "./Photo";

export interface AttractionEdit {
    id: number;
    cityId: number;
    appUserId: number;
    name: string;
    photos: Photo[];
    introduction: string;
    accessInfo: string;
    latitude: number;
    longitude: number;
    radius: number;
    //attractionCategoryId: number[];
    categorizations: {attractionCategoryId: number}[];

    mainPhotoId?: number;
    websiteUrl?: string;
}
