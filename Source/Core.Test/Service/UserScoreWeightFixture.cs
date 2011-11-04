using System;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using Domain.Entities;
    using Repository;
    using Service;

    public class UserScoreWeightFixture
    {
        private const float ScorePercent = 0.01f;
        private const float AdminMultiply = 4f;
        private const float ModaratorMultiply = 2f;

        private readonly Mock<IVoteRepository> _repository;
        private readonly UserScoreWeight _strategy;

        public UserScoreWeightFixture()
        {
            _repository = new Mock<IVoteRepository>();
            _strategy = new UserScoreWeight(_repository.Object, ScorePercent, AdminMultiply, ModaratorMultiply);
        }

        [Fact]
        public void Calculate_Should_Return_Correct_Weight()
        {
            var user1 = new Mock<User>();
            var user2 = new Mock<User>();
            var user3 = new Mock<User>();

            var moderator = new Mock<User>();
            var admin = new Mock<User>();

            user1.SetupGet(u => u.Id).Returns(1);
            user2.SetupGet(u => u.Id).Returns(1);
            user3.SetupGet(u => u.Id).Returns(1);
            user1.SetupGet(u => u.CurrentScore).Returns(100);
            user2.SetupGet(u => u.CurrentScore).Returns(200);
            user3.SetupGet(u => u.CurrentScore).Returns(300);

            moderator.SetupGet(u => u.Id).Returns(1);
            admin.SetupGet(u => u.Id).Returns(1);
            moderator.SetupGet(u => u.Role).Returns(Role.Moderator);
            admin.SetupGet(u => u.Role).Returns(Role.Administrator);

            var user1Vote = new Mock<Vote>();
            var user2Vote = new Mock<Vote>();
            var user3Vote = new Mock<Vote>();

            var moderatorVote = new Mock<Vote>();
            var adminVote = new Mock<Vote>();

            user1Vote.Setup(v => v.ByUser).Returns(user1.Object);
            user2Vote.Setup(v => v.ByUser).Returns(user2.Object);
            user3Vote.Setup(v => v.ByUser).Returns(user3.Object);

            moderatorVote.Setup(v => v.ByUser).Returns(moderator.Object);
            adminVote.Setup(v => v.ByUser).Returns(admin.Object);

            var story = new Mock<Story>();

            _repository.Setup(r => r.FindAfter(It.IsAny<long>(), It.IsAny<DateTime>())).Returns(new[] { user1Vote.Object, user2Vote.Object, user3Vote.Object, moderatorVote.Object, adminVote.Object }).Verifiable();

            Assert.Equal(
                            (
                                (Convert.ToDouble(user1.Object.CurrentScore) * ScorePercent) +
                                (Convert.ToDouble(user2.Object.CurrentScore) * ScorePercent) +
                                (Convert.ToDouble(user3.Object.CurrentScore) * ScorePercent) +
                                ((Convert.ToDouble(user3.Object.CurrentScore) * ScorePercent) * ModaratorMultiply) +
                                ((Convert.ToDouble(user3.Object.CurrentScore) * ScorePercent) * AdminMultiply)
                            ), 
                            _strategy.Calculate(SystemTime.Now(), story.Object)
                        );

            _repository.Verify();
        }
    }
}