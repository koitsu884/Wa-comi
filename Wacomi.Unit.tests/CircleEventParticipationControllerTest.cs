using Microsoft.AspNetCore.Mvc;
using Wacomi.API.Controllers;
using Wacomi.API.Data;
using Wacomi.API.Dto;
using Wacomi.API.Models;
using Wacomi.Xunit;
using Wacomi.Xunit.FakeRepositories;
using Wacomi.Xunit.MockRepositories;
using Xunit;

namespace Wacomi.Unit.tests
{
    public class CircleEventParticipationControllerTest  : ControllerTest, IClassFixture<ContextMapperFixture>
    {
        CircleEventParticipationController _controller;
        ICircleRepository _repo;
        IPhotoRepository _photoRepo;
        IAppUserRepository _appUserRepo;
        INotificationRepository _notificationRepo;
        ContextMapperFixture _fixture;

        public CircleEventParticipationControllerTest(ContextMapperFixture fixture)
        {
            this._fixture = fixture;
            _repo = new CircleRepoFake(this._fixture.Context);
            _photoRepo = new PhotoRepoFake(this._fixture.Context);
            _appUserRepo = new AppUserRepoFake(this._fixture.Context);
            _notificationRepo = new NotificationRepoFake(this._fixture.Context);

            _controller = new CircleEventParticipationController(_repo, _notificationRepo, _appUserRepo, this._fixture.Mapper);
        }

        [Fact]
        public void Post_FailedAsUserNotMatch()
        {
            var loggedInUserId = 2;
            //Arrange
            _controller.ControllerContext = SetLoginUser(loggedInUserId);
            //Act
            var Result = _controller.Post(new CircleEventParticipation(){AppUserId = 4, CircleEventId = 1}).Result;

            Assert.IsType<UnauthorizedResult>(Result);
        }

        [Fact]
        public void Post_FailedAsCircleNotFound()
        {
            var loggedInUserId = 2;
            //Arrange
            _controller.ControllerContext = SetLoginUser(loggedInUserId);
            //Act
            var Result = _controller.Post(new CircleEventParticipation(){AppUserId = loggedInUserId, CircleEventId = 10}).Result;

            Assert.IsType<NotFoundObjectResult>(Result);
        }

        [Fact]
        public void Post_FailedAsAlreadyParticipated()
        {
            var loggedInUserId = 2;
            //Arrange
            _controller.ControllerContext = SetLoginUser(loggedInUserId);
            //Act
            var Result = _controller.Post(new CircleEventParticipation(){AppUserId = loggedInUserId, CircleEventId = 1}).Result;
            Result = _controller.Post(new CircleEventParticipation(){AppUserId = loggedInUserId, CircleEventId = 1}).Result;

            Assert.IsType<BadRequestObjectResult>(Result);
        }

        [Fact]
        public void Post_SuccessAndConfirm()
        {
            var loggedInUserId = 2;
            //Arrange
            _controller.ControllerContext = SetLoginUser(loggedInUserId);
            //Act
            var Result = _controller.Post(new CircleEventParticipation(){AppUserId = loggedInUserId, CircleEventId = 1}).Result as CreatedAtRouteResult;

            Assert.IsType<CreatedAtRouteResult>(Result);
            Assert.Equal(CircleEventParticipationStatus.Confirmed, (Result.Value as CircleEventParticipationForReturnDto).Status);
        }
    }
}