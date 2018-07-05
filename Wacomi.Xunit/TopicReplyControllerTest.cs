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

        private MemberProfile TestMemberData = new MemberProfile{
            Id = 1,
            AppUser = new AppUser(){
                DisplayName = "MemberTest",
                MainPhotoUrl =  "http:abc.eft.jpg"
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
