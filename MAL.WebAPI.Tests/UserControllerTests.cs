using MAL.Services;
using NSubstitute;
using MAL.Common;
using AutoMapper;
using MAL.WebAPI.Controllers;
using System;
using Xunit;

namespace MAL.WebAPI.Tests
{
    public class UserControllerTests
    {
        public UserControllerTests()
        {

        }

        [Fact]
        public void GetAllUsers_WhenLoginSuccessful_ReturnsAllUsers()
        {
            // Arrange
            IUserService userService = Substitute.For<IUserService>();
            IConfigProvider configProvider = Substitute.For<IConfigProvider>();
            IMapper mapper = Substitute.For<IMapper>();

            UserController userController = new UserController(userService, mapper);

            // Act
            userController.GetAllUsers();

            // Assert
            userService.Received(1).GetAllUsers();
        }

    }
}
