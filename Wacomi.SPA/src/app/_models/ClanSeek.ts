export interface ClanSeek {
    id: number;
    title: string;
    categoryId: number;
    categoryName: string;
    memberId: number;
    memberName: string;
    mainPhotoUrl: string;
    websiteUrl: string;
    email: string;
    isActive: boolean;
    description: string;
    locationId: number;
    locationName: string;
    created: Date;
    lastActive: Date;
}
