import { Photo } from "./Photo";
import { Blog } from "./Blog";

export interface MemberProfile {
    id: number;
    gender: string;
    dateOfBirth: Date;
    introduction: string;
    interests: string;
    homeTownId: number;
    homeTown: string;
}

/*public int Id { get; set; }

        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public string Introduction { get; set; }
        public string Interests { get; set; }
        public int? CityId { get; set; }
        public string City { get; set; }
        public int? HomeTownId { get; set; }
        public string HomeTown { get; set; }
        public string MainPhotoUrl { get; set; }*/
