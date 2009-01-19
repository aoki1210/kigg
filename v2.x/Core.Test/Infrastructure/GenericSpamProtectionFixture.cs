using System;
using System.Collections.Specialized;

using Moq;
using Xunit;
using Xunit.Extensions;

namespace Kigg.Core.Test
{
    using Infrastructure;
    using Kigg.Test.Infrastructure;

    public class GenericExternalSpamProtectionFixture : BaseFixture
    {
        private const string Version = "1.0";
        private const string ApiKey = "a key";

        private const string BaseUrl = "rest.akismet.com";
        private static readonly string ApiKeyCheckUrl = "http://{0}/{1}/verify-key".FormatWith(BaseUrl, Version);
        private static readonly string CommentCheckUrl = "http://{0}.{1}/{2}/comment-check".FormatWith(ApiKey, BaseUrl, Version);
        private static readonly string SubmitUrl = "http://{0}.{1}/{2}/submit-spam".FormatWith(ApiKey, BaseUrl, Version);
        private static readonly string FalsePositiveUrl = "http://{0}.{1}/{2}/submit-ham".FormatWith(ApiKey, BaseUrl, Version);

        private const string ValidApiKeyResponse = "valid";

        private readonly Mock<IHttpForm> _httpForm;
        private readonly GenericExternalSpamProtection _spamProtection;

        public GenericExternalSpamProtectionFixture()
        {
            _httpForm = new Mock<IHttpForm>();
            _spamProtection = new GenericExternalSpamProtection("Akismet", BaseUrl, ApiKey, Version, settings.Object, _httpForm.Object);
        }

