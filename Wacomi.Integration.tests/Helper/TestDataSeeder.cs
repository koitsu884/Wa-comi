using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Wacomi.API.Data;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.Integration.tests.Helper
{
    public class TestUser {
        public int AppUserId { get; set;}
        public string DisplayName { get; set;}
        public Account Account { get; set;}
    }

    public static class TestData {
        // public static TestUser[] TestUsers = new[]
        // {
        //     new TestUser{
        //         AppUserId = 10,
        //         DisplayName = "TestMember1",
        //         Account = new Account{
        //             UserName = "TestMember1",
        //             Email = "abc.def@gmail.com",
        //             EmailConfirmed = true
        //         }
        //     },

        //      new TestUser{
        //         AppUserId = 11,
        //         DisplayName = "TestMember2",
        //         Account = new Account{
        //             UserName = "TestMember2",
        //             Email = "bbb.bbb@gmail.com",
        //             EmailConfirmed = true
        //         }
        //     },
        //      new TestUser{
        //         AppUserId = 12,
        //         DisplayName = "TestMember3",
        //         Account = new Account{
        //             UserName = "TestMember3",
        //             Email = "eee.fff@gmail.com",
        //             EmailConfirmed = true
        //         }
        //     },
        //      new TestUser{
        //         AppUserId = 13,
        //         DisplayName = "TestMember4",
        //         Account = new Account{
        //             UserName = "TestMember4",
        //             Email = "dda.aab@outlook.com",
        //             EmailConfirmed = true
        //         }
        //     }
        // };

        // public static int GetAppUserIdByName(string userName)
        // {
        //     return TestUsers.First(u => u.Account.UserName == userName).AppUserId;
        // }

        // public static Circle[] TestCircles = new Circle[]{
        //         new Circle{ Id = 1, Name = "サークル1", Description = "テストサークル1号", AppUserId = 10, CategoryId = 1, CityId = 1},
        //         new Circle{ Id = 2, Name = "サークル2", Description = "テストサークル2号", AppUserId = 10, CategoryId = 2, ApprovalRequired = true, CityId = 2},
        //         new Circle{ Id = 3, Name = "サークル3", Description = "テストサークル3号", AppUserId = 12, CategoryId = 3},
        //         new Circle{ Id = 4, Name = "サークル4", Description = "テストサークル3号", AppUserId = 11, CategoryId = 1, ApprovalRequired = true, CityId = 1}
        //     };

        // public static CircleMember[] TestCircleMembers = new CircleMember[]{
        //         new CircleMember{CircleId = 1, AppUserId = 10, Role = CircleRoleEnum.OWNER},
        //         new CircleMember{CircleId = 2, AppUserId = 10, Role = CircleRoleEnum.OWNER},
        //         new CircleMember{CircleId = 3, AppUserId = 12, Role = CircleRoleEnum.OWNER},
        //         new CircleMember{CircleId = 4, AppUserId = 11, Role = CircleRoleEnum.OWNER}
        //     };

        // public static CircleRequest[] testRequets = new CircleRequest[]{
        //         new CircleRequest{ AppUserId = 12, CircleId = 1},
        //         new CircleRequest{ AppUserId = 12, CircleId = 2},
        //         new CircleRequest{ AppUserId = 11, CircleId = 3}
        //     };

    }
    
    public class TestDataSeeder : DatabaseSeeder
    {
        public string TestPassword = "P@ssw0rd!!";

        public static Dictionary<string, int> userNameIdMap = new Dictionary<string, int> {
            {"TestMember1", 11},
            {"TestMember2", 12},
            {"TestMember3", 13},
            {"TestMember4", 14},
            {"TestMember5", 15},
            {"TestMember6", 16},
        };

        public static int GetAppUserIdByName(string name){
            return userNameIdMap[name];
        }

        public TestUser[] TestUsers = new[]
        {
            new TestUser{
                AppUserId = userNameIdMap["TestMember1"],
                DisplayName = "TestMember1",
                Account = new Account{
                    UserName = "TestMember1",
                    Email = "abc.def@gmail.com",
                    EmailConfirmed = true
                }
            },

             new TestUser{
                AppUserId =  userNameIdMap["TestMember2"],
                DisplayName = "TestMember2",
                Account = new Account{
                    UserName = "TestMember2",
                    Email = "bbb.bbb@gmail.com",
                    EmailConfirmed = true
                }
            },
             new TestUser{
                AppUserId =  userNameIdMap["TestMember3"],
                DisplayName = "TestMember3",
                Account = new Account{
                    UserName = "TestMember3",
                    Email = "eee.fff@gmail.com",
                    EmailConfirmed = true
                }
            },
             new TestUser{
                AppUserId =  userNameIdMap["TestMember4"],
                DisplayName = "TestMember4",
                Account = new Account{
                    UserName = "TestMember4",
                    Email = "dda.aab@outlook.com",
                    EmailConfirmed = true
                }
            },
            new TestUser{
                AppUserId =  userNameIdMap["TestMember5"],
                DisplayName = "TestMember5",
                Account = new Account{
                    UserName = "TestMember5",
                    Email = "yyy.yyy@outlook.com",
                    EmailConfirmed = true
                }
            },
            new TestUser{
                AppUserId =  userNameIdMap["TestMember6"],
                DisplayName = "TestMember6",
                Account = new Account{
                    UserName = "TestMember6",
                    Email = "ggg.ggg@outlook.com",
                    EmailConfirmed = true
                }
            },
        };

        public Circle[] TestCircles = new Circle[]{
                new Circle{ Id = 1, Name = "サークル1", Description = "テストサークル1号", AppUserId = GetAppUserIdByName("TestMember1"), CategoryId = 1, CityId = 1},
                new Circle{ Id = 2, Name = "サークル2", Description = "テストサークル2号", AppUserId = GetAppUserIdByName("TestMember1"), CategoryId = 2, ApprovalRequired = true, CityId = 2},
                new Circle{ Id = 3, Name = "サークル3", Description = "テストサークル3号", AppUserId = GetAppUserIdByName("TestMember3"), CategoryId = 3},
                new Circle{ Id = 4, Name = "サークル4", Description = "テストサークル3号", AppUserId = GetAppUserIdByName("TestMember2"), CategoryId = 1, ApprovalRequired = true, CityId = 1}
            };

        public CircleMember[] TestCircleMembers = new CircleMember[]{
                new CircleMember{CircleId = 1, AppUserId = GetAppUserIdByName("TestMember1"), Role = CircleRoleEnum.OWNER},
                new CircleMember{CircleId = 2, AppUserId = GetAppUserIdByName("TestMember1"), Role = CircleRoleEnum.OWNER},
                new CircleMember{CircleId = 3, AppUserId = GetAppUserIdByName("TestMember3"), Role = CircleRoleEnum.OWNER},
                new CircleMember{CircleId = 4, AppUserId = GetAppUserIdByName("TestMember2"), Role = CircleRoleEnum.OWNER},
                new CircleMember{CircleId = 1, AppUserId = GetAppUserIdByName("TestMember3"), Role = CircleRoleEnum.MEMBER},
                new CircleMember{CircleId = 1, AppUserId = GetAppUserIdByName("TestMember4"), Role = CircleRoleEnum.MEMBER},
                new CircleMember{CircleId = 1, AppUserId = GetAppUserIdByName("TestMember5"), Role = CircleRoleEnum.MEMBER},
                new CircleMember{CircleId = 1, AppUserId = GetAppUserIdByName("TestMember6"), Role = CircleRoleEnum.MEMBER},
            };

        public CircleEvent[] TestCircleEvents = new CircleEvent[]{
                new CircleEvent{ Id = 1, Title = "サークル1 イベント１ Max 2", Description = "サークル1 イベント１ Max 2", MaxNumber = 2, AppUserId = GetAppUserIdByName("TestMember1"), CircleId = 1, CityId = 1},
                new CircleEvent{ Id = 2, Title = "サークル1 イベント2 承認制", Description = "サークル1 イベント2 承認制", AppUserId = GetAppUserIdByName("TestMember1"), CircleId = 1, ApprovalRequired = true},
        };

        public CircleTopic[] TestCircleTopicss = new CircleTopic[]{
                new CircleTopic {Id = 1, CircleId = 1, AppUserId = GetAppUserIdByName("TestMember1"), Title = "トピック１", Description = "テスト　トピック１号"}
            };

        public CircleTopicComment[] TestCircleTopicComments = new CircleTopicComment[] {
            new CircleTopicComment { CircleTopicId = 1, CircleId = 1, AppUserId = GetAppUserIdByName("TestMember1"), Comment = "コメント　テスト"}
        };

        public CircleRequest[] testRequets = new CircleRequest[]{
                new CircleRequest{ AppUserId = GetAppUserIdByName("TestMember3"), CircleId = 1},
                new CircleRequest{ AppUserId = GetAppUserIdByName("TestMember3"), CircleId = 2},
                new CircleRequest{ AppUserId = GetAppUserIdByName("TestMember2"), CircleId = 1}
            };


        public TestDataSeeder(UserManager<Account> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context) : base(userManager, roleManager, context)
        {
        }

        override public void Seed()
        {
            _context.Database.EnsureDeleted();
            base.Seed();
            SeedTestUsers();
            SeedTestCircles();
        }

        public void SeedTestCircles(){
            foreach(var circle in TestCircles){
                if(!_context.Circles.Any(c => c.Name == circle.Name))
                    _context.Add(circle);
            }

            foreach(var circleMember in TestCircleMembers){
                if(!_context.CircleMembers.Any(cm => cm.AppUserId == circleMember.AppUserId && cm.CircleId == circleMember.CircleId))
                    _context.Add(circleMember);
            }

            foreach(var request in testRequets){
                if(!_context.CircleRequests.Any(r => r.AppUserId == request.AppUserId && r.CircleId == request.CircleId))
                    _context.Add(request);
            }

            foreach(var circleTopic in TestCircleTopicss){
                _context.Add(circleTopic);
            }

            foreach(var circleTopicComment in TestCircleTopicComments){
                _context.Add(circleTopicComment);
            }

            foreach(var circleEvent in TestCircleEvents){
                _context.Add(circleEvent);
            }

           _context.SaveChanges();
        }

        public void SeedTestUsers()
        {
            foreach (var user in TestUsers)
            {
                if (!_userManager.Users.Any(u => u.UserName == user.Account.UserName))
                {
                    var result = _userManager.CreateAsync(user.Account, TestPassword).Result;
                    if (result.Succeeded)
                    {
                        var appUser = new AppUser()
                        {
                            Id = user.AppUserId,
                            AccountId = user.Account.Id,
                            DisplayName = user.DisplayName,
                            UserType = "Member"
                        };
                        _context.Add(appUser);
                        _context.SaveChanges();

                        var newMember = new MemberProfile() { AppUserId = appUser.Id };
                        _context.Add(newMember);
                        if (_context.SaveChanges() > 0)
                        {
                            appUser.UserProfileId = newMember.Id;
                        }
                        _context.SaveChanges();
                    }
                }
            }
        }
    }
}