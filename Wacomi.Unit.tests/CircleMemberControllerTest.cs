using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wacomi.API.Controllers;
using Wacomi.API.Data;
using Wacomi.API.Helper;
using Wacomi.API.Models;
using Wacomi.Xunit.FakeRepositories;
using Wacomi.Xunit.MockRepositories;
using Xunit;

namespace Wacomi.Xunit
{
    public class CircleMemberControllerTest : ControllerTest, IClassFixture<ContextMapperFixture>
    {
        CircleMemberController _controller;
        ICircleRepository _repo;
        IPhotoRepository _photoRepo;
        IAppUserRepository _appUserRepo;
        INotificationRepository _notificationRepo;
        ContextMapperFixture fixture;

        public CircleMemberControllerTest(ContextMapperFixture fixture)
        {
            this.fixture = fixture;
            _repo = new CircleRepoFake(this.fixture.Context);
            _photoRepo = new PhotoRepoFake(this.fixture.Context);
            _appUserRepo = new AppUserRepoFake(this.fixture.Context);
            _notificationRepo = new NotificationRepoFake(this.fixture.Context);

            _controller = new CircleMemberController(_repo, _appUserRepo, _notificationRepo, this.fixture.Mapper);
        }

        [Fact]
        public void Post_SendRequest_FailedAsAlreadyMember()
        {
            var loggedInUserId = 1;
            //Arrange
            _controller.ControllerContext = SetLoginUser(loggedInUserId);
            //Act
            var BadRequestResult = _controller.SendCircleRequest(new CircleRequest(){AppUserId = 1, CircleId = 1}).Result;

            Assert.IsType<BadRequestObjectResult>(BadRequestResult);
        }

         [Fact]
        public void Post_SendRequest_FailedAsCircleNotFound()
        {
            var loggedInUserId = 1;
            //Arrange
            _controller.ControllerContext = SetLoginUser(loggedInUserId);
            //Act
            var result = _controller.SendCircleRequest(new CircleRequest(){AppUserId = 1, CircleId = 10}).Result;

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Post_SendRequest_FailedAsRequestAlreadySent()
        {
            var loggedInUserId = 1;
            //Arrange
            _controller.ControllerContext = SetLoginUser(loggedInUserId);
            //Act
            var result = _controller.SendCircleRequest(new CircleRequest(){AppUserId = 1, CircleId = 2}).Result;

            Assert.IsType<BadRequestObjectResult>(result);
        }



        [Fact]
        public void Post_ApproveRequest_FailedByNonOwner()
        {
            var loggedInUserId =2;
            //Arrange
            _controller.ControllerContext = SetLoginUser(loggedInUserId);
            //Act
            var unauthorizeResult = _controller.ApproveCircleRequest(new CircleRequest(){AppUserId = 10, CircleId = 1}).Result;

            Assert.IsType<UnauthorizedResult>(unauthorizeResult);
        }

        [Fact]
        public void Post_ApproveRequest_RequestNotFound()
        {
            var loggedInUserId =1;
            //Arrange
            _controller.ControllerContext = SetLoginUser(loggedInUserId);
            //Act
            var notFoundResult = _controller.ApproveCircleRequest(new CircleRequest(){AppUserId = 10, CircleId = 1}).Result;

            Assert.IsType<NotFoundObjectResult>(notFoundResult);
        }

        [Fact]
        public void Post_ApproveRequest_Success()
        {
            var loggedInUserId =1;
            var userId = 3;
            var circleId = 1;
            int requestCounts = _repo.GetRequestsForCircle(circleId).Result.Count();
            //Arrange
            _controller.ControllerContext = SetLoginUser(loggedInUserId);
            //Act
            var createdResult = _controller.ApproveCircleRequest(new CircleRequest(){AppUserId = userId, CircleId = circleId}).Result;
            var notifications = _notificationRepo.GetNotifications(userId).Result;

            Assert.IsType<CreatedAtRouteResult>(createdResult);
            Assert.Equal(requestCounts - 1,  _repo.GetRequestsForCircle(circleId).Result.Count());
            Assert.NotNull( _repo.GetCircleMember(userId, circleId).Result);
            Assert.True(notifications.Count(n => n.AppUserId == userId && n.NotificationType == NotificationEnum.CircleRequestAccepted) == 1);
        }
    }
}