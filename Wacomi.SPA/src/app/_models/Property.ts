import { AppUser } from "./AppUser";
import { Photo } from "./Photo";
import { Category } from "./Category";

export interface Property {
    id: number;
    appUser: AppUser;
    photos: Photo[];
    mainPhoto: Photo;
    isActive:boolean;
    title:string;
    description: string;
    cityName:string;
    cityId: number;
    latitude?: number;
    longitude?: number;
    hasPet: boolean;
    hasChild: boolean;
    dateAvailable: Date;
    internet: number;
    gender: number;
    minTerm: number;
    maxTerm:number;
    rent: number;
    categories: Category[];
    dateCreated: Date;
    dateUpdated: Date;
}