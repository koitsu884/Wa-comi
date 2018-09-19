using System.Collections.Generic;
using Wacomi.API.Models;

namespace Wacomi.API.Dto
{
    public class CircleForReturnDto
    {
        public int Id{ get; set;}
        public string Name{ get; set;}
        public CircleCategory Category { get; set;}
        public City City{ get; set;}
        public string Description{ get; set;}
        bool ApprovalRequired { get; set;}
        public ICollection<AppUserForListDto> CircleMemberList {get; set;}
        public virtual ICollection<PhotoForReturnDto> Photos{ get; set;}
        public virtual ICollection<CircleTopicForReturnDto> Topics{ get; set;}
        public PhotoForReturnDto MainPhoto { get; set;}
    }
}