using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Web.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shopping.Controllers;
using Shopping.Core.Commands;
using Shopping.Core.Domains;
using Shopping.Core.Dtos.User;
using Shopping.Core.Queries;
using Shouldly;
using Xunit;

namespace Shopping.Tests.UnitTests.Controllers
{
    public class UserControllerTests : BaseControllerTests
    {
        readonly Mock<ILogger<UserController>> _logger;

        public UserControllerTests()
        {
            _logger = new Mock<ILogger<UserController>>();
        }

        #region POST

        [Fact]
        public async Task CreateUser_ReturnsCreatedAt_WhenUserCreated()
        {
            //Arrange
            var userCommand = new RegisterUserCommand(
                email: string.Empty,
                firstName: String.Empty,
                lastName: String.Empty,
                password: String.Empty,
                dateOfBirth: DateTime.Now,
                userName: string.Empty
            );
            ApplicationUser createdUser = null;

            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
                .Returns((RegisterUserCommand command, CancellationToken c) => Task.FromResult(createdUser =
                    new ApplicationUser()
                    {
                        PasswordHash = command.Password,
                        FirstName = command.FirstName,
                        LastName = command.LastName,
                        UserName = command.UserName,
                        Email = command.Email,
                        SecurityStamp = Guid.NewGuid().ToString()
                    }));

            var controller = new UserController(mockMediator.Object);

            //Act
            var result = await controller.RegisterUser(userCommand, It.IsAny<CancellationToken>());

            //Assert
            mockMediator.Verify(x =>
                x.Send(It.Is<RegisterUserCommand>(r => r == userCommand), It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<CreatedAtRouteResult>(result);
            Assert.Equal(createdUser, (result as CreatedAtRouteResult)?.Value);
        }

        [Fact]
        public async Task CreateUser_ReturnsConflict_WhenUserExists()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
                .Throws<DuplicateException>();
            var controller = new UserController(mockMediator.Object);

            //Act/Assert
            await Assert.ThrowsAsync<DuplicateException>(async () =>
            {
                await controller.RegisterUser(new RegisterUserCommand(
                    email: string.Empty,
                    firstName: String.Empty,
                    lastName: String.Empty,
                    password: String.Empty,
                    dateOfBirth: DateTime.Now,
                    userName: string.Empty
                ), default);
            });
        }

        #endregion

        #region GET

        [Fact]
        public async Task Should_Get_User_When_User_Found_And_Returns_Ok()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(It.IsAny<UserQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new UserDetailInfoDto(){Id = 1}));
            var controller = new UserController(mockMediator.Object);

            //Act
            var result = await controller.FindUserById(1);

            //Assert
            result.Result.ShouldNotBeNull();
            result.Result.ShouldBeOfType<OkObjectResult>();
            (result.Result as ObjectResult)?.Value.ShouldBeOfType<UserDetailInfoDto>();
            ((result.Result as ObjectResult)?.Value as UserDetailInfoDto)?.Id.ShouldBe(1);
        }

        [Fact]
        public async Task Throw_NotFoundException_In_Find_User_When_User_Not_Found()
        {
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(It.IsAny<UserQuery>(), It.IsAny<CancellationToken>()))
                .Throws<NotFoundException>();

            var controller = new UserController(mockMediator.Object);

            //Act/Assert
            await Should.ThrowAsync<NotFoundException>(async () => { await controller.FindUserById(0); });
        }

        [Fact]
        public async Task Get_ReturnsOkCollectionOfProfileView_WhenAnyUserFound()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(It.IsAny<FilterUserQuery>(), default))
                .Returns(Task.FromResult<IEnumerable<UserDetailInfoDto>>(new List<UserDetailInfoDto>()
                    {new UserDetailInfoDto()}));
            var controller = new UserController(mockMediator.Object);

            //Execute
            var result = await controller.FindUsers(new FilterUserQuery(), default);

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenUsersNotFound()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(It.IsAny<FilterUserQuery>(), default)).Throws<NotFoundException>();
            var controller = new UserController(mockMediator.Object);


            //Act/Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await controller.FindUsers(new FilterUserQuery(), default);
            });

            //NOTE: NotFoundException response handled byt the pipeline
        }

        #endregion

        #region PUT

        [Fact]
        public async Task Should_Update_User_And_Return_NoContentResult()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            var user = new ApplicationUser()
            {
                Id = 1,
                PasswordHash = String.Empty,
                FirstName = String.Empty,
                LastName = String.Empty,
                UserName = String.Empty,
                Email = String.Empty,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var message = new UpdateUserInfoCommand(
                1,
                "Test",
                "Test"
            );
            mockMediator.Setup(x => x.Send(It.IsAny<UpdateUserInfoCommand>(), default))
                .Returns<UpdateUserInfoCommand, CancellationToken>((command, c) =>
                {
                    user.FirstName = command.FirstName;
                    user.LastName = command.LastName;
                    return Task.FromResult(Unit.Value);
                });
            var controller = new UserController(mockMediator.Object);

            SetAuthenticationContext(controller);
            //Act
            var result = await controller.UpdateUserInfo(message);

            //Assert
            result.ShouldNotBeNull();
            user.FirstName.ShouldBe("Test");
            user.LastName.ShouldBe("Test");
            result.ShouldBeAssignableTo<NoContentResult>();
        }

        [Fact]
        public async Task Should_Throw_NotFoundException_When_Profile_Not_Found()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(It.IsAny<UpdateUserInfoCommand>(), default))
                .Throws<NotFoundException>();
            var controller = new UserController(mockMediator.Object);

            SetAuthenticationContext(controller);

            //Act/Assert
            await Should.ThrowAsync<NotFoundException>(async () =>
            {
                await controller.UpdateUserInfo(new UpdateUserInfoCommand(
                    id: 0,
                    firstName: String.Empty,
                    lastName: String.Empty
                ));
            });
            //NOTE: NotFoundException response handled byt the pipeline
        }


        [Fact]
        public async Task Should_Update_Password_And_Return_NoContentResult()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(It.IsAny<UpdateUserPasswordCommand>(), default))
                .Returns(Task.FromResult(Unit.Value));
            var controller = new UserController(mockMediator.Object);

            SetAuthenticationContext(controller);

            //Act
            var result = await controller.UpdateUserPassword(new UpdateUserPasswordCommand(
                userId: 1,
                oldPassword: String.Empty,
                newPassword: String.Empty
            ));

            //Assert
            result.ShouldNotBeNull();
            result.ShouldBeAssignableTo<NoContentResult>();
        }

        [Fact]
        public async Task Throw_NotFoundException_In_Update_Password_When_Profile_Not_Found()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(It.IsAny<UpdateUserPasswordCommand>(), default))
                .Returns(Task.FromResult(Unit.Value));
            var controller = new UserController(mockMediator.Object);

            SetAuthenticationContext(controller);

            //Act/Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await controller.UpdateUserPassword(new UpdateUserPasswordCommand(
                    userId: 0,
                    oldPassword: String.Empty,
                    newPassword: String.Empty
                ));
            });
        }

        #endregion
    }
}