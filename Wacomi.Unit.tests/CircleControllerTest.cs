using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wacomi.API.Controllers;
using Wacomi.API.Data;
using Wacomi.API.Dto;
using Wacomi.API.Helper;
using Wacomi.Xunit.MockRepositories;
using Xunit;

namespace Wacomi.Xunit
{
    public class CircleControllerTest
    {
        CircleController _controller;
        ICircleRepository _repo;
        IPhotoRepository _photoRepo;
        IAppUserRepository _appUserRepo;
        ApplicationDbContext _context;
        public CircleControllerTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            var context = new ApplicationDbContext(optionsBuilder.Options);

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfiles());
            });
            var mapper = mockMapper.CreateMapper();

            _repo = new CircleRepoFake(context);
            _photoRepo = new PhotoRepoFake(context);
            _appUserRepo = new AppUserRepoFake(context);

            _controller = new CircleController(_appUserRepo, mapper, _repo, _photoRepo, null, null);
        }

        [Fact]
        public void Get_ExistingUserId_ReturnsOkResult()
        {
            //Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, CommonTestData.AppUserList[0].AccountId)
                    }))
                }
            };
            //Act
            var okResult = _controller.Get(2).Result;

            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void Get_ExistingUserId_ReturnRightItem()
        {
            //Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, CommonTestData.AppUserList[0].AccountId)
                    }))
                }
            };
            //Act 
            var okResult = _controller.Get(2).Result as OkObjectResult;
            //Assert
            Assert.IsType<CircleForReturnDto>(okResult.Value);
            Assert.Equal(2, (okResult.Value as CircleForReturnDto).Id);
        }

        [Fact]
        public void Get_ExistingCircle_ReturnItemForOwnerWithRelatingFlags()
        {
            //Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, CommonTestData.AppUserList.Find(au => au.Id == 3).AccountId)
                    }))
                }
            };

            var okResult = _controller.Get(2).Result as OkObjectResult;

            Assert.IsType<CircleForReturnDto>(okResult.Value);
            Assert.Equal(true, (okResult.Value as CircleForReturnDto).IsManageable);
            Assert.Equal(true, (okResult.Value as CircleForReturnDto).IsMember);
        }

        [Fact]
        public void Get_ExistingCircle_ReturnItemForMemberWithRelatingFlags()
        {
            //Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, CommonTestData.AppUserList.Find(au => au.Id == 2).AccountId)
                    }))
                }
            };

            var okResult = _controller.Get(1).Result as OkObjectResult;

            Assert.IsType<CircleForReturnDto>(okResult.Value);
            Assert.Equal(false, (okResult.Value as CircleForReturnDto).IsManageable);
            Assert.Equal(true, (okResult.Value as CircleForReturnDto).IsMember);
        }

        [Fact]
        public void Get_ExistingCircle_ReturnItemForAnonymouthUser()
        {
            //Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, CommonTestData.AppUserList.Find(au => au.Id == 3).AccountId)
                    }))
                }
            };

            var okResult = _controller.Get(1).Result as OkObjectResult;

            Assert.IsType<CircleForReturnDto>(okResult.Value);
            Assert.Equal(false, (okResult.Value as CircleForReturnDto).IsManageable);
            Assert.Equal(false, (okResult.Value as CircleForReturnDto).IsMember);
        }

        [Fact]
        public void DeleteExistingCircleByMember_ReturnUnauthorized()
        {
            //Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, CommonTestData.AppUserList.Find(au => au.Id == 2).AccountId)
                    }))
                }
            };

            var result = _controller.Delete(1).Result;

            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}