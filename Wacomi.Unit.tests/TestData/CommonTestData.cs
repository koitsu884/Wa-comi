using System.Collections.Generic;
using Wacomi.API.Models;

namespace Wacomi.Xunit.MockRepositories
{
    public static class CommonTestData
    {
        static public List<AppUser> AppUserList = new List<AppUser>(){
                new AppUser(){Id = 1, DisplayName="User 1", AccountId="AccountId_1"},
                new AppUser(){Id = 2, DisplayName="User 2", AccountId="AccountId_2"},
                new AppUser(){Id = 3, DisplayName="User 3", AccountId="AccountId_3"},
            };
    }
}