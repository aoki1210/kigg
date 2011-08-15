using System;
using System.Collections.Generic;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Repository;
    using Service;

    public class VoteWeightFixture
    {
        private const float SameIpWeight = 5f;
        private const float DifferentIpWeight = 10f;

        private readonly Mock<IVoteRepository> _repository;
        private readonly VoteWeight _strategy;

        public VoteWeightFixture()
        {
            _repository = new Mock<IVoteRepository>();

            _strategy = new VoteWeight(_repository.Object, SameIpWeight, DifferentIpWeight);
        }

        [Fact]
        public void Calculate_Should_Return_Correct_Weight_For_Same_Ip()
        {
            var story = Setup("192.168.0.1", "192.168.0.1", "192.168.0.1");

            Assert.Equal((SameIpWeight * 2), _strategy.Calculate(SystemTime.Now(), story.Object));
        }

        [Fact]
        public void Calculate_Should_Return_Correct_Weight_For_Different_Ip()
        {
            var story = Setup("192.168.0.1", "192.168.0.2", "192.168.0.3");

            Assert.Equal((DifferentIpWeight * 2), _strategy.Calculate(SystemTime.Now(), story.Object));
        }

        [Fact]
        public void Calculate_Should_Return_Correct_Weight_For_Both_Ips()
        {
            var story = Setup("192.168.0.1", "192.168.0.1", "192.168.0.2");

            Assert.Equal((SameIpWeight + DifferentIpWeight), _strategy.Calculate(SystemTime.Now(), story.Object));
        }

        [Fact]
        public void Calculate_Should_Use_Vote_Repository()
        {
            var story = Setup("192.168.0.1", "192.168.0.1", "192.168.0.2");

            _strategy.Calculate(SystemTime.Now(), story.Object);

            _repository.Verify();
        }

        private Mock<Story> Setup(string ownerIp, params string[] promoterIps)
        {
            var ownerId = 1;

            var story = new Mock<Story>();
            var owner = new Mock<User>();

            owner.SetupGet(u => u.Id).Returns(ownerId);
            story.SetupGet(s => s.PostedBy).Returns(owner.Object);
            story.SetupGet(s => s.FromIPAddress).Returns(ownerIp);

            var votes = new List<Vote>();

            foreach (string ip in promoterIps)
            {
                var promoter = new Mock<User>();
                promoter.SetupGet(u => u.Id).Returns(1);

                var vote = new Mock<Vote>();
                vote.SetupGet(c => c.FromIPAddress).Returns(ip);
                vote.SetupGet(c => c.ByUser).Returns(promoter.Object);

                votes.Add(vote.Object);
            }

            _repository.Setup(r => r.FindAfter(It.IsAny<long>(), It.IsAny<DateTime>())).Returns(votes).Verifiable();

            return story;
        }
    }
}