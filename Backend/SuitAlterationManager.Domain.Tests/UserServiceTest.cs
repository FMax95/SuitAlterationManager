using NSubstitute;
using SuitAlterationManager.Domain.Base.Validation;
using SuitAlterationManager.Domain.SystemManagement;
using SuitAlterationManager.Domain.SystemManagement.DTO;
using SuitAlterationManager.Domain.SystemManagement.Repositories;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using SuitAlterationManager.Domain.Tests.Builder;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using DomainService = SuitAlterationManager.Domain.SystemManagement.Services;

namespace SuitAlterationManager.Domain.Tests
{
    public class UserServiceTest
    {
        private IUserRepository userRepository;
        private DomainService.Interfaces.IUserService userService;

        public UserServiceTest()
        {
            this.userRepository = Substitute.For<IUserRepository>();
            this.userService = new DomainService.UserService(userRepository);
        }

        [Theory(DisplayName = "Create New User")]
        [InlineData("a@b.it", "password")]
        public async void CreateUser(string email, string password)
        {
            List<GroupID> groups = new List<GroupID>() { new GroupID(Guid.NewGuid()) };
            UserCreatedDTO user = await userService.CreateUser(email, password, groups);

            Assert.Equal(email, user.Email);
            Assert.NotEmpty(user.Password);
            Assert.True(user.IsEnabled);
            Assert.Single(user.Groups);
        }

        [Theory(DisplayName = "Create New User With invalid Email")]
        [InlineData("mailnonvalida", "password")]
        public void CreateUserWithInvalidMail(string email, string password)
        {
            List<GroupID> groups = new List<GroupID>() { new GroupID(Guid.NewGuid()) };
            Task act() => userService.CreateUser(email, password, groups);

            var expectedException = new DomainException(ErrorCodes.EmailNotValid);
            var thrownException = Assert.ThrowsAsync<DomainException>(act);
            Assert.Equal(expectedException.Code, thrownException.Result.Code);
        }

        [Theory(DisplayName = "Create New User Without groups")]
        [InlineData("a@b.it", "password")]
        public void CreateUserWithoutGroups(string email, string password)
        {
            List<GroupID> groups = new List<GroupID>();
            Task act() => userService.CreateUser(email, password, groups);

            var expectedException = new DomainException(ErrorCodes.UserWithoutGroups);
            var thrownException = Assert.ThrowsAsync<DomainException>(act);
            Assert.Equal(expectedException.Code, thrownException.Result.Code);
        }

        [Theory(DisplayName = "Create New User With a Mail already in use")]
        [InlineData("a@b.it", "password")]
        public void CreateUserWithMailAlreadyUsed(string email, string password)
        {
            List<GroupID> groups = new List<GroupID>() { new GroupID(Guid.NewGuid()) };
            userRepository.ExistsWithEmailAsync(email).Returns(true);
            Task act() => userService.CreateUser(email, password, groups);

            var expectedException = new DomainException(ErrorCodes.EmailDuplicated);
            var thrownException = Assert.ThrowsAsync<DomainException>(act);
            Assert.Equal(expectedException.Code, thrownException.Result.Code);
        }

        [Theory(DisplayName = "Update User")]
        [InlineData("nuovamail@mail.it")]
        public void UpdateUser(string email)
        {
            List<GroupID> groups = new List<GroupID>() { new GroupID(Guid.NewGuid()) };
            UserID userID = new UserID(Guid.NewGuid());
            User user = new UserBuilder();
            userRepository.GetAsync(userID).Returns(user);
            userRepository.ExistsWithEmailAsync(email).Returns(false);

            userService.UpdateUser(userID, email, true, groups);

            Assert.Equal(email, user.Email);
        }
    }
}
