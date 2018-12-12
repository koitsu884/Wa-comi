import { Photo } from "./Photo";

export interface CircleEdit {
    id?: number;
    appUserId?: number;
    name: string;
    mainPhotoId?: number;
    categoryId: number;
    cityId?: number;
    description :string;
    approvalRequired: boolean;

    photos: Photo[];
}

// public string Name{ get; set;}
//         [Required]
//         public int CategoryId { get; set;}
//         public int? CityId{ get; set;}
//         [Required]
//         [MaxLength(5000)]
//         public string Description{ get; set;}
//         bool ApprovalRequired { get; set;} = false;
//         [Required]
//         public int AppUserId{ get; set;}