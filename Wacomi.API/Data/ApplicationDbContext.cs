using Wacomi.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Wacomi.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<BlackList> BlackLists { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogFeed> BlogFeeds { get; set; }
        public DbSet<BlogPreference> BlogPreferences { get; set; }
        public DbSet<BusinessUser> BusinessUsers { get; set; }
        public DbSet<ClanSeek> ClanSeeks { get; set; }
        public DbSet<DailyTopic> DailyTopics { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberSetting> MemberSettings { get; set; }
        public DbSet<BusinessUser> Businesses { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Photo> Photos { get; set; }
        // public DbSet<PropertySeek> PropertySeeks { get; set;}
        public DbSet<TopicComment> TopicComments { get; set; }
        public DbSet<TopicCommentFeel> TopicCommentFeels { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<HomeTown> HomeTowns { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Blog>()
                .HasIndex(b => b.Category);
            builder.Entity<Blog>()
                .HasIndex(b => b.Category2);
            builder.Entity<Blog>()
                .HasIndex(b => b.Category3);

            //---- Friend ----
            builder.Entity<Friend>()
                .HasKey(fr => new { fr.MemberId, fr.FriendMemberid });

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
            //---- Black List ----
            builder.Entity<BlackList>()
                .HasOne(bl => bl.Member)
                .WithMany(m => m.MyBlackLists)
                .HasForeignKey(bl => bl.BlockedMemberId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<BlackList>()
                .HasOne(bl => bl.BlockedMember)
                .WithMany(m => m.NoAccessMembers)
                .HasForeignKey(bl => bl.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            //---- Messaging ----
            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(u => u.MessageSent)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(u => u.MessageReceived)
                .OnDelete(DeleteBehavior.ClientSetNull);
            //---- Topic Like ----
            builder.Entity<TopicLike>()
                .HasKey(tl => new { tl.SupportMemberId, tl.DairyTopicId });

            builder.Entity<TopicLike>()
                .HasOne(tl => tl.SupportMember)
                .WithMany(m => m.LikedTopic)
                .HasForeignKey(tl => tl.DairyTopicId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<TopicLike>()
                .HasOne(tl => tl.DairyTopic)
                .WithMany(dt => dt.TopicLikes)
                .HasForeignKey(tl => tl.SupportMemberId)
                .OnDelete(DeleteBehavior.Cascade);

            //---- Topic Comment Feel ----
            builder.Entity<TopicCommentFeel>()
                .HasKey(tcl => new { tcl.MemberId, tcl.CommentId });

            builder.Entity<TopicCommentFeel>()
                .HasOne(tcl => tcl.Member)
                .WithMany(m => m.TopicCommentFeels)
                .HasForeignKey(tcl => tcl.CommentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<TopicCommentFeel>()
                .HasOne(tcl => tcl.Comment)
                .WithMany(tcl => tcl.TopicCommentFeels)
                .HasForeignKey(tcl => tcl.CommentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}