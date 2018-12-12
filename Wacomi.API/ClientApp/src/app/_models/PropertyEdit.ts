import { TermEnum, RentTypeEnum } from "./PropertySearchOptions";
import { Photo } from "./Photo";

export interface PropertyEdit {
    id: number;
    appUserId: number;
    photos: Photo[];
    mainPhotoId: number;
    isActive:boolean;
    title:string;
    description: string;
    cityId: number;
    rentType: RentTypeEnum;
    latitude?: number;
    longitude?: number;
    hasPet: boolean;
    hasChild: boolean;
    dateAvailable: Date;
    internet: number;
    gender: number;
    minTerm: TermEnum;
    maxTerm:TermEnum;
    rent: number;
    categorizations: {propertySeekCategoryId: number}[];
}
