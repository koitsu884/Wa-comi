export interface ClanSeek {
    id: number;
    title: string;
    categoryId: number;
    categoryName: string;
    appUserId: number;
    displayName: string;
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
/*public string Title{ get; set;}
        public int? CategoryId{ get; set;}
        [Required]
        public int AppUserId{ get; set;}
        public string MainPhotoUrl{ get; set;}
        public string WebsiteUrl{ get; set;}
        public string Email { get; set;}
        public string Description{get; set;}
        public int LocationId{get; set;}
        public DateTime Created{get; set;} = DateTime.Now;*/