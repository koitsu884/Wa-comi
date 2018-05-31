using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Wacomi.API.Controllers;
using Wacomi.API.Data;
using Wacomi.API.Dto;
using Wacomi.API.Helper;
using Wacomi.API.Models;
using Xunit;
using Xunit.Abstractions;

namespace Wacomi.Xunit
{
    public class TopicReplyControllerTest
    {
        private readonly ITestOutputHelper output;
        private DateTime currentDateTime = DateTime.Now;
        private TopicReply OkData1 = new TopicReply
        {
            Reply = "テスト　リプライ OK",
            TopicCommentId = 1,
            MemberId = 1,
            DisplayName = "MemberTest",
            MainPhotoUrl = "http://aaa.bbb.jpg"
        };

        private Member TestMemberData = new Member{
            Id = 1,
            MainPhotoUrl = "http:abc.eft.jpg",
            Identity = new AppUser(){
                DisplayName = "MemberTest"
            }
        };

        public TopicReplyControllerTest(ITestOutputHelper output)
        {
            this.output = output;
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfiles());
            });
        }

        [Fact]
        public async void GetByTopicTest()
        {
            int topicCommentId = 1;
            var mockRepo = new Mock<IDataRepository>();

            mockRepo.Setup(repo => repo.GetTopicRepliesByCommentId(topicCommentId)).Returns(Task.FromResult(GetCommentReplies(topicCommentId)));
            var controller = new DailyTopicReplyController(mockRepo.Object, Mapper.Instance);

            //Act
            var result = await controller.GetByTopic(topicCommentId);

            //Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            output.WriteLine(okResult.Value.ToString());

            var model = okResult.Value as IEnumerable<TopicReplyForReturnDto>;
            output.WriteLine(model.ToString());
            Assert.NotNull(model);

            Assert.Equal(Mapper.Instance.Map<IEnumerable<TopicReplyForReturnDto>>(GetCommentReplies(topicCommentId)).ToString(), model.ToString());
        }



        [Fact]
        public async void Post_MemberNotFound()
        {
            int topicCommentId = 0;
            int memberId = 0;
            var mockRepo = new Mock<IDataRepository>();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfiles());
            });
            mockRepo.Setup(repo => repo.GetTopicRepliesByCommentId(topicCommentId)).Returns(Task.FromResult(GetCommentReplies(topicCommentId)));
            mockRepo.Setup(repo => repo.GetMember(memberId)).Returns(Task.FromResult((Member)null));
            var controller = new DailyTopicReplyController(mockRepo.Object, Mapper.Instance);

            // Act
            var result = await controller.Post(this.OkData1);
            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async void Post_TopicNotFound()
        {
            int topicCommentId = 0;
            var mockRepo = new Mock<IDataRepository>();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfiles());
            });
            mockRepo.Setup(repo => repo.GetTopicRepliesByCommentId(topicCommentId)).Returns(Task.FromResult((IEnumerable<TopicReply>)null));
            var controller = new DailyTopicReplyController(mockRepo.Object, Mapper.Instance);

            // Act
            var result = await controller.Post(this.OkData1);
            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async void Post_Ok()
        {
            var mockRepo = new Mock<IDataRepository>();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfiles());
            });
            mockRepo.Setup(repo => repo.TopicCommentExists(this.OkData1.TopicCommentId)).Returns(Task.FromResult(true));
            mockRepo.Setup(repo => repo.GetMember(this.OkData1.MemberId.GetValueOrDefault())).Returns(Task.FromResult(this.TestMemberData));
            mockRepo.Setup(repo => repo.Add(this.OkData1));
            mockRepo.Setup(repo => repo.SaveAll()).Returns(Task.FromResult(true));

            var controller = new DailyTopicReplyController(mockRepo.Object, Mapper.Instance);

            // Act
            var result = await controller.Post(this.OkData1);
            // Assert
            Assert.IsType<CreatedAtRouteResult>(result);
            var createdResult = result as CreatedAtRouteResult;
            var model = createdResult.Value as TopicReplyForReturnDto;
            Assert.Equal(model.MainPhotoUrl, this.TestMemberData.MainPhotoUrl);
            Assert.Equal(model.DisplayName, this.TestMemberData.Identity.DisplayName);
        }

        private IEnumerable<TopicReply> GetCommentReplies(int topicCommentId)
        {
            var replies = new List<TopicReply>();
            replies.Add(new TopicReply()
            {
                Reply = "テスト　リプライ OK",
                TopicCommentId = topicCommentId,
                MemberId = 1,
                DisplayName = "MemberTest",
                MainPhotoUrl = "http://aaa.bbb.jpg",
                Created = this.currentDateTime
            });
            replies.Add(new TopicReply()
            {
                Reply = "テスト　リプライ OK 2",
                TopicCommentId = topicCommentId,
                MemberId = 2,
                DisplayName = "メンバーテスト２",
                MainPhotoUrl = "http://aaa.bbb.jpg",
                Created = this.currentDateTime
            });
            return replies;
        }
    }
}