        [Fact]
        public void Default_Constructor_Should_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new GenericExternalSpamProtection("Dummy", BaseUrl, ApiKey, Version, settings.Object));
        }

        [Fact]
        public void Api_Key_Should_Be_Valid()
        {
            _httpForm.Expect(h => h.Post(ApiKeyCheckUrl, It.IsAny<NameValueCollection>())).Returns(ValidApiKeyResponse);
            _httpForm.Expect(h => h.Post(CommentCheckUrl, It.IsAny<NameValueCollection>())).Returns("true");

            Assert.DoesNotThrow(() => _spamProtection.IsSpam(CreateDummyContent()));
        }

        [Fact]
        public void Should_Throw_Exception_When_Api_Key_Is_Not_Valid()
        {
            _httpForm.Expect(h => h.Post(ApiKeyCheckUrl, It.IsAny<NameValueCollection>())).Returns("invalid");

            Assert.Throws<InvalidOperationException>(() => _spamProtection.IsSpam(CreateDummyContent()));
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        public void IsSpam_Should_Return_Correct_Result(string response, bool result)
        {
            _httpForm.Expect(h => h.Post(ApiKeyCheckUrl, It.IsAny<NameValueCollection>())).Returns(ValidApiKeyResponse);
            _httpForm.Expect(h => h.Post(CommentCheckUrl, It.IsAny<NameValueCollection>())).Returns(response);

            var content = CreateDummyContent();
            content.Extra.Add("foo", "bar");

            var isSpam = _spamProtection.IsSpam(content);

            Assert.Equal(result, isSpam);
        }

        [Fact]
        public void IsSpam_Should_Return_False_When_Null_Response_Is_Received()
        {
            _httpForm.Expect(h => h.Post(ApiKeyCheckUrl, It.IsAny<NameValueCollection>())).Returns(ValidApiKeyResponse);
            _httpForm.Expect(h => h.Post(CommentCheckUrl, It.IsAny<NameValueCollection>())).Returns((string) null);

            Assert.False(_spamProtection.IsSpam(CreateDummyContent()));
        }

        [Fact]
        public void IsSpam_Should_Use_HttpForm()
        {
            _httpForm.Expect(h => h.Post(ApiKeyCheckUrl, It.IsAny<NameValueCollection>())).Returns(ValidApiKeyResponse).Verifiable();
            _httpForm.Expect(h => h.Post(CommentCheckUrl, It.IsAny<NameValueCollection>())).Returns("false").Verifiable();

            _spamProtection.IsSpam(CreateDummyContent());

            _httpForm.Verify();
        }

        [Fact]
        public void IsSpam_Should_Forward_To_Next_Handler_When_Content_Is_Not_Spam()
        {
            var next = new Mock<ISpamProtection>();

            _spamProtection.NextHandler = next.Object;

            _httpForm.Expect(h => h.Post(ApiKeyCheckUrl, It.IsAny<NameValueCollection>())).Returns(ValidApiKeyResponse);
            _httpForm.Expect(h => h.Post(CommentCheckUrl, It.IsAny<NameValueCollection>())).Returns("false");

            next.Expect(sp => sp.IsSpam(It.IsAny<SpamCheckContent>())).Returns(false).Verifiable();

            _spamProtection.IsSpam(CreateDummyContent());

            next.Verify();
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        public void IsSpam_Async_Should_Return_Correct_Result(string response, bool result)
        {
            _httpForm.Expect(h => h.Post(ApiKeyCheckUrl, It.IsAny<NameValueCollection>())).Returns(ValidApiKeyResponse);
            _httpForm.Expect(h => h.PostAsync(CommentCheckUrl, It.IsAny<NameValueCollection>(), It.IsAny<Action<string>>(), It.IsAny<Action<Exception>>())).Callback((string url, NameValueCollection formFields, Action<string> onComplete, Action<Exception> onError) => onComplete(response));

            _spamProtection.IsSpam(CreateDummyContent(), (source, isSpam) => Assert.Equal(result, isSpam));
        }

        [Fact]
        public void IsSpam_Async_Should_Forward_To_Next_Handler_When_Not_Spam()
        {
            var next = new Mock<ISpamProtection>();

            _spamProtection.NextHandler = next.Object;

            _httpForm.Expect(h => h.Post(ApiKeyCheckUrl, It.IsAny<NameValueCollection>())).Returns(ValidApiKeyResponse);
            _httpForm.Expect(h => h.PostAsync(CommentCheckUrl, It.IsAny<NameValueCollection>(), It.IsAny<Action<string>>(), It.IsAny<Action<Exception>>())).Callback((string url, NameValueCollection formFields, Action<string> onComplete, Action<Exception> onError) => onComplete("foo"));

            next.Expect(sp => sp.IsSpam(It.IsAny<SpamCheckContent>(), It.IsAny<Action<string, bool>>())).Verifiable();

            _spamProtection.IsSpam(CreateDummyContent(), delegate { });

            next.Verify();
        }

        [Fact]
        public void IsSpam_Should_Return_False_When_Exception_Occurrs()
        {
            _httpForm.Expect(h => h.Post(ApiKeyCheckUrl, It.IsAny<NameValueCollection>())).Returns(ValidApiKeyResponse);
            _httpForm.Expect(h => h.PostAsync(CommentCheckUrl, It.IsAny<NameValueCollection>(), It.IsAny<Action<string>>(), It.IsAny<Action<Exception>>())).Callback((string url, NameValueCollection formFields, Action<string> onComplete, Action<Exception> onError) => onError(new InvalidOperationException()));

            _spamProtection.IsSpam(CreateDummyContent(), (source, isSpam) => Assert.False(isSpam));
        }

        [Fact]
        public void IsSpam_Should_Forward_To_Next_Handler_When_Exception_Occurrs()
        {
            var next = new Mock<ISpamProtection>();

            _spamProtection.NextHandler = next.Object;

            _httpForm.Expect(h => h.Post(ApiKeyCheckUrl, It.IsAny<NameValueCollection>())).Returns(ValidApiKeyResponse);
            _httpForm.Expect(h => h.PostAsync(CommentCheckUrl, It.IsAny<NameValueCollection>(), It.IsAny<Action<string>>(), It.IsAny<Action<Exception>>())).Callback((string url, NameValueCollection formFields, Action<string> onComplete, Action<Exception> onError) => onError(new InvalidOperationException()));

            next.Expect(sp => sp.IsSpam(It.IsAny<SpamCheckContent>(), It.IsAny<Action<string, bool>>())).Verifiable();

            _spamProtection.IsSpam(CreateDummyContent(), delegate { });

            next.Verify();
        }

        [Fact]
        public void IsSpam_Async_Should_Should_Use_HttpForm()
        {
            _httpForm.Expect(h => h.Post(ApiKeyCheckUrl, It.IsAny<NameValueCollection>())).Returns(ValidApiKeyResponse).Verifiable();
            _httpForm.Expect(h => h.PostAsync(CommentCheckUrl, It.IsAny<NameValueCollection>(), It.IsAny<Action<string>>(), It.IsAny<Action<Exception>>())).Verifiable();

            _spamProtection.IsSpam(CreateDummyContent(), delegate { });

            _httpForm.Verify();
        }

        [Fact]
        public void MarkAsSpam_Should_Use_HttpForm()
        {
            _httpForm.Expect(h => h.Post(ApiKeyCheckUrl, It.IsAny<NameValueCollection>())).Returns(ValidApiKeyResponse).Verifiable();
            _httpForm.Expect(h => h.PostAsync(SubmitUrl, It.IsAny<NameValueCollection>())).Verifiable();

            _spamProtection.MarkAsSpam(CreateDummyContent());

            _httpForm.Verify();
        }

        [Fact]
        public void MarkAsFalsePositive_Should_Use_HttpForm()
        {
            _httpForm.Expect(h => h.Post(ApiKeyCheckUrl, It.IsAny<NameValueCollection>())).Returns(ValidApiKeyResponse).Verifiable();
            _httpForm.Expect(h => h.PostAsync(FalsePositiveUrl, It.IsAny<NameValueCollection>())).Verifiable();

            _spamProtection.MarkAsFalsePositive(CreateDummyContent());

            _httpForm.Verify();
        }

        private static SpamCheckContent CreateDummyContent()
        {
            return new SpamCheckContent
                       {
                           Content = "dummy content",
                           ContentType = "social-news",
                           Url = "http://dummystory.com",
                           UserAgent = "Firefox",
                           UserIPAddress = "192.168.0.1",
                           UserName = "dummy user"
                       };
        }
    }
}