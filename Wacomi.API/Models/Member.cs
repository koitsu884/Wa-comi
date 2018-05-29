using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class Member : UserBase
    {
        public string SearchId{ get; set;}
        //Private Profiles
        public string Gender { get; set;}
        public DateTime? DateOfBirth { get; set; }
        //Public Profiles
        public string Introduction { get; set;}
        public string Interests {get; set;}
        public int? CityId { get; set;}
        public City City { get; set;}
        public int? HomeTownId { get; set;}
        public HomeTown HomeTown {get; set;}
        //Friend
        public ICollection<Friend> Friends{ get; set;}
        public ICollection<FriendRequest> FriendRequestSent{get; set;}
        public ICollection<FriendRequest> FriendRequestReceived{get; set;}
        //ClanSeek
        public ICollection<ClanSeek> ClanSeekPosted{ get; set;}
        //Property
        //public ICollection<PropertySeek> PropertySeekPosted{ get; set;}
        //Blog
        public ICollection<BlogPreference> BlogPreferences{ get; set;}

        //Private Message
        public ICollection<Message> MessageSent { get; set; }
        public ICollection<Message> MessageReceived { get; set;}
        public ICollection<BlackList> MyBlackLists { get; set; }
        public ICollection<BlackList> NoAccessMembers { get; set; }

        //Dairy Topic
        // public ICollection<TopicLike> LikedTopic { get; set; }
        public ICollection<TopicCommentFeel> TopicCommentFeels { get; set;}
        public bool BannedFromTopic{ get; set;}
        public int BannedCount{get; set;}
    }
}