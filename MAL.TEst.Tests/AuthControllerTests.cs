using AutoFixture;
using AutoMapper;
using FluentAssertions;
using MAL.DTO;
using MAL.Services;
using MAL.TEst;
using MAL.TEst.Controllers;
using MAL.WebAPI;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace MAL.TEst.Tests
{
    public class AuthControllerTests
    {
        private readonly AuthController _authController;
        private readonly IUserService _userService;
        public AuthControllerTests()
        {  
            var mapper = Substitute.For<IMapper>();
            _userService = Substitute.For<IUserService>();
            _authController = new AuthController(_userService, mapper);
        }

        [Fact]
        public void GetAuthToken_WhenLoginSuccessful()
        {
            // Arrange
            _userService.LoginUser(Arg.Any<UserDto>()).Returns("JwtTokenString");

            Fixture fixture = new Fixture();
            var userLoginViewModel = fixture.Create<UserLoginViewModel>();

            // Act
            var response = _authController.GetAuthToken(userLoginViewModel);
            var result = response as OkObjectResult;

            // Assert
            result.Value.ToString().Should().Be("{ token = JwtTokenString }");
        }

        [Fact]
        public void GetAuthToken_WhenLoginFailed()
        {
            // Arrange
            _userService.LoginUser(Arg.Any<UserDto>()).Returns("");

            Fixture fixture = new Fixture();
            var userLoginViewModel = fixture.Create<UserLoginViewModel>();

            // Act
            var response = _authController.GetAuthToken(userLoginViewModel);
            var result = response as UnauthorizedResult;

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }
    }
}
