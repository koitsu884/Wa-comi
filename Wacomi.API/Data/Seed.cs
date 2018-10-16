using System.Linq;
using System.Threading.Tasks;
using Wacomi.API.Models;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.EntityFrameworkCore;

namespace Wacomi.API.Data
{
    public class Seed
    {
        public void SeedData(
           UserManager<Account> userManager,
            // UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context
        )
        {
            SeedRoles(roleManager);
            SeedUsers(userManager, context);
            SeedClanSeekCategories(context);
            SeedCircleCategories(context);
            SeedPropertySeekCategories(context);
            SeedAttractionCategories(context);
            SeedDailyTopic(context);
            SeedOthers(context);
        }

        public void SeedDailyTopic(ApplicationDbContext context)
        {
            string[] initialDailyTopics = {
                "今食べたい物",
                "今行きたい所",
                "今何してる？",
                "明日の予定",
                "週末の予定",
                "新発見",
                "ボケて",
                "適当に独り言"};

            foreach (var initialDailyTopic in initialDailyTopics)
            {
                if (!context.DailyTopics.Any(h => h.Title == initialDailyTopic))
                {
                    var result = context.DailyTopics.AddAsync(new DailyTopic() { Title = initialDailyTopic, IsTemporary = false }).Result;
                    if (result == null)
                    {
                        throw new Exception("Failed to create daily topic " + initialDailyTopic);
                    }
                }
            }

            context.SaveChanges();
        }

        public void SeedClanSeekCategories(ApplicationDbContext context)
        {
            string[] clanSeekCategories = {
                "友達",
                "サークル",
                "イベント",
                "勉強・自己啓発",
                "旅行"};

            foreach (var clanSeekCategory in clanSeekCategories)
            {
                if (!context.ClanSeekCategories.Any(h => h.Name == clanSeekCategory))
                {
                    var result = context.ClanSeekCategories.AddAsync(new ClanSeekCategory() { Name = clanSeekCategory }).Result;
                    if (result == null)
                    {
                        throw new Exception("Failed to create clan seek category " + clanSeekCategory);
                    }
                }
            }

            context.SaveChanges();
        }

        public void SeedCircleCategories(ApplicationDbContext context)
        {
            string[] circleCategories = {
                "交流会",
                "音楽",
                "スポーツ",
                "アウトドア",
                "グルメ",
                "旅行",
                "ゲーム",
                "趣味",
                "同年代",
                "勉強・自己啓発",
                "仕事・ビジネス",
                "子育て",
                "インターネット",
                "その他"
                };

            foreach (var circleCategory in circleCategories)
            {
                if (!context.CircleCategories.Any(h => h.Name == circleCategory))
                {
                    var result = context.CircleCategories.AddAsync(new CircleCategory() { Name = circleCategory }).Result;
                    if (result == null)
                    {
                        throw new Exception("Failed to create clan seek category " + circleCategory);
                    }
                }
            }

            context.SaveChanges();
        }

        public void SeedAttractionCategories(ApplicationDbContext context)
        {
            string[] attractionCategories = {
                "自然",
                "海",
                "山",
                "公園",
                "アクティビティ",
                "有名観光地",
                "芸術",
                "グルメ",
                "マーケット",
                "ショッピング・お土産",
                "動物",
                "絶景巡り",
                "建築物"};

            foreach (var attractionCategory in attractionCategories)
            {
                if (!context.AttractionCategories.Any(h => h.Name == attractionCategory))
                {
                    var result = context.AttractionCategories.AddAsync(new AttractionCategory() { Name = attractionCategory }).Result;
                    if (result == null)
                    {
                        throw new Exception("Failed to create attraction category " + attractionCategory);
                    }
                }
            }

            context.SaveChanges();
        }

        public void SeedPropertySeekCategories(ApplicationDbContext context)
        {
            string[] propertySeekCategories = {
                "駐車場有",
                "英語環境",
                "カップルOK",
                "食事付き"};

            foreach (var propertySeekCategory in propertySeekCategories)
            {
                if (!context.PropertySeekCategories.Any(h => h.Name == propertySeekCategory))
                {
                    var result = context.PropertySeekCategories.AddAsync(new PropertySeekCategory() { Name = propertySeekCategory }).Result;
                    if (result == null)
                    {
                        throw new Exception("Failed to create property seek category " + propertySeekCategory);
                    }
                }
            }

            context.SaveChanges();
        }

        public void SeedUsers(UserManager<Account> userManager, ApplicationDbContext context)
        {
            string password = "P@ssw0rd!!";

            Account user = new Account
            {
                UserName = "Admin",
                Email = "kazunori.hayashi.nz@gmail.com",
                EmailConfirmed = true
            };

            if (!userManager.Users.Any(u => u.UserName == "Admin"))
            {
                var result = userManager.CreateAsync(user, password).Result;
                if (result.Succeeded)
                {
                    var appUser = new AppUser()
                    {
                        AccountId = user.Id,
                        DisplayName = user.UserName,
                        UserType = "Admin"
                    };

                    context.Add(appUser);
                    context.SaveChanges();
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
        }
        public void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Administrator", "Member", "Business" };


            foreach (var role in roles)
            {
                if (!roleManager.RoleExistsAsync(role).Result)
                {
                    var result = roleManager.CreateAsync(new IdentityRole(role)).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception("Failed to create role " + role);
                    }
                }
            }
        }

