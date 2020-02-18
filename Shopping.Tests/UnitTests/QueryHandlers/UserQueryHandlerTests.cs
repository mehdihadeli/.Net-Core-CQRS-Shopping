using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Web.Common.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using Shopping.Application.QueryHandlers.User;
using Shopping.Core.Domains;
using Shopping.Core.Dtos.User;
using Shopping.Core.Queries;
using Shopping.Core.Services;
using Shopping.Infrastructure.Persistence.Identity;
using Shopping.Tests.UnitTests.Common;
using Shouldly;
using Xunit;

namespace Shopping.Tests.UnitTests.QueryHandlers
{
    [Collection("MappingCollection")]
    public class UserQueryHandlerTests
    {
        private readonly IMapper _mapper;

        public UserQueryHandlerTests(MappingFixture fixture)
        {
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task Should_Get_User_Profile_When_User_Found()
        {
            //Arrange
            var applicationUser = new ApplicationUser
            {
                Email = String.Empty,
                UserName = String.Empty,
                FirstName = "Test",
                LastName = "Test",
                Id = 1,
                PasswordHash = String.Empty
            };
            var message = new UserQuery(applicationUser.Id);
            
            Mock<IUserManagerService> userManagerService = new Mock<IUserManagerService>();
            userManagerService.Setup(s => s.GetUserById(It.IsAny<int>())).ReturnsAsync(applicationUser);
            var sut = new UserQueryHandler(userManagerService.Object, _mapper);

            //ActT
            var result = await sut.Handle(message, default);

            //Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType<UserDetailInfoDto>();
            result.Id.ShouldBe(applicationUser.Id);
            result.FirstName.ShouldBe(applicationUser.FirstName);
            result.LastName.ShouldBe(applicationUser.LastName);
        }

        [Fact]
        public async Task Should_Throw_NotFoundException_In_Get_User_Profile_When_User_Not_Found()
        {
            //Arrange
            var applicationUser = new ApplicationUser
            {
                Email = String.Empty,
                UserName = String.Empty,
                FirstName = "Test",
                LastName = "Test",
                Id = 1,
                PasswordHash = String.Empty
            };
            var message = new UserQuery(applicationUser.Id);

            Mock<IUserManagerService> userManagerService = new Mock<IUserManagerService>();
            userManagerService.Setup(s => s.GetUserById(It.IsAny<int>())).Throws<NotFoundException>();
            var sut = new UserQueryHandler(userManagerService.Object, _mapper);

            //Act/Assert
            await Assert.ThrowsAsync<NotFoundException>(async () => { await sut.Handle(message, default); });
        }


        [Fact]
        public async Task Should_Get_User_Profiles_With_Specific_Input_Filter_And_When_Users_Found()
        {
            //Arrange
            List<ApplicationUser> users = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Email = String.Empty,
                    UserName = String.Empty,
                    FirstName = "Test",
                    LastName = "Test",
                    Id = 1,
                    PasswordHash = String.Empty
                }
            };
            
            Mock<IUserManagerService> userManagerService = new Mock<IUserManagerService>();
            userManagerService.Setup(s => s.GetUsers(
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<int>(),
                It.IsAny<int>()
            )).ReturnsAsync(users);

            var sut = new FilterUserQueryHandler(userManagerService.Object, _mapper);

            //Act
            var result = (await sut.Handle(new FilterUserQuery(), default)).ToList();

            //Assert
            result.ShouldNotBeNull();
            result.ShouldBeAssignableTo<IEnumerable<UserDetailInfoDto>>();
            result.Count.ShouldBe(1);
        }

        [Fact]
        public async Task Throw_NotFoundException_For_Get_User_Profiles_And_When_User_NotFound()
        {
            //Arrange
            Mock<IUserManagerService> userManagerService = new Mock<IUserManagerService>();
            userManagerService.Setup(s => s.GetUsers(
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<int>(),
                It.IsAny<int>()
            )).Throws<NotFoundException>();
            
            var sut = new FilterUserQueryHandler(userManagerService.Object, _mapper);
        
            //Act/Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
               var res = (await sut.Handle(new FilterUserQuery(), default));
            });
        }
    }
}