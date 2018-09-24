using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wacomi.API.Data;
using Wacomi.API.Helper;
using Wacomi.Xunit.MockRepositories;

namespace Wacomi.Xunit
{
    public abstract class ControllerTest
    {
        protected ControllerContext SetLoginUser(int userId){
            return new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, CommonTestData.AppUserList.Find(au => au.Id == userId).AccountId)
                    }))
                }
            };
        }
    }
}