import { IDataItemWithMultiplePhotos } from "./DataItemWithMultiplePhotos";
import { IAppUserLinkable } from "./IAppUserLinkable";
import { Category } from "./Category";
import { City } from "./City";

export interface Circle extends IDataItemWithMultiplePhotos, IAppUserLinkable {
    id: number;
    name: string;
    categoryName: string;
    categoryId: number;
    cityName: string;
    cityId: number;
    description :string;
    approvalRequired: boolean;
}
// public int Id{ get; set;}
// public AppUserForReturnDto AppUser{ get; set;}
// public string Name{ get; set;}
// public CircleCategory Category { get; set;}
// public City City{ get; set;}
// public string Description{ get; set;}
// bool ApprovalRequired { get; set;}
// public ICollection<AppUserForListDto> CircleMemberList {get; set;}
// public virtual ICollection<PhotoForReturnDto> Photos{ get; set;}
// public virtual ICollection<CircleTopicForReturnDto> Topics{ get; set;}
// public PhotoForReturnDto MainPhoto { get; set;}
// public bool IsActive { get; set;}
// public DateTime DateCreated { get; set;}
// public DateTime DateUpdated { get; set;}