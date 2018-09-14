using System.Collections.Generic;
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

            CreateMap<AttractionCategory, CategoryForReturnDto>();
            CreateMap<AttractionUpdateDto, Attraction>();
            CreateMap<Attraction, AttractionForReturnDto>()
              .ForMember(a => a.CityName, opt => opt.MapFrom(src => src.City.Name))
              .ForMember(a => a.Categories, opt => opt.ResolveUsing((src) => {
                  List<AttractionCategory> categories = new List<AttractionCategory> ();
                  if(src.Categorizations != null)
                  {
                    foreach( var categorization in src.Categorizations){
                      categories.Add(categorization.AttractionCategory);
                    }
                  }
                  return categories;
              }
              ))
              .ForMember(a => a.ScoreAverage, opt => opt.ResolveUsing((src) => {
                if(src.AttractionReviews != null && src.AttractionReviews.Count() > 0)
                  return src.AttractionReviews.Where(ar => ar.Score > 0).Average(ar => ar.Score);
                return 0;
              }
              ))
              .ForMember(m => m.LikedCount, opt => opt.MapFrom(src => src.AttractionLikes.Count()))
              .ForMember(m => m.ReviewedCount, opt => opt.MapFrom(src => src.AttractionReviews.Count()))
              .ForMember(m => m.MainPhoto, opt => opt.MapFrom(src => src.MainPhoto));

            CreateMap<AttractionReview, AttractionReviewForReturnDto>()
              .ForMember(a => a.AttractionName, opt => opt.MapFrom(src => src.Attraction.Name))
              .ForMember(a => a.AttractionThumbnailUrl, opt => opt.MapFrom(src => src.Attraction.MainPhoto.GetThumbnailUrl()))
              .ForMember(a => a.CityName, opt => opt.MapFrom(src => src.Attraction.City.Name))
              .ForMember(a => a.AppUserName, opt => opt.MapFrom(src => src.AppUser.DisplayName))
              .ForMember(a => a.AppUserIconUrl, opt => opt.MapFrom(src => src.AppUser.MainPhoto.GetIconUrl()))
              .ForMember(a => a.LikedCount, opt => opt.MapFrom(src => src.AttractionReviewLikes.Count()))
              .ForMember(a => a.MainPhoto, opt => opt.MapFrom(src => src.MainPhoto));

            CreateMap<AttractionReviewUpdateDto, AttractionReview>();

            CreateMap<UserRegistrationDto, Account>();

            CreateMap<AppUser, AppUserForReturnDto>()
              .ForMember(m => m.MainPhoto, opt => opt.MapFrom(src => src.MainPhoto))
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
              .ForMember(bf => bf.BlogImageUrl, opt => opt.MapFrom(src => src.Blog.Photo.GetIconUrl()))
              .ForMember(bf => bf.WriterName, opt => opt.MapFrom(src => src.Blog.Owner.DisplayName))
              .ForMember(bf => bf.LikedCount, opt => opt.MapFrom(src => src.FeedLikes.Count))
              .ForMember(bf => bf.CommentCount, opt => opt.MapFrom(src => src.FeedComments.Count));

            CreateMap<BlogFeedComment, CommentForReturnDto>()
              .ForMember(cr => cr.OwnerRecordClass, opt => opt.MapFrom("BlogFeed"))
              .ForMember(cr => cr.OwnerRecordId, opt => opt.MapFrom(src => src.BlogFeedId))
              .ForMember(cr => cr.IconUrl, opt => opt.MapFrom(src => src.AppUser.MainPhoto.GetIconUrl()));

            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<Photo, PhotoForReturnDto>()
              .ForMember(cr => cr.ThumbnailUrl, opt => opt.MapFrom(src => src.GetThumbnailUrl()))
              .ForMember(cr => cr.IconUrl, opt => opt.MapFrom(src => src.GetIconUrl()));

            CreateMap<ClanSeekForCreationDto, ClanSeek>();
            CreateMap<ClanSeekUpdateDto, ClanSeek>();
            CreateMap<ClanSeek, ClanSeekForReturnDto>()
              .ForMember(cs => cs.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
              .ForMember(cs => cs.LocationName, opt => opt.MapFrom(src => src.Location.Name))
              .ForMember(cs => cs.MainPhoto, opt => opt.MapFrom(src => src.MainPhoto))
              .ForMember(cs => cs.AppUserIconUrl, opt => opt.MapFrom(src => src.AppUser.MainPhoto.IconUrl))
              .ForMember(cs => cs.DisplayName, opt => opt.MapFrom(src => src.AppUser.DisplayName));

            CreateMap<PropertyUpdateDto, Property>();

            CreateMap<Property, PropertyForReturnDto>()
              .ForMember(p => p.CityName, opt => opt.MapFrom(src => src.City.Name))
              .ForMember(p => p.Categories, opt => opt.ResolveUsing((src) => {
                  List<PropertySeekCategory> categories = new List<PropertySeekCategory> ();
                  if(src.Categorizations != null)
                  {
                    foreach( var categorization in src.Categorizations){
                      categories.Add(categorization.PropertySeekCategory);
                    }
                  }
                  return categories;
              }
              ));
            CreateMap<PropertySeekCategory, CategoryForReturnDto>();

            CreateMap<DailyTopicCreationDto, DailyTopic>();
            CreateMap<DailyTopic, DailyTopicForReturnDto>()
              .ForMember(dtr => dtr.LikedCount, opt => opt.MapFrom(src => src.TopicLikes.Count));

            CreateMap<TopicComment, TopicCommentForReturnDto>()
               .ForMember(tc => tc.IconUrl, opt => opt.MapFrom(src => src.AppUser.MainPhoto.GetIconUrl()));
              // .ForMember(tcl => tcl.DisplayName, opt => opt.MapFrom(src => src.Member.Identity.DisplayName));

            CreateMap<TopicComment, TopicCommentListForReturnDto>()
              .ForMember(tc => tc.IconUrl, opt => opt.MapFrom(src => src.AppUser.MainPhoto.GetIconUrl()))
              .ForMember(tcl => tcl.ReplyCount,
                         opt => opt.MapFrom(src => src.TopicReplies.Count()))
              .ForMember(tcl => tcl.LikedCount,
                         opt => opt.MapFrom(src => src.TopicCommentFeels.Where(tcf => tcf.Feeling == FeelingEnum.Like).Count()));

            CreateMap<TopicLike, TopicLikeForReturnDto>();
            CreateMap<TopicReply, TopicReplyForReturnDto>()
            .ForMember(tc => tc.IconUrl, opt => opt.MapFrom(src => src.AppUser.MainPhoto.GetIconUrl()));

            CreateMap<TopicReply, CommentForReturnDto>()
              .ForMember(cr => cr.OwnerRecordClass, opt => opt.MapFrom("TopicCommentId"))
              .ForMember(cr => cr.OwnerRecordId, opt => opt.MapFrom(src => src.TopicCommentId))
              .ForMember(tc => tc.IconUrl, opt => opt.MapFrom(src => src.AppUser.MainPhoto.GetIconUrl()))
              .ForMember(cr => cr.Comment, opt => opt.MapFrom(src => src.Reply));

            CreateMap<TopicCommentFeel, TopicCommentFeelForReturnDto>();

            CreateMap<Friend, FriendForReturnDto>()
              .ForMember(f => f.FriendDisplayName,
                        opt => opt.MapFrom(src => src.FriendMember.AppUser.DisplayName))
              .ForMember(f => f.FriendPhotoUrl,
                        opt => opt.MapFrom(src => src.FriendMember.AppUser.MainPhoto.Url));

            CreateMap<FriendRequest, FriendRequestReceivedForReturnDto>()
              .ForMember(fr => fr.SenderDisplayName, opt => opt.MapFrom(src => src.Sender.AppUser.DisplayName))
              .ForMember(fr => fr.SenderPhotoUrl, opt => opt.MapFrom(src => src.Sender.AppUser.MainPhoto.Url));

            CreateMap<FriendRequest, FriendRequestSentForReturnDto>()
              .ForMember(fr => fr.RecipientDisplayName, opt => opt.MapFrom(src => src.Recipient.AppUser.DisplayName))
              .ForMember(fr => fr.RecipientPhotoUrl, opt => opt.MapFrom(src => src.Recipient.AppUser.MainPhoto.Url));

            CreateMap<Message, MessageForReturnDto>()
              .ForMember(mr => mr.RecipientPhotoUrl, opt => opt.MapFrom(src => src.Recipient.MainPhoto.Url))
              .ForMember(mr => mr.RecipientDisplayName, opt => opt.MapFrom(src => src.Recipient.DisplayName))
              .ForMember(mr => mr.SenderPhotoUrl, opt => opt.MapFrom(src => src.Sender.MainPhoto.Url))
              .ForMember(mr => mr.SenderDisplayName, opt => opt.MapFrom(src => src.Sender.DisplayName));
        }
    }
}