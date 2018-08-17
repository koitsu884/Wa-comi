using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Wacomi.API.Migrations
{
    public partial class initialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(127)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(127)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    Region = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClanSeekCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClanSeekCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyTopics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsTemporary = table.Column<bool>(type: "bit", nullable: false),
                    LastDiscussed = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyTopics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomeTowns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Prefecture = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeTowns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PropertySeekCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertySeekCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true),
                    RoleId = table.Column<string>(type: "varchar(127)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true),
                    UserId = table.Column<string>(type: "varchar(127)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(127)", nullable: false),
                    ProviderKey = table.Column<string>(type: "varchar(127)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "longtext", nullable: true),
                    UserId = table.Column<string>(type: "varchar(127)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(127)", nullable: false),
                    RoleId = table.Column<string>(type: "varchar(127)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(127)", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(127)", nullable: false),
                    Name = table.Column<string>(type: "varchar(127)", nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogFeedComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<int>(type: "int", nullable: true),
                    BlogFeedId = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DisplayName = table.Column<string>(type: "longtext", nullable: true),
                    PhotoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogFeedComments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogFeedLikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BlogFeedId = table.Column<int>(type: "int", nullable: true),
                    SupportAppUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogFeedLikes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(type: "varchar(127)", nullable: true),
                    Category2 = table.Column<string>(type: "varchar(127)", nullable: true),
                    Category3 = table.Column<string>(type: "varchar(127)", nullable: true),
                    DateRssRead = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    FollowerCount = table.Column<int>(type: "int", nullable: false),
                    HatedCount = table.Column<int>(type: "int", nullable: false),
                    HideOwner = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    PhotoId = table.Column<int>(type: "int", nullable: true),
                    RSS = table.Column<string>(type: "longtext", nullable: true),
                    Title = table.Column<string>(type: "longtext", nullable: false),
                    Url = table.Column<string>(type: "longtext", nullable: false),
                    WriterIntroduction = table.Column<string>(type: "longtext", nullable: true),
                    WriterName = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BusinessProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    EstablishedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Introduction = table.Column<string>(type: "longtext", nullable: true),
                    IsCompany = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClanSeeks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    Email = table.Column<string>(type: "longtext", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastActive = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    MainPhotoId = table.Column<int>(type: "int", nullable: true),
                    MemberProfileId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "longtext", nullable: false),
                    WebsiteUrl = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClanSeeks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClanSeeks_ClanSeekCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ClanSeekCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClanSeeks_Cities_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    BannedCount = table.Column<int>(type: "int", nullable: false),
                    BannedFromTopic = table.Column<bool>(type: "bit", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Gender = table.Column<string>(type: "longtext", nullable: true),
                    HomeTownId = table.Column<int>(type: "int", nullable: true),
                    Interests = table.Column<string>(type: "longtext", nullable: true),
                    Introduction = table.Column<string>(type: "longtext", nullable: true),
                    SearchId = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberProfiles_HomeTowns_HomeTownId",
                        column: x => x.HomeTownId,
                        principalTable: "HomeTowns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FriendRequests",
                columns: table => new
                {
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    RecipientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequests", x => new { x.SenderId, x.RecipientId });
                    table.ForeignKey(
                        name: "FK_FriendRequests_MemberProfiles_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "MemberProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendRequests_MemberProfiles_SenderId",
                        column: x => x.SenderId,
                        principalTable: "MemberProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Friends",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    FriendMemberid = table.Column<int>(type: "int", nullable: false),
                    Relationship = table.Column<string>(type: "varchar(127)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friends", x => new { x.MemberId, x.FriendMemberid });
                    table.ForeignKey(
                        name: "FK_Friends_MemberProfiles_FriendMemberid",
                        column: x => x.FriendMemberid,
                        principalTable: "MemberProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Friends_MemberProfiles_MemberId",
                        column: x => x.MemberId,
                        principalTable: "MemberProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberSettings",
                columns: table => new
                {
                    AppUserId = table.Column<string>(type: "varchar(127)", nullable: false),
                    AllowCancidateSearch = table.Column<bool>(type: "bit", nullable: false),
                    AllowFriendSearch = table.Column<bool>(type: "bit", nullable: false),
                    AppUserId1 = table.Column<int>(type: "int", nullable: true),
                    ShowDOB = table.Column<bool>(type: "bit", nullable: false),
                    ShowGender = table.Column<bool>(type: "bit", nullable: false),
                    ShowHomeTown = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberSettings", x => x.AppUserId);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateRead = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    RecipientId = table.Column<int>(type: "int", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<int>(type: "int", nullable: true),
                    ClanSeekId = table.Column<int>(type: "int", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    PropertySeekId = table.Column<int>(type: "int", nullable: true),
                    PublicId = table.Column<string>(type: "longtext", nullable: true),
                    StorageType = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_ClanSeeks_ClanSeekId",
                        column: x => x.ClanSeekId,
                        principalTable: "ClanSeeks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<string>(type: "varchar(127)", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DisplayName = table.Column<string>(type: "longtext", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsPremium = table.Column<bool>(type: "bit", nullable: false),
                    LastActive = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    MainPhotoId = table.Column<int>(type: "int", nullable: true),
                    UserProfileId = table.Column<int>(type: "int", nullable: false),
                    UserType = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUsers_AspNetUsers_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUsers_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppUsers_Photos_MainPhotoId",
                        column: x => x.MainPhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BlogFeeds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BlogId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PhotoId = table.Column<int>(type: "int", nullable: true),
                    PublishingDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Title = table.Column<string>(type: "longtext", nullable: true),
                    Url = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogFeeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogFeeds_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogFeeds_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PropertySeeks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    Email = table.Column<string>(type: "longtext", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastActive = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    MainPhotoId = table.Column<int>(type: "int", nullable: true),
                    OwnerAppUserId = table.Column<string>(type: "longtext", nullable: false),
                    OwnerAppUserId1 = table.Column<int>(type: "int", nullable: true),
                    WebsiteUrl = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertySeeks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertySeeks_PropertySeekCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "PropertySeekCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropertySeeks_Cities_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropertySeeks_Photos_MainPhotoId",
                        column: x => x.MainPhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropertySeeks_AppUsers_OwnerAppUserId1",
                        column: x => x.OwnerAppUserId1,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TopicComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DisplayName = table.Column<string>(type: "longtext", nullable: true),
                    PhotoId = table.Column<int>(type: "int", nullable: true),
                    TopicTitle = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopicComments_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TopicComments_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TopicLikes",
                columns: table => new
                {
                    SupportAppUserId = table.Column<int>(type: "int", nullable: false),
                    DailyTopicId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicLikes", x => new { x.SupportAppUserId, x.DailyTopicId });
                    table.ForeignKey(
                        name: "FK_TopicLikes_DailyTopics_DailyTopicId",
                        column: x => x.DailyTopicId,
                        principalTable: "DailyTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TopicLikes_AppUsers_SupportAppUserId",
                        column: x => x.SupportAppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TopicCommentFeels",
                columns: table => new
                {
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    Feeling = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicCommentFeels", x => new { x.AppUserId, x.CommentId });
                    table.ForeignKey(
                        name: "FK_TopicCommentFeels_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TopicCommentFeels_TopicComments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "TopicComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TopicReplies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DisplayName = table.Column<string>(type: "longtext", nullable: true),
                    PhotoId = table.Column<int>(type: "int", nullable: true),
                    Reply = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    TopicCommentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicReplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopicReplies_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TopicReplies_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TopicReplies_TopicComments_TopicCommentId",
                        column: x => x.TopicCommentId,
                        principalTable: "TopicComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_AccountId",
                table: "AppUsers",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_CityId",
                table: "AppUsers",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_MainPhotoId",
                table: "AppUsers",
                column: "MainPhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogFeedComments_AppUserId",
                table: "BlogFeedComments",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogFeedComments_BlogFeedId",
                table: "BlogFeedComments",
                column: "BlogFeedId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogFeedComments_PhotoId",
                table: "BlogFeedComments",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogFeedLikes_BlogFeedId",
                table: "BlogFeedLikes",
                column: "BlogFeedId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogFeedLikes_SupportAppUserId",
                table: "BlogFeedLikes",
                column: "SupportAppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogFeeds_BlogId",
                table: "BlogFeeds",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogFeeds_PhotoId",
                table: "BlogFeeds",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogFeeds_PublishingDate",
                table: "BlogFeeds",
                column: "PublishingDate");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_Category",
                table: "Blogs",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_Category2",
                table: "Blogs",
                column: "Category2");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_Category3",
                table: "Blogs",
                column: "Category3");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_DateRssRead",
                table: "Blogs",
                column: "DateRssRead");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_OwnerId",
                table: "Blogs",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_PhotoId",
                table: "Blogs",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessProfiles_AppUserId",
                table: "BusinessProfiles",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClanSeeks_AppUserId",
                table: "ClanSeeks",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClanSeeks_CategoryId",
                table: "ClanSeeks",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ClanSeeks_Created",
                table: "ClanSeeks",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_ClanSeeks_LocationId",
                table: "ClanSeeks",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClanSeeks_MainPhotoId",
                table: "ClanSeeks",
                column: "MainPhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_ClanSeeks_MemberProfileId",
                table: "ClanSeeks",
                column: "MemberProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_RecipientId",
                table: "FriendRequests",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_FriendMemberid",
                table: "Friends",
                column: "FriendMemberid");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_Relationship",
                table: "Friends",
                column: "Relationship");

            migrationBuilder.CreateIndex(
                name: "IX_MemberProfiles_AppUserId",
                table: "MemberProfiles",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberProfiles_HomeTownId",
                table: "MemberProfiles",
                column: "HomeTownId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberSettings_AppUserId1",
                table: "MemberSettings",
                column: "AppUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_DateCreated",
                table: "Messages",
                column: "DateCreated");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RecipientId",
                table: "Messages",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_AppUserId",
                table: "Photos",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_ClanSeekId",
                table: "Photos",
                column: "ClanSeekId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_PropertySeekId",
                table: "Photos",
                column: "PropertySeekId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertySeeks_CategoryId",
                table: "PropertySeeks",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertySeeks_LocationId",
                table: "PropertySeeks",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertySeeks_MainPhotoId",
                table: "PropertySeeks",
                column: "MainPhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertySeeks_OwnerAppUserId1",
                table: "PropertySeeks",
                column: "OwnerAppUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_TopicCommentFeels_CommentId",
                table: "TopicCommentFeels",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicComments_AppUserId",
                table: "TopicComments",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicComments_PhotoId",
                table: "TopicComments",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicLikes_DailyTopicId",
                table: "TopicLikes",
                column: "DailyTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicReplies_AppUserId",
                table: "TopicReplies",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicReplies_PhotoId",
                table: "TopicReplies",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicReplies_TopicCommentId",
                table: "TopicReplies",
                column: "TopicCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogFeedComments_Photos_PhotoId",
                table: "BlogFeedComments",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogFeedComments_AppUsers_AppUserId",
                table: "BlogFeedComments",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogFeedComments_BlogFeeds_BlogFeedId",
                table: "BlogFeedComments",
                column: "BlogFeedId",
                principalTable: "BlogFeeds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogFeedLikes_AppUsers_SupportAppUserId",
                table: "BlogFeedLikes",
                column: "SupportAppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogFeedLikes_BlogFeeds_BlogFeedId",
                table: "BlogFeedLikes",
                column: "BlogFeedId",
                principalTable: "BlogFeeds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Photos_PhotoId",
                table: "Blogs",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_AppUsers_OwnerId",
                table: "Blogs",
                column: "OwnerId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessProfiles_AppUsers_AppUserId",
                table: "BusinessProfiles",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClanSeeks_Photos_MainPhotoId",
                table: "ClanSeeks",
                column: "MainPhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClanSeeks_AppUsers_AppUserId",
                table: "ClanSeeks",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClanSeeks_MemberProfiles_MemberProfileId",
                table: "ClanSeeks",
                column: "MemberProfileId",
                principalTable: "MemberProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberProfiles_AppUsers_AppUserId",
                table: "MemberProfiles",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberSettings_AppUsers_AppUserId1",
                table: "MemberSettings",
                column: "AppUserId1",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AppUsers_RecipientId",
                table: "Messages",
                column: "RecipientId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AppUsers_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_AppUsers_AppUserId",
                table: "Photos",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_PropertySeeks_PropertySeekId",
                table: "Photos",
                column: "PropertySeekId",
                principalTable: "PropertySeeks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_AspNetUsers_AccountId",
                table: "AppUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_Cities_CityId",
                table: "AppUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ClanSeeks_Cities_LocationId",
                table: "ClanSeeks");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertySeeks_Cities_LocationId",
                table: "PropertySeeks");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_Photos_MainPhotoId",
                table: "AppUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ClanSeeks_Photos_MainPhotoId",
                table: "ClanSeeks");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertySeeks_Photos_MainPhotoId",
                table: "PropertySeeks");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BlogFeedComments");

            migrationBuilder.DropTable(
                name: "BlogFeedLikes");

            migrationBuilder.DropTable(
                name: "BusinessProfiles");

            migrationBuilder.DropTable(
                name: "FriendRequests");

            migrationBuilder.DropTable(
                name: "Friends");

            migrationBuilder.DropTable(
                name: "MemberSettings");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "TopicCommentFeels");

            migrationBuilder.DropTable(
                name: "TopicLikes");

            migrationBuilder.DropTable(
                name: "TopicReplies");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "BlogFeeds");

            migrationBuilder.DropTable(
                name: "DailyTopics");

            migrationBuilder.DropTable(
                name: "TopicComments");

            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "ClanSeeks");

            migrationBuilder.DropTable(
                name: "PropertySeeks");

            migrationBuilder.DropTable(
                name: "ClanSeekCategories");

            migrationBuilder.DropTable(
                name: "MemberProfiles");

            migrationBuilder.DropTable(
                name: "PropertySeekCategories");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "HomeTowns");
        }
    }
}
