import { Photo } from "./Photo";

export interface IDataItemWithMultiplePhotos {
    photos: Photo[];
    mainPhotoId?: number;
    mainPhoto?: Photo;
}
