using System;
using System.Threading.Tasks;
using Common.Web.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shopping.Application.Services;
using Shopping.Core.Domains;
using Shopping.Infrastructure.Persistence;
using Shopping.Tests.UnitTests.Common;
using Shouldly;
using Xunit;

namespace Shopping.Tests.UnitTests.Services
{
    public class UserManagerServiceTests
    {
        #region GetUser

        [Fact]
        public async Task Should_GetUser_With_Valid_Id()
        {
            //Arrange
            var user = new ApplicationUser
            {
                Email = String.Empty,
                UserName = String.Empty,
                FirstName = "Test",
                LastName = "Test",
                Id = 1,
                PasswordHash = String.Empty
            };
                
            var dbSetMock = new Mock<DbSet<ApplicationUser>>();
            var dbContextMock = new Mock<ApplicationIdentityDbContext>();
            dbContextMock.Setup(s => s.Set<ApplicationUser>()).Returns(dbSetMock.Object);

            var userManagerMock = IdentityMockHelpers.MockUserManager<ApplicationUser>();
            userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<String>()))
                .Returns(Task.FromResult(user));
            var roleManagerMock = IdentityMockHelpers.MockRoleManager<ApplicationRole>().Object;
            UserManagerService sut = new UserManagerService(userManagerMock.Object, roleManagerMock);

            //Act
            var returnUser = await sut.GetUserById(user.Id);

            //Assert
            returnUser.ShouldNotBeNull();
            returnUser.ShouldBeOfType<ApplicationUser>();
            returnUser.Id.ShouldBe(user.Id);
            returnUser.FirstName.ShouldBe(user.FirstName);
            returnUser.LastName.ShouldBe(user.LastName);
        }


        [Fact]
        public async Task Should_Throw_NotFoundException_When_User_Not_Found()
        {
            //Arrange
            var user = new ApplicationUser
            {
                Email = String.Empty,
                UserName = String.Empty,
                FirstName = "Test",
                LastName = "Test",
                Id = 1,
                PasswordHash = String.Empty
            };
            
            var dbSetMock = new Mock<DbSet<ApplicationUser>>();
            var dbContextMock = new Mock<ApplicationIdentityDbContext>();
            dbContextMock.Setup(s => s.Set<ApplicationUser>()).Returns(dbSetMock.Object);

            var userManagerMock = IdentityMockHelpers.MockUserManager<ApplicationUser>();
            userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<String>()))
                .Throws<NotFoundException>();
            var roleManagerMock = IdentityMockHelpers.MockRoleManager<ApplicationRole>().Object;
            UserManagerService sut = new UserManagerService(userManagerMock.Object, roleManagerMock);
            
            //Act/Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await sut.GetUserById(user.Id);
            });
        }

        #endregion

        #region GetUsers

        #endregion
    }
}