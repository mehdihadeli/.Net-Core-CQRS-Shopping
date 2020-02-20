using System;
using System.Threading;
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
using Shopping.Infrastructure.Persistence;
using Shopping.Tests.UnitTests.Common;
using Xunit;
using Xunit.Abstractions;

namespace Shopping.Tests.UnitTests.CommandHandlers.User
{
    [Collection("MappingCollection")]
    public class RegisterUserCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly ITestOutputHelper _output;
        private readonly ILogger<RegisterUserCommandHandler> _logger;

        public RegisterUserCommandHandlerTests(MappingFixture mappingFixture, ITestOutputHelper output)
        {
            _mapper = mappingFixture.Mapper;
            _output = output;
            _logger = output.BuildLoggerFor<RegisterUserCommandHandler>();
        }

        [Fact]
        public async Task Should_Raise_UserRegisteredEvent_When_UserRegistered()
        {
            //Arrange
            var mediatorMock = new Mock<IMediator>();
            Mock<IUserManagerService> userManagerService = new Mock<IUserManagerService>();
            Mock<IIdentityDataContext> identityDataContext = new Mock<IIdentityDataContext>();

            var applicationUser = new ApplicationUser()
            {
                Email = String.Empty,
                UserName = String.Empty,
                FirstName = String.Empty,
                LastName = String.Empty,
                Id = 1,
                PasswordHash = String.Empty
            };

            userManagerService.Setup(r => r.CreateUser(It.IsAny<ApplicationUser>(), It.IsAny<String>()))
                .Returns(Task.FromResult(applicationUser));


            mediatorMock.Setup(p => p.Publish(It.IsAny<UserRegisteredEvent>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var sut = new RegisterUserCommandHandler(identityDataContext.Object, _mapper, _logger,
                mediatorMock.Object, userManagerService.Object);

            //Act
            await sut.Handle(new RegisterUserCommand(
                firstName: String.Empty,
                lastName: String.Empty,
                password: String.Empty,
                email: String.Empty,
                dateOfBirth: DateTime.Now,
                userName: String.Empty
            ), It.IsAny<CancellationToken>());

            //Assert
            mediatorMock.Verify(m => m.Publish(It.Is<UserRegisteredEvent>(cc => cc.Id == applicationUser.Id),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Should_Throws_DuplicateException_When_User_Already_Exists()
        {
            //Arrange
            var mediatorMock = new Mock<IMediator>();
            Mock<IUserManagerService> userManagerService = new Mock<IUserManagerService>();
            Mock<IIdentityDataContext> identityDataContext = new Mock<IIdentityDataContext>();

            var applicationUser = new ApplicationUser()
            {
                Email = String.Empty,
                UserName = String.Empty,
                FirstName = String.Empty,
                LastName = String.Empty,
                Id = 1,
                PasswordHash = String.Empty
            };
            userManagerService.Setup(r => r.CreateUser(It.IsAny<ApplicationUser>(), It.IsAny<String>()))
                .Throws<DuplicateException>();

            mediatorMock.Setup(p => p.Publish(It.IsAny<UserRegisteredEvent>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var sut = new RegisterUserCommandHandler(identityDataContext.Object, _mapper, _logger,
                mediatorMock.Object, userManagerService.Object);

            //Act/Assert
            await Assert.ThrowsAsync<DuplicateException>(async () =>
            {
                await sut.Handle(new RegisterUserCommand(
                    firstName: String.Empty,
                    lastName: String.Empty,
                    password: String.Empty,
                    email: String.Empty,
                    dateOfBirth: DateTime.Now,
                    userName: String.Empty
                ), It.IsAny<CancellationToken>());
            });
        }
    }
}