        public void SeedOthers(ApplicationDbContext context)
        {
            string[] prefectures = {"北海道","青森県","岩手県","宮城県","秋田県","山形県","福島県",
            "茨城県","栃木県","群馬県","埼玉県","千葉県","東京都","神奈川県",
            "新潟県","富山県","石川県","福井県","山梨県","長野県","岐阜県",
            "静岡県","愛知県","三重県","滋賀県","京都府","大阪府","兵庫県",
            "奈良県","和歌山県","鳥取県","島根県","岡山県","広島県","山口県",
            "徳島県","香川県","愛媛県","高知県","福岡県","佐賀県","長崎県",
            "熊本県","大分県","宮崎県","鹿児島県","沖縄県"
            };
            //-36.844642, 174.766620
            string[] northCities = {
                "オークランド,-36.844642,174.766620",
                "ハミルトン,-37.787674,175.283395",
                "ウェリントン,-41.285848,174.777361",
                "タウランガ,-37.686854,176.165348",
                "ロトルア,-38.140,176.240",
                "パーマストンノース,-40.350,175.610",
                "タウポ,-38.690,176.080",
                "ネイピア,-39.490,176.900",
                "ファンガレイ,-35.720,174.310",
                "ワンガヌイ,-39.930,175.030",
                "ギズボーン,-38.660,178.020",
                "その他北島,-36.844642,174.766620"};
            string[] southCities = {
                "クライストチャーチ,-43.530,172.640",
                "クイーンズタウン,-45.040,168.640",
                "ダニーデン,-45.880,170.480",
                "ネルソン,-41.290,173.240",
                "カイコウラ,-42.401555,173.681829",
                "インバーカーギル,-46.410,168.370",
                "ティマル,-44.380,171.220",
                "オアマル,-45.070,170.980",
                "テカポ,-44.004915,170.477285",
                "その他南島,-43.530,172.640"};

            string[] etc = {
                "NZ全域",
                "NZ国外(日本)",
                "NZ国外(その他)"
            };

            foreach (var prefecture in prefectures)
            {
                if (!context.HomeTowns.Any(h => h.Prefecture == prefecture))
                {
                    var result = context.HomeTowns.AddAsync(new HomeTown() { Prefecture = prefecture }).Result;
                    if (result == null)
                    {
                        throw new Exception("Failed to create hometown " + prefecture);
                    }
                }
            }

            foreach (var northCity in northCities)
            {
                var data = northCity.Split(',');
                var city = context.Cities.FirstOrDefault(c => c.Name == data[0]);
                if (city == null)
                {
                    var result = context.Cities.AddAsync(new City()
                    {
                        Region = "北島",
                        Name = data[0],
                        Latitude = Convert.ToDouble(data[1]),
                        Longitude = Convert.ToDouble(data[2])
                    }).Result;
                    if (result == null)
                    {
                        throw new Exception("Failed to create city " + northCity);
                    }
                }
                else
                {
                    city.Name = data[0];
                    city.Latitude = Convert.ToDouble(data[1]);
                    city.Longitude = Convert.ToDouble(data[2]);
                }
            }

            foreach (var southCity in southCities)
            {
                var data = southCity.Split(',');
                var city = context.Cities.FirstOrDefault(c => c.Name == data[0]);
                if (city == null)
                {
                    var result = context.Cities.AddAsync(new City()
                    {
                        Region = "南島",
                        Name = data[0],
                        Latitude = Convert.ToDouble(data[1]),
                        Longitude = Convert.ToDouble(data[2])
                    }).Result;

                    if (result == null)
                    {
                        throw new Exception("Failed to create city " + southCity);
                    }
                }
                else
                {
                    city.Name = data[0];
                    city.Latitude = Convert.ToDouble(data[1]);
                    city.Longitude = Convert.ToDouble(data[2]);
                }
            }

            foreach (var etcCity in etc)
            {
                var city = context.Cities.FirstOrDefault(c => c.Name == etcCity);
                if (city == null)
                {
                    var result = context.Cities.AddAsync(new City()
                    {
                        Region = "その他",
                        Name = etcCity,
                    }).Result;

                    if (result == null)
                    {
                        throw new Exception("Failed to create city " + etcCity);
                    }
                }
                else
                {
                    city.Name = etcCity;
                }
            }

            foreach (var etcCity in etc)
            {
                var city = context.Cities.FirstOrDefault(c => c.Name == etcCity);
                if (city == null)
                {
                    var result = context.Cities.AddAsync(new City()
                    {
                        Region = "その他",
                        Name = etcCity,
                    }).Result;

                    if (result == null)
                    {
                        throw new Exception("Failed to create city " + etcCity);
                    }
                }
                else
                {
                    city.Name = etcCity;
                }
            }

            context.SaveChanges();
        }


    }
}