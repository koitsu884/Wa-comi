using System.Linq;
using System.Threading.Tasks;
using Wacomi.API.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace Wacomi.API.Data
{
    public static class Seed
    {
        // private static readonly ApplicationDbContext _context;
        // private static readonly UserManager<AppUser> _userManager;
        // private static readonly RoleManager<IdentityRole> _roleManager;
        // public static Seed(ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        // {
        //     this._roleManager = roleManager;
        //     this._userManager = userManager;
        //     this._context = context;
        // }

        public static void SeedData(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context
        )
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
            SeedOthers(context);
        }

        public static void SeedUsers(UserManager<AppUser> userManager)
        {
            string password = "P@ssw0rd!!";

            AppUser user = new AppUser
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
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
        }
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
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

        public static void SeedOthers(ApplicationDbContext context)
        {
            string[] prefectures = {"北海道","青森県","岩手県","宮城県","秋田県","山形県","福島県",
            "茨城県","栃木県","群馬県","埼玉県","千葉県","東京都","神奈川県",
            "新潟県","富山県","石川県","福井県","山梨県","長野県","岐阜県",
            "静岡県","愛知県","三重県","滋賀県","京都府","大阪府","兵庫県",
            "奈良県","和歌山県","鳥取県","島根県","岡山県","広島県","山口県",
            "徳島県","香川県","愛媛県","高知県","福岡県","佐賀県","長崎県",
            "熊本県","大分県","宮崎県","鹿児島県","沖縄県"
            };
            string[] northCities = {
                "オークランド",
                "ハミルトン",
                "ウェリントン",
                "タウランガ",
                "ロトルア",
                "パーマストンノース",
                "タウポ",
                "ネイピア",
                "ファンガレイ",
                "ワンガヌイ",
                "ギズボーン",
                "その他"};
            string[] southCities = {
                "クライストチャーチ",
                "クイーンズタウン",
                "ダニーデン",
                "ネルソン",
                "カイコウラ",
                "インバーカーギル",
                "ティマル",
                "オアマル",
                "テカポ",
                "その他"};

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
                if (!context.Cities.Any(c => c.Name == northCity))
                {
                    var result = context.Cities.AddAsync(new City() { Region = "北島", Name = northCity }).Result;
                    if (result == null)
                    {
                        throw new Exception("Failed to create city " + northCity);
                    }
                }
            }

            foreach (var southCity in southCities)
            {
                if (!context.Cities.Any(c => c.Name == southCity))
                {
                    var result = context.Cities.AddAsync(new City() { Region = "南島", Name = southCity }).Result;
                    if (result == null)
                    {
                        throw new Exception("Failed to create city " + southCity);
                    }
                }
            }
            context.SaveChanges();
        }


    }
}