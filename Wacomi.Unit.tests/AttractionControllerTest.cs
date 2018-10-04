using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wacomi.API.Controllers;
using Wacomi.API.Data;
using Wacomi.API.Dto;
using Wacomi.API.Helper;
using Wacomi.Xunit.FakeRepositories;
using Wacomi.Xunit.MockRepositories;
using Xunit;

namespace Wacomi.Xunit
{
    public class AttractionControllerTest
    {
        IAttractionRepository _repo;
        IPhotoRepository _photoRepo;
        IAppUserRepository _appUserRepo;
        ApplicationDbContext _context;
        AttractionController _controller;
        public AttractionControllerTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            var context = new ApplicationDbContext(optionsBuilder.Options);

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfiles());
            });
            var mapper = mockMapper.CreateMapper();

            _photoRepo = new PhotoRepoFake(context);
            _appUserRepo = new AppUserRepoFake(context);
            _repo = new AttractionRepoFake(context);

            _controller = new AttractionController(_appUserRepo, mapper, null, _repo, _photoRepo, null);
        }

        [Fact]
        public void Get_ExistingAttractionId_ReturnsOkResult()
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
            var okResult = _controller.Get(1).Result;

            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void Get_ExistingAttractionrId_ReturnRightItem()
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
            Assert.IsType<AttractionForReturnDto>(okResult.Value);
            Assert.Equal(2, (okResult.Value as AttractionForReturnDto).Id);
        }

        [Fact]
        public void Get_CategoryFilter_ReturnNothing()
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
            var okResult = _controller.GetAttractions("5,6").Result as OkObjectResult;
            //Assert
            Assert.Empty(okResult.Value as IEnumerable<AttractionForReturnDto>);
        }

        [Fact]
        public void Get_CategoryFilter_ReturnCategory3Records()
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
            var okResult = _controller.GetAttractions("3,6").Result as OkObjectResult;
            //Assert
            Assert.Equal(2, (okResult.Value as IEnumerable<AttractionForReturnDto>).Count());
        }

    }
}