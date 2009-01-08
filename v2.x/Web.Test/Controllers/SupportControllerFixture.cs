using System.Web.Mvc;

using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using DomainObjects;
    using Infrastructure;
    using Repository;
    using Kigg.Test.Infrastructure;

    public class SupportControllerFixture : BaseFixture
    {
        private readonly Mock<IEmailSender> _emailSender;
        private readonly SupportController _controller;

        public SupportControllerFixture()
        {
            _emailSender = new Mock<IEmailSender>();

            var userRepository = new Mock<IUserRepository>();

            _controller = new SupportController(_emailSender.Object)
                              {
                                  Settings = settings.Object,
                                  UserRepository = userRepository.Object
                              };

            var httpContext = _controller.MockHttpContext();

            httpContext.User.Identity.ExpectGet(i => i.Name).Returns("DummyUser");
            httpContext.User.Identity.ExpectGet(i => i.IsAuthenticated).Returns(true);

            userRepository.Expect(r => r.FindByUserName(It.IsAny<string>())).Returns(new Mock<IUser>().Object);
        }

        [Fact]
        public void Faq_Should_Render_Default_View()
        {
            var result = (ViewResult) _controller.Faq();

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public void Contact_Should_Render_Default_View()
        {
            var result = (ViewResult) _controller.Contact();

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public void Contact_Should_Send_Email()
        {
            JsonViewData viewData = (JsonViewData) ((JsonResult)_controller.Contact("xxx@xxx.com", new string('x', 4), new string('x', 16))).Data;

            Assert.True(viewData.isSuccessful);
        }

        [Fact]
        public void Contact_Should_Use_EmailSender_To_Send_Email()
        {
            _emailSender.Expect(s => s.NotifyFeedback(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            _controller.Contact("xxx@xxx.com", new string('x', 4), new string('x', 16));

            _emailSender.Verify();
        }

        [Fact]
        public void Contact_Should_Return_Error_When_Blank_Email_Is_Passed()
        {
            JsonViewData viewData = (JsonViewData)((JsonResult)_controller.Contact(string.Empty, new string('x', 4), new string('x', 16))).Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Email cannot be blank.", viewData.errorMessage);
        }

        [Fact]
        public void Contact_Should_Return_Error_When_Invalid_Email_Is_Passed()
        {
            JsonViewData viewData = (JsonViewData)((JsonResult)_controller.Contact("xxx", new string('x', 4), new string('x', 16))).Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Invalid email format.", viewData.errorMessage);
        }

        [Fact]
        public void Contact_Should_Return_Error_When_Blank_Name_Is_Passed()
        {
            JsonViewData viewData = (JsonViewData)((JsonResult)_controller.Contact("xxx@xxx.com", string.Empty, new string('x', 16))).Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Name cannot be blank.", viewData.errorMessage);
        }

        [Fact]
        public void Contact_Should_Return_Error_When_Name_Is_Less_Than_Four_Character()
        {
            JsonViewData viewData = (JsonViewData)((JsonResult)_controller.Contact("xxx@xxx.com", "xxx", new string('x', 16))).Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Name cannot be less than 4 character.", viewData.errorMessage);
        }

        [Fact]
        public void Contact_Should_Return_Error_When_Blank_Message_Is_Passed()
        {
            JsonViewData viewData = (JsonViewData)((JsonResult)_controller.Contact("xxx@xxx.com", new string('x', 4), string.Empty)).Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Message cannot be blank.", viewData.errorMessage);
        }

        [Fact]
        public void Contact_Should_Return_Error_When_Message_Is_Less_Than_Sixteen_Character()
        {
            JsonViewData viewData = (JsonViewData)((JsonResult)_controller.Contact("xxx@xxx.com", "xxxx", new string('x', 15))).Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Message cannot be less than 16 character.", viewData.errorMessage);
        }

        [Fact]
        public void About_Should_Render_Default_View()
        {
            var result = (ViewResult) _controller.About();

            Assert.Equal(string.Empty, result.ViewName);
        }
    }
}