﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using Wacomi.API.Data;
using Wacomi.API.Models;

namespace Wacomi.API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180822022013_someUpdates")]
    partial class someUpdates
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Wacomi.API.Models.Account", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Wacomi.API.Models.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountId")
                        .IsRequired();

                    b.Property<int?>("CityId");

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("DisplayName");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsPremium");

                    b.Property<DateTime>("LastActive");

                    b.Property<int?>("MainPhotoId");

                    b.Property<int>("UserProfileId");

                    b.Property<string>("UserType");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("CityId");

                    b.HasIndex("MainPhotoId");

                    b.ToTable("AppUsers");
                });

            modelBuilder.Entity("Wacomi.API.Models.Blog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Category");

                    b.Property<string>("Category2");

                    b.Property<string>("Category3");

                    b.Property<DateTime?>("DateRssRead");

                    b.Property<string>("Description");

                    b.Property<int>("FollowerCount");

                    b.Property<int>("HatedCount");

                    b.Property<bool>("HideOwner");

                    b.Property<bool>("IsActive");

                    b.Property<int>("OwnerId");

                    b.Property<int?>("PhotoId");

                    b.Property<string>("RSS");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<string>("Url")
                        .IsRequired();

                    b.Property<string>("WriterIntroduction");

                    b.Property<string>("WriterName");

                    b.HasKey("Id");

                    b.HasIndex("Category");

                    b.HasIndex("Category2");

                    b.HasIndex("Category3");

                    b.HasIndex("DateRssRead");

                    b.HasIndex("OwnerId");

                    b.HasIndex("PhotoId");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("Wacomi.API.Models.BlogFeed", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BlogId");

                    b.Property<bool>("IsActive");

                    b.Property<int?>("PhotoId");

                    b.Property<DateTime?>("PublishingDate");

                    b.Property<string>("Title");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("BlogId");

                    b.HasIndex("PhotoId");

                    b.HasIndex("PublishingDate");

                    b.ToTable("BlogFeeds");
                });

            modelBuilder.Entity("Wacomi.API.Models.BlogFeedComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AppUserId");

                    b.Property<int?>("BlogFeedId");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("DisplayName");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("BlogFeedId");

                    b.ToTable("BlogFeedComments");
                });

            modelBuilder.Entity("Wacomi.API.Models.BlogFeedLike", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("BlogFeedId");

                    b.Property<int?>("SupportAppUserId");

                    b.HasKey("Id");

                    b.HasIndex("BlogFeedId");

                    b.HasIndex("SupportAppUserId");

                    b.ToTable("BlogFeedLikes");
                });

            modelBuilder.Entity("Wacomi.API.Models.BusinessProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AppUserId");

                    b.Property<DateTime?>("EstablishedDate");

                    b.Property<string>("Introduction");

                    b.Property<bool>("IsCompany");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("BusinessProfiles");
                });

            modelBuilder.Entity("Wacomi.API.Models.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Region");

                    b.HasKey("Id");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("Wacomi.API.Models.ClanSeek", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AppUserId");

                    b.Property<int?>("CategoryId");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<string>("Email");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime>("LastActive");

                    b.Property<int>("LocationId");

                    b.Property<int?>("MainPhotoId");

                    b.Property<int?>("MemberProfileId");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<string>("WebsiteUrl");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Created");

                    b.HasIndex("LocationId");

                    b.HasIndex("MainPhotoId");

                    b.HasIndex("MemberProfileId");

                    b.ToTable("ClanSeeks");
                });

            modelBuilder.Entity("Wacomi.API.Models.ClanSeekCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("ClanSeekCategories");
                });

            modelBuilder.Entity("Wacomi.API.Models.DailyTopic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsTemporary");

                    b.Property<DateTime>("LastDiscussed");

                    b.Property<string>("Title");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("DailyTopics");
                });

            modelBuilder.Entity("Wacomi.API.Models.Feedback", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<int?>("SenderId");

                    b.Property<string>("SenderName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("SenderId");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("Wacomi.API.Models.Friend", b =>
                {
                    b.Property<int>("MemberId");

                    b.Property<int>("FriendMemberid");

                    b.Property<string>("Relationship")
                        .IsRequired();

                    b.HasKey("MemberId", "FriendMemberid");

                    b.HasIndex("FriendMemberid");

                    b.HasIndex("Relationship");

                    b.ToTable("Friends");
                });

            modelBuilder.Entity("Wacomi.API.Models.FriendRequest", b =>
                {
                    b.Property<int>("SenderId");

                    b.Property<int>("RecipientId");

                    b.HasKey("SenderId", "RecipientId");

                    b.HasIndex("RecipientId");

                    b.ToTable("FriendRequests");
                });

            modelBuilder.Entity("Wacomi.API.Models.HomeTown", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Prefecture");

                    b.HasKey("Id");

                    b.ToTable("HomeTowns");
                });

            modelBuilder.Entity("Wacomi.API.Models.MemberProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AppUserId");

                    b.Property<int>("BannedCount");

                    b.Property<bool>("BannedFromTopic");

                    b.Property<DateTime?>("DateOfBirth");

                    b.Property<string>("Gender");

                    b.Property<int?>("HomeTownId");

                    b.Property<string>("Interests");

                    b.Property<string>("Introduction");

                    b.Property<string>("SearchId");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("HomeTownId");

                    b.ToTable("MemberProfiles");
                });

            modelBuilder.Entity("Wacomi.API.Models.MemberSetting", b =>
                {
                    b.Property<string>("AppUserId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AllowCancidateSearch");

                    b.Property<bool>("AllowFriendSearch");

                    b.Property<int?>("AppUserId1");

                    b.Property<bool>("ShowDOB");

                    b.Property<bool>("ShowGender");

                    b.Property<bool>("ShowHomeTown");

                    b.HasKey("AppUserId");

                    b.HasIndex("AppUserId1");

                    b.ToTable("MemberSettings");
                });

            modelBuilder.Entity("Wacomi.API.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateRead");

                    b.Property<bool>("IsRead");

                    b.Property<int>("RecipientId");

                    b.Property<int>("SenderId");

                    b.Property<string>("Title")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("DateCreated");

                    b.HasIndex("RecipientId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Wacomi.API.Models.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AppUserId");

                    b.Property<int?>("ClanSeekId");

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("Description");

                    b.Property<int?>("PropertySeekId");

                    b.Property<string>("PublicId");

                    b.Property<int>("StorageType");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("ClanSeekId");

                    b.HasIndex("PropertySeekId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("Wacomi.API.Models.PropertySeek", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CategoryId");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<string>("Email");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime>("LastActive");

                    b.Property<int>("LocationId");

                    b.Property<int?>("MainPhotoId");

                    b.Property<string>("OwnerAppUserId")
                        .IsRequired();

                    b.Property<int?>("OwnerAppUserId1");

                    b.Property<string>("WebsiteUrl");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("LocationId");

                    b.HasIndex("MainPhotoId");

                    b.HasIndex("OwnerAppUserId1");

                    b.ToTable("PropertySeeks");
                });

            modelBuilder.Entity("Wacomi.API.Models.PropertySeekCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("PropertySeekCategories");
                });

            modelBuilder.Entity("Wacomi.API.Models.TopicComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AppUserId");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<DateTime>("Created");

                    b.Property<string>("DisplayName");

                    b.Property<int?>("PhotoId");

                    b.Property<string>("TopicTitle");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("PhotoId");

                    b.ToTable("TopicComments");
                });

            modelBuilder.Entity("Wacomi.API.Models.TopicCommentFeel", b =>
                {
                    b.Property<int>("AppUserId");

                    b.Property<int>("CommentId");

                    b.Property<int>("Feeling");

                    b.HasKey("AppUserId", "CommentId");

                    b.HasIndex("CommentId");

                    b.ToTable("TopicCommentFeels");
                });

            modelBuilder.Entity("Wacomi.API.Models.TopicLike", b =>
                {
                    b.Property<int>("SupportAppUserId");

                    b.Property<int>("DailyTopicId");

                    b.HasKey("SupportAppUserId", "DailyTopicId");

                    b.HasIndex("DailyTopicId");

                    b.ToTable("TopicLikes");
                });

            modelBuilder.Entity("Wacomi.API.Models.TopicReply", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AppUserId");

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("DisplayName");

                    b.Property<int?>("PhotoId");

                    b.Property<string>("Reply")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<int>("TopicCommentId");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("PhotoId");

                    b.HasIndex("TopicCommentId");

                    b.ToTable("TopicReplies");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Wacomi.API.Models.Account")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Wacomi.API.Models.Account")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Wacomi.API.Models.Account")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Wacomi.API.Models.Account")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Wacomi.API.Models.AppUser", b =>
                {
                    b.HasOne("Wacomi.API.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Wacomi.API.Models.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId");

                    b.HasOne("Wacomi.API.Models.Photo", "MainPhoto")
                        .WithMany()
                        .HasForeignKey("MainPhotoId");
                });

            modelBuilder.Entity("Wacomi.API.Models.Blog", b =>
                {
                    b.HasOne("Wacomi.API.Models.AppUser", "Owner")
                        .WithMany("Blogs")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Wacomi.API.Models.Photo", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId");
                });

            modelBuilder.Entity("Wacomi.API.Models.BlogFeed", b =>
                {
                    b.HasOne("Wacomi.API.Models.Blog", "Blog")
                        .WithMany()
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Wacomi.API.Models.Photo", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId");
                });

            modelBuilder.Entity("Wacomi.API.Models.BlogFeedComment", b =>
                {
                    b.HasOne("Wacomi.API.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");

                    b.HasOne("Wacomi.API.Models.BlogFeed", "BlogFeed")
                        .WithMany("FeedComments")
                        .HasForeignKey("BlogFeedId");
                });

            modelBuilder.Entity("Wacomi.API.Models.BlogFeedLike", b =>
                {
                    b.HasOne("Wacomi.API.Models.BlogFeed", "BlogFeed")
                        .WithMany("FeedLikes")
                        .HasForeignKey("BlogFeedId");

                    b.HasOne("Wacomi.API.Models.AppUser", "SupportAppUser")
                        .WithMany()
                        .HasForeignKey("SupportAppUserId");
                });

            modelBuilder.Entity("Wacomi.API.Models.BusinessProfile", b =>
                {
                    b.HasOne("Wacomi.API.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Wacomi.API.Models.ClanSeek", b =>
                {
                    b.HasOne("Wacomi.API.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Wacomi.API.Models.ClanSeekCategory", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.HasOne("Wacomi.API.Models.City", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Wacomi.API.Models.Photo", "MainPhoto")
                        .WithMany()
                        .HasForeignKey("MainPhotoId");

                    b.HasOne("Wacomi.API.Models.MemberProfile")
                        .WithMany("ClanSeekPosted")
                        .HasForeignKey("MemberProfileId");
                });

            modelBuilder.Entity("Wacomi.API.Models.Feedback", b =>
                {
                    b.HasOne("Wacomi.API.Models.AppUser", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId");
                });

            modelBuilder.Entity("Wacomi.API.Models.Friend", b =>
                {
                    b.HasOne("Wacomi.API.Models.MemberProfile", "FriendMember")
                        .WithMany()
                        .HasForeignKey("FriendMemberid");

                    b.HasOne("Wacomi.API.Models.MemberProfile", "Member")
                        .WithMany("Friends")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Wacomi.API.Models.FriendRequest", b =>
                {
                    b.HasOne("Wacomi.API.Models.MemberProfile", "Sender")
                        .WithMany("FriendRequestSent")
                        .HasForeignKey("RecipientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Wacomi.API.Models.MemberProfile", "Recipient")
                        .WithMany("FriendRequestReceived")
                        .HasForeignKey("SenderId");
                });

            modelBuilder.Entity("Wacomi.API.Models.MemberProfile", b =>
                {
                    b.HasOne("Wacomi.API.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Wacomi.API.Models.HomeTown", "HomeTown")
                        .WithMany()
                        .HasForeignKey("HomeTownId");
                });

            modelBuilder.Entity("Wacomi.API.Models.MemberSetting", b =>
                {
                    b.HasOne("Wacomi.API.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId1");
                });

            modelBuilder.Entity("Wacomi.API.Models.Message", b =>
                {
                    b.HasOne("Wacomi.API.Models.AppUser", "Recipient")
                        .WithMany("MessageReceived")
                        .HasForeignKey("RecipientId");

                    b.HasOne("Wacomi.API.Models.AppUser", "Sender")
                        .WithMany("MessageSent")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Wacomi.API.Models.Photo", b =>
                {
                    b.HasOne("Wacomi.API.Models.AppUser")
                        .WithMany("Photos")
                        .HasForeignKey("AppUserId");

                    b.HasOne("Wacomi.API.Models.ClanSeek")
                        .WithMany("Photos")
                        .HasForeignKey("ClanSeekId");

                    b.HasOne("Wacomi.API.Models.PropertySeek")
                        .WithMany("Photos")
                        .HasForeignKey("PropertySeekId");
                });

            modelBuilder.Entity("Wacomi.API.Models.PropertySeek", b =>
                {
                    b.HasOne("Wacomi.API.Models.PropertySeekCategory", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.HasOne("Wacomi.API.Models.City", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Wacomi.API.Models.Photo", "MainPhoto")
                        .WithMany()
                        .HasForeignKey("MainPhotoId");

                    b.HasOne("Wacomi.API.Models.AppUser", "OwnerAppUser")
                        .WithMany()
                        .HasForeignKey("OwnerAppUserId1");
                });

            modelBuilder.Entity("Wacomi.API.Models.TopicComment", b =>
                {
                    b.HasOne("Wacomi.API.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");

                    b.HasOne("Wacomi.API.Models.Photo", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId");
                });

            modelBuilder.Entity("Wacomi.API.Models.TopicCommentFeel", b =>
                {
                    b.HasOne("Wacomi.API.Models.AppUser", "AppUser")
                        .WithMany("TopicCommentFeels")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Wacomi.API.Models.TopicComment", "Comment")
                        .WithMany("TopicCommentFeels")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Wacomi.API.Models.TopicLike", b =>
                {
                    b.HasOne("Wacomi.API.Models.DailyTopic", "DailyTopic")
                        .WithMany("TopicLikes")
                        .HasForeignKey("DailyTopicId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Wacomi.API.Models.AppUser", "SupportAppUser")
                        .WithMany()
                        .HasForeignKey("SupportAppUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Wacomi.API.Models.TopicReply", b =>
                {
                    b.HasOne("Wacomi.API.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");

                    b.HasOne("Wacomi.API.Models.Photo", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId");

                    b.HasOne("Wacomi.API.Models.TopicComment", "TopicComment")
                        .WithMany("TopicReplies")
                        .HasForeignKey("TopicCommentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
