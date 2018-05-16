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
    [Migration("20180504020721_MadeSomeDatesNullable")]
    partial class MadeSomeDatesNullable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

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

            modelBuilder.Entity("Wacomi.API.Models.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("Created");

                    b.Property<string>("DisplayName");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<DateTime>("LastActive");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<int>("RelatedUserClassId");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<string>("UserType");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Wacomi.API.Models.BlackList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BlockedMemberId");

                    b.Property<bool>("IsSerious");

                    b.Property<int>("MemberId");

                    b.HasKey("Id");

                    b.HasIndex("BlockedMemberId");

                    b.HasIndex("MemberId");

                    b.ToTable("BlackLists");
                });

            modelBuilder.Entity("Wacomi.API.Models.Blog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Category");

                    b.Property<string>("Description");

                    b.Property<int>("FollowerCount");

                    b.Property<int>("HatedCount");

                    b.Property<bool>("HideOwner");

                    b.Property<bool>("IsActive");

                    b.Property<int?>("OwnerId");

                    b.Property<string>("RSS");

                    b.Property<string>("Title");

                    b.Property<string>("Url");

                    b.Property<string>("WriterIntroduction");

                    b.Property<string>("WriterName");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("Wacomi.API.Models.BlogPreference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BlogId");

                    b.Property<int>("MemberId");

                    b.Property<int>("Preference");

                    b.HasKey("Id");

                    b.HasIndex("BlogId");

                    b.HasIndex("MemberId");

                    b.ToTable("BlogPreferences");
                });

            modelBuilder.Entity("Wacomi.API.Models.BusinessUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CityId");

                    b.Property<DateTime?>("EstablishedDate");

                    b.Property<string>("IdentityId")
                        .IsRequired();

                    b.Property<string>("Introduction");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsCompany");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("IdentityId");

                    b.ToTable("BusinessUser");
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

                    b.Property<string>("Category");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<string>("Email");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime>("LastActive");

                    b.Property<string>("Location");

                    b.Property<int>("OwnerId");

                    b.Property<string>("WebsiteUrl");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("ClanSeeks");
                });

            modelBuilder.Entity("Wacomi.API.Models.DailyTopic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime>("LastDiscussed");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("DailyTopics");
                });

            modelBuilder.Entity("Wacomi.API.Models.Friend", b =>
                {
                    b.Property<int>("MemberId");

                    b.Property<int>("FriendMemberid");

                    b.Property<string>("Relationship");

                    b.HasKey("MemberId", "FriendMemberid");

                    b.HasIndex("FriendMemberid");

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

            modelBuilder.Entity("Wacomi.API.Models.Member", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BannedCount");

                    b.Property<bool>("BannedFromTopic");

                    b.Property<int?>("CityId");

                    b.Property<DateTime?>("DateOfBirth");

                    b.Property<string>("Gender");

                    b.Property<int?>("HomeTownId");

                    b.Property<string>("IdentityId")
                        .IsRequired();

                    b.Property<string>("Interests");

                    b.Property<string>("Introduction");

                    b.Property<bool>("IsActive");

                    b.Property<string>("SearchId");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("HomeTownId");

                    b.HasIndex("IdentityId");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("Wacomi.API.Models.MemberSetting", b =>
                {
                    b.Property<int>("MemberId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AllowCancidateSearch");

                    b.Property<bool>("AllowFriendSearch");

                    b.Property<int?>("MemberId1");

                    b.Property<bool>("ShowDOB");

                    b.Property<bool>("ShowGender");

                    b.Property<bool>("ShowHomeTown");

                    b.HasKey("MemberId");

                    b.HasIndex("MemberId1");

                    b.ToTable("MemberSettings");
                });

            modelBuilder.Entity("Wacomi.API.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("DateRead");

                    b.Property<bool>("IsRead");

                    b.Property<DateTime>("MessageSent");

                    b.Property<int>("RecipientId");

                    b.Property<int>("SenderId");

                    b.HasKey("Id");

                    b.HasIndex("RecipientId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Wacomi.API.Models.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("BusinessUserId");

                    b.Property<int?>("ClanSeekId");

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("Description");

                    b.Property<int?>("MemberId");

                    b.Property<string>("PublicId");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("BusinessUserId");

                    b.HasIndex("ClanSeekId");

                    b.HasIndex("MemberId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("Wacomi.API.Models.TopicComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment")
                        .IsRequired();

                    b.Property<int?>("MemberId");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.ToTable("TopicComments");
                });

            modelBuilder.Entity("Wacomi.API.Models.TopicCommentFeel", b =>
                {
                    b.Property<int?>("MemberId");

                    b.Property<int>("CommentId");

                    b.Property<int>("Feeling");

                    b.HasKey("MemberId", "CommentId");

                    b.HasIndex("CommentId");

                    b.ToTable("TopicCommentFeels");
                });

            modelBuilder.Entity("Wacomi.API.Models.TopicLike", b =>
                {
                    b.Property<int>("SupportMemberId");

                    b.Property<int>("DairyTopicId");

                    b.HasKey("SupportMemberId", "DairyTopicId");

                    b.HasIndex("DairyTopicId");

                    b.ToTable("TopicLike");
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
                    b.HasOne("Wacomi.API.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Wacomi.API.Models.AppUser")
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

                    b.HasOne("Wacomi.API.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Wacomi.API.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Wacomi.API.Models.BlackList", b =>
                {
                    b.HasOne("Wacomi.API.Models.Member", "Member")
                        .WithMany("MyBlackLists")
                        .HasForeignKey("BlockedMemberId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Wacomi.API.Models.Member", "BlockedMember")
                        .WithMany("NoAccessMembers")
                        .HasForeignKey("MemberId");
                });

            modelBuilder.Entity("Wacomi.API.Models.Blog", b =>
                {
                    b.HasOne("Wacomi.API.Models.Member", "Owner")
                        .WithMany("Blogs")
                        .HasForeignKey("OwnerId");
                });

            modelBuilder.Entity("Wacomi.API.Models.BlogPreference", b =>
                {
                    b.HasOne("Wacomi.API.Models.Blog", "Blog")
                        .WithMany()
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Wacomi.API.Models.Member", "Member")
                        .WithMany("BlogPreferences")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Wacomi.API.Models.BusinessUser", b =>
                {
                    b.HasOne("Wacomi.API.Models.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId");

                    b.HasOne("Wacomi.API.Models.AppUser", "Identity")
                        .WithMany()
                        .HasForeignKey("IdentityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Wacomi.API.Models.ClanSeek", b =>
                {
                    b.HasOne("Wacomi.API.Models.Member", "Owner")
                        .WithMany("ClanSeekPosted")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Wacomi.API.Models.Friend", b =>
                {
                    b.HasOne("Wacomi.API.Models.Member", "FriendMember")
                        .WithMany()
                        .HasForeignKey("FriendMemberid");

                    b.HasOne("Wacomi.API.Models.Member", "Member")
                        .WithMany("Friends")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Wacomi.API.Models.FriendRequest", b =>
                {
                    b.HasOne("Wacomi.API.Models.Member", "Sender")
                        .WithMany("FriendRequestSent")
                        .HasForeignKey("RecipientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Wacomi.API.Models.Member", "Recipient")
                        .WithMany("FriendRequestReceived")
                        .HasForeignKey("SenderId");
                });

            modelBuilder.Entity("Wacomi.API.Models.Member", b =>
                {
                    b.HasOne("Wacomi.API.Models.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId");

                    b.HasOne("Wacomi.API.Models.HomeTown", "HomeTown")
                        .WithMany()
                        .HasForeignKey("HomeTownId");

                    b.HasOne("Wacomi.API.Models.AppUser", "Identity")
                        .WithMany()
                        .HasForeignKey("IdentityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Wacomi.API.Models.MemberSetting", b =>
                {
                    b.HasOne("Wacomi.API.Models.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId1");
                });

            modelBuilder.Entity("Wacomi.API.Models.Message", b =>
                {
                    b.HasOne("Wacomi.API.Models.Member", "Recipient")
                        .WithMany("MessageReceived")
                        .HasForeignKey("RecipientId");

                    b.HasOne("Wacomi.API.Models.Member", "Sender")
                        .WithMany("MessageSent")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Wacomi.API.Models.Photo", b =>
                {
                    b.HasOne("Wacomi.API.Models.BusinessUser")
                        .WithMany("Photos")
                        .HasForeignKey("BusinessUserId");

                    b.HasOne("Wacomi.API.Models.ClanSeek")
                        .WithMany("Photos")
                        .HasForeignKey("ClanSeekId");

                    b.HasOne("Wacomi.API.Models.Member")
                        .WithMany("Photos")
                        .HasForeignKey("MemberId");
                });

            modelBuilder.Entity("Wacomi.API.Models.TopicComment", b =>
                {
                    b.HasOne("Wacomi.API.Models.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId");
                });

            modelBuilder.Entity("Wacomi.API.Models.TopicCommentFeel", b =>
                {
                    b.HasOne("Wacomi.API.Models.Member", "Member")
                        .WithMany("TopicCommentFeels")
                        .HasForeignKey("CommentId");

                    b.HasOne("Wacomi.API.Models.TopicComment", "Comment")
                        .WithMany("TopicCommentFeels")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Wacomi.API.Models.TopicLike", b =>
                {
                    b.HasOne("Wacomi.API.Models.Member", "SupportMember")
                        .WithMany("LikedTopic")
                        .HasForeignKey("DairyTopicId");

                    b.HasOne("Wacomi.API.Models.DailyTopic", "DairyTopic")
                        .WithMany("TopicLikes")
                        .HasForeignKey("SupportMemberId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
