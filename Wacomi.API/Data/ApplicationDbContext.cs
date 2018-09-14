using Wacomi.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Wacomi.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<Account, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Attraction> Attractions { get; set; }
        public DbSet<AttractionCategory> AttractionCategories { get; set; }
        public DbSet<AttractionReview> AttractionReviews { get; set; }
        public DbSet<AttractionLike> AttractionLikes { get; set; }
        public DbSet<AttractionReviewLike> AttractionReviewLikes { get; set; }
        // public DbSet<BlackList> BlackLists { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogFeed> BlogFeeds { get; set; }
        public DbSet<BlogFeedLike> BlogFeedLikes { get; set;}
        public DbSet<BlogFeedComment> BlogFeedComments { get; set;}
        // public DbSet<BlogPreference> BlogPreferences { get; set; }
        public DbSet<BusinessProfile> BusinessProfiles { get; set; }
        public DbSet<ClanSeek> ClanSeeks { get; set; }
        public DbSet<ClanSeekCategory> ClanSeekCategories { get; set;}
        public DbSet<Property> Properties{ get; set;}
        public DbSet<PropertySeekCategory> PropertySeekCategories { get; set;}
        public DbSet<DailyTopic> DailyTopics { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<MemberProfile> MemberProfiles { get; set; }
        public DbSet<MemberSetting> MemberSettings { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Photo> Photos { get; set; }
        // public DbSet<PropertySeek> PropertySeeks { get; set;}
        public DbSet<TopicComment> TopicComments { get; set; }
        public DbSet<TopicCommentFeel> TopicCommentFeels { get; set; }
        public DbSet<TopicReply> TopicReplies { get; set; }
        public DbSet<TopicLike> TopicLikes { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<HomeTown> HomeTowns { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // builder.Entity<Account>()
            //     .HasOne(a => a.AppUser)
            //     .WithOne(u => u.Account)
            //     .HasForeignKey<AppUser>(a => a.AccountId);

            builder.Entity<AttractionCategorization>()
                .HasKey(ac => new { ac.AttractionId, ac.AttractionCategoryId} );
            builder.Entity<AttractionCategorization>()
                .HasOne(ac => ac.Attraction)
                .WithMany(a => a.Categorizations)
                .HasForeignKey(ac => ac.AttractionId);
            builder.Entity<AttractionCategorization>()
                .HasOne(ac => ac.AttractionCategory)
                .WithMany(a => a.Categorizations)
                .HasForeignKey(ac => ac.AttractionCategoryId);

            builder.Entity<TopicCommentFeel>()
                .HasOne(tcl => tcl.Comment)
                .WithMany(tcl => tcl.TopicCommentFeels)
                .HasForeignKey(tcl => tcl.CommentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AttractionLike>()
                .HasKey(al => new { al.AppUserId, al.AttractionId });
            builder.Entity<AttractionReviewLike>()
                .HasKey(arl => new { arl.AppUserId, arl.AttractionReviewId });
            builder.Entity<AttractionReview>()
                .HasIndex(a => a.AttractionId);

            builder.Entity<Blog>()
                .HasIndex(b => b.Category);
            builder.Entity<Blog>()
                .HasIndex(b => b.Category2);
            builder.Entity<Blog>()
                .HasIndex(b => b.Category3);
            builder.Entity<Blog>()
                .HasIndex(b => b.DateRssRead);

            builder.Entity<BlogFeed>()
                .HasIndex(bf => bf.PublishingDate);

            builder.Entity<ClanSeek>()
                .HasIndex(c => c.Created);
            builder.Entity<ClanSeek>()
                .HasIndex(c => c.CategoryId);
            builder.Entity<ClanSeek>()
                .HasIndex(c => c.LocationId);

            //---- Friend ----
            builder.Entity<Friend>()
                .HasKey(fr => new { fr.MemberId, fr.FriendMemberid });
            builder.Entity<Friend>()
                .HasIndex(fr => fr.Relationship);

            builder.Entity<Friend>()
                .HasOne(fr => fr.Member)
                .WithMany(f => f.Friends)
                .HasForeignKey(fr => fr.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Friend>()
                .HasOne(fr => fr.FriendMember)
                .WithMany()
                .HasForeignKey(fr => fr.FriendMemberid)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //---- Friend Request ----
            builder.Entity<FriendRequest>()
                .HasKey(fr => new { fr.SenderId, fr.RecipientId });

            builder.Entity<FriendRequest>()
                .HasOne(fr => fr.Sender)
                .WithMany(s => s.FriendRequestSent)
                .HasForeignKey(fr => fr.RecipientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<FriendRequest>()
                .HasOne(fr => fr.Recipient)
                .WithMany(fr => fr.FriendRequestReceived)
                .HasForeignKey(fr => fr.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            // //---- Black List ----
            // builder.Entity<BlackList>()
            //     .HasOne(bl => bl.Member)
            //     .WithMany(m => m.MyBlackLists)
            //     .HasForeignKey(bl => bl.BlockedMemberId)
            //     .OnDelete(DeleteBehavior.Cascade);
            // builder.Entity<BlackList>()
            //     .HasOne(bl => bl.BlockedMember)
            //     .WithMany(m => m.NoAccessMembers)
            //     .HasForeignKey(bl => bl.MemberId)
            //     .OnDelete(DeleteBehavior.ClientSetNull);
            //---- Messaging ----
            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(u => u.MessageSent)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(u => u.MessageReceived)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Message>()
                .HasIndex(m => m.DateCreated);

            builder.Entity<Notification>()
                .HasIndex(n => n.AppUserId);
            builder.Entity<Notification>()
                .HasIndex(n => n.RecordType);
            builder.Entity<Notification>()
                .HasIndex(n => n.RecordId);

            builder.Entity<Property>()
                .HasIndex(p => p.DateAvailable);
            builder.Entity<Property>()
                .HasIndex(p => p.HasChild);
            builder.Entity<Property>()
                .HasIndex(p => p.HasPet);
            builder.Entity<Property>()
                .HasIndex(p => p.Internet);
            builder.Entity<Property>()
                .HasIndex(p => p.Gender);
            builder.Entity<Property>()
                .HasIndex(p => p.IsActive);
            builder.Entity<Property>()
                .HasIndex(p => p.Latitude);
            builder.Entity<Property>()
                .HasIndex(p => p.Longitude);
            builder.Entity<Property>()
                .HasIndex(p => p.MaxTerm);
            builder.Entity<Property>()
                .HasIndex(p => p.MinTerm);
            builder.Entity<Property>()
                .HasIndex(p => p.Rent);
            
            builder.Entity<PropertyCategorization>()
                .HasKey(ac => new { ac.PropertyId, ac.PropertySeekCategoryId} );
            builder.Entity<PropertyCategorization>()
                .HasOne(ac => ac.Property)
                .WithMany(a => a.Categorizations)
                .HasForeignKey(ac => ac.PropertyId);
            builder.Entity<PropertyCategorization>()
                .HasOne(ac => ac.PropertySeekCategory)
                .WithMany(a => a.Categorizations)
                .HasForeignKey(ac => ac.PropertySeekCategoryId);
            //---- Topic Like ----
            builder.Entity<TopicLike>()
                .HasKey(tl => new { tl.SupportAppUserId, tl.DailyTopicId });

            // builder.Entity<TopicLike>()
            //     .HasOne(tl => tl.SupportUser)
            //     .WithOne()
            //     .HasForeignKey(tl => tl.DairyTopicId)
            //     .OnDelete(DeleteBehavior.ClientSetNull);

            // builder.Entity<TopicLike>()
            //     .HasOne(tl => tl.DairyTopic)
            //     .WithMany(dt => dt.TopicLikes)
            //     .HasForeignKey(tl => tl.SupportMemberId)
            //     .OnDelete(DeleteBehavior.Cascade);

            //---- Topic Comment Feel ----
            builder.Entity<TopicCommentFeel>()
                .HasKey(tcl => new { tcl.AppUserId, tcl.CommentId });

            builder.Entity<TopicCommentFeel>()
                .HasOne(tcl => tcl.AppUser)
                .WithMany(u => u.TopicCommentFeels)
                .HasForeignKey(tcl => tcl.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TopicCommentFeel>()
                .HasOne(tcl => tcl.Comment)
                .WithMany(tcl => tcl.TopicCommentFeels)
                .HasForeignKey(tcl => tcl.CommentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}