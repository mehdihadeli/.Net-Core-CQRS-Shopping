using System;
using System.Threading.Tasks;
using AutoMapper;
using Common.Web.Common.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Shopping.Application.CommandHandlers.User;
using Shopping.Core.Commands;
using Shopping.Core.Domains;
using Shopping.Core.Events;
using Shopping.Core.Services;
using Shopping.Infrastructure.Persistence.Identity;
using Shopping.Tests.UnitTests.Common;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Shopping.Tests.UnitTests.CommandHandlers.User
{
    [Collection("MappingCollection")]
    public class UpdateUserInfoCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly ITestOutputHelper _output;
        private readonly ILogger<UpdateUserInfoCommandHandler> _logger;

        public UpdateUserInfoCommandHandlerTests(MappingFixture mappingFixture, ITestOutputHelper output)
        {
            _mapper = mappingFixture.Mapper;
            _output = output;
            _logger = output.BuildLoggerFor<UpdateUserInfoCommandHandler>();
        }
        [Fact]
        public async Task Should_Update_User_Profile()
        {
            //Arrange
            Mock<IUserManagerService> userManagerService = new Mock<IUserManagerService>();
            Mock<IIdentityDataContext> identityDataContext = new Mock<IIdentityDataContext>();

            var message = new UpdateUserInfoCommand(
                id: 1,
                firstName: "Test",
                lastName: "Test"
            );

            var applicationUser = new ApplicationUser()
            {
                Email = String.Empty,
                UserName = String.Empty,
                FirstName = String.Empty,
                LastName = String.Empty,
                Id = 1,
                PasswordHash = String.Empty
            };

            userManagerService.Setup(r => r.GetUserById(It.Is<int>(id => id == message.Id)))
                .Returns(Task.FromResult(applicationUser));

            userManagerService.Setup(r => r.UpdateProfile(It.IsAny<int>(), It.IsAny<String>(), It.IsAny<String>()))
                .Returns((int id, string firstName, string lastName) =>
                {
                    applicationUser.FirstName = firstName;
                    applicationUser.LastName = lastName;
                    return Task.FromResult(applicationUser);
                });


            var sut = new UpdateUserInfoCommandHandler(identityDataContext.Object, userManagerService.Object);

            //Act
            var result = await sut.Handle(message, default);
            //Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType<Unit>();
            applicationUser.FirstName.ShouldBe(message.FirstName);
            applicationUser.LastName.ShouldBe(message.LastName);
        }

        [Fact]
        public async Task Should_Throw_NotFoundException_In_Update_User_Profile_When_User_Not_Found()
        {
            //Arrange
            Mock<IUserManagerService> userManagerService = new Mock<IUserManagerService>();
            Mock<IIdentityDataContext> identityDataContext = new Mock<IIdentityDataContext>();

            var message = new UpdateUserInfoCommand(
                id: 1,
                firstName: "Test2",
                lastName: "Test2"
            );

            var applicationUser = new ApplicationUser()
            {
                Email = String.Empty,
                UserName = String.Empty,
                FirstName = "Test",
                LastName = "Test",
                Id = 1,
                PasswordHash = String.Empty
            };

            userManagerService.Setup(r => r.GetUserById(It.Is<int>(id => id == message.Id)))
                .Returns(Task.FromResult(applicationUser));

            userManagerService.Setup(r => r.UpdateProfile(It.IsAny<int>(), It.IsAny<String>(), It.IsAny<String>()))
                .Throws<NotFoundException>();

            var sut = new UpdateUserInfoCommandHandler(identityDataContext.Object, userManagerService.Object);

            //Act/Assert
            await Assert.ThrowsAsync<NotFoundException>(async () => { await sut.Handle(message, default); });
        }
    }
}