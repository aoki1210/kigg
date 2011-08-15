using System;

using Moq;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Kigg.Test.Infrastructure;

    public abstract class DecoratedRepositoryFixture : BaseFixture
    {
        protected static User CreateStubUser()
        {
            var user = new Mock<User>();

            user.SetupGet(u => u.Id).Returns(1);
            user.SetupGet(u => u.UserName).Returns("Stub");
            user.SetupGet(u => u.Email).Returns("stub@tdd.com");

            return user.Object;
        }

        protected static Category CreateStubCategory()
        {
            var category = new Mock<Category>();

            category.SetupGet(c => c.Id).Returns(1);
            category.SetupGet(c => c.Name).Returns("Stub");

            return category.Object;
        }

        protected static Tag CreateStubTag()
        {
            var tag = new Mock<Tag>();

            tag.SetupGet(t => t.Id).Returns(1);
            tag.SetupGet(t => t.Name).Returns("Stub");

            return tag.Object;
        }

        protected static Story CreateStubStory()
        {
            var story = new Mock<Story>();

            story.SetupGet(s => s.Id).Returns(1);
            story.SetupGet(s => s.Title).Returns("Stub");
            story.SetupGet(s => s.BelongsTo).Returns(CreateStubCategory());

            return story.Object;
        }

        protected static Comment CreateStubComment()
        {
            var comment = new Mock<Comment>();

            comment.SetupGet(c => c.Id).Returns(1);
            comment.SetupGet(c => c.ForStory).Returns(CreateStubStory());

            return comment.Object;
        }

        protected static Vote CreateStubVote()
        {
            var story = new Mock<Story>();

            story.SetupGet(s => s.Title).Returns("Stub Title");
            story.SetupGet(s => s.Url).Returns("Stub Url");

            var vote = new Mock<Vote>();

            vote.SetupGet(v => v.ForStory).Returns(story.Object);

            return vote.Object;
        }

        protected static KnownSource CreateStubKnownSource()
        {
            var knownSource = new Mock<KnownSource>();

            knownSource.SetupGet(ks => ks.Url).Returns("http://knownsoure.com");
            knownSource.SetupGet(ks => ks.Grade).Returns(KnownSourceGrade.A);

            return knownSource.Object;
        }
    }
}