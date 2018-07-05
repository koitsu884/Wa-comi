using System.Linq;
using AutoMapper;
using Wacomi.API.Dto;
using Wacomi.API.Models;

namespace Wacomi.API.Helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Account, AccountForReturnDto>();
            CreateMap<AccountUpdateDto, Account>();
            CreateMap<UserRegistrationDto, Account>();

            CreateMap<AppUser, AppUserForReturnDto>()
              .ForMember(m => m.City, opt => opt.MapFrom(src => src.City.Name));
            CreateMap<AppUserUpdateDto, AppUser>();

            // CreateMap<MemberRegistrationDto, AppUser>();
            // CreateMap<MemberRegistrationDto, Member>();
            // CreateMap<MemberProfileUpdateDto, AppUser>();
            CreateMap<MemberProfileUpdateDto, MemberProfile>();
            CreateMap<MemberProfile, MemberProfileForReturnDto>()
            // .ForMember(m => m.DisplayName, opt => opt.MapFrom(src => src.Identity.DisplayName))
            // .ForMember(m => m.Created, opt => opt.MapFrom(src => src.Identity.Created))
            // .ForMember(m => m.LastActive, opt => opt.MapFrom(src => src.Identity.LastActive))
            // .ForMember(m => m.City, opt => opt.MapFrom(src => src.AppUser.City.Name))
            .ForMember(m => m.HomeTown, opt => opt.MapFrom(src => src.HomeTown.Prefecture));
            // CreateMap<MemberProfile, MemberForListDto>()
            //   .ForMember(m => m.DisplayName, opt => opt.MapFrom(src => src.AppUser.DisplayName);

            CreateMap<BusinessProfileUpdateDto, BusinessProfile>();
            CreateMap<BusinessProfile, BusinessProfileForReturnDto>();
              // .ForMember(m => m.DisplayName, opt => opt.MapFrom(src => src.Identity.DisplayName))
              // .ForMember(m => m.Created, opt => opt.MapFrom(src => src.Identity.Created))
              // .ForMember(m => m.LastActive, opt => opt.MapFrom(src => src.Identity.LastActive))
      //        .ForMember(m => m.City, opt => opt.MapFrom(src => src.AppUser.City.Name));

            CreateMap<BlogForUpdateDto, Blog>();
            CreateMap<Blog, BlogForReturnDto>();
            CreateMap<BlogFeed, BlogFeedForReturnDto>()
              .ForMember(bf => bf.OwnerId, opt => opt.MapFrom(src => src.Blog.OwnerId))
              .ForMember(bf => bf.BlogTitle, opt => opt.MapFrom(src => src.Blog.Title))
              .ForMember(bf => bf.BlogImageUrl, opt => opt.MapFrom(src => src.Blog.BlogImageUrl))
              .ForMember(bf => bf.WriterName, opt => opt.MapFrom(src => src.Blog.WriterName));

            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<Photo, PhotoForReturnDto>();

            CreateMap<ClanSeekForCreationDto, ClanSeek>();
            CreateMap<ClanSeekUpdateDto, ClanSeek>();
            CreateMap<ClanSeek, ClanSeekForReturnDto>()
              .ForMember(cs => cs.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
              .ForMember(cs => cs.LocationName, opt => opt.MapFrom(src => src.Location.Name))
              .ForMember(cs => cs.DisplayName, opt => opt.MapFrom(src => src.AppUser.DisplayName))
            ;
            ;

            CreateMap<PropertySeekForCreationDto, PropertySeek>();
            CreateMap<PropertySeekUpdateDto, PropertySeek>();

            CreateMap<DailyTopicCreationDto, DailyTopic>();
            CreateMap<DailyTopic, DailyTopicForReturnDto>()
              .ForMember(dtr => dtr.LikedCount, opt => opt.MapFrom(src => src.TopicLikes.Count));

            CreateMap<TopicComment, TopicCommentForReturnDto>();
              // .ForMember(tc => tc.MainPhotoUrl, opt => opt.MapFrom(src => src.Member.MainPhotoUrl));
              // .ForMember(tcl => tcl.DisplayName, opt => opt.MapFrom(src => src.Member.Identity.DisplayName));

            CreateMap<TopicComment, TopicCommentListForReturnDto>()
              // .ForMember(tcl => tcl.MainPhotoUrl, opt => opt.MapFrom(src => src.Member.MainPhotoUrl))
              .ForMember(tcl => tcl.AppUserId, opt => opt.MapFrom(src => src.Member.AppUserId))
              .ForMember(tcl => tcl.ReplyCount,
                         opt => opt.MapFrom(src => src.TopicReplies.Count()))
              .ForMember(tcl => tcl.LikedCount,
                         opt => opt.MapFrom(src => src.TopicCommentFeels.Where(tcf => tcf.Feeling == FeelingEnum.Like).Count()));

            CreateMap<TopicLike, TopicLikeForReturnDto>();

            CreateMap<TopicReply, TopicReplyForReturnDto>()
              .ForMember(tr => tr.AppUserId, opt => opt.MapFrom(src => src.Member.AppUserId));

            CreateMap<TopicCommentFeel, TopicCommentFeelForReturnDto>();

            CreateMap<Friend, FriendForReturnDto>()
              .ForMember(f => f.FriendDisplayName,
                        opt => opt.MapFrom(src => src.FriendMember.AppUser.DisplayName))
              .ForMember(f => f.FriendPhotoUrl,
                        opt => opt.MapFrom(src => src.FriendMember.AppUser.MainPhotoUrl));

            CreateMap<FriendRequest, FriendRequestReceivedForReturnDto>()
              .ForMember(fr => fr.SenderDisplayName, opt => opt.MapFrom(src => src.Sender.AppUser.DisplayName))
              .ForMember(fr => fr.SenderPhotoUrl, opt => opt.MapFrom(src => src.Sender.AppUser.MainPhotoUrl));

            CreateMap<FriendRequest, FriendRequestSentForReturnDto>()
              .ForMember(fr => fr.RecipientDisplayName, opt => opt.MapFrom(src => src.Recipient.AppUser.DisplayName))
              .ForMember(fr => fr.RecipientPhotoUrl, opt => opt.MapFrom(src => src.Recipient.AppUser.MainPhotoUrl));

            CreateMap<Message, MessageForReturnDto>()
              .ForMember(mr => mr.RecipientPhotoUrl, opt => opt.MapFrom(src => src.Recipient.MainPhotoUrl))
              .ForMember(mr => mr.RecipientDisplayName, opt => opt.MapFrom(src => src.Recipient.DisplayName))
              .ForMember(mr => mr.SenderPhotoUrl, opt => opt.MapFrom(src => src.Sender.MainPhotoUrl))
              .ForMember(mr => mr.SenderDisplayName, opt => opt.MapFrom(src => src.Sender.DisplayName));
        }
    }
}