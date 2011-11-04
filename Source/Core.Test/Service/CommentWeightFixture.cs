using System;
using System.Collections.Generic;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using Domain.Entities;
    using Repository;
    using Service;

    public class CommentWeightFixture
    {
        private const float OwnerScore = 1f;
        private const float SameIpScore = 2f;
        private const float DifferentIpScore = 4f;

        private readonly Mock<ICommentRepository> _repository;
        private readonly CommentWeight _strategy;

        public CommentWeightFixture()
        {
            _repository = new Mock<ICommentRepository>();
            _strategy = new CommentWeight(_repository.Object, OwnerScore, SameIpScore, DifferentIpScore);
        }

        [Fact]
        public void Calculate_Should_Return_Correct_Weight_For_Author()
        {
            var story = Setup("192.168.0.1");

            Assert.Equal(OwnerScore, _strategy.Calculate(SystemTime.Now(), story.Object));
        }

        [Fact]
        public void Calculate_Should_Return_Correct_Weight_For_Same_IPAddress()
        {
            var story = Setup("192.168.0.1", "192.168.0.1" , "192.168.0.1");

            Assert.Equal(OwnerScore + (SameIpScore * 2), _strategy.Calculate(SystemTime.Now(), story.Object));
        }

        [Fact]
        public void Calculate_Should_Return_Correct_Weight_For_Different_IPAddress()
        {
            var story = Setup("192.168.0.1", "192.168.0.2" , "192.168.0.3");

            Assert.Equal(OwnerScore + (DifferentIpScore * 2), _strategy.Calculate(SystemTime.Now(), story.Object));
        }

        [Fact]
        public void Calculate_Should_Return_Correct_Weight_For_All_Combination()
        {
            var story = Setup("192.168.0.1", "192.168.0.1", "192.168.0.2");

            Assert.Equal((OwnerScore + SameIpScore + DifferentIpScore), _strategy.Calculate(SystemTime.Now(), story.Object));
        }

        private Mock<Story> Setup(string ownerIp, params string[] ipAddresses)
        {
            var comments = new List<Comment>();
            var ownerId = 1;

            var story = new Mock<Story>();
            var owner = new Mock<User>();

            owner.SetupGet(u => u.Id).Returns(ownerId);
            story.SetupGet(s => s.PostedBy).Returns(owner.Object);
            story.SetupGet(s => s.FromIPAddress).Returns(ownerIp);

            var ownerComment = new Mock<Comment>();
            ownerComment.SetupGet(c => c.ByUser).Returns(owner.Object);
            ownerComment.SetupGet(c => c.FromIPAddress).Returns(ownerIp);

            comments.Add(ownerComment.Object);
            int id = 1;
            foreach(string ip in ipAddresses)
            {
                var user = new Mock<User>();
                user.SetupGet(u => u.Id).Returns(++id);

                var comment = new Mock<Comment>();
                comment.SetupGet(c => c.FromIPAddress).Returns(ip);
                comment.SetupGet(c => c.ByUser).Returns(user.Object);

                comments.Add(comment.Object);
            }

            _repository.Setup(r => r.FindAfter(It.IsAny<long>(), It.IsAny<DateTime>())).Returns(comments).Verifiable();

            return story;
        }
    }
}